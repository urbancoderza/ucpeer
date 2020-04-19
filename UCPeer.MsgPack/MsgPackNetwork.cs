using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UCPeer.MsgPack.Collections;
using UCSimpleSocket;

namespace UCPeer.MsgPack
{
	public sealed partial class MsgPackNetwork : INetwork
	{
		private readonly TcpListener _listener;
		private readonly Task _listenWorker;
		private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
		private readonly BlockingCollection<PipelineContext> _receiveQueue;
		private readonly SimpleConcurrentDictionary<Tuple<IPEndPoint, IPEndPoint>, Connection> _connections =
			new SimpleConcurrentDictionary<Tuple<IPEndPoint, IPEndPoint>, Connection>(new ConnectionComparer(), false, true);

		public bool Running { get; } = true;

		public MsgPackNetwork(IPEndPoint localListenerEndPoint = null)
		{
			_receiveQueue = new BlockingCollection<PipelineContext>(new ConcurrentQueue<PipelineContext>());

			if (localListenerEndPoint != null)
			{
				_listener = new TcpListener(localListenerEndPoint ?? new IPEndPoint(IPAddress.Any, 0));
				_listener.Start();

				_listenWorker = new Task(t => Listen(_cancelTokenSource.Token), _cancelTokenSource.Token, TaskCreationOptions.LongRunning);
				_listenWorker.Start();
			}
		}

		private void Listen(CancellationToken token)
		{
			try
			{
				while (_disposed == 0 && !token.IsCancellationRequested)
				{
					while (Running)
					{
						var tcpClient = _listener.AcceptTcpClient();
						token.ThrowIfCancellationRequested();

						if (tcpClient != null)
						{
							var socket = new Connection(Emit, tcpClient, ownsTcpClient: true);
							lock (_connections.SyncRoot)
								_connections[Tuple.Create(socket.LocalEndPoint, socket.RemoteEndPoint)] = socket;
						}
					}
				}
			}
			catch (TaskCanceledException)
			{ }
			catch (OperationCanceledException)
			{ }
		}

		private void Emit(Connection connection, byte[] data, DateTime receivedDateTime)
		{
			if (connection == null)
				return;

			var context = new PipelineContext
			{
				Destination = connection.LocalEndPoint,
				Raw = data,
				ReceivedTime = receivedDateTime,
				Source = connection.RemoteEndPoint
			};

			_receiveQueue.Add(context);
		}

		public Task<PipelineContext> ReceiveAsync(CancellationToken cancelToken)
		{
			PipelineContext context;
			do
			{
				context = _receiveQueue.Take();
			} while (context == null);

			return Task.FromResult(context);
		}

		public async Task SendAsync(PipelineContext context)
		{
			if (context == null)
				return;

			Connection con;
			lock (_connections.SyncRoot)
			{
				var key = Tuple.Create(context.Source, context.Destination);
				con = _connections[key];

				if (con == null)
				{
					var client = new TcpClient(context.Source);
					client.Connect(context.Destination.Address, context.Destination.Port);
					con = new Connection(Emit, client, ownsTcpClient: true);
					_connections[key] = con;
				}
			}

			await con.SendDatagramAsync(context.Raw).ConfigureAwait(false);
		}

		public Task CloseConnectionAsync(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
		{
			lock (_connections.SyncRoot)
			{
				var key = Tuple.Create(localEndPoint, remoteEndPoint);
				var con = _connections[key];
				_connections[key] = null;
				con?.Dispose();			}

			return Task.CompletedTask;
		}
	}
}
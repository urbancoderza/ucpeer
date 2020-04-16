using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace UCPeer.UnitTests.InMemory
{
	internal sealed class DummyNetworkInterface : INetwork
	{
		private static readonly Random _rand = new Random();

		private readonly List<PipelineContext> _outDatas = new List<PipelineContext>();
		private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);
		private PipelineContext _lastContext;

		public DummyNetworkInterface()
		{
		}

		public PipelineContext GenerateRandomInData()
		{
			Thread.Sleep(2000);

			var data = new byte[_rand.Next(0, 1024 * 1024 * 1024)];
			_rand.NextBytes(data);

			_lastContext = new PipelineContext
			{
				Destination = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 123),
				Source = new IPEndPoint(IPAddress.Parse("192.168.168.45"), 321),
				Raw = data
			};

			_resetEvent.Set();

			return _lastContext;
		}

		public IEnumerable<PipelineContext> OutDatas
		{
			get
			{
				return _outDatas.AsReadOnly();
			}
		}

		public async Task<PipelineContext> ReceiveAsync(CancellationToken cancelToken)
		{
			do
			{
				while (!_resetEvent.WaitOne(500))
				{
					if (cancelToken.IsCancellationRequested)
						return await Task.FromResult(null as PipelineContext);
				}

				return await Task.FromResult(_lastContext);
			}
			while (true);
		}

		public async Task SendAsync(PipelineContext context)
		{
			_outDatas.Add(context);
			await Task.CompletedTask;
		}

		public bool Running => true;
	}
}
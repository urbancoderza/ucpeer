using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UCPeer
{
	public sealed partial class Node
	{
		private readonly INetwork _network;
		private readonly IEnumerable<MiddlewareWrapper> _middlewaresOut;
		private readonly IEnumerable<MiddlewareWrapper> _middlewaresIn;
		private readonly Func<PipelineContext, Task> _outDelegateAsync;
		private readonly Func<PipelineContext, Task> _inDelegateAsync;
		private readonly Task _receiveWorker;
		private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
		private readonly Func<PipelineContext, Task> _endpointAction;

		internal Node(INetwork network, Func<PipelineContext, Task> endpointAction, IEnumerable<MiddlewareWrapper> middlewaresIn, IEnumerable<MiddlewareWrapper> middlewaresOut)
		{
			_network = network;
			_endpointAction = endpointAction;
			_middlewaresIn = middlewaresIn;
			_middlewaresOut = middlewaresOut;

			_inDelegateAsync = GetDelegateChainIn(_middlewaresIn.GetEnumerator());
			_outDelegateAsync = GetDelegateChainOut(_middlewaresOut.GetEnumerator());

			_receiveWorker = new Task(t => Receive(_cancelTokenSource.Token), _cancelTokenSource.Token, TaskCreationOptions.LongRunning);
			_receiveWorker.Start();
		}

		private void Receive(CancellationToken token)
		{
			try
			{
				while (_disposed == 0 && !token.IsCancellationRequested)
				{
					while (_network.Running)
					{
						var context = _network.ReceiveAsync(token).Result;
						context.NetworkInterface = _network;
						token.ThrowIfCancellationRequested();

						if (context != null)
							HandleNewDataReceived(context);
					}
				}
			}
			catch (TaskCanceledException)
			{ }
			catch (OperationCanceledException)
			{ }
		}

		private void HandleNewDataReceived(PipelineContext context)
		{
			var task = new Task(async () =>
			{
				await _inDelegateAsync(context).ConfigureAwait(false);
				await (_endpointAction?.Invoke(context)).ConfigureAwait(false);
			});

			task.Start();
		}

		private Func<PipelineContext, Task> GetDelegateChainOut(IEnumerator<MiddlewareWrapper> wrapperEnum)
		{
			if (!wrapperEnum.MoveNext())
				return null;

			var currentWrapper = wrapperEnum.Current;
			var currentMiddleware = (IMiddleware)currentWrapper.GetInstance();

			async Task func(PipelineContext c) => await currentMiddleware.OutDataAsync(c, GetDelegateChainOut(wrapperEnum)).ConfigureAwait(false);
			return func;
		}

		private Func<PipelineContext, Task> GetDelegateChainIn(IEnumerator<MiddlewareWrapper> wrapperEnum)
		{
			if (!wrapperEnum.MoveNext())
				return null;

			var currentWrapper = wrapperEnum.Current;
			var currentMiddleware = (IMiddleware)currentWrapper.GetInstance();

			async Task func(PipelineContext c) => await currentMiddleware.InDataAsync(c, GetDelegateChainIn(wrapperEnum)).ConfigureAwait(false);
			return func;
		}

		public async Task SendAsync(object state)
		{
			var context = new PipelineContext
			{
				State = state,
				ReceivedTime = DateTime.Now
			};
			await _outDelegateAsync(context).ConfigureAwait(false);
			await _network.SendAsync(context).ConfigureAwait(false);
		}

		public async Task SendAsync(PipelineContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			context.ReceivedTime = DateTime.Now;

			await _outDelegateAsync(context).ConfigureAwait(false);
			await _network.SendAsync(context).ConfigureAwait(false);
		}
	}
}

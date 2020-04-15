using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UCTask.Worker;

namespace UCPeer.UnitTests.InMemory
{
	internal sealed class InMemoryNode : WorkerBase, Node<InMemoryNodeContract>
	{
		private readonly List<object> _middlewares;

		public InMemoryNode(List<object> middlewares)
		{
			_middlewares = middlewares;
		}

		public async Task SendAsync(InMemoryNodeContract model)
		{
			IMiddleware<InMemoryNodeContract> lm;

			var last = _middlewares.Last();
			if (last is Type type)
				lm = (IMiddleware<InMemoryNodeContract>)Activator.CreateInstance(type);
			else
				lm = (IMiddleware<InMemoryNodeContract>)last;

			await lm.OutDataAsync(model);
		}

		public void StartNode()
		{
			Start();
		}

		public void StopNode()
		{
			Stop();
		}

		protected override void Cycle(CancellationToken cancellationToken)
		{
			
		}
	}
}
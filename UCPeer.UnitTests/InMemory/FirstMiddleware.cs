using System;
using System.Net;
using System.Threading.Tasks;

namespace UCPeer.UnitTests.InMemory
{
	internal sealed class FirstMiddleware : IMiddleware
	{
		public Task InDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			throw new NotImplementedException();
		}

		public async Task OutDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			if (!(context.State is InMemoryNodeContract state))
			{
				state = new InMemoryNodeContract();
				context.State = state;
			}
			state.Id = (state.Id ?? string.Empty) + nameof(FirstMiddleware);
			context.Destination = new IPEndPoint(IPAddress.Parse("2.2.2.2"), 2);
			context.Source = new IPEndPoint(IPAddress.Parse("3.3.3.3"), 3);

			if (nextAction != null)
				await nextAction(context);
		}
	}
}

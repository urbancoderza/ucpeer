using System;
using System.Net;
using System.Threading.Tasks;

namespace UCPeer.UnitTests.InMemory
{
	internal sealed class SimpleDummyMiddleware : IMiddleware
	{
		public static string Tag;

		public async Task InDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			if (nextAction != null)
				await nextAction(context);
		}

		public async Task OutDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			var contract = context.State as InMemoryNodeContract;
			contract.Id = (contract.Id ?? string.Empty) + nameof(SimpleDummyMiddleware);
			context.Destination = new IPEndPoint(IPAddress.Parse("100.100.100.100"), 9999);
			context.Source = new IPEndPoint(IPAddress.Parse("99.99.99.99"), 8888);

			if (nextAction != null)
				await nextAction(context);
		}
	}
}

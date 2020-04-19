using System;
using System.Threading.Tasks;

namespace UCPeer.UnitTests.InMemory
{
	internal sealed class OptionsBuilderMiddleware : IMiddleware
	{
		private readonly MwOptions _options;

		public OptionsBuilderMiddleware(MwOptions options)
		{
			_options = options;
		}

		public async Task InDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			if (nextAction != null)
				await nextAction(context);
		}

		public async Task OutDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			if (!(context.State is InMemoryNodeContract state))
			{
				state = new InMemoryNodeContract();
				context.State = state;
			}
			state.Id = (state.Id ?? string.Empty) + _options.SomeId;			

			if (nextAction != null)
				await nextAction(context);
		}
	}
}

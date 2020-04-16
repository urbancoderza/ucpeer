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
			state.Id = (state.Id ?? string.Empty) + _options.SomeId;			

			if (nextAction != null)
				await nextAction(context);
		}
	}
}

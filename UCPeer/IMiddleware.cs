using System;
using System.Threading.Tasks;

namespace UCPeer
{
	public interface IMiddleware
	{
		Task OutDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction);
		Task InDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction);
	}
}

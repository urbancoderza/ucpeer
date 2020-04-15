using System.Threading.Tasks;

namespace UCPeer
{
	public interface IMiddleware<TNodeContract> where TNodeContract : new()
	{
		Task OutDataAsync(TNodeContract model, IMiddleware<TNodeContract> nextMiddleware);
		Task InDataAsync(TNodeContract model, IMiddleware<TNodeContract> nextMiddleware);
	}
}

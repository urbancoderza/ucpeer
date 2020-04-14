namespace UCPeer
{
	public interface IMiddleware<TNodeContract> where TNodeContract : new()
	{
		void OutData(TNodeContract model);
		void InData(TNodeContract model);
	}
}

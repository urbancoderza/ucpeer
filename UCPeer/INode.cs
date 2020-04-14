namespace UCPeer
{
	public interface INode<TNodeContract> where TNodeContract : new()
	{
		void StartNode();
		void StopNode();
	}
}

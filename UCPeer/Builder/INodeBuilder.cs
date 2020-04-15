namespace UCPeer.Builder
{
 	public interface INodeBuilder<TNodeContract> where TNodeContract : new()
	{
		INodeBuilder<TNodeContract> Use<TMiddleware>() where TMiddleware : IMiddleware<TNodeContract>, new();
		//INodeBuilder<TNodeContract> Use<TMiddleware>(TMiddleware singleton) where TMiddleware : IMiddleware<TNodeContract>;
		INode<TNodeContract> Build();
	}
}

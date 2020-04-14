namespace UCPeer.Builder
{
	public class NodeBuilder : INodeBuilder<NodeContract>
	{
		public INode<NodeContract> Build()
		{
			throw new System.NotImplementedException();
		}

		public INodeBuilder<NodeContract> Use<TMiddleware>() where TMiddleware : IMiddleware<NodeContract>, new()
		{
			throw new System.NotImplementedException();
		}

		public INodeBuilder<NodeContract> Use<TMiddleware>(TMiddleware singleton) where TMiddleware : IMiddleware<NodeContract>
		{
			throw new System.NotImplementedException();
		}
	}
}

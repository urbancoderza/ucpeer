using System.Collections.Generic;
using UCPeer.Builder;

namespace UCPeer.UnitTests.InMemory
{
	internal sealed class InMemoryNodeBuilder : INodeBuilder<InMemoryNodeContract>
	{
		private readonly List<object> _middlewares = new List<object>();

		public INode<InMemoryNodeContract> Build()
		{
			return new InMemoryNode(_middlewares);
		}

		public INodeBuilder<InMemoryNodeContract> Use<TMiddleware>() where TMiddleware : IMiddleware<InMemoryNodeContract>, new()
		{
			_middlewares.Add(typeof(TMiddleware));
			return this;
		}

		//public INodeBuilder<InMemoryNodeContract> Use<TMiddleware>(TMiddleware singleton) where TMiddleware : IMiddleware<InMemoryNodeContract>
		//{
		//	_middlewares.Add(singleton);
		//	return this;
		//}
	}
}

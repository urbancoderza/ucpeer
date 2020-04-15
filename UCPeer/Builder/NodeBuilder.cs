using System;
using System.Collections.Generic;

namespace UCPeer.Builder
{
	public class NodeBuilder<TNodeContract>
		where TNodeContract : class, new()
	{
		private readonly List<MiddlewareWrapper> _middlewares = new List<MiddlewareWrapper>();

		public Node<TNodeContract> Build()
		{
			throw new System.NotImplementedException();
		}

		public NodeBuilder<TNodeContract> Use(Type middlewareType, MiddlewareScope scope, object options)
		{
			var wrapper = new MiddlewareWrapper(middlewareType, typeof(TNodeContract), scope, options);
			_middlewares.Add(wrapper);
			return this;
		}

		public NodeBuilder<TNodeContract> Use(Type middlewareType, MiddlewareScope scope, Type optionsType, Delegate optionsBuilder)
		{
			var wrapper = new MiddlewareWrapper(middlewareType, typeof(TNodeContract),  scope, optionsType, optionsBuilder);
			_middlewares.Add(wrapper);
			return this;
		}

		public NodeBuilder<TNodeContract> Use<TMiddleware>(TMiddleware singleton)
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			var wrapper = new MiddlewareWrapper(typeof(TMiddleware), typeof(TNodeContract), singleton);
			_middlewares.Add(wrapper);
			return this;
		}
	}
}

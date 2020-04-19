using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UCPeer.Builder
{
	public class NodeBuilder
	{
		private readonly List<MiddlewareWrapper> _middlewares = new List<MiddlewareWrapper>();
		private INetwork _network;
		private Func<PipelineContext, Task> _endpointAction;

		public Node Build()
		{
			if (_network == null)
				throw new NodePipelineException("A network interface must be specified");
			if (_middlewares.Count == 0)
				throw new NodePipelineException("At least one middleware must be specified");

			var reversedMiddlewares = ((IEnumerable<MiddlewareWrapper>)_middlewares).Reverse();
			return new Node(_network, _endpointAction, _middlewares, reversedMiddlewares);
		}

		public NodeBuilder Use(Type middlewareType, MiddlewareScope scope, object options)
		{
			var wrapper = new MiddlewareWrapper(middlewareType, scope, options);
			_middlewares.Add(wrapper);
			return this;
		}

		public NodeBuilder Use(Type middlewareType, MiddlewareScope scope, Type optionsType, Delegate optionsBuilder)
		{
			var wrapper = new MiddlewareWrapper(middlewareType, scope, optionsType, optionsBuilder);
			_middlewares.Add(wrapper);
			return this;
		}

		public NodeBuilder Use<TMiddleware>(TMiddleware singleton)
		{
			var wrapper = new MiddlewareWrapper(typeof(TMiddleware), singleton);
			_middlewares.Add(wrapper);
			return this;
		}

		public NodeBuilder UseNetwork(INetwork network)
		{
			_network = network;
			return this;
		}

		public NodeBuilder UseEndpoint(Func<PipelineContext, Task> endpointAction)
		{
			_endpointAction = endpointAction;
			return this;
		}
	}
}

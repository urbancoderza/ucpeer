using System;

namespace UCPeer.Builder
{
	public static class Extensions
	{
		public static NodeBuilder<TNodeContract> Use<TNodeContract>(this NodeBuilder<TNodeContract> builder, Type middlewareType, MiddlewareScope scope)
			where TNodeContract : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, scope, null);
		}

		public static NodeBuilder<TNodeContract> UseSingleton<TNodeContract>(this NodeBuilder<TNodeContract> builder, Type middlewareType)
			where TNodeContract : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Singleton, null);
		}

		public static NodeBuilder<TNodeContract> UseSingleton<TNodeContract>(this NodeBuilder<TNodeContract> builder, Type middlewareType, object options)
			where TNodeContract : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Singleton, options);
		}

		public static NodeBuilder<TNodeContract> UseTransient<TNodeContract>(this NodeBuilder<TNodeContract> builder, Type middlewareType)
			where TNodeContract : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Transient, null);
		}

		public static NodeBuilder<TNodeContract> UseTransient<TNodeContract>(this NodeBuilder<TNodeContract> builder, Type middlewareType, object options)
			where TNodeContract : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Transient, options);
		}

		public static NodeBuilder<TNodeContract> Use<TNodeContract, TOptions>(this NodeBuilder<TNodeContract> builder, Type middlewareType, MiddlewareScope scope, Action<TOptions> optionsBuilder)
			where TNodeContract : class, new()
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, scope, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder<TNodeContract> UseSingleton<TNodeContract, TOptions>(this NodeBuilder<TNodeContract> builder, Type middlewareType, Action<TOptions> optionsBuilder)
			where TNodeContract : class, new()
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Singleton, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder<TNodeContract> UseTransient<TNodeContract, TOptions>(this NodeBuilder<TNodeContract> builder, Type middlewareType, Action<TOptions> optionsBuilder)
			where TNodeContract : class, new()
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Transient, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder<TNodeContract> Use<TNodeContract, TMiddleware>(this NodeBuilder<TNodeContract> builder, MiddlewareScope scope)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), scope, null);
		}

		public static NodeBuilder<TNodeContract> Use<TNodeContract, TMiddleware>(this NodeBuilder<TNodeContract> builder, MiddlewareScope scope, object options)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), scope, options);
		}

		public static NodeBuilder<TNodeContract> UseSingleton<TNodeContract, TMiddleware>(this NodeBuilder<TNodeContract> builder)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Singleton, null);
		}

		public static NodeBuilder<TNodeContract> UseSingleton<TNodeContract, TMiddleware>(this NodeBuilder<TNodeContract> builder, object options)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Singleton, options);
		}

		public static NodeBuilder<TNodeContract> UseTransient<TNodeContract, TMiddleware>(this NodeBuilder<TNodeContract> builder)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Transient, null);
		}

		public static NodeBuilder<TNodeContract> UseTransient<TNodeContract, TMiddleware>(this NodeBuilder<TNodeContract> builder, object options)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Transient, options);
		}

		public static NodeBuilder<TNodeContract> Use<TNodeContract, TMiddleware, TOptions>(this NodeBuilder<TNodeContract> builder, MiddlewareScope scope, Action<TOptions> optionsBuilder)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), scope, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder<TNodeContract> UseSingleton<TNodeContract, TMiddleware, TOptions>(this NodeBuilder<TNodeContract> builder, Action<TOptions> optionsBuilder)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Singleton, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder<TNodeContract> UseTransient<TNodeContract, TMiddleware, TOptions>(this NodeBuilder<TNodeContract> builder, Action<TOptions> optionsBuilder)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Transient, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder<TNodeContract> Use<TNodeContract, TMiddleware>(this NodeBuilder<TNodeContract> builder, TMiddleware singleton)
			where TNodeContract : class, new()
			where TMiddleware : class, IMiddleware<TNodeContract>, new()
		{
			if (builder == null)
				return null;

			return builder.Use(singleton);
		}
	}
}

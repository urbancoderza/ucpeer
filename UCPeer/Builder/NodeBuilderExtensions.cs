using System;

namespace UCPeer.Builder
{
	public static class NodeBuilderExtensions
	{
		public static NodeBuilder Use(this NodeBuilder builder, Type middlewareType, MiddlewareScope scope)
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, scope, null);
		}

		public static NodeBuilder UseSingleton(this NodeBuilder builder, Type middlewareType)
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Singleton, null);
		}

		public static NodeBuilder UseSingleton(this NodeBuilder builder, Type middlewareType, object options)
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Singleton, options);
		}

		public static NodeBuilder UseTransient(this NodeBuilder builder, Type middlewareType)
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Transient, null);
		}

		public static NodeBuilder UseTransient(this NodeBuilder builder, Type middlewareType, object options)
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Transient, options);
		}

		public static NodeBuilder Use<TOptions>(this NodeBuilder builder, Type middlewareType, MiddlewareScope scope, Action<TOptions> optionsBuilder)
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, scope, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder UseSingleton<TOptions>(this NodeBuilder builder, Type middlewareType, Action<TOptions> optionsBuilder)
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Singleton, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder UseTransient<TOptions>(this NodeBuilder builder, Type middlewareType, Action<TOptions> optionsBuilder)
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(middlewareType, MiddlewareScope.Transient, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder Use<TMiddleware>(this NodeBuilder builder, MiddlewareScope scope)
			where TMiddleware : class, IMiddleware, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), scope, null);
		}

		public static NodeBuilder Use<TMiddleware>(this NodeBuilder builder, MiddlewareScope scope, object options)
			where TMiddleware : class, IMiddleware
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), scope, options);
		}

		public static NodeBuilder UseSingleton<TMiddleware>(this NodeBuilder builder)
			where TMiddleware : class, IMiddleware, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Singleton, null);
		}

		public static NodeBuilder UseSingleton<TMiddleware>(this NodeBuilder builder, object options)
			where TMiddleware : class, IMiddleware
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Singleton, options);
		}

		public static NodeBuilder UseTransient<TMiddleware>(this NodeBuilder builder)
			where TMiddleware : class, IMiddleware, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Transient, null);
		}

		public static NodeBuilder UseTransient<TMiddleware>(this NodeBuilder builder, object options)
			where TMiddleware : class, IMiddleware
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Transient, options);
		}

		public static NodeBuilder Use<TMiddleware, TOptions>(this NodeBuilder builder, MiddlewareScope scope, Action<TOptions> optionsBuilder)
			where TMiddleware : class, IMiddleware
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), scope, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder UseSingleton<TMiddleware, TOptions>(this NodeBuilder builder, Action<TOptions> optionsBuilder)
			where TMiddleware : class, IMiddleware
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Singleton, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder UseTransient<TMiddleware, TOptions>(this NodeBuilder builder, Action<TOptions> optionsBuilder)
			where TMiddleware : class, IMiddleware
			where TOptions : class, new()
		{
			if (builder == null)
				return null;

			return builder.Use(typeof(TMiddleware), MiddlewareScope.Transient, typeof(TOptions), optionsBuilder);
		}

		public static NodeBuilder Use<TMiddleware>(this NodeBuilder builder, TMiddleware singleton)
			where TMiddleware : class, IMiddleware, new()
		{
			if (builder == null)
				return null;

			return builder.Use(singleton);
		}
	}
}

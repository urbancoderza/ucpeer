using System;
using UCPeer.Extensions;

namespace UCPeer
{
	internal sealed class MiddlewareWrapper
	{
		private object _singleton;
		private readonly object _singletonLock = new object();
		private readonly Type _middlewareType;
		private readonly Type _optionsType;
		private readonly Delegate _optionsBuilder;
		private readonly MiddlewareScope _scope;
		private readonly object _options;
		private readonly bool _hasOptions;

		public MiddlewareWrapper(Type middlewareType, MiddlewareScope scope, Type optionsType = null, Delegate optionsBuilder = null)
		{
			if (middlewareType == null)
				throw new ArgumentNullException(nameof(middlewareType));
			if ((optionsType == null && optionsBuilder != null) ||
				(optionsType != null && optionsBuilder == null))
				throw new ArgumentException($"If either '{nameof(optionsType)}' or '{nameof(optionsBuilder)}' is specified (e.g. not null), then the other one must also be specified.");

			ValidateTypes(middlewareType, out _hasOptions, optionsType, optionsBuilder);

			_middlewareType = middlewareType;
			_optionsType = optionsType;
			_optionsBuilder = optionsBuilder;
			_scope = scope;
		}

		public MiddlewareWrapper(Type middlewareType, MiddlewareScope scope, object options = null)
		{
			if (middlewareType == null)
				throw new ArgumentNullException(nameof(middlewareType));
			
			ValidateTypes(middlewareType, out _hasOptions, options?.GetType(), null, options);

			_middlewareType = middlewareType;
			_optionsType = options?.GetType();
			_options = options;
			_scope = scope;
		}

		public MiddlewareWrapper(Type middlewareType, object singleton)
		{
			if (middlewareType == null)
				throw new ArgumentNullException(nameof(middlewareType));

			ValidateTypes(middlewareType, out _hasOptions, null, null, null);

			_middlewareType = middlewareType;
			_scope = MiddlewareScope.Singleton;

			_singleton = singleton;
		}

		private static void ValidateTypes(Type middlewareType, out bool hasOptions, Type optionsType = null, Delegate optionsBuilder = null, object options = null)
		{
			var interfaceMiddlewareType = typeof(IMiddleware);

			var errorMsg = $"In order for an object of type '{middlewareType.Name}' to be used as a middleware, it must:{Environment.NewLine}";
			var typeError = false;

			if (!middlewareType.IsClass)
			{
				typeError = true;
				errorMsg += "\tbe a reference type (class)";
			}

			if (!interfaceMiddlewareType.IsAssignableFrom(middlewareType))
			{
				typeError = true;
				errorMsg += $"\timplement '{interfaceMiddlewareType}'";
			}

			if ((optionsType != null && optionsBuilder != null) || options != null)
			{
				hasOptions = true;
				if (!middlewareType.HasConstructor(optionsType))
				{
					typeError = true;
					errorMsg += $"\thave a constructor accepting exactly one argument of type '{optionsType}'";
				}
			}
			else
			{
				hasOptions = false;
				if (!middlewareType.HasConstructor())
				{
					typeError = true;
					errorMsg += "\thave a constructor accepting no arguments";
				}
			}

			if (typeError)
				throw new ArgumentException(errorMsg, nameof(middlewareType));

			if (hasOptions)
			{
				errorMsg = $"In order for an object of type '{optionsType.Name}' to be used as a middleware options, it must:{Environment.NewLine}";
				typeError = false;

				if (options == null)
				{
					if (!optionsType.IsClass)
					{
						typeError = true;
						errorMsg += "\tbe a reference type (class)";
					}

					if (!optionsType.HasConstructor())
					{
						typeError = true;
						errorMsg += "\thave a constructor accepting no arguments";
					}

					if (typeError)
						throw new ArgumentException(errorMsg, nameof(optionsType));
				}
			}
		}

		public object GetInstance()
		{
			object toReturn = null;

			switch (_scope)
			{
				case MiddlewareScope.Singleton:
					toReturn = GetSingletonInstance();
					break;

				case MiddlewareScope.Transient:
					toReturn = CreateInstance();
					break;
			}

			return toReturn;
		}

		private object CreateInstance()
		{
			if (_hasOptions)
			{
				if (_options != null)
					return Activator.CreateInstance(_middlewareType, _options);
				else
				{
					var options = Activator.CreateInstance(_optionsType);
					_optionsBuilder.DynamicInvoke(options);
					return Activator.CreateInstance(_middlewareType, options);
				}
			}
			else
			{
				return Activator.CreateInstance(_middlewareType);
			}
		}

		private object GetSingletonInstance()
		{
			if (_singleton == null)
			{
				lock (_singletonLock)
				{
					if (_singleton == null)
						_singleton = CreateInstance();
				}
			}

			return _singleton;
		}
	}
}

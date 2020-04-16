using System;

namespace UCPeer.Extensions
{
	internal static class TypeExtensions
	{
		//public static bool IsAssignableTo(this Type self, string baseTypeName, params Type[] genericArgs)
		//{
		//	if (self == null)
		//		return false;
		//	if (string.IsNullOrWhiteSpace(baseTypeName))
		//		return false;

		//	Type baseType;
		//	if (genericArgs?.Length == 0)
		//	{
		//		baseType = Type.GetType(baseTypeName);
		//		return baseType.IsAssignableFrom(self);
		//	}

		//	baseTypeName += "`" + genericArgs.Length + "[";
		//	foreach (var t in genericArgs)
		//		baseTypeName += t.FullName + ",";
		//	baseTypeName = baseTypeName[0..^1];
		//	baseTypeName += "]";

		//	baseType = Type.GetType(baseTypeName);
		//	if (baseType == null)
		//		return false;

		//	return baseType.IsAssignableFrom(self);
		//}

		//public static bool IsAssignableToMiddleware(this Type self, out Type middlewareInterfaceType)
		//{
		//	middlewareInterfaceType = null;

		//	if (self == null)
		//		return false;

		//	var baseTypeName = typeof(IMiddleware).FullName;
		//	baseTypeName += "[[" + genericArg.AssemblyQualifiedName + "]], ";
		//	baseTypeName += Assembly.GetExecutingAssembly().FullName;

		//	middlewareInterfaceType = Type.GetType(baseTypeName, false, true);
			
		//	if (middlewareInterfaceType == null)
		//		return false;

		//	return middlewareInterfaceType.IsAssignableFrom(self);
		//}

		public static bool HasConstructor(this Type self, params Type[] args)
		{
			if (self == null)
				return false;

			return self.GetConstructor(args) != null;
		}
	}
}

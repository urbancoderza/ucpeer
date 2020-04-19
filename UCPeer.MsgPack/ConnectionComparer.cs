using System;
using System.Collections.Generic;
using System.Net;

namespace UCPeer.MsgPack
{
	internal sealed class ConnectionComparer : IEqualityComparer<Tuple<IPEndPoint, IPEndPoint>>
	{
		public bool Equals(Tuple<IPEndPoint, IPEndPoint> x, Tuple<IPEndPoint, IPEndPoint> y)
		{
			return x == y ||
				(
					x.Item1.Equals(y.Item1) &&
					x.Item2.Equals(y.Item2)
				);
		}

		public int GetHashCode(Tuple<IPEndPoint, IPEndPoint> obj)
		{
			return obj.Item1.GetHashCode() ^ obj.Item2.GetHashCode();
		}
	}
}

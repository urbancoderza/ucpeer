using System.Net;
using UCPeer.Builder;

namespace UCPeer.MsgPack.Builder
{
	public static class NodeBuilderExtensions
	{
		public static NodeBuilder UseMsgPack(this NodeBuilder builder, IPEndPoint conRequestLocalEndPoint = null)
		{
			if (builder == null)
				return null;

			return builder.UseNetwork(new MsgPackNetwork(conRequestLocalEndPoint));
		}
	}
}

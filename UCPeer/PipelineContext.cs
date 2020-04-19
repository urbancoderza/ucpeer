using System;
using System.Net;

namespace UCPeer
{
	public sealed class PipelineContext
	{
		public IPEndPoint Source { get; set; }
		public IPEndPoint Destination { get; set; }
		public byte[] Raw { get; set; }
		public DateTime? ReceivedTime { get; set; }
		public object State { get; set; }
		public INetwork NetworkInterface { get; internal set; }
	}
}
using System;
using System.Runtime.Serialization;

namespace UCPeer
{
	public class NodePipelineException : Exception
	{
		public NodePipelineException(string message) : base(message)
		{
		}

		public NodePipelineException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public NodePipelineException()
		{
		}

		public NodePipelineException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}

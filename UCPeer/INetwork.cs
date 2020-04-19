using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace UCPeer
{
	public interface INetwork
	{
		Task<PipelineContext> ReceiveAsync(CancellationToken cancelToken);
		Task SendAsync(PipelineContext context);
		bool Running { get; }
		Task CloseConnectionAsync(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint);
	}
}

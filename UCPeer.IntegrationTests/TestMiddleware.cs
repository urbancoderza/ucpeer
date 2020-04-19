using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace UCPeer.IntegrationTests
{
	internal sealed class TestMiddleware : IMiddleware
	{
		public async Task InDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			if (nextAction != null)
				await nextAction(context);
		}

		public async Task OutDataAsync(PipelineContext context, Func<PipelineContext, Task> nextAction)
		{
			var addressList = Dns.GetHostEntry("localhost").AddressList;
			var localhost = addressList.First(p => p.AddressFamily == AddressFamily.InterNetwork);

			context.Source = new IPEndPoint(localhost, 7500);
			context.Raw = System.Text.Encoding.UTF8.GetBytes("Hello Mr. Andre");

			if (nextAction != null)
				await nextAction(context);
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UCPeer.Builder;
using UCPeer.MsgPack.Builder;

namespace UCPeer.IntegrationTests
{
	[TestClass]
	public class AllTests
	{
		private Node _node1;
		private Node _node2;
		private IPAddress _localhost;
		private readonly AutoResetEvent _event1 = new AutoResetEvent(false);
		private readonly AutoResetEvent _event2 = new AutoResetEvent(false);

		[TestInitialize]
		public void Setup()
		{
			var addressList = Dns.GetHostEntry("localhost").AddressList;
			_localhost = addressList.First(p => p.AddressFamily == AddressFamily.InterNetwork);

			var ipLocalEndPoint = new IPEndPoint(_localhost, 7000);

			var builder = new NodeBuilder();
			builder
				.UseMsgPack(ipLocalEndPoint)
				.Use<TestMiddleware>(MiddlewareScope.Transient)
				.UseEndpoint(Endpoint1);

			_node1 = builder.Build();

			ipLocalEndPoint = new IPEndPoint(_localhost, 8000);

			builder = new NodeBuilder();
			builder
				.UseMsgPack(ipLocalEndPoint)
				.Use<TestMiddleware>(MiddlewareScope.Transient)
				.UseEndpoint(Endpoint2);

			_node2 = builder.Build();
		}

		private Task Endpoint1(PipelineContext context)
		{
			_event2.Set();

			return Task.CompletedTask;
		}

		private Task Endpoint2(PipelineContext context)
		{
			Assert.IsNotNull(context);
			Assert.IsNotNull(context.Raw);
			Assert.AreEqual(context.Raw.Length, 15);

			var rawStr = System.Text.Encoding.UTF8.GetString(context.Raw);
			Assert.AreEqual(rawStr, "Hello Mr. Andre");

			_event1.Set();

			return Task.CompletedTask;
		}

		[TestCleanup]
		public void Cleanup()
		{
			_node2.Dispose();
			_node1.Dispose();
		}

		[TestMethod]
		public void AllTest()
		{
			_ = _node1.SendAsync(new PipelineContext
			{
				Destination = new IPEndPoint(_localhost, 8000)
			});

			_event1.WaitOne();
		}
	}
}

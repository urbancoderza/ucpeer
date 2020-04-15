using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;
using UCPeer.Builder;
using UCPeer.UnitTests.InMemory;

namespace UCPeer.UnitTests
{
	[TestClass]
	public class CustomNodeBuilderTests
	{
		private static readonly Random _rand = new Random();

		[TestInitialize]
		public void Setup()
		{			
		}

		[TestCleanup]
		public void Cleanup()
		{
		}

		[TestMethod]
		public void EmptyPipeline()
		{
			var builder = new NodeBuilder<InMemoryNodeContract>();
			var node = builder.Build();

			node.StartNode();
			node.StopNode();
		}

		[TestMethod]
		public void DummyNetworkInterfaceOnly()
		{
			var networkMiddleware = new DummyNetworkInterfaceMiddleware();
			var builder = new NodeBuilder<InMemoryNodeContract>();
			builder.Use(networkMiddleware);
			var node = builder.Build();

			node.StartNode();

			var model = new InMemoryNodeContract(MethodBase.GetCurrentMethod().Name + "\t" + DateTime.Now.ToString("HH:mm:ss.fff"), new byte[10]);
			_rand.NextBytes(model.RawData);

			node.SendAsync(model).Wait();
			Assert.AreEqual(networkMiddleware.OutDatas.Count(), 1);

			node.StopNode();
		}
	}
}

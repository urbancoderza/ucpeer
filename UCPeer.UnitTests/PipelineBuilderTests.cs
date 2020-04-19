using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UCPeer.Builder;
using UCPeer.UnitTests.InMemory;

namespace UCPeer.UnitTests
{
	[TestClass]
	public class PipelineBuilderTests
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
			var builder = new NodeBuilder();
			_ = builder.UseNetwork(new DummyNetworkInterface())
				.Use(new SimpleDummyMiddleware())
				.Build();
		}

		[TestMethod]
		public void DummyNetworkInterfaceOnlyOut()
		{
			var network = new DummyNetworkInterface();
			var builder = new NodeBuilder();
			builder.UseNetwork(network);
			builder.UseTransient<SimpleDummyMiddleware>();
			var node = builder.Build();

			var model = new InMemoryNodeContract(MethodBase.GetCurrentMethod().Name, new byte[10]);
			_rand.NextBytes(model.RawData);

			node.SendAsync(new InMemoryNodeContract()).Wait();
			Assert.AreEqual(network.OutDatas.Count(), 1);

			var outData = network.OutDatas.First();
			Assert.AreEqual(outData.Destination.Port, 9999);
			Assert.AreEqual(outData.Source.Port, 8888);
			var state = outData.State as InMemoryNodeContract;
			Assert.IsNotNull(state);
		}

		[TestMethod]
		public void MiddlewareOrderOut()
		{
			var network = new DummyNetworkInterface();
			var builder = new NodeBuilder();
			builder.UseNetwork(network);
			SimpleDummyMiddleware.Tag = MethodBase.GetCurrentMethod().Name;
			builder.UseTransient<SimpleDummyMiddleware>();
			builder.UseSingleton<FirstMiddleware>();
			var node = builder.Build();

			var context = new PipelineContext();
			node.SendAsync(context).Wait();

			Assert.AreEqual(network.OutDatas.Count(), 1);

			var outData = network.OutDatas.First();
			Assert.AreEqual(outData.Destination.Port, 9999);
			Assert.AreEqual(outData.Source.Port, 8888);

			var state = outData.State as InMemoryNodeContract;
			Assert.IsNotNull(state);
			Assert.IsTrue(state.Id.Contains("FirstMiddleware"));
			Assert.IsTrue(state.Id.Contains("SimpleDummyMiddleware"));
		}

		[TestMethod]
		public void OptionsBuilderOut()
		{
			try
			{
				var network = new DummyNetworkInterface();
				var builder = new NodeBuilder();
				builder.UseNetwork(network);
				builder.UseTransient<SimpleDummyMiddleware>();
				builder.UseTransient<OptionsBuilderMiddleware, MwOptions>(o =>
				{
					o.SomeId = "2nd Middleware";
				});
				builder.UseSingleton<FirstMiddleware>();
				var node = builder.Build();

				var context = new PipelineContext();
				node.SendAsync(context).Wait();

				Assert.AreEqual(network.OutDatas.Count(), 1);

				var outData = network.OutDatas.First();
				Assert.AreEqual(outData.Destination.Port, 9999);
				Assert.AreEqual(outData.Source.Port, 8888);

				var state = outData.State as InMemoryNodeContract;
				Assert.IsNotNull(state);
				Assert.IsTrue(state.Id.Contains("SimpleDummyMiddleware"));
				Assert.IsTrue(state.Id.Contains("2nd Middleware"));
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		[TestMethod]
		public void OptionsBuilderIn()
		{
			PipelineContext receivedContext = null;

			var network = new DummyNetworkInterface();
			var builder = new NodeBuilder();
			builder
				.UseNetwork(network)
				.UseTransient<SimpleDummyMiddleware>()
				.UseTransient<OptionsBuilderMiddleware, MwOptions>(o =>
				{
					o.SomeId = "2nd Middleware";
				})
				.UseSingleton<FirstMiddleware>()
				.UseEndpoint(c =>
				{
					receivedContext = c;
					return Task.CompletedTask;
				});

			var node = builder.Build();

			var context = new PipelineContext();
			node.SendAsync(context).Wait();

			Assert.AreEqual(network.OutDatas.Count(), 1);

			var outData = network.OutDatas.First();
			Assert.AreEqual(outData.Destination.Port, 9999);
			Assert.AreEqual(outData.Source.Port, 8888);

			var state = outData.State as InMemoryNodeContract;
			Assert.IsNotNull(state);
			Assert.IsTrue(state.Id.Contains("SimpleDummyMiddleware"));
			Assert.IsTrue(state.Id.Contains("2nd Middleware"));

			context = network.GenerateRandomInData();
			Thread.Sleep(2000);

			Assert.IsNotNull(receivedContext);
			Assert.IsNotNull(context);
			Assert.AreEqual(receivedContext.Raw.Length, context.Raw.Length);
			Assert.AreEqual(receivedContext.Raw[10], context.Raw[10]);
			Assert.AreEqual(receivedContext.Source, context.Source);

			state = receivedContext.State as InMemoryNodeContract;
			Assert.AreEqual(state.Id, "Lekker");
		}
	}
}

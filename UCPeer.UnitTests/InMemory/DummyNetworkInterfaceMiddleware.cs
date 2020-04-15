using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UCPeer.UnitTests.InMemory
{
	internal sealed class DummyNetworkInterfaceMiddleware : IMiddleware<InMemoryNodeContract>
	{
		private static readonly Random _rand = new Random();

		private readonly IMiddleware<InMemoryNodeContract> _prev;
		private readonly List<InMemoryNodeContract> _outDatas  = new List<InMemoryNodeContract>();

		public DummyNetworkInterfaceMiddleware()
		{			
		}

		public Task InDataAsync(InMemoryNodeContract model, IMiddleware<InMemoryNodeContract> next)
		{
			throw new NotImplementedException();
		}

		public async Task OutDataAsync(InMemoryNodeContract model, IMiddleware<InMemoryNodeContract> next)
		{
			await Task.Run(() => _outDatas.Add(model));
		}

		public byte[] GenerateRandomInData(string id = null)
		{
			var toReturn = new byte[_rand.Next(0, 1024 * 1024 * 1024)];
			_rand.NextBytes(toReturn);

			var model = new InMemoryNodeContract(id, toReturn);
			_prev.InDataAsync(model);

			return toReturn;
		}

		public IEnumerable<InMemoryNodeContract> OutDatas
		{
			get
			{
				return _outDatas.AsReadOnly();
			}
		}
	}
}
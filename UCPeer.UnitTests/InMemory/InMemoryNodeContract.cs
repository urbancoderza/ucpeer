namespace UCPeer.UnitTests.InMemory
{
	internal sealed class InMemoryNodeContract
	{
		public InMemoryNodeContract()
		{
		}

		public InMemoryNodeContract(string id)
		{
			Id = id;
		}

		public InMemoryNodeContract(byte[] data)
		{
			RawData = data;
		}

		public InMemoryNodeContract(string id, byte[] data)
		{
			Id = id;
			RawData = data;
		}

		public string Id { get; set; }
		public byte[] RawData { get; private set; }
	}
}

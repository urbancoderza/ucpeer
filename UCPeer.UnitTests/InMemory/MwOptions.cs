using System;

namespace UCPeer.UnitTests.InMemory
{
	public class MwOptions
	{
		public DateTime CreatedDateTime { get; } = DateTime.Now;

		public string SomeId { get; set; }
	}
}

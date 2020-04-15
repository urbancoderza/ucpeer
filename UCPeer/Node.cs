using System;
using System.Threading.Tasks;

namespace UCPeer
{
	public sealed class Node<TNodeContract>
		where TNodeContract : new()
	{
		internal Node()
		{
			// Intentionally empty.
		}

		public void StartNode()
		{

		}

		public void StopNode()
		{

		}

		public async Task SendAsync(TNodeContract model)
		{
			await Task.FromException<NotImplementedException>(new NotImplementedException()).ConfigureAwait(false);
		}
	}
}

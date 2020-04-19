using System;
using System.Threading;

namespace UCPeer.MsgPack
{
	public sealed partial class MsgPackNetwork : IDisposable
	{
		private volatile int _disposed;

		public void Dispose()
		{
			if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
			{
				_cancelTokenSource.Cancel(false);

				if (_listener != null)
				{
					_listener.Stop();
				}

				_cancelTokenSource.Dispose();

				_connections.Dispose();

				_receiveQueue.Dispose();
			}

			GC.SuppressFinalize(this);
		}

		~MsgPackNetwork()
		{
			Dispose();
		}
	}
}
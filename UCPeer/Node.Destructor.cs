﻿using System;
using System.Threading;

namespace UCPeer
{
	public sealed partial class Node : IDisposable
	{
		private volatile int _disposed;

		public void Dispose()
		{
			if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
			{
				_cancelTokenSource.Cancel(false);

				if (_network is IDisposable dispNet)
					dispNet.Dispose();

				_cancelTokenSource.Dispose();
			}

			GC.SuppressFinalize(this);
		}

		~Node()
		{
			Dispose();
		}
	}
}
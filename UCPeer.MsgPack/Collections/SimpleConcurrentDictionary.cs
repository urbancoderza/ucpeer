using System;
using System.Collections.Generic;

namespace UCPeer.MsgPack.Collections
{
	internal sealed class SimpleConcurrentDictionary<TKey, TValue> : IDisposable
		where TValue : class
	{
		public object SyncRoot { get; } = new object();
		private readonly Dictionary<TKey, TValue> _dic;
		private readonly bool _ownsKeys;
		private readonly bool _ownsValues;

		public SimpleConcurrentDictionary(bool ownsKeys = false, bool ownsValues = false)
		{
			_ownsKeys = ownsKeys;
			_ownsValues = ownsValues;

			_dic = new Dictionary<TKey, TValue>();
		}

		public SimpleConcurrentDictionary(IEqualityComparer<TKey> comparer, bool ownsKeys = false, bool ownsValues = false)
		{
			_ownsKeys = ownsKeys;
			_ownsValues = ownsValues;

			_dic = new Dictionary<TKey, TValue>(comparer);
		}

		public TValue this[TKey key]
		{
			get
			{
				lock (SyncRoot)
				{
					if (key == null)
						return null;

					_dic.TryGetValue(key, out TValue toReturn);
					return toReturn;
				}
			}
			set
			{
				lock (SyncRoot)
				{
					if (key == null)
						return;

					if (_dic.TryGetValue(key, out TValue toReturn))
					{
						if (value == null)
							_dic.Remove(key);
						_dic[key] = value;
					}
					else
					{
						if (value == null)
							return;
						_dic.Add(key, value);
					}
				}
			}
		}

		public void Dispose()
		{
			if (_ownsKeys && typeof(IDisposable).IsAssignableFrom(typeof(TKey)))
				foreach (IDisposable key in _dic.Keys)
					key.Dispose();

			if (_ownsValues && typeof(IDisposable).IsAssignableFrom(typeof(TValue)))
				foreach (IDisposable val in _dic.Values)
					val.Dispose();
		}
	}
}

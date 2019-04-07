using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using NDivert.Filter;

namespace NDivert
{
	public abstract class PacketProcessor
	{
		private readonly object _handleLock = new object();
		private readonly FilterDefinition _filter;
		private readonly short _priority;

		private Thread _thread;
		private Interop.WinDivertHandle _handle;

		private bool _isAlive;

		public PacketProcessor(short priority, FilterDefinition filter)
		{
			_isAlive = true;
			_priority = priority;
			_filter = filter;
		}

		public short Priority
		{
			get { return _priority; }
		}

		protected Interop.WinDivertHandle Handle
		{
			get { return _handle; }
		}

		protected object HandleLock
		{
			get { return _handleLock; }
		}

		public bool IsAlive
		{
			get { return _isAlive; }
		}

		public virtual void Start()
		{
			_isAlive = true;

			_thread = new Thread(ThreadProc);
#if DEBUG
			_thread.Name = "NDivert PacketProcessor thread";
#endif
			_thread.Start();
		}

		private void ThreadProc()
		{
			_handle = null;

			try
			{
				_handle = Library.OpenHandle(_filter, WinDivertLayer.Network, _priority, WinDivertFlag.None);
				_isAlive = !_handle.IsInvalid;
				byte[] packet = new byte[1600];
				WinDivertAddress addr;
				int len;
				while (_isAlive)
				{
					bool ok = false;
					lock (_handleLock)
					{
						ok = _handle.Receive(packet, packet.Length, out addr, out len);
					}
					if (ok)
					{
						ProcessPacket(packet, len, in addr);
					}
				}
			}
			finally
			{
				if (null != _handle)
				{
					_handle.Close();
					_handle = null;
				}

			}
		}

		public virtual void Stop()
		{
			_isAlive = false;
			_handle.CancelIo();
		}

		protected abstract void ProcessPacket(byte[] packet, int packetLength, in WinDivertAddress address);
	}
}

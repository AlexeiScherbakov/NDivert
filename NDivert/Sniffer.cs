using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using NDivert.Filter;
using NDivert.Interop;

namespace NDivert
{
	/// <summary>
	/// Base class for sniffer implementation
	/// </summary>
	public abstract class Sniffer
		: IDisposable
	{
		private readonly FilterDefinition _filter;
		private readonly short _priority;

		private Thread _thread;
		private WinDivertHandle _handle;
		private bool _isActive;


		public Sniffer(short priority, FilterDefinition filter)
		{
			_priority = priority;
			_filter = filter;
		}

		~Sniffer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		public void Start()
		{
			_thread = new Thread(ThreadProc);
			_thread.IsBackground = true;
#if DEBUG
			_thread.Name = "NDivert Sniffer thread";
#endif
			_thread.Start();
		}

		private void ThreadProc()
		{
			_handle = null;
			BeforeStart();
			bool error = false;
			try
			{
				_handle = Library.OpenHandle(_filter, WinDivertLayer.Network, _priority, WinDivertFlag.Sniff);
				_isActive = !_handle.IsInvalid;
				byte[] packet = new byte[1600];
				WinDivertAddress addr;
				int len;
				while (_isActive)
				{
					if (_handle.Receive(packet, packet.Length, out addr, out len))
					{
						// поймали пакет
						OnPacketCaptured(packet, len, in addr);
					}
				}
			}
			catch (Exception e)
			{
				error = true;
			}
			finally
			{
				if (null != _handle)
				{
					_handle.Close();
					_handle = null;
				}
				AfterStop(error);
			}
		}

		protected abstract void BeforeStart();

		protected abstract void AfterStop(bool error);

		protected abstract void OnPacketCaptured(byte[] packet, int packetLength, in WinDivertAddress divertAddress);

		public void Stop()
		{
			_isActive = false;
			if (_handle != null)
			{
				_handle.CancelIo();
			}
		}
	}
}

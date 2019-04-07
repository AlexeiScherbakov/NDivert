using System;
using System.Threading;

namespace NDivert.Interop
{
	/// <summary>
	/// TODO - direct driver comm without WinDivert.dll
	/// </summary>
	internal sealed class WinDivertDriverHandle
		: WinDivertHandle
	{
		public static implicit operator WinDivertDriverHandle(IntPtr handle)
		{
			return new WinDivertDriverHandle(handle);
		}

		public WinDivertDriverHandle(IntPtr handle)
			: base(handle)
		{
			
		}

		protected override bool ReleaseHandle()
		{
			var h = Interlocked.Exchange(ref handle, IntPtr.Zero);
			return NativeMethods.Kernel32.CloseHandle(h);
		}

		public override bool Send(byte[] packet, int packetLength, in WinDivertAddress address, out int bytesSend)
		{
			throw new NotImplementedException();
		}

		public override bool Receive(byte[] packet, int packetLength, out WinDivertAddress address, out int bytesReceived)
		{
			throw new NotImplementedException();
		}
	}

}

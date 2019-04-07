using System;

using Microsoft.Win32.SafeHandles;

namespace NDivert.Interop
{
	/// <summary>
	/// Driver handle
	/// </summary>
	public abstract class WinDivertHandle
		: SafeHandleZeroOrMinusOneIsInvalid
	{
		internal WinDivertHandle(IntPtr handle)
			: base(true)
		{
			SetHandle(handle);
		}

		public bool CancelIo()
		{
			return NativeMethods.Kernel32.CancelIoEx(handle, IntPtr.Zero);
		}

		public abstract bool Send(byte[] packet, int packetLength, in WinDivertAddress address, out int bytesSend);
		public abstract bool Receive(byte[] packet, int packetLength, out WinDivertAddress address, out int bytesReceived);
	}

}

using System;
using System.Threading;

namespace NDivert.Interop
{
	/// <summary>
	/// Driver handle object via WinDivert.dll
	/// </summary>
	internal sealed class WinDivertLibHandle
		: WinDivertHandle
	{
		public WinDivertLibHandle(IntPtr handle)
			: base(handle)
		{
		}

		public static implicit operator WinDivertLibHandle(IntPtr handle)
		{
			return new WinDivertLibHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			var h = Interlocked.Exchange(ref handle, IntPtr.Zero);
			return NativeMethods.WinDivert.WinDivertClose(h);
		}

		public override bool Send(byte[] packet, int packetLength, in WinDivertAddress address, out int bytesSend)
		{
			return NativeMethods.WinDivert.WinDivertSend(handle, packet, packetLength,in address, out bytesSend);
		}

		public override bool Receive(byte[] packet, int packetLength, out WinDivertAddress address, out int bytesReceived)
		{
			return NativeMethods.WinDivert.WinDivertRecv(handle, packet, packetLength, out address, out bytesReceived);
		}
	}

}

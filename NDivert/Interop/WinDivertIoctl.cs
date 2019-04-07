using System.Runtime.InteropServices;

namespace NDivert.Interop
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct WinDivertIoctl
	{
		public ushort Magic;
		public byte Version;
		public byte Arg8;
		public long Arg;
	}
}

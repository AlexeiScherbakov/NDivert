using System.Runtime.InteropServices;

namespace NDivert
{
	[StructLayout(LayoutKind.Sequential)]
	public struct WinDivertDataNetwork
	{
		public uint IfIdx;
		public uint SubIfIdx;
	}
}

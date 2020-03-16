using System.Runtime.InteropServices;

namespace NDivert
{
	[StructLayout(LayoutKind.Sequential)]
	public struct WindivertDataReflect
	{
		public long Timestamp;
		public int ProcessId;
		public WinDivertLayer Layer;
		public long Flags;
		public short Priority;
	}
}

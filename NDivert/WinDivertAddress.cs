using System.Runtime.InteropServices;

namespace NDivert
{
	/// <summary>
	/// Divert Address
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct WinDivertAddress
	{
		[FieldOffset(0)]
		public long Timestamp;
		[FieldOffset(8)]
		public byte Layer;
		[FieldOffset(9)]
		public byte Event;
		[FieldOffset(10)]
		public WinDivertAddressFlag Flags;
		[FieldOffset(11)]
		public byte Reserved;
		[FieldOffset(12)]
		public int Reserved2;

		[FieldOffset(16)]
		public WinDivertDataNetwork Network;
		[FieldOffset(16)]
		public WinDivertDataFlow Flow;
		[FieldOffset(16)]
		public WindivertDataSocket Socket;
		[FieldOffset(16)]
		public WindivertDataReflect Reflect;
		[FieldOffset(16)]
		public fixed byte Reserved3[64];
	}
}

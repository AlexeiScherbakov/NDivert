using System.Runtime.InteropServices;

namespace NDivert
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct WinDivertDataFlow
	{
		public ulong EndpointId;
		public ulong ParentEndpointId;
		public int ProcessId;
		public fixed int LocalAddr[4];
		public fixed int RemoteAddr[4];
		public ushort LocalPort;
		public ushort RemotePort;
		public byte Protocol;
	}
}

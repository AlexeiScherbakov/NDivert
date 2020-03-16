using System.Runtime.InteropServices;

namespace NDivert
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct WindivertDataSocket
	{
		public long EndpointId;
		public long ParentEndpointId;
		public int ProcessId;
		public fixed int LocalAddr[4];
		public fixed int RemoteAddr[4];
		public ushort LocalPort;
		public ushort RemotePort;
		public byte Protocol;
	}
}

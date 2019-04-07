using NDivert.Filter;
using System.Runtime.InteropServices;

namespace NDivert.Interop
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct WinDivertIoctlFilter
	{
		public FilterField Field;
		public FilterOperation Test;
		public ushort Success;
		public ushort Failure;
		public fixed int Arg[4];
	}
}

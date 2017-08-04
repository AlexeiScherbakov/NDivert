using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NDivert
{
	/// <summary>
	/// Divert Address
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WinDivertAddress
	{
		public uint IfIdx;
		public uint SubIfIdx;
		public Direction Direction;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NDivert.Interop
{
	internal static class NativeMethods
	{
#if DEBUG
		[DllImport("WinDivert.dll", EntryPoint = "WinDivertOpen",CallingConvention =CallingConvention.Cdecl,CharSet =CharSet.Ansi)]
		public static extern IntPtr WinDivertOpen([MarshalAs(UnmanagedType.LPStr)] string rule, WinDivertLayer layer, short priority, WinDivertFlag flags);
#endif
		[DllImport("WinDivert.dll", EntryPoint = "WinDivertOpen",CallingConvention =CallingConvention.Cdecl)]
		public static extern IntPtr WinDivertOpen(byte[] rule, WinDivertLayer layer, short priority, WinDivertFlag flags);

		[DllImport("WinDivert.dll", EntryPoint = "WinDivertClose",CallingConvention =CallingConvention.Cdecl)]
		[return:MarshalAs(UnmanagedType.Bool)]
		public static extern bool WinDivertClose(IntPtr handle);
	}
}

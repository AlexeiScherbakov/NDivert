using System;
using System.Runtime.InteropServices;

namespace NDivert.Interop
{
	internal static class NativeMethods
	{
		internal const string Kernel32Dll = "Kernel32.dll";
		internal const string WinDivertDll = "WinDivert.dll";

		public static class Kernel32
		{
			[DllImport(Kernel32Dll, EntryPoint = "CloseHandle")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CloseHandle(IntPtr handle);

			[DllImport(Kernel32Dll, EntryPoint = "CancelIoEx")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

			[DllImport(Kernel32Dll, EntryPoint = "GetLastError")]
			public static extern int GetLastError();

		}

		public static class WinDivert
		{
			[DllImport(WinDivertDll, EntryPoint = "WinDivertOpen", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr WinDivertOpen(byte[] rule, WinDivertLayer layer, short priority, WinDivertFlag flags);

			[DllImport(WinDivertDll, EntryPoint = "WinDivertClose", CallingConvention = CallingConvention.Cdecl)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool WinDivertClose(IntPtr handle);

			[DllImport(WinDivertDll, EntryPoint = "WinDivertRecv", CallingConvention = CallingConvention.Cdecl)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool WinDivertRecv(IntPtr handle, byte[] pPacket, int packetLen, out WinDivertAddress addr, out int readLen);

			[DllImport(WinDivertDll, EntryPoint = "WinDivertRecvEx", CallingConvention = CallingConvention.Cdecl)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool WinDivertRecvEx(IntPtr handle, byte[] pPacket, int packetLen, long flags, out WinDivertAddress addr, out int readLen, ref System.Threading.NativeOverlapped overlapped);

			[DllImport(WinDivertDll, EntryPoint = "WinDivertSend", CallingConvention = CallingConvention.Cdecl)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool WinDivertSend(IntPtr handle, byte[] pPacket, int packetLen, in WinDivertAddress addr, out int writelen);
		}
	}
}

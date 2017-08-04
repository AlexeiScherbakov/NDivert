using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NDivert.Interop
{

	internal sealed class WinDivertHandle
		: SafeHandle
	{
		private static IntPtr InvalidHandle = new IntPtr(-1);


		public static implicit operator WinDivertHandle(IntPtr handle)
		{
			return new WinDivertHandle(handle);
		}

		public WinDivertHandle(IntPtr handle)
			: base(InvalidHandle, true)
		{
			SetHandle(handle);
		}

		public override bool IsInvalid
		{
			get { return handle == InvalidHandle; }
		}

		protected override bool ReleaseHandle()
		{
			var h = Interlocked.Exchange(ref handle, IntPtr.Zero);
			return NativeMethods.WinDivertClose(h);
		}
	}
}

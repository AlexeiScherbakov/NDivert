using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NDivert
{
	[Flags]
	public enum WinDivertFlag
		: long
	{
		None = 0,
		Sniff = 0x0001,
		Drop = 0x0002,
		ReadOnly=0x0004,
		WriteOnly=0x0008,
		NoInstall=0x0010,
		Fragments=0x0020
	}
}

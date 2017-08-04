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
		Sniff = 1,
		Drop = 2,
		NoChecksum = 1024
	}
}

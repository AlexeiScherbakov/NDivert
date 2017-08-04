using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NDivert.Filter
{

	/// <summary>
	/// Base TCP & UDP rule part
	/// </summary>
	public interface ICommonTcpUdp
	{
		int Checksum { get; }
		int PayloadLength { get; }
		int SrcPort { get; }
		int DstPort { get; }
	}
}

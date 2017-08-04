using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NDivert.Filter
{
	/// <summary>
	/// IP filter rule
	/// </summary>
	public interface IIP
	{
		int Checksum { get; }
		int DF { get; }
		IPAddress DstAddr { get; }
		int FragOff { get; }
		int HdrLength { get; }
		int Id { get; }
		int Length { get; }
		int MF { get; }
		int Protocol { get; }
		IPAddress SrcAddr { get; }
		int TOS { get; }
		int TTL { get; }
	}
}

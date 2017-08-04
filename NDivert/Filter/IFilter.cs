using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDivert.Filter
{
	/// <summary>
	/// Filter
	/// </summary>
	public interface IFilter
	{
		bool Outbound { get; }
		bool Inbound { get; }
		IIP Ip { get; }

		bool IsTcp { get; }
		ITcp Tcp { get; }

		bool IsUdp { get; }
		IUdp Udp { get; }

		int IfIdx { get; }
		int SubIfIdx { get; }
	}
}

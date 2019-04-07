using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDivert.Filter
{
	/// <summary>
	/// TCP filter rule
	/// </summary>
	public interface ITcp
		:ICommonTcpUdp
	{
		int SeqNum { get; }
		bool Syn { get; }
		bool Rst { get; }
		bool Ack { get; }
	}
}

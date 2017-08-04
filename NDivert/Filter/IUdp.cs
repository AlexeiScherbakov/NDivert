using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDivert.Filter
{
	/// <summary>
	/// Udp filter rule
	/// </summary>

	public interface IUdp
		: ICommonTcpUdp
	{
		int Length { get; }
	}
}

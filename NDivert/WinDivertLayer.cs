using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NDivert.Filter;

namespace NDivert
{
	public enum WinDivertLayer
	{
		/// <summary>
		/// Network layer
		/// </summary>
		Network = 0,
		/// <summary>
		/// Network layer (forwarded packets)
		/// </summary>
		NetworkForward = 1,
		Flow = 2,
		Socket = 3,
		Reflect = 4
	}
}

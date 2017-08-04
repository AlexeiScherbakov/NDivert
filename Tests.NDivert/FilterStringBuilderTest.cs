using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDivert.Filter;
using NUnit.Framework;

namespace Tests.NDivert
{
	[TestFixture]
    public class FilterStringBuilderTest
    {
		[Test]
		public void SimpleFiltersTest()
		{
			Assert.AreEqual("inbound", DivertFilterStringBuilder.MakeFilter(x => x.Inbound));
			Assert.AreEqual("outbound", DivertFilterStringBuilder.MakeFilter(x => x.Outbound));
			Assert.AreEqual("tcp", DivertFilterStringBuilder.MakeFilter(x => x.IsTcp));
			Assert.AreEqual("udp", DivertFilterStringBuilder.MakeFilter(x => x.IsUdp));
			Assert.AreEqual("tcp.SrcPort == 80", DivertFilterStringBuilder.MakeFilter(x => (x.Tcp.SrcPort == 80)));
			Assert.AreEqual("not tcp", DivertFilterStringBuilder.MakeFilter(x => !x.IsTcp));
		}

		[Test]
		public void ComplexFiltersTests()
		{
			Assert.AreEqual("inbound and ((tcp.SrcPort == 80) or (tcp.SrcPort == 443))", DivertFilterStringBuilder.MakeFilter(x => x.Inbound && ((x.Tcp.SrcPort == 80) || x.Tcp.SrcPort == 443)));
			//maybe optimize ()?
			Assert.AreEqual("inbound and (((tcp.SrcPort == 80) or (tcp.SrcPort == 443)) or (tcp.SrcPort == 81))",
				DivertFilterStringBuilder.MakeFilter(x => x.Inbound && ((x.Tcp.SrcPort == 80) || x.Tcp.SrcPort == 443 || x.Tcp.SrcPort == 81)));
		}
    }
}

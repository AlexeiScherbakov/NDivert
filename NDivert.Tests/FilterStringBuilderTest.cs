using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NDivert.Filter;
using NUnit.Framework;

namespace NDivert.Tests
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


		[Test]
		public void FiltersWithConstantsTest()
		{
			IPAddress ip = IPAddress.Parse("8.8.8.8");

			Assert.AreEqual("(inbound and (ip.SrcAddr == 8.8.8.8)) or (outbound and (ip.DstAddr == 8.8.8.8))", DivertFilterStringBuilder.MakeFilter(x => (x.Inbound && (x.Ip.SrcAddr == IPAddress.Parse("8.8.8.8"))) || (x.Outbound && (x.Ip.DstAddr == IPAddress.Parse("8.8.8.8")))));
			Assert.AreEqual("(inbound and (ip.SrcAddr == 8.8.8.8)) or (outbound and (ip.DstAddr == 8.8.8.8))", DivertFilterStringBuilder.MakeFilter(x => (x.Inbound && (x.Ip.SrcAddr == ip)) || (x.Outbound && (x.Ip.DstAddr == ip))));
		}
	}
}

using NDivert.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDivert.Tests.Utils
{
	public static class FilterUtils
	{
		private static readonly Dictionary<FilterField, string> _filterFields;
		private static readonly Dictionary<FilterOperation, string> _filterOperations;

		static FilterUtils()
		{
			_filterFields = new Dictionary<FilterField, string>();
			AddFilterFieldString(FilterField.Zero, "zero");
			AddFilterFieldString(FilterField.Inbound, "inbound");
			AddFilterFieldString(FilterField.Outbound, "outbound");
			AddFilterFieldString(FilterField.IfIdx, "ifIdx");
			AddFilterFieldString(FilterField.SubIfIdx, "subIfIdx");
			AddFilterFieldString(FilterField.Ip, "ip");
			AddFilterFieldString(FilterField.Ipv6, "ipv6");
			AddFilterFieldString(FilterField.Icmp, "icmp");
			AddFilterFieldString(FilterField.Icmpv6, "icmpv6");
			AddFilterFieldString(FilterField.Tcp, "tcp");
			AddFilterFieldString(FilterField.Udp, "udp");
			AddFilterFieldString(FilterField.IpHeaderLength, "ip.HdrLength");
			AddFilterFieldString(FilterField.IpTos, "ip.TOS");
			AddFilterFieldString(FilterField.IpLength, "ip.Length");
			AddFilterFieldString(FilterField.IpId, "ip.Id");
			AddFilterFieldString(FilterField.IpDf, "ip.DF");
			AddFilterFieldString(FilterField.IpMf, "ip.MF");
			AddFilterFieldString(FilterField.IpFragOff, "ip.FragOff");
			AddFilterFieldString(FilterField.IpTtl, "ip.TTL");
			AddFilterFieldString(FilterField.IpProtocol, "ip.Protocol");
			AddFilterFieldString(FilterField.IpChecksum, "ip.Checksum");
			AddFilterFieldString(FilterField.IpSourceAddress, "ip.SrcAddr");
			AddFilterFieldString(FilterField.IpDestinationAddress, "ip.DstAddr");
			AddFilterFieldString(FilterField.Ipv6TrafficClass, "ipv6.TrafficClass");
			AddFilterFieldString(FilterField.Ipv6FlowLabel, "ipv6.FlowLabel");
			AddFilterFieldString(FilterField.Ipv6Length, "ipv6.Length");
			AddFilterFieldString(FilterField.Ipv6NextHeader, "ipv6.NextHdr");
			AddFilterFieldString(FilterField.Ipv6HopLimit, "ipv6.HopLimit");
			AddFilterFieldString(FilterField.Ipv6SourceAddress, "ipv6.SrcAddr");
			AddFilterFieldString(FilterField.Ipv6DestinationAddress, "ipv6.DstAddr");
			AddFilterFieldString(FilterField.IcmpType, "icmp.Type");
			AddFilterFieldString(FilterField.IcmpCode, "icmp.Code");
			AddFilterFieldString(FilterField.IcmpChecksum, "icmp.Checksum");
			AddFilterFieldString(FilterField.IcmpBody, "icmp.Body");
			AddFilterFieldString(FilterField.Icmpv6Type, "icmpv6.Type");
			AddFilterFieldString(FilterField.Icmpv6Code, "icmpv6.Code");
			AddFilterFieldString(FilterField.Icmpv6Checksum, "icmpv6.Checksum");
			AddFilterFieldString(FilterField.Icmpv6Body, "icmpv6.Body");
			AddFilterFieldString(FilterField.TcpSourcePort, "tcp.SrcPort");
			AddFilterFieldString(FilterField.TcpDestinationPort, "tcp.DstPort");
			AddFilterFieldString(FilterField.TcpSequenceNumber, "tcp.SeqNum");
			AddFilterFieldString(FilterField.TcpAckNumber, "tcp.AckNum");
			AddFilterFieldString(FilterField.TcpHeaderLength, "tcp.HdrLength");
			AddFilterFieldString(FilterField.TcpUrgent, "tcp.Urg");
			AddFilterFieldString(FilterField.TcpAck, "tcp.Ack");
			AddFilterFieldString(FilterField.TcpPsh, "tcp.Psh");
			AddFilterFieldString(FilterField.TcpRst, "tcp.Rst");
			AddFilterFieldString(FilterField.TcpSyn, "tcp.Syn");
			AddFilterFieldString(FilterField.TcpFin, "tcp.Fin");
			AddFilterFieldString(FilterField.TcpWindow, "tcp.Window");
			AddFilterFieldString(FilterField.TcpChecksum, "tcp.Checksum");
			AddFilterFieldString(FilterField.TcpUrgPtr, "tcp.UrgPtr");
			AddFilterFieldString(FilterField.TcpPayloadLength, "tcp.PayloadLength");
			AddFilterFieldString(FilterField.UdpSourcePort, "udp.SrcPort");
			AddFilterFieldString(FilterField.UdpDestinationPort, "udp.DstPort");
			AddFilterFieldString(FilterField.UdpLength, "udp.Length");
			AddFilterFieldString(FilterField.UdpChecksum, "udp.Checksum");
			AddFilterFieldString(FilterField.UdpPayloadLength, "udp.PayloadLength");

			_filterOperations = new Dictionary<FilterOperation, string>();
			AddFilterOperationString(FilterOperation.Equals, "==");
			AddFilterOperationString(FilterOperation.NotEquals, "!=");
			AddFilterOperationString(FilterOperation.Less, "<");
			AddFilterOperationString(FilterOperation.LessOrEquals, "<=");
			AddFilterOperationString(FilterOperation.Greater, ">");
			AddFilterOperationString(FilterOperation.GreaterOrEqials, ">=");
		}

		private static void AddFilterFieldString(FilterField field,string str,bool isIpv6Argument=false)
		{
			_filterFields.Add(field, str);
		}

		private static void AddFilterOperationString(FilterOperation operation,string str)
		{
			_filterOperations.Add(operation, str);
		}

		public static unsafe string FilterDump(NDivert.Interop.WinDivertIoctlFilter[] rawFilter)
		{
			StringBuilder ret = new StringBuilder(256);
			
			for (int i = 0; i < rawFilter.Length; i++)
			{
				ret.Append("label_");
				ret.Append(i);
				ret.Append("\n\t");

				ret.Append("if (");

				if (_filterFields.TryGetValue(rawFilter[i].Field,out string value))
				{
					ret.Append(value);
				}
				else
				{
					ret.Append("unknown.Field");
				}
				ret.Append(" ");


				if (_filterOperations.TryGetValue(rawFilter[i].Test,out string text))
				{
					ret.Append(text);
				}
				else
				{
					ret.Append("??");
				}
				ret.Append(" ");


				ret.Append(rawFilter[i].Arg[0]);
				ret.Append(")\n");

				ret.Append("\t\t");
				switch (rawFilter[i].Success)
				{
					case (ushort)FilterResult.Accept:
						ret.Append("return ACCEPT;");
						break;
					case (ushort)FilterResult.Reject:
						ret.Append("return REJECT;");
						break;
					default:
						ret.AppendFormat("goto label_{0}", rawFilter[i].Success);
						break;
				}
				ret.Append("\n\t");
				ret.Append("else\n");

				ret.Append("\t\t");
				switch (rawFilter[i].Failure)
				{
					case (ushort)FilterResult.Accept:
						ret.Append("return ACCEPT;");
						break;
					case (ushort)FilterResult.Reject:
						ret.Append("return REJECT;");
						break;
					default:
						ret.AppendFormat("goto label_{0}", rawFilter[i].Failure);
						break;
				}
				ret.Append("\n");
			}

			return ret.ToString();
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDivert.Filter
{

	/// <summary>
	/// Converts Expression Tree to WinDivert string filter
	/// </summary>
	public class DivertFilterStringBuilder
	{
		private static readonly Dictionary<MemberInfo, string> _memberStrings;

		private static readonly MethodInfo _ipAddrParseMethod;

		static DivertFilterStringBuilder()
		{
			_memberStrings = new Dictionary<MemberInfo, string>();
			// top
			RegisterMember((IFilter x) => x.Inbound, "inbound");
			RegisterMember((IFilter x) => x.Outbound, "outbound");
			RegisterMember((IFilter x) => x.Ip, "ip");
			RegisterMember((IFilter x) => x.IsTcp, "tcp");
			RegisterMember((IFilter x) => x.Tcp, "tcp");
			RegisterMember((IFilter x) => x.IsUdp, "udp");
			RegisterMember((IFilter x) => x.Udp, "udp");
			RegisterMember((IFilter x) => x.IfIdx, "ifIdx");
			RegisterMember((IFilter x) => x.SubIfIdx, "subIfIdx");
			// ip
			RegisterMember((IIP x) => x.Checksum, "Checksum");
			RegisterMember((IIP x) => x.DstAddr, "DstAddr");
			RegisterMember((IIP x) => x.Id, "Id");
			RegisterMember((IIP x) => x.Protocol, "Protocol");
			RegisterMember((IIP x) => x.SrcAddr, "SrcAddr");
			// tcp & udp
			RegisterMember((ICommonTcpUdp x) => x.SrcPort, "SrcPort");
			RegisterMember((ICommonTcpUdp x) => x.DstPort, "DstPort");
			RegisterMember((ICommonTcpUdp x) => x.PayloadLength, "PayloadLength");
			
			// tcp
			RegisterMember((ITcp x)=>x.SeqNum,"SeqNum");
			RegisterMember((ITcp x) => x.Rst, "Rst");
			RegisterMember((ITcp x) => x.Syn, "Syn");
			RegisterMember((ITcp x) => x.Ack, "Ack");
			// udp
			RegisterMember((IUdp x) => x.Length, "Length");
			

			Expression<Func<string, IPAddress>> lambda = (x) => IPAddress.Parse(x);
			var body = (MethodCallExpression)lambda.Body;
			_ipAddrParseMethod = body.Method;
		}

		static void RegisterMember<TType, TProperty>(Expression<Func<TType, TProperty>> expression, string name)
		{
			LambdaExpression lambda = expression;
			MemberExpression memberExpression = (MemberExpression)lambda.Body;
			_memberStrings.Add(memberExpression.Member, name);
		}


		private static string GetOperand(ExpressionType expressionType)
		{
			switch (expressionType)
			{
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					return "and";
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					return "or";
				case ExpressionType.Equal:
					return "==";
				case ExpressionType.NotEqual:
					return "!=";
				case ExpressionType.LessThan:
					return "<";
				case ExpressionType.LessThanOrEqual:
					return "<=";
				case ExpressionType.GreaterThan:
					return ">";
				case ExpressionType.GreaterThanOrEqual:
					return ">=";
			}
			throw new InvalidOperationException();
		}

		private static void WriteConstantExpression(TextWriter writer, ConstantExpression constExpression)
		{
			if (constExpression.Type == typeof(int))
			{
				writer.Write((int)constExpression.Value);
			}
			else if (constExpression.Type == typeof(IPAddress))
			{
				var ip = (IPAddress)constExpression.Value;
				writer.Write(ip.ToString());
			}
		}

		private static void WriteConstant(TextWriter writer, object obj)
		{
			var type = obj.GetType();
			if (type == typeof(int))
			{
				writer.Write((int)obj);
			}
			else if (type == typeof(IPAddress))
			{
				var ip = (IPAddress)obj;
				writer.Write(ip.ToString());
			}
		}

		private static void ProcessExpression(TextWriter writer, Expression expression,bool isInternal)
		{
			ConstantExpression c = null;

			switch (expression.NodeType)
			{
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
					BinaryExpression b = (BinaryExpression)expression;
					if (isInternal)
					{
						writer.Write('(');
					}
					ProcessExpression(writer, b.Left, true);
					writer.Write(' ');
					writer.Write(GetOperand(expression.NodeType));
					writer.Write(' ');
					ProcessExpression(writer, b.Right,true);
					if (isInternal)
					{
						writer.Write(')');
					}
					
					break;
				case ExpressionType.MemberAccess:
					MemberExpression m = (MemberExpression)expression;
					switch (m.Expression.NodeType)
					{
						case ExpressionType.Constant:
							c = (ConstantExpression)m.Expression;
							var type = c.Type;
							var fields = type.GetFields();
							var field = fields[0];
							var obj=field.GetValue(c.Value);
							WriteConstant(writer,obj);
							break;
						default:
							if (m.Expression.NodeType != ExpressionType.Parameter)
							{
								ProcessExpression(writer, m.Expression, true);
								writer.Write('.');
							}
							writer.Write(_memberStrings[m.Member]);
							break;
					}
					
					break;
				case ExpressionType.Constant:
					c = (ConstantExpression)expression;
					WriteConstantExpression(writer, c);
					break;
				case ExpressionType.Call:
					MethodCallExpression call = (MethodCallExpression)expression;
					if (call.Type == typeof(IPAddress))
					{
						if (call.Method == _ipAddrParseMethod)
						{
							c = (ConstantExpression)call.Arguments[0];
							writer.Write(c.Value);
						}
					}
					break;
				case ExpressionType.Not:
					UnaryExpression unary = (UnaryExpression)expression;
					writer.Write("not");
					writer.Write(' ');
					ProcessExpression(writer, unary.Operand, true);
					break;
				default:
					break;
			}
		}

		public static string MakeFilter(Expression<Func<IFilter, bool>> expression)
		{
			StringBuilder filterBuilder = new StringBuilder();
			LambdaExpression lambda = expression;
			Expression body = lambda.Body;
			ProcessExpression(new StringWriter(filterBuilder), body, false);
			return filterBuilder.ToString();
		}

		public static void WriteFilter(byte[] array, Expression<Func<IFilter, bool>> expression)
		{
			MemoryStream m = new MemoryStream(array, true);
			LambdaExpression lambda = expression;
			Expression body = lambda.Body;
			using (var writer = new StreamWriter(m, Encoding.ASCII, 1024))
			{
				ProcessExpression(writer, body, false);
				writer.Flush();
				m.WriteByte(0);
			}
		}
	}
}

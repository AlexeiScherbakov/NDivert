using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using NDivert.Filter;
using NDivert.Interop;

namespace NDivert
{
	/// <summary>
	/// Drops all traffic which is matched by filter rule
	/// </summary>
	public sealed class PacketDropper
		: CriticalFinalizerObject, IDisposable
	{
		private bool _isStarted;
		private List<Expression<Func<IFilter, bool>>> _dropRules;
		private List<WinDivertHandle> _handles;
		public PacketDropper()
		{
			_dropRules = new List<Expression<Func<IFilter, bool>>>();
			_handles = new List<WinDivertHandle>();
		}

		~PacketDropper()
		{
			Stop();
		}

		public void Dispose()
		{
			Stop();
		}

		public bool IsStarted
		{
			get { return _isStarted; }
		}

		public void AddRule(Expression<Func<IFilter, bool>> rule)
		{
			_dropRules.Add(rule);
		}

		public void Start()
		{
			if (_isStarted)
			{
				return;
			}
			byte[] ruleBuffer = new byte[10240];
			for (int i = 0; i < _dropRules.Count; i++)
			{
				DivertFilterStringBuilder.WriteFilter(ruleBuffer, _dropRules[i]);

				IntPtr rawHandle = Interop.NativeMethods.WinDivertOpen(ruleBuffer, WinDivertLayer.Network, 0, WinDivertFlag.Drop);
				//var rule=DivertFilterBuilder.MakeFilter(_dropRules[i]);
				//IntPtr rawHandle = Interop.NativeMethods.WinDivertOpen(rule, WinDivertLayer.Network, 0, WinDivertFlag.Drop);
				WinDivertHandle handle = rawHandle;
				_handles.Add(handle);
			}
			_isStarted = true;
		}

		public void Stop()
		{
			if (_isStarted)
			{
				for (int i = 0; i < _handles.Count; i++)
				{
					_handles[i].Close();
				}
				_handles.Clear();
				_isStarted = false;
			}
		}

		public void Restart()
		{
			Stop();
			Start();
		}
	}
}

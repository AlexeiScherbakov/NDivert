using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NDivert.Filter;
using NDivert.Interop;

namespace NDivert
{
	/// <summary>
	/// Drops all traffic which is matched by filter rules
	/// </summary>
	public sealed class PacketDropper
		: CriticalFinalizerObject, IDisposable
	{
		private bool _isStarted;
		private List<FilterDefinition> _dropRules;
		private List<WinDivertHandle> _handles;
		private short _priority;
		public PacketDropper(short priority)
		{
			_priority = priority;
			_dropRules = new List<FilterDefinition>();
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

		public void AddRule(FilterDefinition rule)
		{
			_dropRules.Add(rule);
		}

		public void Start()
		{
			if (_isStarted)
			{
				return;
			}
			var handles = Library.OpenDropHandles(_priority, _dropRules);
			_handles.AddRange(handles);

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

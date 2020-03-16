using System;
using System.Collections.Generic;
using System.Text;

using NDivert.Filter;
using NDivert.Interop;

namespace NDivert
{
	public static class Library
	{
		private static LibraryMode _libraryMode;

		public static LibraryMode Mode
		{
			get { return _libraryMode; }
			set
			{
				if (_libraryMode == LibraryMode.None)
				{
					_libraryMode = value;
				}
			}
		}

		private static LibraryMode GetSafeLibraryMode()
		{
			if (_libraryMode == LibraryMode.None)
			{
				_libraryMode = LibraryMode.Standard;
			}
			return _libraryMode;
		}


		public static WinDivertHandle OpenHandle(FilterDefinition filter,WinDivertLayer layer,short priority,WinDivertFlag flags)
		{
			byte[] ruleBuffer = new byte[10240];
			return OpenHandle(ruleBuffer, filter, layer, priority, flags);
		}

		private static WinDivertHandle OpenHandle(byte[] ruleBuffer, FilterDefinition filter,WinDivertLayer layer,short priority,WinDivertFlag flags)
		{
			LibraryMode mode = GetSafeLibraryMode();

			if (filter._stringValue != null)
			{
				int count=Encoding.ASCII.GetBytes(filter._stringValue, 0, filter._stringValue.Length, ruleBuffer, 0);
			}
			else
			{
				DivertFilterStringBuilder.WriteFilter(ruleBuffer, filter._filterExpression);
			}

			


			switch (mode)
			{
				case LibraryMode.Standard:
					//var rule=DivertFilterStringBuilder.MakeFilter(filter);
					//IntPtr rawHandle = Interop.NativeMethods.WinDivert.WinDivertOpen(rule, layer,priority,flags);
					IntPtr rawHandle = Interop.NativeMethods.WinDivert.WinDivertOpen(ruleBuffer, layer, priority,flags);
					WinDivertLibHandle wh = rawHandle;
					if (wh.IsInvalid)
					{
						var error = NativeMethods.Kernel32.GetLastError();
						switch (error)
						{
							case 2:
								throw new Exception("Driver WinDivert32.sys or WinDivert64.sys is not found");
							case 5:
								throw new UnauthorizedAccessException("Need Admin");
							case 87:
								throw new ArgumentException("filter expression is invalid", nameof(filter));
							case 577:
								throw new UnauthorizedAccessException("Driver signature verification failed");
							case 654:
								throw new InvalidOperationException("An incompatible version of the WinDivert driver is currently loaded");
							case 1060:
								throw new InvalidOperationException("The handle was opened with the WINDIVERT_FLAG_NO_INSTALL flag and the WinDivert driver is not already installed.");
							case 1275:
								throw new UnauthorizedAccessException("Driver is blocked by other software");
							case 1753:
								throw new InvalidOperationException("Base Filtering Engine service has been disabled");
						}
					}
					return wh;
				case LibraryMode.ManagedOnly:
				default:
					throw new InvalidOperationException();
			}
		}

		internal static WinDivertHandle[] OpenDropHandles(short priority,IList<FilterDefinition> filters)
		{
			byte[] ruleBuffer = new byte[10240];

			WinDivertHandle[] ret = new WinDivertHandle[filters.Count];

			for (int i = 0; i < ret.Length; i++)
			{
				ret[i] = OpenHandle(ruleBuffer, filters[i], WinDivertLayer.Network, priority, WinDivertFlag.Drop);
			}
			return ret;
		}
	}


	public enum LibraryMode
	{
		/// <summary>
		/// Library mode not selected
		/// </summary>
		None,
		/// <summary>
		/// All operations require WinDivert.dll
		/// </summary>
		Standard,
		/// <summary>
		/// Not implemented yet
		/// </summary>
		ManagedOnly
	}
}

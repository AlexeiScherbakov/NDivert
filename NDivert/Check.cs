using System;
using System.Security.Principal;

namespace NDivert
{
	public static class Check
	{
		/// <summary>
		/// Checks if program is started as Admin
		/// </summary>
		public static void RequireAdministrator()
		{
			using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
			{
				WindowsPrincipal principal = new WindowsPrincipal(identity);
				if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
				{
					throw new InvalidOperationException("Application must be run as administrator");
				}
			}
		}
	}
}

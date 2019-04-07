using System;
using System.Runtime.ConstrainedExecution;

namespace NDivert
{
	/// <summary>
	/// Packet logger
	/// </summary>
	public abstract class PacketLogger
		: CriticalFinalizerObject, IDisposable
	{

		protected PacketLogger()
		{

		}

		~PacketLogger()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		protected virtual void Dispose(bool disposing)
		{

		}

		public abstract void AddPacket(byte[] packet, int packetLen);

		public abstract void Open();

		public abstract void Close();
	}
}

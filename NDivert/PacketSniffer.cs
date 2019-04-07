using System.IO;

using NDivert.Filter;

namespace NDivert
{

	public sealed class PacketSniffer
		: Sniffer
	{
		private readonly PacketLogger _packetLogger;

		public PacketSniffer(short priority, FilterDefinition filter, PacketLogger packetLogger)
			: base(priority, filter)
		{
			_packetLogger = packetLogger;
		}


		protected override void BeforeStart()
		{
			_packetLogger.Open();
		}

		protected override void AfterStop(bool error)
		{
			_packetLogger.Close();
		}

		protected override void OnPacketCaptured(byte[] packet, int packetLength, in WinDivertAddress divertAddress)
		{
			_packetLogger.AddPacket(packet, packetLength);
		}


		public static PacketSniffer CreatePcapFileSniffer(short priority, FilterDefinition filter, string fileName, bool overwriteIfExisting)
		{
			if (overwriteIfExisting && File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			LibpcapFileWriter fileWriter = new LibpcapFileWriter(fileName);

			PacketSniffer ret = new PacketSniffer(priority, filter, fileWriter);
			return ret;
		}
	}
}

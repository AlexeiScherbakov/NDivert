namespace NDivert
{
	public enum WinDivertAddressFlag
		:byte
	{
		Sniffed=1,
		Outbound=0b10,
		Loopback=0b100,
		Impostor=0b1000,
		IPv6=0b10000,
		IPChecksum=0b100000,
		TCPChecksum=0b1000000,
		UDPChecksum=0b10000000
	}
}

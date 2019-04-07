using System.Runtime.InteropServices;

namespace NDivert
{
	/// <summary>
	/// Divert Address
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WinDivertAddress
	{
		public long Timestamp;
		public uint IfIdx;
		public uint SubIfIdx;
		public byte Flags;


		private const byte DirectionMask1 = 0b00000001;
		private const byte DirectionMask2 = 0b11111110;

		private const byte LoopbackMask1 = 0b00000010;
		private const byte LoopbackMask2 = 0b11111101;

		private const byte ImpostorMask1 = 0b00000100;
		private const byte ImpostorMask2 = 0b11111011;

		private const byte PseudoIPChecksumMask1 = 0b00001000;
		private const byte PseudoIPChecksumMask2 = 0b11110111;

		private const byte PseudoTCPChecksumMask1 = 0b00010000;
		private const byte PseudoTCPChecksumMask2 = 0b11101111;

		private const byte PseudoUDPChecksumMask1 = 0b00100000;
		private const byte PseudoUDPChecksumMask2 = 0b11011111;

		public Direction Direction
		{
			get { return ((Flags & DirectionMask1) != 0) ? Direction.Inbound : Direction.Outbound; }
			set
			{
				switch (value)
				{
					case Direction.Outbound:
						Flags &= DirectionMask2;
						break;
					case Direction.Inbound:
						Flags |= DirectionMask1;
						break;
				}
			}
		}

		public bool Loopback
		{
			get { return (Flags & LoopbackMask1) != 0; }
			set
			{
				if (value)
				{
					Flags |= LoopbackMask1;
				}
				else
				{
					Flags &= LoopbackMask2;
				}
			}
		}

		public bool Impostor
		{
			get { return (Flags & ImpostorMask1) != 0; }
			set
			{
				if (value)
				{
					Flags |= ImpostorMask1;
				}
				else
				{
					Flags &= ImpostorMask2;
				}
			}
		}

		public bool PseudoIPChecksum
		{
			get { return (Flags & PseudoIPChecksumMask1) != 0; }
			set
			{
				if (value)
				{
					Flags |= PseudoIPChecksumMask1;
				}
				else
				{
					Flags &= PseudoIPChecksumMask2;
				}
			}
		}

		public bool PseudoTCPChecksum
		{
			get { return (Flags & PseudoTCPChecksumMask1) != 0; }
			set
			{
				if (value)
				{
					Flags |= PseudoTCPChecksumMask1;
				}
				else
				{
					Flags &= PseudoTCPChecksumMask2;
				}
			}
		}

		public bool PseudoUDPChecksum
		{
			get { return (Flags & PseudoUDPChecksumMask1) != 0; }
			set
			{
				if (value)
				{
					Flags |= PseudoUDPChecksumMask1;
				}
				else
				{
					Flags &= PseudoUDPChecksumMask2;
				}
			}
		}
	}
}

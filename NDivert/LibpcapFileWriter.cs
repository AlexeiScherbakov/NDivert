using System;
using System.IO;
using System.Threading;

namespace NDivert
{
	/// <summary>
	/// libpcap file writer (implementation is not full - so it will be internal until full implementation)
	/// </summary>
	internal sealed class LibpcapFileWriter
		: PacketLogger
	{
		private readonly string _fileName;
		private BinaryWriter _w;
		private FileStream _file;
		private bool _autoFlush;
		public LibpcapFileWriter(string fileName)
		{
			_fileName = fileName;
		}

		public bool AutoFlush
		{
			get { return _autoFlush; }
			set { _autoFlush = value; }
		}

		public override void Close()
		{
			var w = Interlocked.Exchange(ref _w, null);
			w.Flush();
			w.Close();
			var file = Interlocked.Exchange(ref _file, null);
			file.Close();
		}

		public override void Open()
		{
			if (File.Exists(_fileName))
			{
				OpenAppend();
			}
			else
			{
				OpenNew();
			}
		}

		public void OpenNew()
		{
			var dir = Path.GetDirectoryName(_fileName);
			if (("" != dir) && (!Directory.Exists(dir)))
			{
				Directory.CreateDirectory(dir);
			}
			_file = new FileStream(_fileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.WriteThrough);
			_w = new BinaryWriter(_file, System.Text.Encoding.ASCII, true);
			WriteHeader();
		}



		public void OpenAppend()
		{
			_file = new FileStream(_fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
			_file.Position = _file.Length;
			_w = new BinaryWriter(_file, System.Text.Encoding.ASCII, false);
		}

		private void WriteHeader()
		{
			_w.Write((uint)0xa1b2c3d4);
			_w.Write((short)2);
			_w.Write((short)4);
			_w.Write((int)0);
			_w.Write((uint)0);
			_w.Write((uint)65535);
			// raw ip v4
			_w.Write((int)228);
		}

		private void WriteHeader(uint timestamp, uint utimestamp, uint capturedLen, uint originalLen)
		{
			_w.Write(timestamp);
			_w.Write(utimestamp);
			_w.Write(capturedLen);
			_w.Write(originalLen);
		}

		public void Flush()
		{
			_w.Flush();
		}

		public override void AddPacket(byte[] packet, int packetLen)
		{
			var now = DateTime.UtcNow;
			uint totalSeconds = (uint)((((now.Year * 365 + now.DayOfYear) * 24 + now.Hour) * 60 + now.Minute) * 60 + now.Second);
			WriteHeader(totalSeconds, (uint)now.Millisecond, (uint)packetLen, (uint)packetLen);
			_w.Write(packet, 0, packetLen);

			if (_autoFlush)
			{
				_w.Flush();
			}
		}
	}
}

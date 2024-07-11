using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend;

public class Client
{
	public static async void Run()
	{
		TcpClient _client = new TcpClient("127.0.0.1", 1248);
		NetworkStream _stream = _client.GetStream();

		while (true)
		{
			String? inp = Console.ReadLine();
			if (inp == null)
				continue;
			byte[] data = System.Text.Encoding.ASCII.GetBytes(inp);
			await _stream.WriteAsync(data, 0, data.Length);
		}
	}
}

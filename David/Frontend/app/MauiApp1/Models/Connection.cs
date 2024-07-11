using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MauiApp1.Models;

public class Connection
{
	private NetworkStream _stream;
	
	public Connection(NetworkStream stream)
    {
        _stream = stream;
	}

	public async Task Send(String message)
	{
		Console.WriteLine(message);
		byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
		await _stream.WriteAsync(data);
	}

	public async Task<String> Receive()
	{
		byte[] data = new byte[1024];
		int size = await _stream.ReadAsync(data);
		String message = String.Empty;
		for (int i = 0; i < size; i++)
			message += Convert.ToChar(data[i]);
		Console.WriteLine(message);
		return message;
	}
}

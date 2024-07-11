using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Backend;

public class Connection
{
	private Socket _socket;
	
	public Connection(Socket socket)
    {
        _socket = socket;
	}

	public void Send(String message)
	{
		Console.WriteLine(message);
		byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
		 _socket.Send(data);
	}

	public   String Receive()
	{
		byte[] data = new byte[1024];
		int size =  _socket.Receive(data);
		String message = String.Empty;
		for (int i = 0; i < size; i++)
			message += Convert.ToChar(data[i]);
		Console.WriteLine(message);
		return message;
	}
}

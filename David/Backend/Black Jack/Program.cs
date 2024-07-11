using Frontend;
using Backend;

public class Program
{
	public static void Main(String[] args)
	{
		if (args.Length != 1)
		{
			Console.WriteLine("Enter a single argument.");
			return;
		}

		if (args[0] == "Client")
			Client.Run();
		else if (args[0] == "Server")
			Server.Run();
		else
			Console.WriteLine($"Argument '{args[0]}' not found.");
	}
}

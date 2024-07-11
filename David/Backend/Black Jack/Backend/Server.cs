using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Backend;

public class Server
{
	public static  void Run()
	{
		IPAddress addr = IPAddress.Parse("127.0.0.1");
		int port = 1248;
		TcpListener listener = new(addr, port);
		listener.Start();
		int bankValue = 5000;
		
		while (true)
		{
			Console.WriteLine("Waiting for connection...");

			Socket client = listener.AcceptSocket();

			try
			{
                Connection player = new(client);
                string message = string.Empty;
                int bettingValue = 0;

                while (message != "Bet")
                {
                    message = player.Receive();
                    string[] split = message.Split(' ');
                    if (split.Length != 2)
                    {
                        continue;
                    }
                    message = split[0];

                    bettingValue = int.Parse(split[1]);
                    if (bettingValue > bankValue)
                    {
                        message = string.Empty;
                    }

                }
                var game = new Game();

                player.Send($"Dealer {game.handToString(game.DealerHand)}");
                player.Send($"Player {game.handToString(game.PlayerHand)}");
                while (!game.IsPlayerBust)
                {


                    message = player.Receive();
                    if (message == "Hit")
                    {
                        game.DealCardToPlayer();
                    }
                    else if (message == "Stand")
                    {
                        break;
                    }
                    player.Send($"Player {game.handToString(game.PlayerHand)}");

                }
                while (game.gameIsRunning())
                {
                    if (game.DealerScore > 17) break;
                    game.DealCardToDealer();
                }
                player.Send($"Dealer {game.handToString(game.DealerHand)}");

                if (game.gameWin() > 0) bankValue -= bettingValue;
                else if (game.gameWin() < 0) bankValue += bettingValue;

                player.Send($"Finish {game.gameWin()} {bankValue}");

            }
            catch (SocketException e)
			{
                Console.WriteLine(e);
			}
		}
	}
}

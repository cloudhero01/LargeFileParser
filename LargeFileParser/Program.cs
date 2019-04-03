using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RandomFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "Output.txt";
            Dictionary<int, Player> playerList = new Dictionary<int, Player>();
            Player playerHighestBattingAverage = new Player();
            Player playerHighestOnBasePercentage = new Player();
            
            CreatePlayerStatistics(fileName);
            playerList = GetAllPlayerStatistics(fileName);
                        
            foreach (var playerNumber in playerList.Keys)
            {
                var currentPlayer = playerList[playerNumber];

                playerHighestBattingAverage = GetPlayerHighestBattingAverage(currentPlayer, playerHighestBattingAverage);
                playerHighestOnBasePercentage = GetPlayerHighestOnBasePercentage(currentPlayer, playerHighestOnBasePercentage);
            }

            Console.WriteLine("HIGHEST BATTING AVERAGE: {0}", playerHighestBattingAverage.GetBattingAverage());
            Console.WriteLine("HIGHEST ON BASE PERCENTAGE: {0}", playerHighestOnBasePercentage.GetOnBasePercentage());

            Console.WriteLine("\nPLAYER COUNT: {0}", playerList.Count);
            Console.WriteLine("FINISHED: {0}", DateTime.Now);

            Console.ReadLine();
        }

        static void CreatePlayerStatistics(string fileName)
        {
            Console.WriteLine("Please enter number of lines in file: ");

            int maxPlayerActions;
            int.TryParse(Console.ReadLine(), out maxPlayerActions);

            Console.WriteLine("STARTED: {0}", DateTime.Now);
            Console.WriteLine("Writing to file {0} with size of {1}...", new FileInfo(fileName).FullName, maxPlayerActions);

            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                Random rand = new Random();
                const int maxPlayers = 100;
                char[] charRange;
                int randomNumber;
                string newLine;

                for (int x = 0; x < maxPlayerActions; x++)
                {
                    charRange = new char[] { 'O', 'W', 'H' };
                    randomNumber = rand.Next(1, maxPlayers);
                    newLine = randomNumber.ToString() + charRange[rand.Next(0, 3)].ToString() + Environment.NewLine;
                    byte[] block = new UTF8Encoding(true).GetBytes(newLine.ToString());
                    fileStream.Write(block, 0, newLine.Length);
                }
            };

            Console.WriteLine("File Generated at '{0}'.", new FileInfo(fileName).FullName);
        }

        static Dictionary<int, Player> GetAllPlayerStatistics(string fileName)
        {
            StringBuilder myString = new StringBuilder();
            Dictionary<int, Player> newPlayerList = new Dictionary<int, Player>();

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var playerInfo = reader.ReadLine();
                        int playerNumber;
                        string playerStatisticType;
                        
                        Regex regex = new Regex(@"(\d+)(\w+)");
                        var match = regex.Match(playerInfo);
                        playerNumber = int.Parse(match.Groups[1].Value);
                        playerStatisticType = match.Groups[2].Value;
                        
                        if (newPlayerList.Keys.Contains(playerNumber))
                        {
                            var currentPlayer = newPlayerList[playerNumber];

                            switch (playerStatisticType)
                            {
                                case "O":
                                    currentPlayer.TotalOutsMade = currentPlayer.TotalOutsMade + 1;
                                    break;
                                case "H":
                                    currentPlayer.TotalHits = currentPlayer.TotalHits + 1;
                                    break;
                                case "W":
                                    currentPlayer.TotalWalks = currentPlayer.TotalWalks + 1;
                                    break;
                            }
                        }
                        else
                        {
                            Player newPlayer;

                            switch (playerStatisticType)
                            {
                                case "O":
                                    newPlayer = new Player(playerNumber, PlayerOutsMade: 1);
                                    break;
                                case "H":
                                    newPlayer = new Player(playerNumber, PlayerHits: 1);
                                    break;
                                case "W":
                                    newPlayer = new Player(playerNumber, PlayerWalks: 1);
                                    break;
                                default:
                                    throw new System.Exception("Failed to add player due to incorrect player detail found!");
                            }

                            newPlayerList.Add(newPlayer.Number, newPlayer);
                        }
                    }

                    return newPlayerList;
                }
            }
        }

        static Player GetPlayerHighestBattingAverage(Player currentPlayer, Player bestPlayer)
        {
            var currentBattingAverage = currentPlayer.GetBattingAverage();
            var highestBattingAverage = bestPlayer.GetBattingAverage();

            if (currentBattingAverage > highestBattingAverage)
            {
                return currentPlayer;
            }
            else
            {
                return bestPlayer;
            }
        }

        static Player GetPlayerHighestOnBasePercentage(Player currentPlayer, Player bestPlayer)
        {
            var currentOnBasePercentage = currentPlayer.GetOnBasePercentage();
            var highestOnBasePercentage = bestPlayer.GetOnBasePercentage();

            if (currentOnBasePercentage > highestOnBasePercentage)
            {
                return currentPlayer;
            }
            else
            {
                return bestPlayer;
            }
        }
    }
}

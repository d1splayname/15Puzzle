// Built-in libraries
using System;
using System.Drawing;
using System.Threading;

namespace The15Game
{
	internal class Program
	{
		private static Random RandomNumberGenerator = new Random();

		private static Tile[,] BoardOrder = new Tile[4, 4];
		private static Point SpaceTile;

		private static int NumberOfMoves;

		private static bool Inverted = false;

		private static bool Running = true;

		private static void Main()
		{
			Console.Title = "The 15-Game";

			string[,] TileText = new string[4, 4] { { " 1", " 2", " 3", " 4" }, { " 5", " 6", " 7", " 8" }, { " 9", "10", "11", "12" }, { "13", "14", "15", "  " } };

			for (int X = 0; X < 4; X++)
			{
				for (int Y = 0; Y < 4; Y++)
				{
					BoardOrder[X, Y] = new Tile(X, Y, TileText[X, Y]);
				}
			}

			Scramble();

			ConsoleKey UserInput;

			while (true)
			{
				DisplayBoard(Running);
				DisplayMenuOptions();

				UserInput = Console.ReadKey(true).Key;

				if (UserInput == ConsoleKey.Q)
				{
					Quit();
				}
				else if (UserInput == ConsoleKey.P)
				{
					while (true)
					{
						Thread.Sleep(20);

						if (Console.ReadKey(true).Key == ConsoleKey.P)
						{
							break;
						}
					}
				}
				else if (UserInput == ConsoleKey.R)
				{
					Restart();
				}
				else if (UserInput == ConsoleKey.I)
				{
					Inverted = !Inverted;
				}

				if (!Inverted)
				{
					if (UserInput == ConsoleKey.UpArrow)
					{
						if (SpaceTile.X > 0)
						{
							MoveUp();
							UpdateMoves();
						}
					}
					else if (UserInput == ConsoleKey.DownArrow)
					{
						if (SpaceTile.X < 3)
						{
							MoveDown();
							UpdateMoves();
						}
					}
					else if (UserInput == ConsoleKey.LeftArrow)
					{
						if (SpaceTile.Y > 0)
						{
							MoveLeft();
							UpdateMoves();
						}
					}
					else if (UserInput == ConsoleKey.RightArrow)
					{
						if (SpaceTile.Y < 3)
						{
							MoveRight();
							UpdateMoves();
						}
					}
				}
				else
				{
					if (UserInput == ConsoleKey.DownArrow)
					{
						if (SpaceTile.X > 0)
						{
							MoveUp();
							UpdateMoves();
						}
					}
					else if (UserInput == ConsoleKey.UpArrow)
					{
						if (SpaceTile.X < 3)
						{
							MoveDown();
							UpdateMoves();
						}
					}
					else if (UserInput == ConsoleKey.RightArrow)
					{
						if (SpaceTile.Y > 0)
						{
							MoveLeft();
							UpdateMoves();
						}
					}
					else if (UserInput == ConsoleKey.LeftArrow)
					{
						if (SpaceTile.Y < 3)
						{
							MoveRight();
							UpdateMoves();
						}
					}
				}
			}
		}

		private static void Quit()
		{
			Console.Clear();

			Console.WriteLine("Are you sure you want to quit?");
			Console.WriteLine(@"[y]es \ [n]o:");

			ConsoleKey Input = Console.ReadKey().Key;

			if (Input == ConsoleKey.Y)
			{
				Environment.Exit(0);
			}
			else if (Input == ConsoleKey.N)
			{
				Console.Clear();

				return;
			}
			else
			{
				Console.WriteLine("\nInvalid input.");
				Console.WriteLine("Press any key to coninue...");
				Console.ReadLine();

				Quit();
			}
		}

		private static void Restart()
		{
			Console.Clear();

			Console.WriteLine("Are you sure you want to restart?");
			Console.WriteLine(@"[y]es \ [n]o:");
			ConsoleKey Input = Console.ReadKey().Key;

			if (Input == ConsoleKey.Y)
			{
				NumberOfMoves = 0;
				Scramble();

				Console.Clear();
			}
			else if (Input == ConsoleKey.N)
			{
				Console.Clear();

				return;
			}
			else
			{
				Console.WriteLine("\nInvalid input.");
				Console.WriteLine("Press any key to coninue...");
				Console.ReadLine();

				Restart();
			}
		}

		private static void MoveRight()
		{
			Point RightTile = new Point(SpaceTile.X, SpaceTile.Y + 1);
			SwapTiles(SpaceTile, RightTile);
			SpaceTile = RightTile;
		}

		private static void MoveLeft()
		{
			Point LeftTile = new Point(SpaceTile.X, SpaceTile.Y - 1);
			SwapTiles(SpaceTile, LeftTile);
			SpaceTile = LeftTile;
		}

		private static void MoveDown()
		{
			Point DownTile = new Point(SpaceTile.X + 1, SpaceTile.Y);
			SwapTiles(SpaceTile, DownTile);
			SpaceTile = DownTile;
		}

		private static void MoveUp()
		{
			Point UpTile = new Point(SpaceTile.X - 1, SpaceTile.Y);
			SwapTiles(SpaceTile, UpTile);
			SpaceTile = UpTile;
		}

		#region Displays

		private static void DisplayBoard(bool running)
		{
			if (running)
			{
				for (int I = 0; I < 4; I++)
				{
					Console.SetCursorPosition(0, I * 2);
					Console.WriteLine("+--+--+--+--+");
					Console.SetCursorPosition(0, I * 2 + 1);
					Console.WriteLine($"|{BoardOrder[I, 0].Text}|{BoardOrder[I, 1].Text}|{BoardOrder[I, 2].Text}|{BoardOrder[I, 3].Text}|");
				}

				Console.SetCursorPosition(0, 8);
				Console.WriteLine("+--+--+--+--+");
			}
		}

		private static void DisplayMenuOptions()
		{
			DisplayQuitOption();
			DisplayPauseOption();
			DisplayRestartOption();
			DisplayInvertInputOption();
		}

		private static void DisplayQuitOption()
		{
			Console.SetCursorPosition(17, 0);
			Console.Write("[Q]uit Game");
		}

		private static void DisplayPauseOption()
		{
			Console.SetCursorPosition(17, 1);
			Console.Write("[P]ause");
		}

		private static void DisplayRestartOption()
		{
			Console.SetCursorPosition(17, 2);
			Console.Write("[R]estart");
		}

		private static void DisplayInvertInputOption()
		{
			Console.SetCursorPosition(17, 3);
			Console.Write("[I]nvert Input");
		}

		private static void UpdateMoves()
		{
			IncrementMoves();
			DisplayMoves();
		}

		private static void IncrementMoves()
		{
			NumberOfMoves++;
		}

		private static void DisplayMoves()
		{
			Console.SetCursorPosition(0, 12);
			Console.Write($"Moves: {NumberOfMoves}");
		}

		#endregion Displays

		#region ScrambleBoard

		private static void Scramble()
		{
			int SwapTimes = RandomNumberGenerator.Next(25, 50) * 2;

			Point[] Indecies = new Point[2] { Point.Empty, Point.Empty };

			for (int I = 0; I < SwapTimes; I++)
			{
				do
				{
					Indecies[0] = new Point(RandomNumberGenerator.Next(0, 3), RandomNumberGenerator.Next(0, 3));
					Indecies[1] = new Point(RandomNumberGenerator.Next(0, 3), RandomNumberGenerator.Next(0, 3));
				} while (Indecies[0] == Indecies[1]);

				SwapTiles(Indecies[0], Indecies[1]);
			}

			// Finds the space tile
			for (int X = 0; X < 4; X++)
			{
				for (int Y = 0; Y < 4; Y++)
				{
					if (BoardOrder[X, Y].Text == "  ")
					{
						SpaceTile = new Point(X, Y);
					}
				}
			}
		}

		private static void SwapTiles(Point Index0, Point Index1)
		{
			string Temp = BoardOrder[Index0.X, Index0.Y].Text;
			BoardOrder[Index0.X, Index0.Y].Text = BoardOrder[Index1.X, Index1.Y].Text;
			BoardOrder[Index1.X, Index1.Y].Text = Temp;
		}

		#endregion ScrambleBoard
	}

	public class Tile
	{
		public int X = -1;
		public int Y = -1;
		public string Text = string.Empty;

		public Tile(int x, int y, string text)
		{
			if (x >= 0 && x <= 3 && y >= 0 && y <= 3)
			{
				X = x;
				Y = y;
				Text = text;
			}
		}
	}
}
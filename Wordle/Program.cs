using System;
using System.IO;
using TextCopy;

namespace Wordle
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Set the file paths to the valid/solution words.
			string validFilePath = "../../../valid.txt";
			string solutionFilePath = "../../../solution.txt";

			// Create lists of the words in the valid/solution word files.
			List<string> validWords = new List<string>();
			List<string> solutionWords = new List<string>();

			// Create an integer saying how many words are in each file, starting at 0.
			// If these values are -1, that means there is no words in the file
			int validFileLength = -1;
			int solutionFileLength = -1;

			// Read the valid words file, and append each word to the validWords list.
			try
			{
				StreamReader validReader = new StreamReader(validFilePath);

				while (!validReader.EndOfStream)
				{
					string line = validReader.ReadLine();
					validWords.Add(line);
					validFileLength++;
				}
				validReader.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Environment.Exit(0);
			}

			// Read the solution words file, and append each word to the solutionWords list.
			try
			{
				StreamReader solutionReader = new StreamReader(solutionFilePath);

				while (!solutionReader.EndOfStream)
				{
					string line = solutionReader.ReadLine();
					solutionWords.Add(line);
					solutionFileLength++;
				}
				solutionReader.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Environment.Exit(0);
			}

			// Pick a solution from the solutionWords list
			Random rng = new Random();
			// string solutionWord = solutionWords[rng.Next(solutionWords.Count)];
			string solutionWord = "greet";

			// An array of each character guessed
			char[] guess = new char[5];

			// Array for scoring and checking.
			int[,] scoring = new int[6,5];


			// The character in the word guessed
			int index = 0;

			// What turn the player is on
			int turn = 0;

			// The word that the user guesses. This is used to compare against the list of valid words.
			string guessedWord = null;

			// Array that stores the amount of times each letter is used in the solution word
			int[] solution = new int[26];

			// The position the console needs to print to
			int posLeft = 0;
			int posTop = 0;

			// Print the gameboard to the console
			Console.WriteLine("[ ]  [ ]  [ ]  [ ]  [ ] ");
			Console.WriteLine("");
			Console.WriteLine("[ ]  [ ]  [ ]  [ ]  [ ] ");
			Console.WriteLine("");
			Console.WriteLine("[ ]  [ ]  [ ]  [ ]  [ ] ");
			Console.WriteLine("");
			Console.WriteLine("[ ]  [ ]  [ ]  [ ]  [ ] ");
			Console.WriteLine("");
			Console.WriteLine("[ ]  [ ]  [ ]  [ ]  [ ] ");
			Console.WriteLine("");
			Console.WriteLine("[ ]  [ ]  [ ]  [ ]  [ ] ");

			while (true)
			{
				// Change the cursor position (column) depending on what character index the player is on.
				switch (index)
				{
					case 0:
						posLeft = 1;
						break;
					case 1:
						posLeft = 6;
						break;
					case 2:
						posLeft = 11;
						break;
					case 3:
						posLeft = 16;
						break;
					case 4:
						posLeft = 21;
						break;
				}

				// Change the cursor position (row) depending on what turn the player is on.
				switch (turn)
				{
					case 0:
						posTop = 0;
						break;
					case 1:
						posTop = 2;
						break;
					case 2:
						posTop = 4;
						break;
					case 3:
						posTop = 6;
						break;
					case 4:
						posTop = 8;
						break;
					case 5:
						posTop = 10;
						break;
				}

				// Set the cursor position so the console is printing to the correct box on the gameboard.
				Console.SetCursorPosition(posLeft, posTop);

				// Intercept the keystroke.
				var keyInfo = Console.ReadKey(intercept: true);

				// If the keystroke is a letter...
				if (char.IsLetter(keyInfo.KeyChar) && index < 5)
				{
					// Append it to the guessed characters array.
					guess[index] = char.ToUpper(keyInfo.KeyChar);

					// Echo it to the correct space in the console gameboard.
					Console.Write(guess[index]);

					// Increase the index variable by 1.
					index++;
				}
				else if (keyInfo.Key == ConsoleKey.Backspace && index > 0)
				{
					// Decrease the index variable by 1.
					index--;

					// Remove the character from the array.
					guess[index] = ' ';

					// Clear the character from the console.
					Console.SetCursorPosition(posLeft, posTop);
					Console.Write(' ');
				}
				else if (keyInfo.Key == ConsoleKey.Enter && index == 5)
				{
					// Echo each character in the guess array.
					foreach (char c in guess)
					{
						guessedWord += Char.ToLower(c);
					}

					// Echo each character and increment by one in the solution array. a=0, b=1, c=2, etc.
					foreach (char c in solutionWord)
					{
						solution[(c - 'a')]++;
					}

					// Check to make sure the guessed word is valid.
					if (validWords.Contains(guessedWord))
					{
						// Check the guessed word letter by letter.
						for (int i = 0; i < 5; i++)
						{
							// If the guessed letter is in the correct place in the solution word.
							if (guessedWord[i] == solutionWord[i])
							{
								// Decrement the letter by one in the solution array, so the letter isn't counted twice.
								solution[guessedWord[i] - 'a']--;

								// Add a '2' (correct place) to the turn array for scoring.
								scoring[turn, i] = 2;

								// Move the cursor to the correct position.
								int posLeftCheck = 0;
								switch (i)
								{
									case 0:
										posLeftCheck = 1;
										break;
									case 1:
										posLeftCheck = 6;
										break;
									case 2:
										posLeftCheck = 11;
										break;
									case 3:
										posLeftCheck = 16;
										break;
									case 4:
										posLeftCheck = 21;
										break;
								}

								// Color the letter green in the console.
								Console.SetCursorPosition(posLeftCheck, posTop);
								Console.ForegroundColor = ConsoleColor.Green;
								Console.Write(guess[i]);
								Console.ResetColor();
							}
						}
						
						// Next check the yellow words
						for (int i = 0; i < 5; i++)
						{
							//  Checked letter is in the solution.       The letter is in solution array.      The letter hasn't already been changed.
							if (solutionWord.Contains(guessedWord[i]) && solution[guessedWord[i] - 'a'] > 0 && !(scoring[turn, i] == 2))
							{
								// Decrement the letter by one in the solution array, so the letter isn't counted twice.
								solution[guessedWord[i] - 'a']--;

								// Add a '1' (wrong place) to the turn array for scoring.
								scoring[turn, i] = 1;

								// Move the cursor to the correct position.
								int posLeftCheck = 0;
								switch (i)
								{
									case 0:
										posLeftCheck = 1;
										break;
									case 1:
										posLeftCheck = 6;
										break;
									case 2:
										posLeftCheck = 11;
										break;
									case 3:
										posLeftCheck = 16;
										break;
									case 4:
										posLeftCheck = 21;
										break;
								}

								// Color the letter yellow in the console.
								Console.SetCursorPosition(posLeftCheck, posTop);
								Console.ForegroundColor = ConsoleColor.Yellow;
								Console.Write(guess[i]);
								Console.ResetColor();
							}
							else if (!(scoring[turn, i] == 2))
							{
								// Add a '0' (incorrect) to the turn array for scoring.
								scoring[turn, i] = 0;
							}
						}

						// Check if the game should continue
						switch ((turn, (guessedWord == solutionWord)))
						{
							case (0, true):
							case (1, true):
							case (2, true):
							case (3, true):
							case (4, true):
							case (5, true):
								Console.SetCursorPosition(0, 12);
								Console.WriteLine("You got it right!\n");
								Result(scoring);
								break;

							case (0, false):
							case (1, false):
							case (2, false):
							case (3, false):
							case (4, false):
								turn++;
								break;

							case (5, false):
								Console.SetCursorPosition(0, 12);
								Console.WriteLine("The word was " + solutionWord + "\n");
								Result(scoring);
								break;
						}
					}
					// The word is not valid.
					else
					{
						// For each letter...
						for (int i = 4; i >= 0; i--)
						{
							// Place the cursor in the correct place.
							switch (i)
							{
								case 0:
									posLeft = 1;
									break;
								case 1:
									posLeft = 6;
									break;
								case 2:
									posLeft = 11;
									break;
								case 3:
									posLeft = 16;
									break;
								case 4:
									posLeft = 21;
									break;
							}
							Console.SetCursorPosition(posLeft, posTop);

							// Clear the letter.
							Console.Write(' ');
						}
					}

					// Reset the guessing variables.
					index = 0;
					guessedWord = null;
					Array.Clear(guess, 0, guess.Length);

					// Clear the solution array.
					Array.Clear(solution, 0, solution.Length);
				}
			}
		}

		static void Result(int[,] scoring)
		{
			string copyToClipboard = "";
			string emoji0 = "⬜"; // "Large White Square" U+2B1C unicode symbol. 
			string emoji1 = "🟨"; // "Large Yellow Square" U+1F7E8 unicode symbol
			string emoji2 = "🟩"; // "Large Green Square" U+1F7E9 unicode symbol

			// For each row in the scoring array...
			for (int i = 0; i < 6; i++)
			{
				// For each column in the scoring array...
				for (int j = 0; j < 5; j++)
				{
					// Do the correct actions depending on what the guess was.
					switch (scoring[i, j])
					{
						case 0:
							copyToClipboard += emoji0;
							Console.ForegroundColor = ConsoleColor.White;
							Console.Write("■"); // "Black Square" U+25A0 unicode symbol.
							break;
						case 1:
							copyToClipboard += emoji1;
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.Write("■"); // "Black Square" U+25A0 unicode symbol.
							break;
						case 2:
							copyToClipboard += emoji2;
							Console.ForegroundColor = ConsoleColor.Green;
							Console.Write("■"); // "Black Square" U+25A0 unicode symbol.
							break;
					}
					Console.ResetColor();
				}
				Console.WriteLine();
				copyToClipboard += "\n";
			}

			Console.WriteLine("\nWould you like to copy your game result to the clipboard? (y/n)");
			var keyInfo = Console.ReadKey(intercept: true);
			if (keyInfo.KeyChar == 'y')
			{
				ClipboardService.SetText(copyToClipboard);
			}
		}
	}
}
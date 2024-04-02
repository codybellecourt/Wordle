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
				StreamReader valid = new StreamReader(validFilePath);

				while (!valid.EndOfStream)
				{
					string line = valid.ReadLine();
					validWords.Add(line);
					validFileLength++;
				}
				valid.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Environment.Exit(0);
			}

			// Read the solution words file, and append each word to the solutionWords list.
			try
			{
				StreamReader solution = new StreamReader(solutionFilePath);

				while (!solution.EndOfStream)
				{
					string line = solution.ReadLine();
					solutionWords.Add(line);
					solutionFileLength++;
				}
				solution.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Environment.Exit(0);
			}

			// Pick a solution from the solutionWords list
			Random rng = new Random();
			string solutionWord = solutionWords[rng.Next(solutionWords.Count)];

			// An array of each character guessed
			char[] guess = new char[5];

			// The character in the word guessed
			int index = 0;

			// What turn the player is on
			int turn = 1;

			// The word that the user guesses. This is used to compare against the list of valid words.
			string guessedWord = null;

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
					case 1:
						posTop = 0;
						break;
					case 2:
						posTop = 2;
						break;
					case 3:
						posTop = 4;
						break;
					case 4:
						posTop = 6;
						break;
					case 5:
						posTop = 8;
						break;
					case 6:
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
					// Print each character in the guess array.
					foreach (char c in guess)
					{
						guessedWord += Char.ToLower(c);
					}

					if (validWords.Contains(guessedWord))
					{
						for (int i = 0; i < 5; i++)
						{
							if (guessedWord[i] == solutionWord[i]) // The guessed letter is correct and in the correct position.
							{
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
								Console.SetCursorPosition(posLeftCheck, posTop);
								Console.ForegroundColor = ConsoleColor.Green;
								Console.Write(guess[i]);
								Console.ResetColor();
							}
							else if (solutionWord.Contains(guessedWord[i])) // The guessed letter is correct but in the wrong position.
							{
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
								Console.SetCursorPosition(posLeftCheck, posTop);
								Console.ForegroundColor = ConsoleColor.Yellow;
								Console.Write(guess[i]);
								Console.ResetColor();
							}
						}

						// Reset the index and guessedWord variables for the next turn
						index = 0;
						guessedWord = null;
						Array.Clear(guess, 0, guess.Length);


						// Check if the game should continue
						switch ((turn, (guessedWord == solutionWord)))
						{
							case (1, true):
							case (2, true):
							case (3, true):
							case (4, true):
							case (5, true):
							case (6, true):
								Console.WriteLine("You got it right!");
								break;

							case (1, false):
							case (2, false):
							case (3, false):
							case (4, false):
							case (5, false):
								turn++;
								break;

							case (6, false):
								Console.WriteLine("You suck the word was " + solutionWord);
								break;
						}
					}

					else
					{
						for (int i = 4; i >= 0; i--)
						{
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
							Console.Write(' ');
						}

						// Reset the index and guessedWord variables.
						index = 0;
						guessedWord = null;
						Array.Clear(guess, 0, guess.Length);
					}

				}
			}
		}
	}
}
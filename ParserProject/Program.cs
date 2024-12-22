using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
	static void Main(string[] args)
	{
		Dictionary<string, List<string>> grammar = null;
		bool continueProgram = true;

		// Initial grammar input
		grammar = InputGrammar();

		while (continueProgram)
		{
			// Check string with current grammar
			CheckString(grammar);

			// Show menu and get valid choice
			string choice;
			bool validChoice;
			do
			{
				Console.WriteLine("\n1-Another Grammar.");
				Console.WriteLine("2-Another String.");
				Console.WriteLine("3-Exit");
				Console.Write("Enter ur choice : ");

				choice = Console.ReadLine();
				validChoice = true;

				switch (choice)
				{
					case "1":
						grammar = InputGrammar();
						break;
					case "2":
						// Continue to next iteration to check another string
						break;
					case "3":
						continueProgram = false;
						break;
					default:
						Console.WriteLine("Invalid choice! Please try again.");
						validChoice = false;
						break;
				}
			} while (!validChoice);
		}
	}

	static Dictionary<string, List<string>> InputGrammar()
	{
		Dictionary<string, List<string>> grammar;
		bool isSimple;

		do
		{
			// Initialize grammar dictionary
			grammar = new Dictionary<string, List<string>>
			{
				{ "S", new List<string>() },
				{ "B", new List<string>() }
			};

			Console.WriteLine("\nEnter grammar rules:");

			foreach (var nonTerminal in grammar.Keys)
			{
				grammar[nonTerminal].Clear();
				for (int j = 0; j < 2; j++)
				{
					Console.Write($"Enter rule number {j + 1} for {nonTerminal}: ");
					grammar[nonTerminal].Add(Console.ReadLine());
				}
			}

			isSimple = ValidateGrammar(grammar);

			if (!isSimple)
			{
				Console.WriteLine("\nThe grammar is not simple. Please re-enter the rules.");
				Console.WriteLine("Remember:");
				Console.WriteLine("- Rules must not be empty");
				Console.WriteLine("- Rules must begin with a terminal (lowercase letter)");
				Console.WriteLine("- Rules for the same non-terminal must begin with different terminals");
			}

		} while (!isSimple);

		Console.WriteLine("\nThe grammar is simple.");
		return grammar;
	}

	static bool ValidateGrammar(Dictionary<string, List<string>> grammar)
	{
		foreach (var nonTerminal in grammar.Keys)
		{
			HashSet<char> firstSymbols = new HashSet<char>();
			foreach (var rule in grammar[nonTerminal])
			{
				if (string.IsNullOrEmpty(rule))
				{
					return false;
				}
				if (!char.IsUpper(nonTerminal[0]))
				{
					return false;
				}
				if (!char.IsLower(rule[0]))
				{
					return false;
				}
				if (!firstSymbols.Add(rule[0]))
				{
					return false;
				}
			}
		}
		return true;
	}

	static void CheckString(Dictionary<string, List<string>> grammar)
	{
		Console.Write("\nEnter the string to be checked: ");
		string input = Console.ReadLine();

		bool isAccepted = ParseString(input, grammar);
		Console.WriteLine($"\nYour input string is {(isAccepted ? "Accepted" : "Rejected")}.");
	}

	static bool ParseString(string input, Dictionary<string, List<string>> grammar)
	{
		Stack<char> stack = new Stack<char>();
		List<char> inputChars = input.ToList();
		int currentPosition = 0;

		stack.Push('S');

		while (stack.Count > 0 && currentPosition < input.Length)
		{
			char currentSymbol = stack.Peek();

			Console.WriteLine($"\nStack after checking: [{string.Join(", ", stack.Reverse().Select(c => $"'{c}'"))}]");
			Console.WriteLine($"The rest of unchecked string: [{string.Join(", ", input.Substring(currentPosition).Select(c => $"'{c}'"))}]");

			if (char.IsLower(currentSymbol))
			{
				if (currentPosition < input.Length && currentSymbol == input[currentPosition])
				{
					stack.Pop();
					currentPosition++;
				}
				else
				{
					return false;
				}
			}
			else
			{
				string nonTerminal = currentSymbol.ToString();
				bool ruleFound = false;

				foreach (string rule in grammar[nonTerminal])
				{
					if (currentPosition < input.Length && rule[0] == input[currentPosition])
					{
						stack.Pop();
						for (int i = rule.Length - 1; i >= 0; i--)
						{
							stack.Push(rule[i]);
						}
						ruleFound = true;
						break;
					}
				}

				if (!ruleFound)
				{
					return false;
				}
			}
		}

		Console.WriteLine($"\nFinal stack: [{string.Join(", ", stack.Reverse().Select(c => $"'{c}'"))}]");
		return stack.Count == 0 && currentPosition == input.Length;
	}
}
namespace CompilerTask;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CppScanner
{
	private static Dictionary<string, string> keywords = new Dictionary<string, string>()
	{
		{"for", "keyword"},
		{"while", "keyword"},
		{"if", "keyword"},
		{"else", "keyword"},
		{"int", "data type"},
		{"float", "data type"},
		{"double", "data type"},
		{"char", "data type"},
		{"return", "keyword"},
		{"break", "keyword"},
		{"continue", "keyword"},
		{"const", "keyword"},
		{"void", "keyword"},
		{"bool", "data type"},
		{"switch", "keyword"},
		{"case", "keyword"},
		{"default", "keyword"},
		{"do", "keyword"},
		{"namespace", "keyword"},
		{"using", "keyword"},
		{"struct", "keyword"},
		{"class", "keyword"},
		{"public", "keyword"},
		{"private", "keyword"},
		{"protected", "keyword"},
		{"new", "keyword"},
		{"delete", "keyword"},
		{"try", "keyword"},
		{"catch", "keyword"},
		{"throw", "keyword"}
	};

	private static Dictionary<string, string> operators = new Dictionary<string, string>()
	{
		{"=", "assignment operator"},
		{"+", "addition operator"},
		{"-", "subtraction operator"},
		{"*", "multiplication operator"},
		{"/", "division operator"},
		{"%", "modulus operator"},
		{"++", "increment operator"},
		{"--", "decrement operator"},
		{"==", "equality operator"},
		{"+=", "addition assignment operator"},
		{"-=", "subtraction assignment operator"},
		{"!=", "inequality operator"},
		{">", "greater than operator"},
		{"<", "less than operator"},
		{">=", "greater than or equal to operator"},
		{"<=", "less than or equal to operator"},
		{"&&", "logical AND operator"},
		{"||", "logical OR operator"},
		{"!", "logical NOT operator"},
		{"<<", "left shift operator"},
		{">>", "right shift operator"},
		{"&", "bitwise AND operator"},
		{"|", "bitwise OR operator"},
		{"^", "bitwise XOR operator"},
		{"~", "bitwise NOT operator"},
		{"?", "ternary operator"}
	};

	private static Dictionary<string, string> specialCharacters = new Dictionary<string, string>()
	{
		{"(", "left parenthesis"},
		{")", "right parenthesis"},
		{"{", "left brace"},
		{"}", "right brace"},
		{"[", "left bracket"},
		{"]", "right bracket"},
		{";", "semicolon"},
		{",", "comma"},
		{".", "dot operator"},
		{":", "colon"},
		{"::", "scope resolution operator"},
		{"#", "preprocessor directive"}
	};

	public static void Analyze(string code)
	{
		// Regular expressions for token patterns
		var patterns = new Dictionary<string, string>
		{
			{ "Comment", @"//.*" },  // Matches comments from // to end of line
			{ "Keyword", @"\b(int|double|float|char|const|for|while|if|else|return|void)\b" },
			{ "Identifier", @"\b[a-zA-Z_][a-zA-Z0-9_]*\b" },
			{ "Operator", @"(\+{1,2}|-{1,2}|==|!=|<=|>=|&&|\|\||[=+\-*/%<>!])" },
			{ "NumericConstant", @"\b\d+(\.\d+)?([eE][+-]?\d+)?\b" }, // for numbers like 3.4, 3.4e+6
            { "SpecialCharacter", @"[;,\(\)\{\}]" }
		};

		// Split the code into words based on whitespace and special characters
		var tokens = Regex.Split(code, @"(\s+|;|,|\(|\)|\{|\})");

		foreach (var token in tokens)
		{
			string trimmedToken = token.Trim();

			if (string.IsNullOrEmpty(trimmedToken))
				continue;

			bool matched = false;

			foreach (var pattern in patterns)
			{
				if (Regex.IsMatch(trimmedToken, pattern.Value))
				{
					string description;

					switch (pattern.Key)
					{
						case "Keyword":
							description = keywords.ContainsKey(trimmedToken) ? keywords[trimmedToken] : "unknown keyword";
							break;
						case "Identifier":
							description = "identifier";
							break;
						case "Operator":
							description = operators.ContainsKey(trimmedToken) ? operators[trimmedToken] : "unknown operator";
							break;
						case "NumericConstant":
							description = "numeric constant";
							break;
						case "SpecialCharacter":
							description = specialCharacters.ContainsKey(trimmedToken) ? specialCharacters[trimmedToken] : "unknown special character";
							break;
						default:
							description = "unknown token";
							break;
					}

					Console.WriteLine($"Token -> {trimmedToken}, Description -> {description}");
					matched = true;
					break;
				}
			}

			if (!matched)
			{
				Console.WriteLine($"Token -> {trimmedToken}, Description -> unknown token");
			}
		}
	}

public static void Main()
	{
		Console.WriteLine("Enter C++ code (leave an empty line to finish):");

		List<string> lines = new List<string>();
		string input;

		// Read lines until an empty line is encountered
		while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
		{
			lines.Add(input);
		}

		Console.WriteLine("\nToken Analysis:\n");

		foreach (string line in lines)
		{
			Analyze(line);
		}
	}
}


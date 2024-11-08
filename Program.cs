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
		{"=", "operator"},
		{"+", "operator"},
		{"-", "operator"},
		{"*", "operator"},
		{"/", "operator"},
		{"%", "operator"},
		{"++", "operator"},
		{"--", "operator"},
		{"==", "operator"},
		{"+=", "operator"},
		{"-=", "operator"},
		{"!=", "operator"},
		{">", "operator"},
		{"<", "operator"},
		{">=", "operator"},
		{"<=", "operator"},
		{"&&", "operator"},
		{"||", "operator"},
		{"!", "operator"},
		{"<<", "operator"},
		{">>", "operator"},
		{"&", "operator"},
		{"|", "operator"},
		{"^", "operator"},
		{"~", "operator"},
		{"?", "operator"}
	};

	private static Dictionary<string, string> specialCharacters = new Dictionary<string, string>()
	{
		{"(", "Special Character"},
		{")", "Special Character"},
		{"{", "Special Character"},
		{"}", "Special Character"},
		{"[", "Special Character"},
		{"]", "Special Character"},
		{";", "Special Character"},
		{",", "Special Character"},
		{".", "Special Character"},
		{":", "Special Character"},
		{"::", "Special Character"},
		{"#", "Special Character"}
	};

	public static void Analyze(string code)
	{
		// Regular expressions for token patterns
		var patterns = new Dictionary<string, string>
	{
		{ "SingleLineComment", @"//.*" }, // Matches single-line comments
        { "MultiLineComment", @"/\*.*?\*/" }, // Matches multi-line comments
        { "Keyword", @"\b(int|double|float|char|const|for|while|if|else|return|void)\b" },
		{ "Identifier", @"\b[a-zA-Z_][a-zA-Z0-9_]*\b" },
		{ "Operator", @"(\+{1,2}|-{1,2}|==|!=|<=|>=|&&|\|\||[=+\-*/%<>!])" },
		{ "NumericConstant", @"\b\d+(\.\d+)?([eE][+-]?\d+)?\b" }, // matches numbers like 3.4, 3.4e+6
        { "SpecialCharacter", @"[;,\(\)\{\}\.\[\]]" } // added dot and brackets as special characters
    };

		// Check if there's a single-line comment
		var commentMatch = Regex.Match(code, patterns["SingleLineComment"]);
		if (commentMatch.Success)
		{
			// Print comment token separately
			Console.WriteLine($"Token -> {commentMatch.Value.Trim()}, Description -> comment");

			// Analyze code before the comment
			code = code.Substring(0, commentMatch.Index);
		}

		// Split tokens using a pattern that separates operators and special characters
		var tokenParts = Regex.Split(code, @"(\s+|;|,|\(|\)|\{|\}|\[|\]|\+|\-|\*|\/|%|=|==|!=|<=|>=|&&|\|\||<|>)");

		foreach (var token in tokenParts)
		{
			string trimmedToken = token.Trim();

			if (string.IsNullOrEmpty(trimmedToken))
				continue;

			bool matched = false;

			// Check each pattern to see if the token matches
			foreach (var pattern in patterns)
			{
				if (pattern.Key == "NumericConstant" && trimmedToken.Contains("."))
				{
					// Special handling for numeric constants with multiple dots
					foreach (char ch in trimmedToken)
					{
						if (char.IsDigit(ch))
							Console.WriteLine($"Token -> {ch}, Description -> numeric");
						else if (ch == '.')
							Console.WriteLine($"Token -> {ch}, Description -> special character");
					}
					matched = true;
					break;
				}
				else if (Regex.IsMatch(trimmedToken, pattern.Value))
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

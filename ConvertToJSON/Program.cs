using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace ConvertTOJSON
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Read all lines from the text file
			string[] lines = File.ReadAllLines(@"../../../input.txt");

			// Convert the array to a list
			List<string> words = new List<string>(lines);

			// Serialize the list to JSON
			string json = JsonConvert.SerializeObject(words, Newtonsoft.Json.Formatting.Indented);
	
			// Write the JSON to a file
			File.WriteAllText(@"../../../output.json", json);
		}
	}
}

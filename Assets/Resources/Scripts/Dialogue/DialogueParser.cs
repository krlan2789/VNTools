﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class DialogueParser : MonoBehaviour
{
	// Variabel untuk membaca file txt
	private string rootDir = "Assets/";
	public string csvFilesDir = "Resources/CSVFiles/";
	[SerializeField]
  private List<DialogueLines> lines;
	[HideInInspector]
  public int countDialogueLine;

  public void Init()
  {
		csvFilesDir = rootDir + csvFilesDir;
    lines = new List<DialogueLines>();
  }

	#region Get Data Events
	public string GetIndex(int row, int coloumn)
  {
		if(row < lines.Count)
    {
			if(coloumn < lines[row].ColoumnCount)
				return lines[row].GetColoumn(coloumn);
    }
    return "";
  }
	// Mengosongkan DialogueLines
	public void DeleteDialogueLinesContent()
	{
		if(lines.Count != 0)
		{
			lines.Clear();
		}
	}
	#endregion

	// Membaca File txt
	public void LoadDialogueText(string fileName)
  {
		// Memuat File
		if(File.Exists(csvFilesDir + fileName + ".txt"))
		{
			string file = csvFilesDir + fileName + ".txt";
			string line;
			StreamReader r = new StreamReader(file);
			using (r)
			{
				do
				{
					line = r.ReadLine();
					if (line != null)
					{
						List<string> tempList = new List<string>();
						string[] tempValue = line.Split('"');
						for(int i = 0; i < tempValue.Length; i++)
						{
							if((i % 2) != 0)
							{
								tempList.Add(tempValue[i]);
							}
						}
						string[] lineValue = tempList.ToArray();
						DialogueLines lineEntry = new DialogueLines(lineValue);
						lines.Add(lineEntry);
					}
				} while (line != null);
				r.Close();
			}
			countDialogueLine = lines.Count;
		}
		else
		{
			Debug.Log("File " + fileName + ".txt tidak ditemukan");
		}
  }

	#region SplitCSVLines Event
	/*
	// Memisahkan bagian2 tertentu pada tiap baris file txt
	private string[] SplitCsvLine(string line)
  {
		// splits a CSV row 
		// Character(string) exclude for split
    string pattern = @"
		# Match one value in valid CSV string.
		(?!\s*$)                                      # Don't match empty last value.
		\s*                                           # Strip whitespace before value.
		(?:                                           # Group for value alternatives.
			'(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
		| ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
		| (?<val>[^,'""\s\\]*(?:\s+[^,'""\s\\]+)*)    # or $3: Non-comma, non-quote stuff.
		)                                             # End group of value alternatives.
		\s*                                           # Strip whitespace after value.
		(?:,|$)                                       # Field ends on comma or EOS.
		";
    string[] values = (from Match m in Regex.Matches(line, pattern, 
		                                                 RegexOptions.ExplicitCapture 
		                                                 | RegexOptions.IgnorePatternWhitespace 
		                                                 | RegexOptions.Multiline)
		                   select m.Groups[1].Value).ToArray();
    return values;
  }
	*/
	#endregion
}
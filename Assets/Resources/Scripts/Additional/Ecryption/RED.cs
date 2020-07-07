using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RED : MonoBehaviour
{
	private string[] baseChar = new string[] { "`", "[", "]", "\'", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", ",", ".", "/", "=", "\\", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "-", ";", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "{", "}", "\"", "<", ">", "?", "+", "|", "_", ":", " ", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
	private string[] methodHex = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E" };
	private string[] methodBin = new string[] { "00000000", "00000001", "00000010", "00000011", "00000100", "00000101", "00000110", "00000111", "00001000", "00001001", "00001010", "00001011", "00001100", "00001101", "00001110", "00001111", "00010000", "00010001", "00010010", "00010011", "00010100", "00010101", "00010110", "00010111", "00011000", "00011001", "00011010", "00011011", "00011100", "00011101", "00011110", "00011111", "00100000", "00100001", "00100010", "00100011", "00100100", "00100101", "00100110", "00100111", "00101000", "00101001", "00101010", "00101011", "00101100", "00101101", "00101110", "00101111", "00110000", "00110001", "00110010", "00110011", "00110100", "00110101", "00110110", "00110111", "00111000", "00111001", "00111010", "00111011", "00111100", "00111101", "00111110", "00111111", "01000000", "01000001", "01000010", "01000011", "01000100", "01000101", "01000110", "01000111", "01001000", "01001001", "01001010", "01001011", "01001100", "01001101", "01001110", "01001111", "01010000", "01010001", "01010010", "01010011", "01010100", "01010101", "01010110", "01010111", "01011000", "01011001", "01011010", "01011011", "01011100", "01011101", "01011110" };

	private static RED instance;

	public static RED Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<RED>();
			return instance;
		}
	}

	#region Mengenkripsikan
	public string ProcessEncrypt(string text)
	{
		string result = FirstEncryptCode(text);
		result = SecondEncryptCode(result);
		return result;
	}
	#endregion

	#region Mendeskripsikan
	public string ProcessDecrypt(string text)
	{
		string result = FirstDescryptCode(text);
		result = SecondDecryptCode(result);
		return result;
	}
	#endregion

	#region Metode Enkripsi
	private string FirstEncryptCode(string text)
	{
		string result = "";
		List<string> encryptedText = new List<string>();
		string[] temp = text.Split();
		for(int i = 0; i < temp.Length; i++)
		{
			for(int j = 0; j < baseChar.Length; j++)
			{
				if(temp[i] == baseChar[j])
				{
					encryptedText.Add(" " + methodHex[j]);
				}
			}
		}
		for(int i = 1; i < encryptedText.Count; i++)
		{
			result += encryptedText[i];
		}
		return result;
	}

	private string SecondEncryptCode(string text)
	{
		string result = "";
		List<string> encryptedText = new List<string>();
		string[] temp = text.Split(' ');
		for (int i = 0; i < temp.Length; i++)
		{
			for (int j = 0; j < baseChar.Length; j++)
			{
				if (temp[i] == methodHex[j])
				{
					encryptedText.Add(" " + methodBin[j]);
				}
			}
		}
		for (int i = 1; i < encryptedText.Count; i++)
		{
			result += encryptedText[i];
		}
		return result;
	}
	#endregion

	#region Metode Deskripsi
	private string FirstDescryptCode(string text)
	{
		string result = "";
		List<string> decryptedText = new List<string>();
		string[] temp = text.Split(' ');
		for(int i = 0; i < temp.Length; i++)
		{
			for(int j = 0; j < methodBin.Length; j++)
			{
				if(temp[i] == methodBin[j])
				{
					decryptedText.Add(" " + methodHex[j]);
				}
			}
		}
		for(int i = 0; i < decryptedText.Count; i++)
		{
			result += decryptedText[i];
		}
		return result;
	}
	private string SecondDecryptCode(string text)
	{
		string result = "";
		List<string> decryptedText = new List<string>();
		string[] temp = text.Split(' ');
		for (int i = 0; i < temp.Length; i++)
		{
			for (int j = 0; j < baseChar.Length; j++)
			{
				if (temp[i] == methodHex[j])
				{
					decryptedText.Add(baseChar[j]);
				}
			}
		}
		for (int i = 0; i < decryptedText.Count; i++)
		{
			result += decryptedText[i];
		}
		return result;
	}
	#endregion
}

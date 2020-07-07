using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoder : MonoBehaviour
{
	[SerializeField]
	private char charSplitter;
	private static Decoder instance;

	public static Decoder Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<Decoder>();
			}
			return instance;
		}
	}
	//	Memecah string menjadi beberapa bagian
	public string[] DecodingText(string text)
	{
		string[] words = text.Split(charSplitter);
		return words;
	}
}

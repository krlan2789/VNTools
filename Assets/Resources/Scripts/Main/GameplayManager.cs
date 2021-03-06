﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : SaveGameManager
{
	// Variabel untuk Menu
	private GameObject panelPref, panelGameplay, panelSave, panelLoad, panelQuitConfirm, panelHomeConfirm;
	private GameObject[] backBtnObjs, yesBtnObjs;
	private Button optionSaveBtn, optionLoadBtn, optionPrefBtn, galleryBtn, audioBtn, menuHomeBtn, quitGameBtn;
	[SerializeField]
	private List<Button> backBtn, yesBtn;
	// Variabel untuk Pilihan
	public Button baseChoiceBtn;
	private GameObject panelChoice, panelDialogue;
	private Transform panelChoiceBtn;
	private Button panelDialogueBtn;
	[SerializeField]
	private Button[] choiceBtn;
	// Variabel untuk Dialog
	public Text textDialogue, textName;
	public float typingDelay;
	private int index;
	private bool isTyping = false, cancelTyping = false;
	//	Variabel untuk gambar latar
	public Image bgImg;
	//	Variabel untuk area karakter
	public GameObject charImg;
	public RectTransform charImgArea;
	private List<GameObject> charImgList = new List<GameObject>();
	private int listCharCount;
	// Variabel unutk file .txt
	public string fileDialogue, fileAnswers, fileChoices, fileListChar;
	[SerializeField]
	private DialogueParser dialogueParser;

	#region Initializing
	// Use this for initialization
	private void Start()
	{
		currentScene = SceneManager.GetActiveScene();
		dialogueParser.Init();
		InitSaveLoad();
		InitUI();
		LoadTxtFile();
		LoadDialogue();
		Debug.Log("Current screen width : " + Screen.width);
	}
	// Memberikan event pada UI gameplay
	private void InitUI()
	{
		choiceBtn = new Button[5];
		//	Objek panel
		panelChoice = GameObject.Find("PanelChoice");
		panelChoiceBtn = GameObject.Find("PanelChoiceBtn").GetComponent<Transform>();
		panelDialogue = GameObject.Find("PanelDialogueBox");
		panelPref = GameObject.Find("PanelPreference");
		panelGameplay = GameObject.Find("PanelGameplay");
		panelSave = GameObject.Find("PanelSave");
		panelLoad = GameObject.Find("PanelLoad");
		panelQuitConfirm = GameObject.Find("PanelQuitConfirm");
		panelHomeConfirm = GameObject.Find("PanelHomeConfirm");
		//	Objek button
		galleryBtn = GameObject.Find("GalleryBtn").GetComponent<Button>();
		audioBtn = GameObject.Find("AudioBtn").GetComponent<Button>();
		menuHomeBtn = GameObject.Find("HomeConfirmBtn").GetComponent<Button>();
		quitGameBtn = GameObject.Find("QuitConfirmBtn").GetComponent<Button>();
		panelDialogueBtn = GameObject.Find("PanelDialogueBox").GetComponent<Button>();
		optionSaveBtn = GameObject.Find("OptionSaveBtn").GetComponent<Button>();
		optionLoadBtn = GameObject.Find("OptionLoadBtn").GetComponent<Button>();
		optionPrefBtn = GameObject.Find("OptionPrefBtn").GetComponent<Button>();
		backBtnObjs = GameObject.FindGameObjectsWithTag("BackBtn");
		yesBtnObjs = GameObject.FindGameObjectsWithTag("YesBtn");
		//	Memberi aksi di setiap button
		foreach (GameObject backBtnTemp in backBtnObjs)
		{
			backBtn.Add(backBtnTemp.GetComponent<Button>());
			backBtn[backBtn.Count - 1].onClick.AddListener(() => BackBtn());
		}
		foreach (GameObject yesBtnTemp in yesBtnObjs)
		{
			yesBtn.Add(yesBtnTemp.GetComponent<Button>());
			yesBtn[yesBtn.Count - 1].onClick.AddListener(() => YesBtn());
		}
		quitGameBtn.onClick.AddListener(() => PanelQuitConfirm());
		menuHomeBtn.onClick.AddListener(() => PanelHomeConfirm());
		optionSaveBtn.onClick.AddListener(() => OptionSaveBtn());
		optionLoadBtn.onClick.AddListener(() => OptionLoadBtn());
		optionPrefBtn.onClick.AddListener(() => OptionSettingBtn());
		panelDialogueBtn.onClick.AddListener(() => AddButtonOnClick(-1));
		// Menonaktifkan Panel selain PanelGameplay
		panelGameplay.SetActive(true);
		panelDialogue.SetActive(true);
		panelChoice.SetActive(false);
		panelPref.SetActive(false);
		panelSave.SetActive(false);
		panelLoad.SetActive(false);
		panelQuitConfirm.SetActive(false);
		panelHomeConfirm.SetActive(false);
		// Story Image
		bgImg = GameObject.Find("Background").GetComponent<Image>();
		//charImg = GameObject.Find("CharArea").GetComponent<RectTransform>();
	}
	#endregion
	/******************************************************************************/
	#region Button Events
	protected override void Save()
	{
		savedState = index;
		base.Save();
	}
	protected override void Load()
	{
		base.Load();
		index = savedState;
	}
	private void OptionSaveBtn()
	{
		panelGameplay.SetActive(false);
		panelSave.SetActive(true);
	}
	private void OptionLoadBtn()
	{
		panelGameplay.SetActive(false);
		panelLoad.SetActive(true);
	}
	private void OptionSettingBtn()
	{
		panelGameplay.SetActive(false);
		panelPref.SetActive(true);
	}
	private void PanelQuitConfirm()
	{
		panelQuitConfirm.SetActive(true);
	}
	private void PanelHomeConfirm()
	{
		panelHomeConfirm.SetActive(true);
	}
	private void BackBtn()
	{
		if (panelQuitConfirm.activeInHierarchy)
		{
			panelQuitConfirm.SetActive(false);
		}
		else if (panelHomeConfirm.activeInHierarchy)
		{
			panelHomeConfirm.SetActive(false);
		}
		else if (panelSave.activeInHierarchy)
		{
			panelGameplay.SetActive(true);
			panelSave.SetActive(false);
		}
		else if (panelLoad.activeInHierarchy)
		{
			panelGameplay.SetActive(true);
			panelLoad.SetActive(false);
		}
		else if (panelPref.activeInHierarchy)
		{
			panelGameplay.SetActive(true);
			panelPref.SetActive(false);
		}
	}
	private void YesBtn()
	{
		if(panelHomeConfirm.activeInHierarchy)
		{
			SceneManager.LoadScene(0);
		}
		else if(panelQuitConfirm.activeInHierarchy)
		{
			Application.Quit();
		}
	}
	private void AddButtonOnClick(int index)
	{
		if (!cancelTyping)
		{
			cancelTyping = true;
		}
		if (index >= 0)
		{
			DestroyChoiceBtn();
			LoadDialogue(index);
		}
		else if (index < 0)
		{
			LoadDialogue();
		}
	}
	private void DestroyChoiceBtn()
	{
		GameObject[] cloneObj = GameObject.FindGameObjectsWithTag("ChoiceBtnClone");
		foreach (GameObject clone in cloneObj)
		{
			Destroy(clone);
		}
	}
	#endregion
	/******************************************************************************/
	#region Membaca file txt
	private void LoadTxtFile()
	{
		int lineCount, num = 0;

		#region File List Char
		if(File.Exists(dialogueParser.csvFilesDir + fileListChar + ".txt"))
		{
			dialogueParser.LoadDialogueText(fileListChar);
			listCharCount = dialogueParser.countDialogueLine;
			index = 0;
			do
			{
				PlayerPrefs.SetString("ListChar.char[" + index + "]", dialogueParser.GetIndex(num, 0));
				Debug.Log("Char " + (index + 1) + " : " + PlayerPrefs.GetString("ListChar.char[" + index + "]"));
				num++;
				index++;
			} while (index < listCharCount);
		}
		#endregion

		#region File Dialogue
		if (File.Exists(dialogueParser.csvFilesDir + fileDialogue + ".txt"))
		{
			dialogueParser.LoadDialogueText(fileDialogue);
			lineCount = dialogueParser.countDialogueLine;
			index = 0;
			do
			{
				PlayerPrefs.SetString("Dialogue.dialogueId[" + index + "]", "" + index);
				PlayerPrefs.SetString("Dialogue.name[" + index + "]", dialogueParser.GetIndex(num, 0));
				PlayerPrefs.SetString("Dialogue.dialogue[" + index + "]", dialogueParser.GetIndex(num, 1));
				PlayerPrefs.SetString("Dialogue.charImg[" + index + "]", dialogueParser.GetIndex(num, 2));
				PlayerPrefs.SetString("Dialogue.posCharImg[" + index + "]", dialogueParser.GetIndex(num, 3));
				PlayerPrefs.SetString("Dialogue.locImg[" + index + "]", dialogueParser.GetIndex(num, 4));
				PlayerPrefs.SetString("Dialogue.triggerChoice[" + index + "]", dialogueParser.GetIndex(num, 5));
				index++;
				num++;
			} while (index < lineCount);
			PlayerPrefs.SetInt("Dialogue.Count", lineCount);
			//Debug.Log("File " + fileDialogue + ".txt, is exists. countDialogueLine = " + lineCount);
		}
		else
		{
			Debug.Log("Unable to open " + fileDialogue + ".txt, file doesn't exist.");
		}
		#endregion

		#region File DialogueChoice
		if (File.Exists(dialogueParser.csvFilesDir + fileChoices + ".txt"))
		{
			dialogueParser.LoadDialogueText(fileChoices);
			lineCount = dialogueParser.countDialogueLine - PlayerPrefs.GetInt("Dialogue.Count");
			index = 0;
			do
			{
				PlayerPrefs.SetString("DialogueChoices.choiceId[" + index + "]", "" + index);
				PlayerPrefs.SetString("DialogueChoices.choice1[" + index + "]", dialogueParser.GetIndex(num, 0));
				PlayerPrefs.SetString("DialogueChoices.choice2[" + index + "]", dialogueParser.GetIndex(num, 1));
				PlayerPrefs.SetString("DialogueChoices.choice3[" + index + "]", dialogueParser.GetIndex(num, 2));
				PlayerPrefs.SetString("DialogueChoices.choice4[" + index + "]", dialogueParser.GetIndex(num, 3));
				PlayerPrefs.SetString("DialogueChoices.choice5[" + index + "]", dialogueParser.GetIndex(num, 4));
				index++;
				num++;
			} while (index < lineCount);
			PlayerPrefs.SetInt("DialogueChoices.Count", lineCount);
			//Debug.Log("File " + fileDialogueChoice + ".txt, is exists. countDialogueLine = " + lineCount);
		}
		else
		{
			Debug.Log("Unable to open " + fileChoices + ".txt, file doesn't exist.");
		}
		#endregion

		#region File DialogueAnswer
		if (File.Exists(dialogueParser.csvFilesDir + fileAnswers + ".txt"))
		{
			dialogueParser.LoadDialogueText(fileAnswers);
			lineCount = dialogueParser.countDialogueLine - PlayerPrefs.GetInt("Dialogue.Count") - PlayerPrefs.GetInt("DialogueChoices.Count");
			index = 0;
			do
			{
				PlayerPrefs.SetString("DialogueAnswers.answerId[" + index + "]", "" + index);
				PlayerPrefs.SetString("DialogueAnswers.answerContent[" + index + "]", dialogueParser.GetIndex(num, 0));
				PlayerPrefs.SetString("DialogueAnswers.answerValue[" + index + "]", dialogueParser.GetIndex(num, 1));
				index++;
				num++;
			} while (index < lineCount);
			PlayerPrefs.SetInt("DialogueAnswers.Count", lineCount);
			//Debug.Log("File " + fileDialogueAnswer + ".txt, is exists. countDialogueLine = " + lineCount);
		}
		else
		{
			Debug.Log("Unable to open " + fileChoices + ".txt, file doesn't exist.");
		}
		#endregion
		dialogueParser.DeleteDialogueLinesContent();

		index = PlayerPrefs.GetInt("CurrentState");
	}
	#endregion
	/******************************************************************************/
	#region Melanjutkan dialog
  //  Statik Dialog
	private void LoadDialogue()
	{
    //  Memastikan ketersediaan Dialog
		if (PlayerPrefs.HasKey("Dialogue.dialogueId[" + index + "]"))
		{
      //  Memastikan variabel index lebih kecil dari jumlah dialog (Jumlah baris pada file dialogue)
			if (index < PlayerPrefs.GetInt("Dialogue.Count"))
			{
				int triggerChoice = int.Parse(PlayerPrefs.GetString("Dialogue.triggerChoice[" + index + "]"));
				int answerId = int.Parse(PlayerPrefs.GetString("DialogueChoices.choiceId[" + triggerChoice + "]"));
				textName.text = PlayerPrefs.GetString("Dialogue.name[" + index + "]");
				if (!isTyping)
				{
          // Memulai auto typing
					StartCoroutine(TextScroll(PlayerPrefs.GetString("Dialogue.dialogue[" + index + "]"), triggerChoice, answerId));
				}
				else if (isTyping && !cancelTyping)
				{
          // Membatalkan auto typing
					cancelTyping = true;
				}
				if (triggerChoice == 0)
        {
          //  Memastikan bahwa tidak ada pilihan jawaban
          DestroyChoiceBtn();
					panelChoice.SetActive(false);
					panelDialogue.SetActive(true);
				}
				index++;
			}
		}   
		else
		{
			Debug.Log("Unable to read dialogue, dialogue doesn't exist.");
		}
	}
    //  Dialog dari jawaban yang dipilih
	private void LoadDialogue(int _index)
    {
      //  Memastikan ketersediaan Dialog
      if (PlayerPrefs.HasKey("Dialogue.dialogueId[" + _index + "]"))
      {
        //  Memastikan variabel index lebih kecil dari jumlah dialog (Jumlah baris pada file dialogue)
        if (_index < PlayerPrefs.GetInt("Dialogue.Count"))
			{
				int triggerChoice = int.Parse(PlayerPrefs.GetString("Dialogue.triggerChoice[" + _index + "]"));
				int choiceId = int.Parse(PlayerPrefs.GetString("DialogueChoices.choiceId[" + triggerChoice + "]"));
				textName.text = PlayerPrefs.GetString("Dialogue.name[" + _index + "]");
				if (!isTyping)
				{
          // Memulai auto typing
					StartCoroutine(TextScroll(PlayerPrefs.GetString("Dialogue.dialogue[" + _index + "]"), triggerChoice, choiceId));
				}
				else if (isTyping && !cancelTyping)
				{
          // Membatalkan auto typing
					cancelTyping = true;
				}
				if (triggerChoice == 0)
				{
          //  Memastikan bahwa tidak ada pilihan jawaban
					DestroyChoiceBtn();
					panelChoice.SetActive(false);
					panelDialogue.SetActive(true);
				}
			}
		}
		else
		{
			Debug.Log("Unable to read dialogue, dialogue doesn't exist.");
		}
	}
	#endregion
	/******************************************************************************/
	#region Menampilkan Char image dan Background image
	private void DisplayCharImg(int index)
	{
		//charImg.sprite = Resources.Load("Img/Char/" + PlayerPrefs.GetString("Dialogue.CharImg"));
		string[] temp = Decoder.Instance.DecodingText(PlayerPrefs.GetString("Dialogue.posCharImg[" + index + "]"));
		Debug.Log(PlayerPrefs.GetString("Dialogue.charImg[" + index + "]") + ", " + temp[0] + ", " + temp[1]);

		for (int i = 0; i < listCharCount; i++)
		{
			for (int j = 0; j < listCharCount; j++)
			{
				if (PlayerPrefs.GetString("Dialogue.name[" + i + "]").Equals(PlayerPrefs.GetString("ListChar.char[" + j +"]")))
				{
					Vector3 imagePosPoint = transform.TransformPoint(0, Screen.width / 2, 0);
					charImgList[i] = Instantiate(charImg, imagePosPoint, Quaternion.identity) as GameObject;
				}
			}
		}

		
		if (temp[0].ToUpper() == "AD")
		{

		} else if (temp[0].ToUpper() == "RM")
		{

		}	else if (temp[0].ToUpper() == "OW")
		{

		} else
		{
			Debug.Log("Kode posisi salah!!");
		}
	}
	#endregion
	/******************************************************************************/
	#region Mendapatkan Posisi
	public string[] DecodingText(string text)
	{
		string[] words = text.Split('_');
		return words;
	}
	#endregion
	/******************************************************************************/
	#region Menampilkan pilihan jawaban (jika ada)
	private void ChoiceAnswerAppear(int triggerChoice, int answerId)
	{
		if (int.Parse(PlayerPrefs.GetString("DialogueChoices.choiceId[" + answerId + "]")) != 0)
		{
			panelChoice.SetActive(true);
			panelDialogue.SetActive(false);
			string choice1 = PlayerPrefs.GetString("DialogueChoices.choice1[" + answerId + "]");
			string choice2 = PlayerPrefs.GetString("DialogueChoices.choice2[" + answerId + "]");
			string choice3 = PlayerPrefs.GetString("DialogueChoices.choice3[" + answerId + "]");
			string choice4 = PlayerPrefs.GetString("DialogueChoices.choice4[" + answerId + "]");
			string choice5 = PlayerPrefs.GetString("DialogueChoices.choice5[" + answerId + "]");
			if (int.Parse(choice1) != 0)
			{
				choiceBtn[0] = Instantiate(baseChoiceBtn, panelChoiceBtn.transform) as Button;
				choiceBtn[0].tag = "ChoiceBtnClone";
				choiceBtn[0].name = "ChoiceBtnClone1";
				GameObject.Find("ChoiceBtnClone1").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("DialogueAnswers.answerContent[" + choice1 + "]");
				choiceBtn[0].onClick.AddListener(() => AddButtonOnClick(int.Parse(PlayerPrefs.GetString("DialogueAnswers.answerValue[" + choice1 + "]"))));
			}
			if (int.Parse(choice2) != 0)
			{
				choiceBtn[1] = Instantiate(baseChoiceBtn, panelChoiceBtn.transform) as Button;
				choiceBtn[1].tag = "ChoiceBtnClone";
				choiceBtn[1].name = "ChoiceBtnClone2";
				GameObject.Find("ChoiceBtnClone2").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("DialogueAnswers.answerContent[" + choice2 + "]");
				choiceBtn[1].onClick.AddListener(() => AddButtonOnClick(int.Parse(PlayerPrefs.GetString("DialogueAnswers.answerValue[" + choice2 + "]"))));
			}
			if (int.Parse(choice3) != 0)
			{
				choiceBtn[2] = Instantiate(baseChoiceBtn, panelChoiceBtn.transform) as Button;
				choiceBtn[2].tag = "ChoiceBtnClone";
				choiceBtn[2].name = "ChoiceBtnClone3";
				GameObject.Find("ChoiceBtnClone3").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("DialogueAnswers.answerContent[" + choice3 + "]");
				choiceBtn[2].onClick.AddListener(() => AddButtonOnClick(int.Parse(PlayerPrefs.GetString("DialogueAnswers.answerValue[" + choice3 + "]"))));
			}
			if (int.Parse(choice4) != 0)
			{
				choiceBtn[3] = Instantiate(baseChoiceBtn, panelChoiceBtn.transform) as Button;
				choiceBtn[3].tag = "ChoiceBtnClone";
				choiceBtn[3].name = "ChoiceBtnClone4";
				GameObject.Find("ChoiceBtnClone4").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("DialogueAnswers.answerContent[" + choice4 + "]");
				choiceBtn[3].onClick.AddListener(() => AddButtonOnClick(int.Parse(PlayerPrefs.GetString("DialogueAnswers.answerValue[" + choice4 + "]"))));
			}
			if (int.Parse(choice5) != 0)
			{
				choiceBtn[4] = Instantiate(baseChoiceBtn, panelChoiceBtn.transform) as Button;
				choiceBtn[4].tag = "ChoiceBtnClone";
				choiceBtn[4].name = "ChoiceBtnClone5";
				GameObject.Find("ChoiceBtnClone5").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("DialogueAnswers.answerContent[" + choice5 + "]");
				choiceBtn[4].onClick.AddListener(() => AddButtonOnClick(int.Parse(PlayerPrefs.GetString("DialogueAnswers.answerValue[" + choice5 + "]"))));
			}
		}
	}
	#endregion
	/******************************************************************************/
	#region Auto-typing dan memanggil Fungsi menampilkan pilihan (jika ada)
	private IEnumerator TextScroll(string lineOfText, int dialogTriggerChoice, int dialogueChoiceID)
	{
		int letter = 0;
		textDialogue.text = "";
		isTyping = true;
		cancelTyping = false;
		while (isTyping && !cancelTyping && (letter < lineOfText.Length))
		{
			textDialogue.text += lineOfText[letter];
			letter++;
			yield return new WaitForSeconds(typingDelay);
		}
		textDialogue.text = lineOfText;
		if (textDialogue.text == lineOfText)
		{
			ChoiceAnswerAppear(dialogTriggerChoice, dialogueChoiceID);
		}
		isTyping = false;
		cancelTyping = false;
	}
	#endregion
}
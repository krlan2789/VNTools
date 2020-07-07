using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : SaveGameManager
{
	//	Objek Button
	private Button playBtn, newGameBtn, loadGameBtn, optionsBtn, settingBtn, aboutBtn, galleryBtn, quitDialogBtn, quitYesBtn;
	private GameObject[] backBtnObjs;
	[SerializeField]
	private List<Button> backBtns;
	//	Objek Panel
	private GameObject panelPlay, panelMain, panelOptions, panelSetting, panelGallery, panelAbout, panelQuitDialog, panelLoadGame;
	//	Objek Slider
	private Slider bgmSlider, sfxSlider;
	//	Audio Source
	private AudioSource bgmSource, sfxSource;
	//	Scene Name
	[SerializeField]
	private string newGameSceneName;

	#region Initializing UI
	// Use this for initialization
	private void Start()
	{
		currentScene = SceneManager.GetActiveScene();
		InitSaveLoad();
		InitUI();
	}
	private void InitUI()
	{
		//	Objek Panel
		panelMain = GameObject.Find("PanelMain");
		panelPlay = GameObject.Find("PanelPlay");
		panelLoadGame = GameObject.Find("PanelLoadGame");
		panelOptions = GameObject.Find("PanelOptions");
		panelSetting = GameObject.Find("PanelSetting");
		panelGallery = GameObject.Find("PanelGallery");
		panelAbout = GameObject.Find("PanelAbout");
		panelQuitDialog = GameObject.Find("PanelQuitDialog");
		//  Objek Button
		backBtnObjs = GameObject.FindGameObjectsWithTag("BackBtn");
		playBtn = GameObject.Find("playBtn").GetComponent<Button>();
		newGameBtn = GameObject.Find("newGameBtn").GetComponent<Button>();
		loadGameBtn = GameObject.Find("loadGameBtn").GetComponent<Button>();
		optionsBtn = GameObject.Find("optionsBtn").GetComponent<Button>();
		settingBtn = GameObject.Find("settingBtn").GetComponent<Button>();
		aboutBtn = GameObject.Find("aboutBtn").GetComponent<Button>();
		galleryBtn = GameObject.Find("galleryBtn").GetComponent<Button>();
		quitDialogBtn = GameObject.Find("quitDialogBtn").GetComponent<Button>();
		quitYesBtn = GameObject.Find("quitYesBtn").GetComponent<Button>();
		// Objek Slider
		bgmSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
		sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();
		//	Audio Source
		bgmSource = GameObject.Find("BGMSource").GetComponent<AudioSource>();
		sfxSource = GameObject.Find("SFXSource").GetComponent<AudioSource>();
		//  Nonaktifkan panel-panel kecuali PanelMain
		panelMain.SetActive(true);
		panelPlay.SetActive(false);
		panelLoadGame.SetActive(false);
		panelOptions.SetActive(false);
		panelSetting.SetActive(false);
		panelGallery.SetActive(false);
		panelAbout.SetActive(false);
		panelQuitDialog.SetActive(false);
		//	Memberi aksi di setiap button
		foreach (GameObject btn in backBtnObjs)
		{
			backBtns.Add(btn.GetComponent<Button>());
			backBtns[backBtns.Count - 1].onClick.AddListener(() => BackBtn());
		}
		playBtn.onClick.AddListener(() => PlayBtn());
		newGameBtn.onClick.AddListener(() => NewGameBtn());
		loadGameBtn.onClick.AddListener(() => LoadGameBtn());
		optionsBtn.onClick.AddListener(() => OptionsBtn());
		settingBtn.onClick.AddListener(() => SettingBtn());
		aboutBtn.onClick.AddListener(() => AboutBtn());
		galleryBtn.onClick.AddListener(() => GalleryBtn());
		quitDialogBtn.onClick.AddListener(() => QuitDialogBtn());
		quitYesBtn.onClick.AddListener(() => QuitGame());
		//	Memberi aksi pada slider
		bgmSlider.onValueChanged.AddListener(delegate { BGMSlider(); });
		sfxSlider.onValueChanged.AddListener(delegate { SFXSlider(); });
	}
	#endregion

	#region Button Events
	protected override void Load()
	{
		base.Load();
		SceneManager.LoadScene(newGameSceneName);
	}
	private void NewGameBtn()
	{
		PlayerPrefs.SetInt("CurrentState", 0);
		SceneManager.LoadScene(newGameSceneName);
  }
	private void PlayBtn()
  {
    panelPlay.SetActive(true);
    panelMain.SetActive(false);
  }
	private void LoadGameBtn()
  {
    panelLoadGame.SetActive(true);
    panelPlay.SetActive(false);
  }
	private void OptionsBtn()
  {
    panelOptions.SetActive(true);
    panelMain.SetActive(false);
  }
	private void SettingBtn()
  {
    panelSetting.SetActive(true);
    panelOptions.SetActive(false);
  }
	private void AboutBtn()
  {
    panelAbout.SetActive(true);
    panelOptions.SetActive(false);
  }
	private void GalleryBtn()
  {
    panelGallery.SetActive(true);
    panelOptions.SetActive(false);
  }
	private void QuitDialogBtn()
	{
		panelQuitDialog.SetActive(true);
	}
	private void QuitGame()
	{
		Application.Quit();
	}
	private void BackBtn()
  {
    if(panelOptions.activeInHierarchy)
    {
      panelOptions.SetActive(false);
      panelMain.SetActive(true);
    }
    else if(panelSetting.activeInHierarchy)
    {
			PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
			PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
			PlayerPrefs.Save();
      panelSetting.SetActive(false);
      panelOptions.SetActive(true);
    }
    else if(panelGallery.activeInHierarchy)
    {
      panelGallery.SetActive(false);
      panelOptions.SetActive(true);
    }
    else if(panelAbout.activeInHierarchy)
    {
      panelAbout.SetActive(false);
      panelOptions.SetActive(true);
    }
		else if(panelPlay.activeInHierarchy)
		{
			panelPlay.SetActive(false);
			panelMain.SetActive(true);
		}
		else if(panelLoadGame.activeInHierarchy)
		{
			panelLoadGame.SetActive(false);
			panelPlay.SetActive(true);
		}
    else if(panelQuitDialog.activeInHierarchy)
    {
      panelQuitDialog.SetActive(false);
    }
  }
	private void BGMSlider()
	{
		bgmSource.volume = bgmSlider.value;
	}
	private void SFXSlider()
	{
		sfxSource.volume = sfxSlider.value;
	}
	#endregion
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGameManager : MonoBehaviour
{
	private static SaveGameManager instance;

	//	Object Button
	private GameObject[] tempBtnObjs;
	[SerializeField]
	protected List<Button> loadBtns, saveBtns;
	protected GameObject selectedGameobject;
	protected Scene currentScene;
	protected int savedState;

	public static SaveGameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<SaveGameManager>();
			}
			return instance;
		}
	}

	#region Initializing Save-Load button
	protected void InitSaveLoad()
	{
		//	Save Button
		tempBtnObjs = GameObject.FindGameObjectsWithTag("SaveBtn");
		if(tempBtnObjs != null)
		{
			foreach (GameObject btn in tempBtnObjs)
			{
				btn.GetComponentInChildren<Text>().text = btn.name;
				saveBtns.Add(btn.GetComponent<Button>());
				saveBtns[saveBtns.Count - 1].onClick.AddListener(() => Save());
			}
		}
		//	Mengosongkan variabel tempBtnObjs
		tempBtnObjs = null;
		//	Load Button
		tempBtnObjs = GameObject.FindGameObjectsWithTag("LoadBtn");
		if(tempBtnObjs != null)
		{
			foreach (GameObject btn in tempBtnObjs)
			{
				btn.GetComponentInChildren<Text>().text = btn.name;
				loadBtns.Add(btn.GetComponent<Button>());
				loadBtns[loadBtns.Count - 1].onClick.AddListener(() => Load());
			}
		}
	}
	#endregion

	#region Button Events
	protected virtual void Save()
	{
		selectedGameobject = EventSystem.current.currentSelectedGameObject;
		Debug.Log(selectedGameobject.tag + " - " + selectedGameobject.GetComponentInChildren<Text>().text);
		PlayerPrefs.SetString("SavedState" + selectedGameobject.name, (savedState-1).ToString());
	}

	protected virtual void Load()
	{
		selectedGameobject = EventSystem.current.currentSelectedGameObject;
		Debug.Log(selectedGameobject.tag + " - " + selectedGameobject.GetComponentInChildren<Text>().text);
		savedState = int.Parse(PlayerPrefs.GetString("SavedState" + selectedGameobject.name));
		PlayerPrefs.SetInt("CurrentState", savedState);
	}

	public Vector3 StringToVector(string value)
	{
		// (1, 25, 6);
		value = value.Trim(new char[] { '(', ')' });
		// 1, 25, 6;
		value = value.Replace(" ", "");
		// 1,25,6
		string[] pos = value.Split(',');
		// [0]=1 [1]=25 [2]=6
		return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
	}

	public Quaternion StringToQuaternion(string value)
	{
		// (1, 25, 6, 0);
		value = value.Trim(new char[] { '(', ')' });
		// 1, 25, 6, 0;
		value = value.Replace(" ", "");
		// 1,25,6,0
		string[] pos = value.Split(',');
		// [0]=1 [1]=25 [2]=6 [3]=0
		return new Quaternion(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]), float.Parse(pos[3]));
	}
	#endregion
}

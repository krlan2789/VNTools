using System.Collections;
using UnityEngine;

public class Account : Connect
{
	[SerializeField]
	private string inputUsername, inputEmail, inputPassword, inputConfirmPassword;
	[SerializeField]
	private string fileName;

	public string InputUsername { get { return inputUsername; } }
	public string InputEmail { get { return inputEmail; } }
	public string InputPassword { get { return inputPassword; } }
	public string InputConfirmPassword { get { return inputConfirmPassword; } }

	private void Start()
	{
		form = new WWWForm();
	}

	public IEnumerator Login(string email, string password)
	{
		Debug.Log(ConnectToURL(fileName));
		form.AddField("authMode", "login");
		form.AddField("inputEmail", email);
		form.AddField("inputUsername", "");
		form.AddField("inputPassword", password);
		WWW www = new WWW(ConnectToURL(fileName), form);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			RecordError.Record(www.error);
		}
		Debug.Log(www.text);
	}

	public IEnumerator Registry(string username, string email, string password, string verifyPassword)
	{
		Debug.Log(ConnectToURL(fileName));
		form.AddField("authMode", "registry");
		form.AddField("inputUsername", username);
		form.AddField("inputPassword", password);
		form.AddField("inputVerifyPassword", verifyPassword);
		form.AddField("inputEmail", email);
		WWW www = new WWW(ConnectToURL(fileName), form);
		yield return www;
		Debug.Log(www.text);
	}
}

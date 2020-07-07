using System;
using UnityEngine;
using UnityEngine.UI;

public class Index : MonoBehaviour
{
	public InputField usernameField, emailField, passwordField, confirmPasswordField;
	public Button loginBtn, registerBtn;

	public Account account;
	private DateTime dateTime;

	private void Start()
	{
		RecordError.Record("while opening game.");
		loginBtn.onClick.AddListener(() => LoginBtn());
		registerBtn.onClick.AddListener(() => RegisterBtn());
	}
	
	private void Update ()
	{
		if(Input.GetKeyDown(KeyCode.L))
		{
			StartCoroutine(account.Login(account.InputEmail, account.InputPassword));
		}
		else if(Input.GetKeyDown(KeyCode.R))
		{
			StartCoroutine(account.Registry(account.InputUsername, account.InputEmail, account.InputPassword, account.InputConfirmPassword));
		}
	}

	private void LoginBtn()
	{
		StartCoroutine(account.Login(emailField.text, passwordField.text));
	}

	private void RegisterBtn()
	{
		StartCoroutine(account.Registry(usernameField.text, emailField.text, passwordField.text, confirmPasswordField.text));
	}
}

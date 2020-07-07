using UnityEngine;

public abstract class Connect : MonoBehaviour
{
	[SerializeField]
	protected string url = "http://localhost/vn-engine-db/";
	protected WWWForm form;

	protected string ConnectToURL(string file)
	{
		return url + file + ".php";
	}
}

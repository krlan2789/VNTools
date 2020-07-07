using System.Collections.Generic;
using System.Xml.Serialization;
[System.Serializable]
public class ErrorLog
{
	[XmlArray("ErrorList")]
	public List<ErrorItems> listRecorded = new List<ErrorItems>();
}

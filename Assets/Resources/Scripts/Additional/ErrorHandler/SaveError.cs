using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class SaveError
{
	private static FileStream stream;

	public static void WriteToXml(ErrorLog errorLog)
	{
		if(File.Exists(Application.dataPath + "/error_log.xml"))
		{
			ErrorLog temp = new ErrorLog();
			temp = Load();
			foreach (ErrorItems item in temp.listRecorded)
			{
				errorLog.listRecorded.Add(item);
			}
			Save(errorLog);
		}
		else
		{
			Save(errorLog);
		}
	}
	private static void Save(ErrorLog errorLog)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ErrorLog));
		stream = new FileStream(Application.dataPath + "/error_log.xml", FileMode.Create);
		serializer.Serialize(stream, errorLog);
		stream.Close();
	}
	private static ErrorLog Load()
	{
		ErrorLog errorLog = new ErrorLog();
		XmlSerializer serializer = new XmlSerializer(typeof(ErrorLog));
		stream = new FileStream(Application.dataPath + "/error_log.xml", FileMode.Open);
		errorLog = serializer.Deserialize(stream) as ErrorLog;
		stream.Close();
		return errorLog;
	}
}

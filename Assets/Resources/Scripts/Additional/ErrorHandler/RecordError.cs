using System;
using UnityEngine;
[SerializeField]
public static class RecordError
{
	private static DateTime dateTime;
	public static ErrorLog errorLog = new ErrorLog();

	public static void Record(string message)
	{
		dateTime = DateTime.Now;
		string dateRecorded = dateTime.ToUniversalTime().ToString("yyyy/MM/dd");
		string timeRecorded = dateTime.ToUniversalTime().ToString("HH:mm:ss");
		string errorMessage = "Error: " + message;
		ErrorItems item = new ErrorItems();
		item.date = dateRecorded;
		item.time = timeRecorded;
		item.message = errorMessage;
		errorLog.listRecorded.Add(item);
		SaveError.WriteToXml(errorLog);
	}
}

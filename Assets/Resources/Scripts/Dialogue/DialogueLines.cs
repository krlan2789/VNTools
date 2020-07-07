[System.Serializable]
public class DialogueLines
{
	private string[] coloumn;
	public string GetColoumn(int index)
	{
		return coloumn[index];
	}
  public DialogueLines(string[] _coloumn)
  {
		coloumn = _coloumn;
  }
	public int ColoumnCount { get { return coloumn.Length; } }
}

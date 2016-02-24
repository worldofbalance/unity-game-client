using UnityEngine;
using System.Collections;

public class ConvergeHint
{
	public int hintId { get; set; }
	public string text { get; set; }

	public ConvergeHint (int hintId, string text)
	{
		this.hintId = hintId;
		this.text = text;
	}

}


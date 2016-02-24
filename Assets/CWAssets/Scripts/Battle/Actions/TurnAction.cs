using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
public abstract class TurnAction  {
	protected int intCount;
	protected int stringCount;
	protected List<int> intList;
	protected List<string> stringList;

	public TurnAction(int intCount, int stringCount, List<int> intList, List<string> stringList)
	{
		this.intCount = intCount;
		this.stringCount = stringCount;
		this.intList = intList;
		this.stringList = stringList;
	}

	public abstract void readData();

	public abstract void execute();
}
}
using UnityEngine;
using System.Collections;

public class Statistic : MonoBehaviour {

	private int turnCount;	//number of turns
	private int treeDown;	//number of tree planted
	private int treeDestroy;	//number of treeDestroy;
	private int preyDown;	//number of prey put down
	private int preyEaten;	//number of prey be eaten by preditor

	public Statistic(){
		turnCount = 0;
		treeDown = 0;
		treeDestroy = 0;
		preyDown = 0;
		preyEaten = 0;
	}

	public int getTurnCount(){
		return turnCount;
	}

	public int getTreeDown(){
		return treeDown;
	}

	public int getTreeDestroy(){
		return treeDestroy;
	}

	public int getPreyDown(){
		return preyDown;
	}

	public int getPreyEaten(int num){
		return preyEaten;
	}
			
	public void setTurnCount(int num){
		turnCount += num;
	}

	public void setTreeDown(int num){
		treeDown += num;
	}

	public void setTreeDestroy(int num){
		treeDestroy += num;
	}

	public void setPreyDown(int num){
		preyDown += num;
	}

	public void setPreyEaten(int num){
		preyEaten += num;
	}



}

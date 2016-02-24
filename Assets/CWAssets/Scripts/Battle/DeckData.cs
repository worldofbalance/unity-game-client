using UnityEngine;
using System.Collections;
using System;
namespace CW{
public class DeckData {

	// Use this for initialization
	private Stack cards = null;
	public bool isBuilt;

	//public DeckData(ArrayList cards){
		// Convert from arraylist here
	//}

	public DeckData(){
		isBuilt = false;
		cards = new Stack();
	}

	public CardData popCard(){
		CardData temp = default(CardData);
		if (cards.Count > 1 && isBuilt){
			temp = (CardData) cards.Pop ();
			//Debug.Log(" CardID popped: " + temp.cardID );
		}
		return temp;
	}

	public void pushCard(CardData card){
		cards.Push(card);
		//Debug.Log("Pushed card deck data");
	}

	public int getSize(){
		return cards.Count;
	}

	public void setBuilt(bool isBuilt){
		this.isBuilt = isBuilt;
		// don't initialize prefabs until tihs is called
		// you could call your "createDeck() here
		// createDeck()
	}


}



public class CardData {

	public int cardID {get; set;}
	public int health {get; set;}
	public int attack {get; set;}
	public int level {get; set;}
	public int dietType {get; set;}
	public string speciesName {get; set;}
	public string description {get; set;}
	//public string type {get; set;}

	public CardData(int cardID, int health, int attack, int level, int dietType,  
	            string speciesName, string description){
		this.cardID = cardID;
		this.health = health;
		this.attack = attack;
		this.dietType = dietType;
		this.level = level;
		this.speciesName = speciesName;
		this.description = description;
		//this.type = type;
	}

	public CardData(int cardID, int health, int attack, int level){
		this.cardID = cardID;
		this.health = health;
		this.attack = attack;
		//this.dietType = dietType;
		this.level = level;
		//this.speciesName = speciesName;
		//this.description = description;
	}


	}}
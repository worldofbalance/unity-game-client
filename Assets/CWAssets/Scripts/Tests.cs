using UnityEngine;
using System.Collections;
namespace CW{
public class Tests : MonoBehaviour {

	private int matchID;

	private int playerID1;
	// Hardcded player2 for match initialization in built version
	private int playerID2 = 108;
	private bool isActive = false;
	private bool isReady = false;
	private bool opponentIsReady = false;

	private int handPosition = 3;
	private int fieldPosition = 4;
	private int cardID = 17;
	private int attack = 5000;
	// deal card
	private int level = 2;
	private int health = 33;
	private int power = 21;
	// create deck from cards
	ArrayList pseudoDeck = new ArrayList();
	DeckData deck = new DeckData();


	public Tests(){
	}

	public void initMatch (){
		TestInit ();
		//TestMatch ();
	}

	public void runTest(){
	
			TestMatch ();
			TestDealCard();
			TestSummon ();
			TestAttack ();
			TestTreeAttack();
			TestGetDeck ();
			if (deck.isBuilt){
				TestInstantiatedDeck();
			}
			TestEndTurn ();
			TestGameOver();
			//TestQuitMatch ();
			// TODO:
			//TestGetDeck();
	}

	public void TestGameOver(){
		int wonGame = 1;
		int matchID = GameState.matchID;
		CWGame.networkManager.Send ( MatchOverProtocol.Prepare (matchID, wonGame), 
		                     ProcessGameOver);	
	}

	public void ProcessGameOver(NetworkResponse response) {
		ResponseMatchOver args = response as ResponseMatchOver;
		int statusResponse = args.status;
		Debug.Log ("MatchOver status: " + statusResponse);
	
	}

	public void TestInstantiatedDeck(){
		int testNum = 10;
		CardData tempCard;
		for (int i = 0; i < testNum; i++){
			tempCard = deck.popCard();
			Debug.Log ("Pop top card : " + tempCard.cardID );
		}

	}


	public void TestDealCard(){
		CWGame.networkManager.Send ( DealCardProtocol.Prepare (playerID1, 5), 
		                     ProcessDealCard);	
	}


	public void ProcessDealCard(NetworkResponse response){
		ResponseDealCard args = response as ResponseDealCard;
		int status = args.status;
	
		Debug.Log ("DealCard status (actions in queue?): " + status);
	}


	public void TestGetDeck () {
		
		Debug.Log ("TestDeck");
		CWGame.networkManager.Send ( GetDeckProtocol.Prepare (GameState.player.GetID()), 
		                     ProcessGetDeck);
	}


	public void ProcessGetDeck(NetworkResponse response){
		ResponseGetDeck args = response as ResponseGetDeck;
		int numFields = args.numFields;
		int numCards = args.numCards;
		DeckData deck = args.deck;
		CardData card = deck.popCard();

		Debug.Log("Deck Response: deck count: " + deck.getSize() + 
		          " numCards: " + numCards + " num fields: " + numFields); 
		Debug.Log ("Card data: cardID: " + card.cardID + " diet Type: " +
		           card.dietType + " SpeciesName: " + card.speciesName);
	}


	public void TestQuitMatch (){
		CWGame.networkManager.Send ( QuitMatchProtocol.Prepare (matchID), 
		                     ProcessQuitMatch);
	}

	public void ProcessQuitMatch(NetworkResponse response){
		ResponseQuitMatch args = response as ResponseQuitMatch;
		bool opponentReadyResponse = false;
		Debug.Log ("Quit Match Response: opponentIsReady :" + opponentReadyResponse);
	}

	public void TestEndTurn (){
		CWGame.networkManager.Send ( EndTurnProtocol.Prepare (matchID), 
			ProcessEndTurn);
	}



	public void ProcessEndTurn(NetworkResponse response){
		ResponseEndTurn args = response as ResponseEndTurn;
		bool activeResponse = false;
		//Debug.Log ("End Turn Response: isActive :" + activeResponse);
	}


	public void TestTreeAttack(){
		CWGame.networkManager.Send (
			TreeAttackProtocol.Prepare (matchID, attack), 
			ProcessTreeAttack);
	}
	
	public void ProcessTreeAttack(NetworkResponse response) {
		ResponseTreeAttack args = response as ResponseTreeAttack;

		Debug.Log("TreeAttack attack: " + args.status );
		
	}

	public void TestAttack(){
		CWGame.networkManager.Send (
			CardAttackProtocol.Prepare (matchID, attack, fieldPosition), 
			ProcessCardAttack);
	}

	public void ProcessCardAttack(NetworkResponse response) {
		ResponseCardAttack args = response as ResponseCardAttack;

		Debug.Log("CardAttack MatchID:  " + args.status);

	}


	public void TestSummon(){
	//	CWGame.networkManager.Send (
	//		SummonCardProtocol.Prepare (), 
	//		ProcessSummonCard);
	}

	public void ProcessSummonCard(NetworkResponse response) {
		ResponseSummonCard args = response as ResponseSummonCard;
	
		Debug.Log("Summon Response MatchID:  " );
	}

	public void TestMatch() {
		CWGame.networkManager.Send (
			MatchStatusProtocol.Prepare (playerID1, "testName"), 
			ProcessMatchStatus);
	}
	
	
	public void ProcessMatchStatus(NetworkResponse response) {
		ResponseMatchStatus args = response as ResponseMatchStatus;
		isActive = args.isActive;	
		opponentIsReady = args.opponentIsReady;
		Debug.Log("Player iActive: " + isActive + " opponentIsReady " + opponentIsReady );
	}


	public void TestInit (){
		playerID1 = GameState.player.GetID();
		CWGame.networkManager.Send (
			MatchInitProtocol.Prepare (GameState.player.GetID (), playerID2), ProcessMatchInit);
			Debug.Log("Init for player: " + playerID1);
	}
	
	public void ProcessMatchInit(NetworkResponse response) {
		ResponseMatchInit args = response as ResponseMatchInit;
		if (args.status == 0) {
			matchID = args.matchID;
			Debug.Log("MatchID set to: " + matchID);
		}
	}
	
}
}


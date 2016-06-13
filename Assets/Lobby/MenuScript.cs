using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	public bool menuOpen = false;
	public GameObject whosOnlineMenu;
	public GameObject statusContainer;
	public Text[] playerArray;
	public bool statusOpen;
	public Text player1box;
	private string name;
	private string score;

	public ShopPopUP shopPopUp;


	void awake(){
		statusOpen = false;
		playerArray = new Text[18];
		//shopPopUp.UpdateTextFields ();
	}

	void update(){
		Debug.Log ("IN MENU SCRIPT");
	}

	public void OpenWhosOnline(){
		if (menuOpen) 
			CloseAllMenus ();

        //Call method to request players 
        CurrentlyOnline online = this.GetComponent<CurrentlyOnline>();
        Debug.Log("Currently Online instantiated");
        online.requestOnlinePlayers(handleOnlinePlayers);
        


        Debug.Log ("You Pressed WHOS ONLINE?");
	//EventSystemManager sets this item to take priority over bckground objects (mouseEvents)
		EventSystem.current.SetSelectedGameObject(whosOnlineMenu);
		whosOnlineMenu.SetActive (true);
		menuOpen = true;

	}

    private void handleOnlinePlayers(Dictionary<int, Player> playerList)
    {
        //Display the onlinePlayer Response
		int i =0;
        foreach (KeyValuePair<int, Player> entry in playerList)
        {
			score = entry.Value.xp.ToString();
            name = entry.Value.name;
			playerArray [i].text = name + "            " +score;
			i++;
        }

    }

    public void OpenStatus(){
		if (menuOpen) 
			CloseAllMenus ();
			//Debug.Log ("You Pressed STATUS");
			//Camera.main.GetComponent<MapCamera>().Move(GameState.player.GetID());

		statusContainer.SetActive (true);
		menuOpen = true;
		statusOpen = true;
	}

	public void OpenMiniGames(){
		if (menuOpen) 
			CloseAllMenus ();
			Debug.Log ("You Pressed MiniGames");
			menuOpen = true;

	}

	public void OpenConvergence(){
		Debug.Log("You Pressed Convergence");
		gameObject.AddComponent <ConvergeGUI>();
		menuOpen=true;
	}

	public void OpenCardsfWild(){
		Debug.Log("You Pressed CardsOfWild");
		menuOpen=true;
	}

	public void OpenDontEatMe(){
		Debug.Log("You Pressed Dont Eat Me");
		gameObject.AddComponent <DontEatMeGUI>();
		menuOpen=true;
		CloseAllMenus ();
	}
		
	public void OpenMultiplayerGames(){
		Debug.Log("You Pressed Open Multiplayer Games");
		gameObject.AddComponent <MultiplayerGames>();
		//menuOpen=true;
	}


	public void OpenClashOfSpecies(){
		Debug.Log("You Pressed Clash of Species");
		gameObject.AddComponent <ClashOfSpeciesGUI>();
		menuOpen=true;
	}
		

	public void CloseAllMenus(){
		Debug.Log("Closing all menus");
		menuOpen = false;
		whosOnlineMenu.SetActive (false);
		statusContainer.SetActive(false);
		statusOpen = false;
	}

	public bool checkIfOpen(){
		Debug.Log ("STATUS WINDOW OPEN: "+statusOpen);
		return statusOpen;
	}

}

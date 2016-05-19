using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
public class TopPlayers : MonoBehaviour
{
    // This class is used to send a network request to 
    // retrieve the top rated players from the server

	public Text topPlayer1;
	public Text topPlayer2;
	public Text topPlayer3;
    private String[] topPlayerNames = new String[3];
    private int[] topPlayerScores = new int[3];
    public delegate void Callback (String[] topPlayerNames, int[] topPlayerscores);
    //private Callback passedInFunc;
    
    public void requestTopPlayers(/*Callback callback*/)
    {
        //passedInFunc = callback;

        Game.networkManager.Send(
            TopListProtocol.Prepare(),
            ProcessTopList
       );
    }

    private void ProcessTopList(NetworkResponse response)
    {
        ResponseTopList args = response as ResponseTopList;
        topPlayerNames[0] = args.name1;
        topPlayerNames[1] = args.name2;
        topPlayerNames[2] = args.name3;
        topPlayerScores[0] = args.score1;
        topPlayerScores[1] = args.score2;
        topPlayerScores[2] = args.score3;
		topPlayer1.text = "1) "+ topPlayerNames[0] + ": "+ topPlayerScores[0] ;
		topPlayer2.text = "2) "+ topPlayerNames[1] + ": "+ topPlayerScores[1] ;
		topPlayer3.text = "3) "+ topPlayerNames[2] + ": "+ topPlayerScores[2] ;
        //passedInFunc(topPlayerNames, topPlayerScores);
        
    }

	// Use this for initialization
	void Start () {
		requestTopPlayers ();

	}

}

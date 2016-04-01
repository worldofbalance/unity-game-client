using UnityEngine;
using System;
using System.Collections;

public class TopPlayers : MonoBehaviour
{
    // This class is used to send a network request to 
    // retrieve the top rated players in the database
    
    

    private String[] topPlayerNames = new String[3];
    private int[] topPlayerScores = new int[3];
    public delegate void Callback ();
    private Callback passedInFunc;

    // Use this for initialization
    void Start()
    {

    }

    //// Used to test functionality
    //
    //void OnGUI()
    //{
    //    if (Input.GetKeyDown("return") || Input.GetMouseButtonDown(0))
    //    {
    //        getTopPlayers();
    //        String[] testTopNames = getTopPlayerNames();
    //        int[] testTopScores = getTopPlayerScores();
    //        Debug.Log ("rank 1 player: " + testTopNames[0] + " with " + testTopScores[0] + " points.");
    //        Debug.Log ("rank 2 player: " + testTopNames[1] + " with " + testTopScores[1] + " points.");
    //        Debug.Log ("rank 3 player: " + testTopNames[2] + " with " + testTopScores[2] + " points.");
    //    }
    //}

    public void getTopPlayers(Callback callback)
    {
       passedInFunc = callback;

       NetworkManager.Send(
            TopListProtocol.Prepare(),
            ProcessTopList
       );
    }
    
    // have to find a way to make these methods wait for response
    public String[] getTopPlayerNames()
    {
        return topPlayerNames;
    }

    public int[] getTopPlayerScores()
    {
        return topPlayerScores;
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

        passedInFunc();
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}

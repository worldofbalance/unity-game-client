using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorAI : MonoBehaviour
{

    public List<Transform> prey;
    public Transform Target;
    

    void Start()
    {
        Target = null;
        var rb = GetComponent<Rigidbody>();
        prey = new List<Transform>();

        GameObject[] fish = GameObject.FindGameObjectsWithTag("NpcFish");
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] opponent = GameObject.FindGameObjectsWithTag("Opponent");
        // calculate the resulting array size:
        var size = fish.Length + player.Length + opponent.Length;
        GameObject[] ItemsInList = new GameObject[size];
 
        foreach (GameObject _prey in fish)
        {
            prey.Add(_prey.transform);
        }
        foreach (GameObject _prey in player)
        {
            prey.Add(_prey.transform);
        }
        foreach (GameObject _prey in opponent)
        {
            prey.Add(_prey.transform);
        }
    }


    public void CalculateDistance()
    {
        prey.Sort(delegate (Transform t1, Transform t2) {
            return Vector3.Distance(t1.transform.position, transform.position).CompareTo(Vector3.Distance(t2.transform.position, transform.position));
        });
        Target = prey[0];
    }

    public void MoveToPrey()
    {

            CalculateDistance();
        transform.position = Vector3.MoveTowards(transform.position, Target.position, 60 * Time.deltaTime);


    }

    void Update()
    {
        MoveToPrey();
        

        


    }
}


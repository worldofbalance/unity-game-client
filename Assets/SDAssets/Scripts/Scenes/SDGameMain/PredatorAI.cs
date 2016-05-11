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
        GameObject[] ItemsInList = GameObject.FindGameObjectsWithTag("NpcFish");
        foreach (GameObject _prey in ItemsInList)
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


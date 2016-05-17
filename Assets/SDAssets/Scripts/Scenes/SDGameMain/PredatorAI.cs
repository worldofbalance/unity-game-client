using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorAI : MonoBehaviour
{

    public List<Transform> prey;
    public Transform SelectedTarget;

    void Start()
    {
        SelectedTarget = null;
        prey = new List<Transform>();
        AddPreyToList();
    }

    public void AddPreyToList()
    {
        GameObject[] ItemsInList = GameObject.FindGameObjectsWithTag("NpcFish");
        foreach (GameObject _prey in ItemsInList)
        {
            AddTarget(_prey.transform);
        }
    }

    public void AddTarget(Transform newprey)
    {
        prey.Add(newprey);
    }

    public void CalculateDistance()
    {
        prey.Sort(delegate (Transform t1, Transform t2) {
            return Vector3.Distance(t1.transform.position, transform.position).CompareTo(Vector3.Distance(t2.transform.position, transform.position));
        });

    }

    public void MoveToPrey()
    {
        if (SelectedTarget == null)
        {
            CalculateDistance();
            SelectedTarget = prey[0];
        }


    }

    void Update()
    {
        MoveToPrey();
        float dist = Vector3.Distance(SelectedTarget.transform.position, transform.position);

        transform.position = Vector3.MoveTowards(transform.position, SelectedTarget.position, 60 * Time.deltaTime);


    }
}


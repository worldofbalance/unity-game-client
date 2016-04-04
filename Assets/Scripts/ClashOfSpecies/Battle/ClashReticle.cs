using UnityEngine;
using System.Collections;

public class ClashReticle : MonoBehaviour
{

    // Use this for initialization
    ClashHealthBar bar;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ally" || other.gameObject.tag == "Enemy")
        {
            var bar = other.gameObject.GetComponentsInChildren<ClashHealthBar>(true);
            bar[0].gameObject.SetActive(true);
        }

//        bar.dummyTest();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ally" || other.gameObject.tag == "Enemy")
        {
            var bar = other.gameObject.GetComponentsInChildren<ClashHealthBar>(true);
            bar[0].gameObject.SetActive(false);
        }
    }
}

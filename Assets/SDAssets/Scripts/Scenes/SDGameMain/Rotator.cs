/* 
 * File Name: Rotator.cs
 * Description: Randomly rotates attached objects.
 *              *For now, this script is attached to only the prey prefab.
 */ 

using UnityEngine;
using System.Collections;

namespace SD {
public class Rotator : MonoBehaviour {

    void Update () {
        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
    }
}
}

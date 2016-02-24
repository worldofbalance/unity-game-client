using UnityEngine;
using System.Collections;

public class ClashUnitAttributes : MonoBehaviour {
	public string species_name {get; set;}
	public int species_id {get; set;}
	public int hp {get; set;}
	public int attack {get; set;}
	public int prefab_id {get; set;} // carnivore, herbivore, omnivore, plant
	public float movement_speed {get; set;}
	public float attack_range {get; set;} //distance apart from target to attack
	public float attack_speed {get; set;} //attacking interval 1 attack per attackSpeed seconds
	

	// Use this for initialization
	void Start () {
		this.hp = 100;
		//this.SetMoveSpeed (0.00); //move speed for plants are 0
	}
	
	// Update is called once per frame
	void Update () {
	}

	void TakeDamage(int damage) {
		hp -= damage;
	}
}

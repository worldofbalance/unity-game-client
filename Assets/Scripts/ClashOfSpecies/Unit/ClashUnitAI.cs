using UnityEngine;
using System.Collections;

public class ClashUnitAI : MonoBehaviour {
	ClashUnitAttributes attr;
	ClashNavMeshAI ai;
	public float enemyDistance;
	public GameObject enemy;
	bool attacking;
	public bool hasEnemy;
	public string enemyTag;

	// Use this for initialization
	void Start () {
		attr = GetComponent<ClashUnitAttributes> () as ClashUnitAttributes;
		ai = GetComponent<ClashNavMeshAI> () as ClashNavMeshAI;
		attacking = false;
		hasEnemy = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasEnemy && enemy != null) {
			//attack if it has an enemy
			enemyDistance = Vector3.Distance (enemy.transform.position, transform.position);
			if (enemyDistance <= attr.attack_range) {
				if (!attacking) {
					Invoke ("ApplyDamage", attr.attack_speed);	//do damage every attackInt seconds
					attacking = true;
				}
			}
			if(enemy.GetComponent<ClashUnitAttributes>().hp <= 0) {
				Destroy(enemy);
				ai.SetTarget(null);
				enemy = null;
				hasEnemy = false;
			}
		} else {
			//search for an enemy
			this.SetEnemy(FindClosestEnemy(enemyTag));
		}
	}

	public void SetEnemy(GameObject enemy) {
		this.enemy = enemy;
		ai.SetTarget (enemy);
		hasEnemy = true;
	}

	public void ApplyDamage() {
		//call the method on the enemy to take damage
		if (enemy != null) {
			enemy.SendMessage ("TakeDamage", attr.attack, SendMessageOptions.RequireReceiver);
		}
		attacking = false;

	}

	public GameObject FindClosestEnemy(string enemyTag) {
		GameObject[] enemies;
		enemies = GameObject.FindGameObjectsWithTag (enemyTag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 ourPosition = transform.position;

		if (enemies.Length == 0)
			return closest;

		foreach (GameObject enemy in enemies) {
			Vector3 distanceDifference = (enemy.transform.position - ourPosition);
			float currentDistance = distanceDifference.sqrMagnitude;
			if (currentDistance < distance) {
				closest = enemy;
				distance = currentDistance;
			}
		}
		return closest;
	}
}

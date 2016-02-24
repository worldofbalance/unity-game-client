using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

	public Camera gameCamera = null;
	public bool mouseDown = false;
	protected GameObject obj = null;
	protected enum GAME_TYPE : int {Plant = 0, Herbivore = 1, Carnivore = 2};
	protected int objectType;

	// Current Health
	[SerializeField]
	int speciesId = 0;

	// Use this for initialization
	public virtual void Start () {
	
		/*
		gameCamera = GameObject.Find("MainCamera").camera;
		/*print (gameObject.name);
		print (gameObject.GetInstanceID ());
		this.obj = GameObject.Find (gameObject.name);
		this.obj = GameObject.Find (gameObject.name);*/
		/*
		mouseDown = false;
*/
	}
	
	// Update is called once per frame
	public virtual void Update () {

		/*
		//Update object position to pos
		if (mouseDown) {
			Vector3 pos = gameCamera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x,
		                                                        Input.mousePosition.y, 10));

			obj.transform.position = new Vector3 (pos.x, pos.y, 0);
		}
		*/

	}

	public void SetObjectType(int type) {
		objectType = type;
	}

	public int GetObjectType() {
		return objectType;
	}

	public int GetSpeciesId() {
		return speciesId;
	}

	private string GetTagFromObjectType(int type) {
		switch (type) {
		case 0:
			return "Plant";
		case 1:
			return "Herbivore";
		case 2:
			return "Carnivore";
		default: 
			print("Non enum ObjectType called!");
			return "BadEnum";
		}
	}


	GameObject FindClosestType() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag(GetTagFromObjectType(this.objectType));
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

	public virtual void OnMouseDown() {
		/*

		this.obj = FindClosestType ();
		if (this.obj != null) {
			mouseDown = true;
		}

*/
	}

	public virtual void OnMouseUp() {

		/*
		mouseDown = false;
		*/
	}
}

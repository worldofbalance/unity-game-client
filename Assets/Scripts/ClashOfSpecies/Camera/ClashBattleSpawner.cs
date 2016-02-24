using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ClashBattleSpawner : MonoBehaviour {
    /*
	public ClashDefenseController cdc;
	GameObject required_object, unit;
	
	void Awake() {
		required_object = GameObject.Find ("Persistent Object");
		
		if (required_object == null) {
			Application.LoadLevel ("ClashSplash");
		}
	}
	
	// Use this for initialization
	void Start () {
		pd = required_object.GetComponent<ClashPersistentData> ();
		//Debug.Log (EventSystem.current.IsPointerOverGameObject ());
	}
	
	
	// Update is called once per frame
	void Update () {
		SpawnInvader ();
	}
	
	private void SpawnInvader() {
		if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {	//mouse 1 pressed
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f) && hit.collider.gameObject.tag == "Terrain") {
				Toggle active_toggle = cdc.toggleGroup.GetActiveToggle();
				if(active_toggle != null) {
					int list_index = active_toggle.GetComponent<ClashBattleToggle>().list_index;
					bool isDeployed = pd.attackerInfo.offense[list_index].isDeployed;
					if(!isDeployed) {
						switch(pd.attackerInfo.offense[list_index].prefab_id) {
						case 0:
							unit = Instantiate(Resources.Load ("Prefabs/ClashOfSpecies/Unit/Plant", typeof(GameObject)), hit.point, Quaternion.identity) as GameObject;
							break;
						case 1:
							unit = Instantiate(Resources.Load ("Prefabs/ClashOfSpecies/Unit/Carnivore", typeof(GameObject)), hit.point, Quaternion.identity) as GameObject;
							break;
						case 2:
							unit = Instantiate(Resources.Load ("Prefabs/ClashOfSpecies/Unit/Herbivore", typeof(GameObject)), hit.point, Quaternion.identity) as GameObject;
							break;
						case 3:
							unit = Instantiate(Resources.Load ("Prefabs/ClashOfSpecies/Unit/Omnivore", typeof(GameObject)), hit.point, Quaternion.identity) as GameObject;
							break;
						}
						unit.tag = "Ally";
						unit.GetComponent<ClashUnitAttributes>().species_id = pd.attackerInfo.offense[list_index].species_id;
						unit.GetComponent<ClashUnitAttributes>().species_name = pd.attackerInfo.offense[list_index].species_name;
						unit.GetComponent<ClashUnitAttributes>().prefab_id = pd.attackerInfo.offense[list_index].prefab_id;
						unit.GetComponent<ClashUnitAttributes>().hp = pd.attackerInfo.offense[list_index].hp;
						unit.GetComponent<ClashUnitAttributes>().attack = pd.attackerInfo.offense[list_index].attack;
						unit.GetComponent<ClashUnitAttributes>().attack_speed = pd.attackerInfo.offense[list_index].attack_speed;
						unit.GetComponent<ClashUnitAttributes>().movement_speed = pd.attackerInfo.offense[list_index].movement_speed;
						pd.attackerInfo.offense[list_index].isDeployed = true;
						cdc.toggleGroup.GetActiveToggle().GetComponent<ClashBattleToggle>().toggle.isOn = false;
						//cdc.toggleGroup.GetActiveToggle().GetComponent<ClashBattleToggle>().toggle.interactable = false;
						//cdc.toggleGroup.GetActiveToggle().GetComponent<ClashBattleToggle>().toggle.enabled = false;
						
					}
				}
			}
		}
	}
    */
}

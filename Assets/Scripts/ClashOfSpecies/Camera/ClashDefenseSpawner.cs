using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class ClashDefenseSpawner : MonoBehaviour {
    /*
	public ClashDefenseController cdc;
	GameObject required_object, unit;

	void Awake() {}
	
	// Use this for initialization
	void Start () {
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
					int list_index = active_toggle.GetComponent<ClashDefenseToggle>().list_index;
					bool isDeployed = pd.defenderInfo.defense[list_index].isDeployed;
					if(!isDeployed) {
						switch(pd.defenderInfo.defense[list_index].prefab_id) {
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
						unit.GetComponent<ClashUnitAttributes>().species_id = pd.defenderInfo.defense[list_index].species_id;
						unit.GetComponent<ClashUnitAttributes>().species_name = pd.defenderInfo.defense[list_index].species_name;
						unit.GetComponent<ClashUnitAttributes>().prefab_id = pd.defenderInfo.defense[list_index].prefab_id;
						unit.GetComponent<ClashUnitAttributes>().hp = pd.defenderInfo.defense[list_index].hp;
						unit.GetComponent<ClashUnitAttributes>().attack = pd.defenderInfo.defense[list_index].attack;
						unit.GetComponent<ClashUnitAttributes>().attack_speed = pd.defenderInfo.defense[list_index].attack_speed;
						unit.GetComponent<ClashUnitAttributes>().movement_speed = pd.defenderInfo.defense[list_index].movement_speed;
						pd.defenderInfo.defense[list_index].isDeployed = true;
						cdc.toggleGroup.GetActiveToggle().GetComponent<ClashDefenseToggle>().toggle.isOn = false;
						//cdc.toggleGroup.GetActiveToggle().GetComponent<ClashDefenseToggle>().toggle.interactable = false;
						//cdc.toggleGroup.GetActiveToggle().GetComponent<ClashDefenseToggle>().toggle.enabled = false;

					}
				}
            }
        }
    }
    */
}
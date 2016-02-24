using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ClashDefenseSetup : MonoBehaviour {

    private Dictionary<int, int> remaining = new Dictionary<int, int>();
    private ClashGameManager manager;
    private ClashSpecies selected;
    private Terrain terrain;
    private ToggleGroup toggleGroup;

    public HorizontalLayoutGroup unitList;
    public GameObject defenseItemPrefab;

	public GameObject errorCanvas;
	public Text errorMessage;

	void Awake() {
        manager = GameObject.Find("MainObject").GetComponent<ClashGameManager>();
		toggleGroup = unitList.GetComponent<ToggleGroup>();
    }
    
	// Use this for initialization
	void Start () {
        var terrainObject = Resources.Load<GameObject>("Prefabs/ClashOfSpecies/Terrains/" + manager.pendingDefenseConfig.terrain);
        terrain = (Instantiate(terrainObject, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Terrain>();
        terrain.transform.position = Vector3.zero;
        terrain.transform.localScale = Vector3.one;

        Camera.main.GetComponent<ClashBattleCamera>().target = terrain;

        foreach (var species in manager.pendingDefenseConfig.layout.Keys) {
			var currentSpecies = species;
            var item = Instantiate(defenseItemPrefab) as GameObject;
            remaining.Add(currentSpecies.id, 5);

			var itemReference = item.GetComponent<ClashUnitListItem>();

            var texture = Resources.Load<Texture2D>("Images/" + currentSpecies.name);
			itemReference.toggle.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			itemReference.toggle.onValueChanged.AddListener((val) => {
                if (val) {
                    selected = currentSpecies;
					itemReference.toggle.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                } else {
                    selected = null;
					itemReference.toggle.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            });

			itemReference.toggle.group = toggleGroup;
            item.transform.SetParent(unitList.transform);
            item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 0.0f);
            item.transform.localScale = Vector3.one;
			itemReference.amountLabel.text = remaining[currentSpecies.id].ToString();
        }
	}

    void Update() {

		if (selected == null) return;

		if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100000, LayerMask.GetMask("Terrain"))) {
				NavMeshHit placement;
				if (NavMesh.SamplePosition(hit.point, out placement, 1000, 1)) {
					var allyResource = Resources.Load<GameObject>("Prefabs/ClashOfSpecies/Units/" + selected.name);
					var allyObject = Instantiate(allyResource, placement.position, Quaternion.identity) as GameObject;
                    allyObject.tag = "Ally";
                    
					Vector2 normPos = new Vector2(placement.position.x - terrain.transform.position.x,
					                              placement.position.z - terrain.transform.position.z);
					normPos.x = normPos.x / terrain.terrainData.size.x;
					normPos.y = normPos.y / terrain.terrainData.size.z;

					manager.pendingDefenseConfig.layout[selected].Add(normPos);
                    remaining[selected.id]--;

					var toggle = toggleGroup.ActiveToggles().FirstOrDefault();
					toggle.transform.parent.GetComponent<ClashUnitListItem>().amountLabel.text = remaining[selected.id].ToString();

                    if (remaining[selected.id] == 0) {
                        toggle.enabled = false;
                        toggle.interactable = false;
                        selected = null;
                    } 
				}
			}
		}
    }

	public void ReturnToShop() {
		Game.LoadScene("ClashDefenseShop");
	}

	public void ConfirmDefense() {
 		if(GameObject.FindGameObjectsWithTag("Ally").Count() != 25) {
            errorCanvas.SetActive(true);
			errorMessage.text = "Place all your units down before confirming";
			return;
        }
        
        var pending = manager.pendingDefenseConfig;
        var mappedLayout = pending.layout.ToDictionary((el) => el.Key.id, (el) => el.Value);

		var request = ClashDefenseSetupProtocol.Prepare(pending.terrain, mappedLayout);

        NetworkManager.Send(request, (res) => {
            var response = res as ResponseClashDefenseSetup;
            if (response.valid) {
                manager.defenseConfig = manager.pendingDefenseConfig;
                manager.pendingDefenseConfig = null;
                Game.LoadScene("ClashMain");
            } else {
				errorCanvas.SetActive(true);
				errorMessage.text = "Error in saving data to the DB";
			}
        });
	}

	public void ConfirmError() {
		errorMessage.text = ""; 
		errorCanvas.SetActive (false);
	}
}

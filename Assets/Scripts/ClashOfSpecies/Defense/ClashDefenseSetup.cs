using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClashDefenseSetup : MonoBehaviour
{

    private Dictionary<int, int> remaining = new Dictionary<int, int>();
    private ClashGameManager manager;
    private ClashSpecies selected;
    private Terrain terrain;
    private ToggleGroup toggleGroup;

    public HorizontalLayoutGroup unitList;
    public GameObject defenseItemPrefab;

    public GameObject errorCanvas;
    public Text errorMessage;

    COSAbstractInputController cosInController;

    public float terrainCameraPadding = 40;

    private float minX, maxX, minZ, maxZ;

    void Awake()
    {
        try
        {
            manager = GameObject.Find("MainObject").GetComponent<ClashGameManager>();    
        }
        catch (Exception ex)
        {
            manager = GetComponent<ClashGameManager>();
        }

        toggleGroup = unitList.GetComponent<ToggleGroup>();
    }
    
    // Use this for initialization
    void Start()
    {
        try
        {
            var terrainObject = Resources.Load<GameObject>("Prefabs/ClashOfSpecies/Terrains/" + manager.pendingDefenseConfig.terrain);
            terrain = (Instantiate(terrainObject, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Terrain>();
        }
        catch (Exception ex)
        {
            terrain = Terrain.activeTerrain;
        }
        terrain.transform.position = Vector3.zero;
        terrain.transform.localScale = Vector3.one;

        minX = terrainCameraPadding;
        maxX = Terrain.activeTerrain.terrainData.size.x - terrainCameraPadding;
        minZ = terrainCameraPadding;
        maxZ = Terrain.activeTerrain.terrainData.size.z - terrainCameraPadding;

//        Camera.main.GetComponent<ClashBattleCamera>().target = terrain;

        foreach (var species in manager.pendingDefenseConfig.layout.Keys)
        {
            var currentSpecies = species;
            var item = Instantiate(defenseItemPrefab) as GameObject;
            remaining.Add(currentSpecies.id, 5);

            var itemReference = item.GetComponent<ClashUnitListItem>();

            var texture = Resources.Load<Texture2D>("Images/" + currentSpecies.name);
            itemReference.toggle.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            itemReference.toggle.onValueChanged.AddListener((val) =>
                {
                    if (val)
                    {
                        selected = currentSpecies;
                        itemReference.toggle.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    }
                    else
                    {
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

        if (manager.isRunningOnMobile)
            cosInController = ScriptableObject.CreateInstance<COSMobileInputControler>();
        else
            cosInController = ScriptableObject.CreateInstance<COSDesktopInputController>();
        cosInController.InputControllerAwake(Terrain.activeTerrain);
    }

    void Update()
    {
        RaycastHit hit = cosInController.InputUpdate(Camera.main);

        if (selected == null)
            return;

        if (cosInController.TouchState == COSTouchState.TerrainTapped)
        {
            var allyObject = cosInController.SpawnAlly(hit, selected, remaining, toggleGroup);

            if (allyObject != null)
            {

                Vector2 normPos = new Vector2(allyObject.transform.position.x - terrain.transform.position.x,
                                      allyObject.transform.position.z - terrain.transform.position.z);
                normPos.x = normPos.x / terrain.terrainData.size.x;
                normPos.y = normPos.y / terrain.terrainData.size.z;

                manager.pendingDefenseConfig.layout[selected].Add(normPos);

                if (remaining[selected.id] == 0)
                    selected = null;
            }
        }

    }

    void LateUpdate()
    {
        Vector3 pos = new Vector3(
                          Mathf.Clamp(Camera.main.transform.position.x, minX, maxX),
                          Camera.main.transform.position.y, 
                          Mathf.Clamp(Camera.main.transform.position.z, minZ, maxZ));
        Camera.main.transform.position = pos;
    }

    public void ReturnToShop()
    {
        Game.LoadScene("ClashDefenseShop");
    }

    public void ConfirmDefense()
    {
        if (GameObject.FindGameObjectsWithTag("Ally").Count() != 25)
        {
            errorCanvas.SetActive(true);
            errorMessage.text = "Place all your units down before confirming";
            return;
        }
        
        var pending = manager.pendingDefenseConfig;
        var mappedLayout = pending.layout.ToDictionary((el) => el.Key.id, (el) => el.Value);

        var request = ClashDefenseSetupProtocol.Prepare(pending.terrain, mappedLayout);

        NetworkManagerCOS.getInstance().Send(request, (res) =>
            {
                var response = res as ResponseClashDefenseSetup;
                if (response.valid)
                {
                    manager.defenseConfig = manager.pendingDefenseConfig;
                    manager.pendingDefenseConfig = null;
                    Game.LoadScene("ClashMain");
                }
                else
                {
                    errorCanvas.SetActive(true);
                    errorMessage.text = "Error in saving data to the DB";
                }
            });
    }

    public void ConfirmError()
    {
        errorMessage.text = ""; 
        errorCanvas.SetActive(false);
    }
}

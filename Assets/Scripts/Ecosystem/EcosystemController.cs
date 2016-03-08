using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class EcosystemController : MonoBehaviour {

    public Ecosystem ecosystem { get; set; }
    public Dictionary<int, GameObject> zoneList { get; private set; }
    public GameObject desert_tile;
    public GameObject jungle_tile;
    public GameObject arctic_tile;
    public GameObject grasslands_tile;
    public GameObject outline;
    public bool isLeaving;
    public Vector3 lowerBound { get; set; }
    public Vector3 upperBound { get; set; }

    void Awake() {
        List<Zone> hexEco = new List<Zone>();
        hexEco.Add(new Zone(1, 0, 1, 1));
        hexEco.Add(new Zone(2, 0, 2, 1));
        hexEco.Add(new Zone(3, 0, 3, 1));
        hexEco.Add(new Zone(4, 1, 1, 1));
        hexEco.Add(new Zone(5, 1, 2, 1));
        hexEco.Add(new Zone(6, 1, 3, 1));
        hexEco.Add(new Zone(7, 1, 4, 1));
        hexEco.Add(new Zone(8, 2, 0, 1));
        hexEco.Add(new Zone(9, 2, 1, 1));
        hexEco.Add(new Zone(10, 2, 2, 1));
        hexEco.Add(new Zone(11, 2, 3, 1));
        hexEco.Add(new Zone(12, 2, 4, 1));
        hexEco.Add(new Zone(13, 3, 1, 1));
        hexEco.Add(new Zone(14, 3, 2, 1));
        hexEco.Add(new Zone(15, 3, 3, 1));
        hexEco.Add(new Zone(16, 3, 4, 1));
        hexEco.Add(new Zone(17, 4, 1, 1));
        hexEco.Add(new Zone(18, 4, 2, 1));
        hexEco.Add(new Zone(19, 4, 3, 1));
        Generate(hexEco, new Color32(119, 119, 119, 255));
    }

    // Use this for initialization
    void Start(){
        Game.StartEnterTransition();
        Camera.main.GetComponent<EcosystemCamera>().Setup();

        if (GameState.ecosystem != null) {
            ecosystem = GameState.ecosystem;
        }

        Generate(ecosystem.zones, ecosystem.player.color);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 80, 30), "World") && !isLeaving) {
            Camera.main.GetComponent<EcosystemCamera>().TriggerLeaving();
            isLeaving = true;
            Game.SwitchScene("World");
        }
    }

    public void Generate(List<Zone> zones, Color color) {
        GameObject map = GameObject.Find("Map");

        zoneList = new Dictionary<int, GameObject>();

        foreach (Zone zone in zones) {
            GameObject zoneObject = null;

            switch (zone.terrain_type) {
                case 1:
                    zoneObject = Instantiate(desert_tile) as GameObject;
                    break;
                case 2:
                    zoneObject = Instantiate(jungle_tile) as GameObject;
                    break;
                case 3:
                    zoneObject = Instantiate(grasslands_tile) as GameObject;
                    break;
                case 4:
                    zoneObject = Instantiate(arctic_tile) as GameObject;
                    break;
            }

            if (zoneObject != null) {
                zoneObject.name = "Zone " + zone.row + "-" + zone.column;
                zoneObject.transform.parent = gameObject.transform;
                zoneObject.transform.position = new Vector3(zone.column * 13.85f + (zone.row % 2 == 0 ? 7 : 0), 0, zone.row * -11.95f);

                Zone ztmp = zoneObject.AddComponent<Zone>();
                ztmp.zone_id = zone.zone_id;
                ztmp.row = zone.row;
                ztmp.column = zone.column;
                ztmp.terrain_type = zone.terrain_type;
                ztmp.v_capacity = zone.v_capacity;
                ztmp.player_id = zone.player_id;

                zoneList.Add(zone.row * 40 + zone.column, zoneObject);
                // Borders
                GameObject highlight = Instantiate(outline) as GameObject;
                highlight.transform.position = zoneObject.transform.position + new Vector3(0, 0.1f, 0);
                highlight.transform.parent = zoneObject.transform;

                highlight.GetComponent<Renderer>().material.color = color;

                zoneObject.transform.localScale *= Constants.ECO_HEX_SCALE;
                zoneObject.transform.position *= Constants.ECO_HEX_SCALE;
            }
        }
        // Recenter
        Vector3 temp = Vector3.zero;
        foreach (GameObject o in zoneList.Values) {
            temp += o.transform.position;
            o.transform.parent = null;
        }
        temp /= zoneList.Count;
        map.transform.position = temp;
        foreach (GameObject o in zoneList.Values) {
            o.transform.parent = map.transform;
        }

        map.transform.position = Vector3.zero;
        // Bounds
        foreach (GameObject o in zoneList.Values) {
            lowerBound = new Vector3(Mathf.Min(o.transform.position.x, lowerBound.x), 0, Mathf.Min(o.transform.position.z, lowerBound.z));
            upperBound = new Vector3(Mathf.Max(o.transform.position.x, upperBound.x), 0, Mathf.Max(o.transform.position.z, upperBound.z));
        }
        // Enlarge Land
        Vector3 scale = GameObject.Find("Land").transform.localScale;
        scale.x *= Mathf.Max(1, Mathf.Abs(upperBound.x - lowerBound.x) / 150);
        scale.z *= Mathf.Max(1, Mathf.Abs(upperBound.z - lowerBound.z) / 150);
    }
}

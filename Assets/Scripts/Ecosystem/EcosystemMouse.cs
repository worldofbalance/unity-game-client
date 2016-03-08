using UnityEngine;

using System.Collections;

public class EcosystemMouse : MonoBehaviour {

    public string type = "";
    public GameObject roamingCursor;
    public Zone zone { get; set; }

    // Use this for initialization
    void Start() {
        roamingCursor.GetComponent<Renderer>().material.color = new Color32(0, 181, 248, 255);
        roamingCursor.transform.localScale *= Constants.ECO_HEX_SCALE;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("1")) {
            type = "African Elephant";
        }

        if (Input.GetKeyDown("2")) {
            type = "Decaying Material";
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            type = "";
        }

        if (type == "") {
            roamingCursor.SetActive(false);
        } else {
            roamingCursor.SetActive(true);
        }

        if (roamingCursor.activeInHierarchy) {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                zone = hit.transform.gameObject.GetComponent<Zone>();

                roamingCursor.transform.position = hit.transform.gameObject.transform.position + new Vector3(0, 0.1f, 0);
            }

            if (Input.GetMouseButtonDown(0)) {
                roamingCursor.SetActive(false);
                Create(type, roamingCursor.transform.position);
                type = "";
            }
        }
    }

    void OnGUI() {
        if (roamingCursor.activeInHierarchy) {
            GUIExtended.Label(new Rect((Screen.width - 100) / 2, Screen.height * 0.75f, 100, 100), "Choose Location", new GUIStyle(GUI.skin.label), Color.black, Color.white);
        }
    }

    public void Create(string type, Vector3 position) {
        GameObject species = Instantiate(Resources.Load("Prefabs/" + type)) as GameObject;
        species.transform.position = position + new Vector3(0, 0.1f, 0);
        species.transform.GetChild(0).localEulerAngles = new Vector3(40, 180, 0);
    }
}

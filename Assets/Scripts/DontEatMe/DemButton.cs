using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    //Button prefab
    public GameObject buttonPrefab;

    //Canvas Object
    //public GameObject canvasObject;
    public GameObject mainUIObject;

    //Panel Object
    public GameObject panelObject;


    //width of button
    private float xSize;

    //height of button
    private float ySize;

    //id of button
    private int buttonId;

    private float height;
    private float width;


    // Loading Resources and initialize button size
    void Awake()
    {
        buttonPrefab = Resources.Load<GameObject>("DontEatMe/Prefabs/Button");

        //canvasObject = GameObject.Find("Canvas");
        panelObject = GameObject.Find("Canvas/Panel");
        mainUIObject = GameObject.Find("Canvas/mainUI");
        //panelObject = GameObject.Find("Canvas/mainUI/Panel");

        xSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        ySize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        buttonId = 0;

        height = Screen.height;
        width = Screen.width;
    }


    // Instantiate and place the buttons on the scene
    public GameObject CreateButton(float xPos, float yPos, string name)
    {
        GameObject button = Instantiate(buttonPrefab) as GameObject;
        button.name = name;
        buttonId += 1;

        //button.transform.SetParent(canvasObject.transform);
        button.transform.SetParent(mainUIObject.transform);

        // Set the position of the button

        button.GetComponent<RectTransform>().sizeDelta = new Vector2(xSize, ySize);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
        button.GetComponent<RectTransform>().pivot = new Vector2(0, 1);


        return button;
    }


    // Create image for the button
    public void SetButtonImage(DemAnimalFactory species, GameObject button)
    {
        GameObject buttonImage = new GameObject(species.GetName());
        buttonImage.transform.SetParent(button.transform);

        // Set the layer to UI layer
        buttonImage.layer = 5;

        // Sets image and its position on the button 
        buttonImage.AddComponent<Image>();
        buttonImage.GetComponent<Image>().sprite = species.GetImage();
        buttonImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        buttonImage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        buttonImage.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        buttonImage.GetComponent<RectTransform>().offsetMax = new Vector2(-7, -7);
        buttonImage.GetComponent<RectTransform>().offsetMin = new Vector2(7, 7);
    }




    // Create text for the button
    public void SetButtonText(GameObject button, string text)
    {
        GameObject buttonText = new GameObject("buttonTxt");
        buttonText.transform.SetParent(button.transform);

        // Set the layer to UI layer
        buttonText.layer = 5;

        //Set text and its position on the button
        buttonText.AddComponent<Text>();
        buttonText.GetComponent<Text>().font = Resources.Load<Font>("Fonts/Chalkboard");
		buttonText.GetComponent<Text> ().fontSize = (int)(Screen.width/42);
        buttonText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        buttonText.GetComponent<Text>().color = Color.white;
        buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
		buttonText.GetComponent<RectTransform> ().sizeDelta = button.GetComponent<RectTransform> ().sizeDelta;

        buttonText.GetComponent<Text>().text = text;
    }


    // Change the size of the button
    public void setSize(float newSizeX, float newSizeY)
    {
        xSize = newSizeX;
        ySize = newSizeY;
    }


    // Return the button id
    public int getButtonId()
    {
        return buttonId;
    }


    // Return the width of the button
    public float getXSize()
    {
        return xSize;
    }


    // Return the height of the button
    public float getYSize()
    {
        return ySize;
    }


    // When mouse cursor is over an object
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set the tooltip as active when mouse cursor is over a button
        panelObject.SetActive(true);
        panelObject.transform.position = new Vector3(Input.mousePosition.x + 180, Input.mousePosition.y);
        if (this.gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
      panelObject.transform.GetChild (1).GetComponent<Text> ().text = 
        this.gameObject.transform.GetChild (0).gameObject.name;
            //panelObject.transform.GetChild(2).GetComponent<Text>().text = ;
      panelObject.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = "Biomass : " +
        SpeciesConstants.Biomass (this.gameObject.transform.GetChild (0).gameObject.name).ToString();
        }

        else
        {
            //panelObject.transform.GetChild(1).GetComponent<Text>().text = this.gameObject.transform.GetChild(1).gameObject.name;
            panelObject.transform.GetChild(1).GetComponent<Text>().text = this.gameObject.transform.GetChild(1).gameObject.name;

      panelObject.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text =  "Biomass : " +
                SpeciesConstants.Biomass (this.gameObject.transform.GetChild(1).gameObject.name).ToString();
        }

        panelObject.transform.GetChild(1).gameObject.SetActive(true);
    }


    // When cursor moves away from an object
    public void OnPointerExit(PointerEventData eventData)
    {
        // Sets tooltip as inactive
        panelObject.SetActive(false);
    }


}


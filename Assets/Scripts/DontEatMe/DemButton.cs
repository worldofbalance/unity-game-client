using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    //Button prefab
    public GameObject buttonPrefab;

    //Canvas Object
    public GameObject canvasObject;

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
        canvasObject = GameObject.Find("Canvas");
        panelObject = GameObject.Find("Canvas/Panel");

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

        button.transform.SetParent(canvasObject.transform);

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
        buttonImage.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1);
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
        buttonText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        buttonText.GetComponent<Text>().color = Color.black;
        buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        buttonText.GetComponent<Text>().text = text;

    }


    // Change the size of the button
    public void setSize(float newSizeX, float newSizeY)
    {
        //buttonPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(newSizeX, newSizeY);
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


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == this.gameObject)
        {
            if (this.gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                panelObject.transform.GetChild(0).GetComponent<Image>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
                panelObject.transform.GetChild(1).GetComponent<Text>().text = this.gameObject.transform.GetChild(0).gameObject.name;
                //panelObject.transform.GetChild(2).GetComponent<Text>().text = ;
            }

            else
            {
                panelObject.transform.GetChild(0).GetComponent<Image>().sprite = this.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;
                panelObject.transform.GetChild(1).GetComponent<Text>().text = this.gameObject.transform.GetChild(1).gameObject.name;
            }

                panelObject.transform.GetChild(0).gameObject.SetActive(true);
                panelObject.transform.GetChild(1).gameObject.SetActive(true);
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panelObject.transform.GetChild(0).gameObject.SetActive(false);
        panelObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    
}


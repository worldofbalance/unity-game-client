using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DemButton : MonoBehaviour//, IPointerClickHandler
{


    //Button prefab
    public GameObject buttonPrefab;

    //Canvas Object
    public GameObject canvasObject;


    //width of button
    private float xSize;

    //height of button
    private float ySize;

    //id of button
    private int buttonId;


    // Loading Resources and initialize button size
    void Awake()
    {
        buttonPrefab = Resources.Load<GameObject>("DontEatMe/Prefabs/Button");
        canvasObject = GameObject.Find("Canvas");

        xSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        ySize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        buttonId = 0;

    }


    // Instantiate and place the buttons on the scene
    public GameObject CreateButton(float xPos, float yPos)
    {
        GameObject button = Instantiate(buttonPrefab) as GameObject;
        button.name = buttonId.ToString();
        buttonId += 1;

        button.transform.SetParent(canvasObject.transform);

        // Set the position of the button
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);

        return button;
    }


    // Create image for the button
    public void MakeButtonImage(DemAnimalFactory species, GameObject button)
    {
        GameObject buttonImage = new GameObject("buttonImg");
        buttonImage.transform.SetParent(button.transform);

        // Set the layer to UI layer
        buttonImage.layer = 5;

        // Sets image and its position on the button 
        buttonImage.AddComponent<Image>();
        buttonImage.GetComponent<Image>().sprite = species.GetImage();
        buttonImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        buttonImage.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1);
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


}


using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DemButton : MonoBehaviour, IPointerClickHandler
{

    //Button prefab
    public GameObject buttonPrefab;

    //Canvas Object
    public GameObject canvasObject;

    //width of button
    private float xSize;

    //height of button
    private float ySize; 


    //Button properties
    public float SizeX { get; set; }

    public float SizeY { get; set; }


    // Loading Resources and initialize button size
    void Awake()
    {
        buttonPrefab = Resources.Load<GameObject>("DontEatMe/Prefabs/Button");
        canvasObject = GameObject.Find("Canvas");

        xSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        ySize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
    }


    // Instantiate and place the buttons on the scene
    public void MakeButton(int numberButtons, int type, float xPos, float yPos)
    {

        for (int i = 0; i < numberButtons; i++)
        {
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.name = "Button" + i;
            
            button.transform.SetParent(canvasObject.transform);

            // Set the position of the button
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0 - (yPos + i * (ySize-2)));

            // Attach this script to the button to detect button clicks
            button.AddComponent<DemButton>();

        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked " + gameObject.name);
    }


    // Change the size of the button
    public void setSize(float newSizeX, float newSizeY)
    {
        this.SizeX = newSizeX;
        this.SizeY = newSizeY;
    }

    

}


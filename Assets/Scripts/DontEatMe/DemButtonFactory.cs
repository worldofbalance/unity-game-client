using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography;

/**
    NOTES: the DemButtonFactory class needs an actual DemButton class; the current approach to button creation and
    manipulation follows a paradigm very close to pure procedural rather than object-oriented. For instance, instead of
    the Create method returning a new, say, DemButton object with its own respective accessors, mutators, etc., it
    returns more or less a "generic" GameObject. Such objects can really only be manipulated after creation by using
    C-like function calls to DemButtonFactory methods (for instance, "SetButtonText", "SetButtonImage", etc. must
    contain a "button" object as an argument).
    A suggestion to those who may be working on this in the future is to re-implement the DemButtonFactory class to
    be object-oriented, and create a custom "DemButton" class whose objects contain any and all methods currently in
    DemButtonFactory for which a "button" object must be passed as an argument.
    Although not completely necessary, it will help to simplify individual button creation and manipulation.
*/
public class DemButtonFactory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Button layer level
    public static int BUTTON_LAYER = 5;

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


    Color disabledColor = new Color32(120, 120, 120, 255); // Custom "disabled" button color
    Color highlightedColor = new Color32(218, 165, 32, 255); // Custom "highlighted" button color
    Color pressedColor = new Color32(173, 255, 47, 255); // Custom "pressed" button color

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
    public GameObject CreateButton (float xPos, float yPos, string name)
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

        // Set custom ColorBlock
        ColorBlock cb = button.GetComponent<Button>().colors;
        cb.disabledColor = disabledColor;
        cb.highlightedColor = highlightedColor;
        cb.pressedColor = pressedColor;
        button.GetComponent<Button>().colors = cb;

        return button;
    }

    /**
        Sets the normal color for a specified button.
        The normal color is defined by the Button component's ColorBlock.normalColor variable.

        @param  button  a GameObject representing a button
        @param  color   a new Color or Color32 object
    */
    public void SetButtonNormalColor (GameObject button, Color color)
    {
        ColorBlock cb = button.GetComponent<Button>().colors;
        cb.normalColor = color;
        button.GetComponent<Button>().colors = cb;
    }

    /**
        Sets the highlighted color for a specified button.
        The highlighted color is defined by the Button component's ColorBlock.highlightedColor variable.

        @param  button  a GameObject representing a button
        @param  color   a new Color or Color32 object
    */
    public void SetButtonHighlightedColor (GameObject button, Color color)
    {
        ColorBlock cb = button.GetComponent<Button>().colors;
        cb.highlightedColor = color;
        button.GetComponent<Button>().colors = cb;
    }


    // Create image for the button
    // TODO: consider swapping order of parameters to match SetButtonText and SetButtonIcon methods
    public void SetButtonImage(DemAnimalFactory species, GameObject button)
    {
        GameObject buttonImage = new GameObject(species.GetName());
        buttonImage.transform.SetParent(button.transform);

        // Set the layer to UI layer
        buttonImage.layer = DemButtonFactory.BUTTON_LAYER;

        // Sets image and its position on the button 
        buttonImage.AddComponent<Image>();
        buttonImage.GetComponent<Image>().sprite = species.GetImage();
        buttonImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        buttonImage.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        buttonImage.GetComponent<RectTransform>().anchorMax = Vector2.one;

        buttonImage.GetComponent<RectTransform>().offsetMax = new Vector2(-7, -7);
        buttonImage.GetComponent<RectTransform>().offsetMin = new Vector2(7, 7);

    }

    /** Sets the icon image for the button.
        This method is similar in outcome to SetButtonImage but allows for an arbitrary icon to be specified.

        @param  button      a GameObject representing a button
        @param  iconPath    path to the desired icon image relative to 'Assets/Resources/' (string)
                            Note: the image texture type must be set to "Sprite (2D and UI)"
    */
    public void SetButtonIcon (GameObject button, string iconPath, Color colorOverlay)
    {
        // Create icon image, set parent
        GameObject buttonIcon = new GameObject("buttonIcon");
        buttonIcon.transform.SetParent(button.transform);
        // Place on button layer
        buttonIcon.layer = DemButtonFactory.BUTTON_LAYER;

        // Position icon on button
        buttonIcon.AddComponent<Image>();
        buttonIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(iconPath);
        buttonIcon.GetComponent<Image>().color = colorOverlay;
        RectTransform rt = buttonIcon.GetComponent<RectTransform>();
        // Set anchor => justify left, keep icon proportionally correct
        rt.anchoredPosition = Vector2.left;
        rt.anchorMin = Vector2.zero;

        // Isolate button icon image
        Image icon = buttonIcon.GetComponent<Image>();

        Rect buttonRect = button.GetComponent<RectTransform>().rect;
        float iconWtH = icon.sprite.rect.width/icon.sprite.rect.height;
        float buttonWtH = buttonRect.width / buttonRect.height;




        Debug.Log("iconWtH for '" + iconPath + "' = " + iconWtH);
        //if (iconWtH > 1) rt.anchorMax = new Vector2(1/iconWtH, 1f);
        if (iconWtH > buttonWtH) rt.anchorMax = new Vector2
        (
            (buttonRect.width * icon.sprite.rect.height) / 
            (buttonRect.height * icon.sprite.rect.width),
            1f
        );
        else if (iconWtH < buttonWtH) rt.anchorMax = new Vector2
        (
            (buttonRect.height * icon.sprite.rect.width) / 
            (buttonRect.width * icon.sprite.rect.height),
            1f
        );
        else rt.anchorMax = new Vector2(1/buttonWtH, 1f);

        // Set offset
        rt.offsetMin = new Vector2(7, 7);
        rt.offsetMax = new Vector2(-7, -7);
    }


    // Create text for the button
    public void SetButtonText(GameObject button, string text)
    {
        GameObject buttonText = new GameObject("buttonTxt");
        buttonText.transform.SetParent(button.transform);

        // Set the layer to UI layer
        buttonText.layer = DemButtonFactory.BUTTON_LAYER;

        //Set text and its position on the button
        buttonText.AddComponent<Text>();
        buttonText.GetComponent<Text>().font = Resources.Load<Font>("Fonts/Dresden Elektronik");
		buttonText.GetComponent<Text> ().fontSize = (int)(Screen.width/45);
        buttonText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        buttonText.GetComponent<Text>().color = Color.white;
        buttonText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		buttonText.GetComponent<RectTransform> ().sizeDelta = button.GetComponent<RectTransform>().sizeDelta;
        // Apply button text
        buttonText.GetComponent<Text>().text = text;
    }

    /**
        Sets the button text color.

        @param  button  a GameObject representing a button
        @param  color   a Color object
    */
    public void SetButtonTextColor (GameObject button, Color color)
    {
        //button.GetComponent<Text>().color = color;
        button.transform.GetComponentInChildren<Text>().color = color;
    }


    /**
        Sets the button text font size.

        @param  button  a GameObject representing a button
        @param size font size in points (int)
    */
    public void SetButtonTextFontSize (GameObject button, int size)
    {
        button.GetComponentInChildren<Text>().fontSize = size > 0 ? size : 1; 
    }

    /* TODO:    this tends to be confusing: two "setSize" methods, one for the actual DemButtonFactory instance for 
                subsequent button creation, and the other for resizing an initialized button object.
                Consider a name change for one of them (e.g. "setCreationSize" or "setButtonSize", etc.)
    */
    // Change the size of the button
    public void setSize(float newSizeX, float newSizeY)
    {
        xSize = newSizeX;
        ySize = newSizeY;
    }

    public void setSize (GameObject button, float newSizeX, float newSizeY)
    {
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(newSizeX, newSizeY);
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


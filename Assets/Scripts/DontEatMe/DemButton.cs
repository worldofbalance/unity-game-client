using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DemButton : MonoBehaviour
{
    /* DATA MEMBERS */

    // Main components
    public GameObject mainUIObject; // Main UI canvas
    public GameObject panelObject; // Panel object for tooltips
    public GameObject buttonPrefab; // Button prefab (background)
    public GameObject buttonImage; // Button image
    private int buttonId; // Unique button ID
    public int ButtonID // Define property accessor
    {
        get { return buttonId; }
    }

    // Dimensional components
    private float width; // Button width in pixels
    public float Width // Define property accessor and mutator
    { 
        get { return width; }
        set { width = (value > 0 ? value : width); }
    }
    private float height; // Button height in pixels
    public float Height // Define property accessor and mutator
    {
        get { return height; }
        set { height = (value > 0 ? value : height); }
    }

    // Font components
    private Font buttonFont; // Common font used by all buttons
    private string fontName; // String of font name used
    private string fontDirectory; // Relative directory of font asset
    public int fontSize; // Font size for button text

    /* METHODS */

    /**
        Initializes components and loads all resources.
    */
    void Awake ()
    {
        // Initialize main components
        buttonPrefab = Resources.Load<GameObject>("DontEatMe/Prefabs/Button");
        panelObject = GameObject.Find("Canvas/Panel");
        mainUIObject = GameObject.Find("Canvas/mainUI");

        // Initialize secondary components
        width = buttonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        height = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        buttonId = 0;
        buttonImage = null;

        // Initialize font properties
        fontName = "Chalkboard";
        fontDirectory = "Fonts";
        buttonFont = Resources.Load<Font>(fontDirectory + "/" + fontName);
        fontSize = (int)(Screen.width / 42);
    }

    /**
        Instantiates a button and places it on the scene at a specified position.
        The position is determined by both an X- and Y-offset in pixels, with (0,0) being the upper left corner.

        @param  xPos    x-axis position relative to top left of screen
        @param  yPos    y-axis position relative to top left of screen
        @param  name    the button name (for reference only)

        @return a GameObject representing a button
    */
    public GameObject CreateButton (float xPos, float yPos, string name)
    {
        GameObject button = Instantiate(buttonPrefab) as GameObject;
        button.name = name;
        buttonId += 1;

        button.transform.SetParent(mainUIObject.transform);

        // Set the position of the button
        RectTransform btnTransform = button.GetComponent<RectTransform>();
        btnTransform.sizeDelta = new Vector2(width, height);
        btnTransform.anchoredPosition = new Vector2(xPos, yPos);
        btnTransform.pivot = Vector2.up;

        return button;
    }

    /**
        Sets a button's displayed image.
        This version utilizes an associated image specified by a DemAnimalFactory object, thus limiting the type of
        image to a plant, prey, or predator.

        @param  species a DemAnimalFactory object
        @param  button  a button in the form of a GameObject returned by a call to CreateButton
    */
    public void SetButtonImage (DemAnimalFactory species, GameObject button)
    {
        buttonImage = new GameObject(species.GetName());
        buttonImage.transform.SetParent(button.transform);

        // Set the layer to UI layer
        buttonImage.layer = 5;

        // Sets image and its position on the button
        buttonImage.AddComponent<Image>().sprite = species.GetImage();
        RectTransform imgTransform = buttonImage.GetComponent<RectTransform>();
        imgTransform.anchoredPosition = Vector2.zero;
        imgTransform.anchorMin = Vector2.zero;
        imgTransform.anchorMax = Vector2.one;
        imgTransform.offsetMax = new Vector2(-7, -7);
        imgTransform.offsetMin = new Vector2(7, 7);
    }

    /**
        Sets a button's displayed text.

        @param  button  a button in the form of a GameObject returned by a call to CreateButton
        @param  text    a string containing the button's text
    */
    public void SetButtonText (GameObject button, string text)
    {
        // Create text object and set the parent button
        GameObject textObject = new GameObject("buttonTxt");
        textObject.transform.SetParent(button.transform);

        // Set the layer to UI layer
        textObject.layer = 5;

        // Add text component to button object and set its properties
        Text buttonText = textObject.AddComponent<Text>();
        buttonText.font = buttonFont;
        buttonText.fontSize = fontSize;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        buttonText.text = text;
        // Set text anchor and resize to button object
        RectTransform textTransform = textObject.GetComponent<RectTransform>();
        textTransform.anchoredPosition = Vector2.zero;
        textTransform.sizeDelta = button.GetComponent<RectTransform>().sizeDelta;
    }

    /**
        Returns the button's Font object.

        @return a Font object
    */
    public Font GetFont ()
    {
        return buttonFont;
    }

    /**
        Returns the name of the button's font.

        @return a string
    */
    public string GetFontName ()
    {
        return fontName;
    }

    /**
        Returns the relative directory in which the font resides.

        @return a string
    */
    public string GetFontDirectory ()
    {
        return fontDirectory;
    }

    /**
        Sets the button font.
        Both the font name and font object are updated, as well as the font directory if specified.
        If a font fails to load properly, the current values will remain unchanged.

        @param  name    a string representing a font name

        @return true if font set successfully, false otherwise
    */
    public bool SetFont (string name)
    {
        if ((buttonFont = Resources.Load<Font>(fontDirectory + "/" + name)) != null)
        {
            fontName = name;
            return true;
        } 
        else
            buttonFont = Resources.Load<Font>(fontDirectory + "/" + fontName);

        return false;
    }

    /**
        Sets the directory in which the button font resides.
        Note that the directory name specified is by default relative to the root "Assets/Resources/"; thus, specifying
        a new directory "myFonts" will evaluate to "Assets/Resources/myFonts".

        @param  dir the name of the new directory

        @return true if the directory set successfully, false otherwise (e.g. doesn't exist)
    */
    public bool SetFontDirectory (string dir)
    {
        if (System.IO.Directory.Exists("Assets/Resources/" + dir))
        {
            fontDirectory = dir;
            return true;
        }
        else
            return false;
    }
}
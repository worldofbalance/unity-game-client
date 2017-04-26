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
    public GameObject buttonImage;

    // Main objects
    public GameObject mainObject;
    public GameObject mainUIObject;

    //Panel Object
    public GameObject panelObject;

    // Stats popup object (supersedes panelObject)
    private GameObject buttonStatsBox;
    private Text statsSpeciesName;
    private Text statsBiomassTier;
    private Text statsBiomassText;
    private Text statsMetabolismText;
    private Image statsSpeciesIcon;
    private Image statsBiomassIcon;
    private Image statsMetabolismIcon;
    // Stats popup easing components
    private Color statsBoxBaseColor;
    private Vector3 statsBoxBaseScale;
    private Vector3 statsBoxBaseRotation;
    private IEnumerator easeEnumerator;
    private Coroutine easeCoroutine;


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

    /**
        Called on script load as a precursor to all other methods.
        Use for inializing objects.
    */
    void Awake()
    {
        buttonPrefab = Resources.Load<GameObject>("DontEatMe/Prefabs/Button");

        //canvasObject = GameObject.Find("Canvas");
        panelObject = GameObject.Find("Canvas/Panel");
        //panelObject.SetActive(false);
        mainObject = GameObject.Find("MainObject");
        mainUIObject = GameObject.Find("Canvas/mainUI");
        //panelObject = GameObject.Find("Canvas/mainUI/Panel");

        // Initialize stats popup and its components
        buttonStatsBox = GameObject.Find("Canvas/BuildButtonStatsBox");
        statsSpeciesName = GameObject.Find("Canvas/BuildButtonStatsBox/SpeciesName").GetComponent<Text>();
        statsBiomassTier = GameObject.Find("Canvas/BuildButtonStatsBox/BiomassTier").GetComponent<Text>();
        statsBiomassText = GameObject.Find("Canvas/BuildButtonStatsBox/BiomassText").GetComponent<Text>();
        statsMetabolismText = GameObject.Find("Canvas/BuildButtonStatsBox/MetabolismText").GetComponent<Text>();
        statsSpeciesIcon = GameObject.Find("Canvas/BuildButtonStatsBox/StatsIcon").GetComponent<Image>();
        statsBiomassIcon = GameObject.Find("Canvas/BuildButtonStatsBox/BiomassIcon").GetComponent<Image>();
        statsMetabolismIcon = GameObject.Find("Canvas/BuildButtonStatsBox/MetabolismIcon").GetComponent<Image>();
        // Initialize easing components (colors, rotations)
        statsBoxBaseColor = buttonStatsBox.GetComponent<Image>().color;
        statsBoxBaseScale = buttonStatsBox.transform.localScale;
        statsBoxBaseRotation = buttonStatsBox.transform.localEulerAngles;
        easeEnumerator = null;
        easeCoroutine = null;

        xSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        ySize = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        buttonId = 0;

        height = Screen.height;
        width = Screen.width;
    }

    /**
        Called after Awake on script start.
        Use to set properties of object initialized in Awake.
    */
    void Start ()
    {
        buttonStatsBox.SetActive(false);
    }


    // Instantiate and place the buttons on the scene
    public GameObject CreateButton (float xPos, float yPos, string name)
    {
        //GameObject button = Instantiate(buttonPrefab) as GameObject;
        GameObject button = Instantiate<GameObject>(buttonPrefab);

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

    /**
        Sets the pressed color for a specified button.
        The highlighted color is defined by the Button component's ColorBlock.pressedColor variable.

        @param  button  a GameObject representing a button
        @param  color   a new Color or Color32 object
    */
    public void SetButtonPressedColor (GameObject button, Color color)
    {
        ColorBlock cb = button.GetComponent<Button>().colors;
        cb.pressedColor = color;
        button.GetComponent<Button>().colors = cb;
    }

    /**
        Sets the disabled color for a specified button.
        The highlighted color is defined by the Button component's ColorBlock.disabledColor variable.

        @param  button  a GameObject representing a button
        @param  color   a new Color or Color32 object
    */
    public void SetButtonDisabledColor (GameObject button, Color color)
    {
        ColorBlock cb = button.GetComponent<Button>().colors;
        cb.disabledColor = color;
        button.GetComponent<Button>().colors = cb;
    }

    // Create image for the button
    // TODO: consider swapping order of parameters to match SetButtonText and SetButtonIcon methods
    public void SetButtonImage (DemAnimalFactory species, GameObject button)
    {
        buttonImage = new GameObject(species.GetName());
        buttonImage.transform.SetParent(button.transform);

        // Set the layer to UI layer
        buttonImage.layer = DemButtonFactory.BUTTON_LAYER;

        // Sets image and its position on the button 
        buttonImage.AddComponent<Image>();
        buttonImage.GetComponent<Image>().sprite = species.GetImage();
        buttonImage.GetComponent<Image>().preserveAspect = true;
        buttonImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        buttonImage.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        buttonImage.GetComponent<RectTransform>().anchorMax = Vector2.one;

        buttonImage.GetComponent<RectTransform>().offsetMax = new Vector2(-7, -7);
        buttonImage.GetComponent<RectTransform>().offsetMin = new Vector2(7, 7);
    }

    /** Sets the icon image for the button.
        This method is similar in outcome to SetButtonImage but allows for an arbitrary icon to be specified.

        @param  button          a GameObject representing a button
        @param  iconPath        path to the desired icon image relative to 'Assets/Resources/' (string)
                                Note: the image texture type must be set to "Sprite (2D and UI)"
        @param  colorOverlay    a Color object to overlay the icon
    */
    public void SetButtonIcon (GameObject button, string iconPath, Color colorOverlay, Vector2 anchor)
    {
        // Create icon image, set parent
        GameObject buttonIcon = new GameObject("buttonIcon");
        buttonIcon.transform.SetParent(button.transform);
        // Place on button (UI) layer
        buttonIcon.layer = DemButtonFactory.BUTTON_LAYER;

        // Position icon on button
        buttonIcon.AddComponent<Image>();
        buttonIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(iconPath);
        buttonIcon.GetComponent<Image>().color = colorOverlay;
        RectTransform rt = buttonIcon.GetComponent<RectTransform>();
        // Set anchor
        rt.anchoredPosition = anchor;
        rt.anchorMin = Vector2.zero;

        // Isolate button icon image
        Image icon = buttonIcon.GetComponent<Image>();

        // Perform Rect transformations
        Rect buttonRect = button.GetComponent<RectTransform>().rect;
        
        float iconWtH = icon.sprite.rect.width/icon.sprite.rect.height;
        float buttonWtH = buttonRect.width / buttonRect.height;

        // Maintain aspet ratio: icon width-to-height > button width-to-height
        if (iconWtH > buttonWtH) rt.anchorMax = new Vector2
        (
            (buttonRect.width * icon.sprite.rect.height) / 
            (buttonRect.height * icon.sprite.rect.width),
            1f
        );
        // Maintain aspet ratio: icon width-to-height < button width-to-height
        else if (iconWtH < buttonWtH) rt.anchorMax = new Vector2
        (
            (buttonRect.height * icon.sprite.rect.width) / 
            (buttonRect.width * icon.sprite.rect.height),
            1f
        );
        // Maintain aspet ratio: icon width-to-height == button width-to-height
        else rt.anchorMax = new Vector2(1/buttonWtH, 1f);

        // Set screen offset
        rt.offsetMin = new Vector2(7, 7);
        rt.offsetMax = new Vector2(-7, -7);
    }


    // Create text for the button
    public void SetButtonText (GameObject button, string text, bool relativeToParent = false)
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
        string speciesName = this.gameObject.transform.GetChild(0).gameObject.name;
        // Parse associated species type, return if neither plant nor prey
        int speciesType = SpeciesConstants.SpeciesType(speciesName); // Parse species type (0 = plant, 1 = prey)
        if (speciesType != 0 && speciesType != 1) return;

        // Set species name and icon
        statsSpeciesName.text = speciesName;

        // Set supplementary data
        // For Plant buttons:
        if (speciesType == 0 && mainObject.GetComponent<BuildMenu>().PlantMenuActive())
        {
            // Set main icon
            statsSpeciesIcon.sprite = Resources.Load<Sprite>("DontEatMe/Sprites/plant_icon");
            statsSpeciesIcon.color = mainObject.GetComponent<BuildMenu>().plantIconColor; //new Color32(46, 139, 87, 255);
            // Set Biomass tier level, text, and icon color
            statsBiomassTier.text = "1";
            statsBiomassText.text = SpeciesConstants.Biomass(speciesName).ToString();
            statsBiomassIcon.color = new Color32(146, 200, 120, 203);
            // Disable metabolism components
            statsMetabolismText.gameObject.SetActive(false);
            statsMetabolismIcon.gameObject.SetActive(false);

            // Set buttonStatsBox active and position it over the button
            buttonStatsBox.SetActive(false);
            buttonStatsBox.transform.position = this.gameObject.transform.position;
            easeEnumerator = EaseInButtonStatsBox();
            easeCoroutine = StartCoroutine(easeEnumerator);
            //buttonStatsBox.SetActive(true);
        }
        // For Prey buttons
        else if (speciesType == 1 && !mainObject.GetComponent<BuildMenu>().PlantMenuActive())
        {
            // Set main icon
            statsSpeciesIcon.sprite = Resources.Load<Sprite>("DontEatMe/Sprites/prey_icon");
            statsSpeciesIcon.color = mainObject.GetComponent<BuildMenu>().preyIconColor; //new Color32(210, 105, 30, 255);
            // Set Biomass tier level, text, and icon color
            statsBiomassTier.text = "2";
            statsBiomassText.text = SpeciesConstants.Biomass(speciesName).ToString();
            statsBiomassIcon.color = new Color32(200, 175, 120, 203);
            // Enable and set metabolism components
            statsMetabolismText.gameObject.SetActive(true);
            statsMetabolismIcon.gameObject.SetActive(true);
            statsMetabolismText.text = SpeciesConstants.Metabolism(speciesName).ToString();

            // Set buttonStatsBox active and position it over the button
            buttonStatsBox.SetActive(false);
            buttonStatsBox.transform.position = this.gameObject.transform.position;
            easeEnumerator = EaseInButtonStatsBox();
            easeCoroutine = StartCoroutine(easeEnumerator);
        }
    }


    // When cursor moves away from an object
    public void OnPointerExit(PointerEventData eventData)
    {
        // Sets tooltip as inactive
        if (easeCoroutine != null) StopCoroutine(easeCoroutine);
        buttonStatsBox.SetActive(false);
    }

    /**
        Eases in the buttonStatsBox object.
        The color alpha channel and y-axis rotation are modified each iteration to create the effect.
        
        @param  easing  a floating point value denoting the step size of each iteration
    */
    IEnumerator EaseInButtonStatsBox (float easing = 0.15f)
    {
        // Delay one tenth second to start
        yield return new WaitForSeconds(0.1f);
        // Set statsBox active
        buttonStatsBox.SetActive(true);
        // Set final states
        Vector3 finalRotation = statsBoxBaseRotation;
        float finalAlpha = statsBoxBaseColor.a;

        // Initialize transitional variables
        Vector3 statRotation = statsBoxBaseRotation;
        Color statsColor = buttonStatsBox.GetComponent<Image>().color;
        float rotation = -30f;
        float alpha = 0;
        statRotation.x = rotation;
        statsColor.a = alpha;
        buttonStatsBox.transform.localEulerAngles = statRotation;
        buttonStatsBox.GetComponent<Image>().color = statsColor;

        // Ease until threshold reached
        while (rotation < finalRotation.x - 0.1f)
        {
            // Update transitional variables
            rotation += (finalRotation.x - rotation) * easing;
            statRotation.x = rotation;
            alpha += (finalAlpha - alpha) * easing;
            statsColor.a = alpha;
            // Apply changes to statsBox
            buttonStatsBox.transform.localEulerAngles = statRotation;
            buttonStatsBox.GetComponent<Image>().color = statsColor;
            // Stall for next iteration
            yield return new WaitForSeconds(0.01f);
        }
        // Post-threshold states set to base
        buttonStatsBox.transform.localEulerAngles = statsBoxBaseRotation;
        buttonStatsBox.GetComponent<Image>().color = statsBoxBaseColor;
    }
}
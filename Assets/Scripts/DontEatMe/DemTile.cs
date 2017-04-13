using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Remoting;
using System.IO;
using System;

/**
    The DemTile class represents a single tile of the Don't Eat Me grid board.
*/
public class DemTile : MonoBehaviour
{
  	public int idX; // X-coord for DemTile
  	public int idY; // Y-coord for DemTile
    public Color currentColor; // Current tile color
    public Color rangeColor; // Color for plant range indicator pulse

    public bool available;  // True if tile available for building

    private Vector3 center;

  	public GameObject resident; // Resident object (HerbivoreObject or PlantObject) placed on tile
    private GameObject nextPredator;
    private BuildMenu buildMenu;
    public  GameObject mainObject;
    public GameObject panelObject;
    private DemMain main;
    private DemTurnSystem turnSystem;

    // StatsBox objects (static --> no two instances required)
    public static GameObject statsBox;                 // Parent object
    public static GameObject nameStat;                 // Species name
    public static GameObject iconStat;                 // Species type icon
    public static GameObject trait1Stat;               // Species trait (primary)
    public static GameObject trait2Stat;               // Species trait (secondary)
    public static GameObject traitIcon1;               // Species trait icon (primary)
    public static GameObject traitIcon2;               // Species trait icon (secondary)
    public static GameObject infoPopup;                // Info popup object
    public static Color statsBoxBaseColor;             // Base color for the statsBox object
    public static Vector3 statsBoxBaseScale;           // Base localScale for the statsBox object
    public static Vector3 statsBoxBaseRotation;        // Base localEulersAngles for the statsBox object 
    public static ColorBlock infoPopupBaseColors;      // Base ColorBlock for the infoPopup Button

    // InfoBox objects
    public static GameObject infoBox;                  // Parent object
    public static GameObject infoSpeciesName;          // Species name
    public static GameObject infoSpeciesType;          // Species type (plant/prey/predator)
    public static GameObject infoSpeciesTypeIcon;      // " " icon
    public static GameObject infoBiomass;              // Species biomass
    public static GameObject infoBiomassIcon;          // " " icon
    public static GameObject infoSpeciesTrait1;        // Species trait (primary)
    public static GameObject infoSpeciesTrait2;        // Species trait (secondary)
    public static GameObject infoSpeciesTrait1Icon;    // " " (primary) icon
    public static GameObject infoSpeciesTrait2Icon;    // " " (secondary) icon
    public static GameObject infoPredPreyHeader;       // Predator / prey list header
    public static GameObject infoPredPreyList;         // " " list grid
    public static GameObject infoPredPreyIcon;         // " " header icon
    public static GameObject infoPredPreyDivider;      // " " header divider
    public static GameObject infoLore;                 // Species lore
    public static GameObject infoCloseButton;          // Close button for infoBox

    // InfoLegendTooltip objects
    public static GameObject infoLegendTooltip;        // Parent object
    public static GameObject infoLegendName;           // Attribute name
    public static GameObject infoLegendIcon;           // Attribute icon
    public static GameObject infoLegendLore;           // Attribute lore

    public static Color infoPopupBaseNormalColor;      // Base Color for the infoPopup's Button normal state
    public static Color infoPopupBaseHighlightColor;   // Base Color for the infoPopup's Button highlighted state

    private IEnumerator easeEnumerator;     // Enumerator for statsBox easing routine
    private Coroutine easeCoroutine;        // Easing coroutine for the statsBox object

    private IEnumerator pulse; // Called as a coroutine to start/stop pulsing of tile colors
    private bool hasPulse; // Denotes if tile has currently active pulse
    private const float defaultPulseFrequency = 0.625f; // Default pulse frequency
    private const float defaultPulseFactor = 0.1f; // Default value for synchronizing pulse modulation with frequency
    public float DefaultPulseFrequency { get { return defaultPulseFrequency; } } // Public accessor

    // Values for defining a restore point for pulse
    private Color restoreColor1;
    private Color restoreColor2;
    private float restoreColorFrequency;

    private static int pulseTick; // Synchronizes each pulse
    private static float pulseFactor; // Synchronizes pulse modulation with frequency
    private static float masterPulseFrequency; // Master pulse frequency
    private static bool masterPulseEnabled; // True if master pulse clock enabled

    private DemTile[] allTiles;

    /**
        Initializes the DemTile object's properties prior to game load.
    */
    void Awake ()
    {
        // Initialize main components
        mainObject = GameObject.Find ("MainObject");
        panelObject = GameObject.Find("Canvas/Panel");
        buildMenu = mainObject.GetComponent<BuildMenu> ();
        main = mainObject.GetComponent<DemMain> ();
        turnSystem = mainObject.GetComponent<DemTurnSystem> ();

        // Initialize StatsBox objects
        statsBox = GameObject.Find("Canvas/StatsBox");
        nameStat = GameObject.Find("Canvas/StatsBox/SpeciesName");
        iconStat = GameObject.Find("Canvas/StatsBox/StatsIcon");
        trait1Stat = GameObject.Find("Canvas/StatsBox/SpeciesTrait1");
        trait2Stat = GameObject.Find("Canvas/StatsBox/SpeciesTrait2");
        traitIcon1 = GameObject.Find("Canvas/StatsBox/TraitIcon1");
        traitIcon2 = GameObject.Find("Canvas/StatsBox/TraitIcon2");
        infoPopup = GameObject.Find("Canvas/StatsBox/InfoPopup");
        // ...InfoBox objects
        infoBox = GameObject.Find("Canvas/InfoBox");
        infoSpeciesName = GameObject.Find("Canvas/InfoBox/InfoSpeciesName");
        infoSpeciesType = GameObject.Find("Canvas/InfoBox/InfoSpeciesType");
        infoSpeciesTypeIcon= GameObject.Find("Canvas/InfoBox/InfoSpeciesTypeIcon");
        infoBiomass = GameObject.Find("Canvas/InfoBox/InfoBiomass");
        infoBiomassIcon = GameObject.Find("Canvas/InfoBox/InfoBiomassIcon");
        infoSpeciesTrait1 = GameObject.Find("Canvas/InfoBox/InfoSpeciesTrait1");
        infoSpeciesTrait2 = GameObject.Find("Canvas/InfoBox/InfoSpeciesTrait2");
        infoSpeciesTrait1Icon = GameObject.Find("Canvas/InfoBox/InfoSpeciesTrait1Icon");
        infoSpeciesTrait2Icon = GameObject.Find("Canvas/InfoBox/InfoSpeciesTrait2Icon");
        infoPredPreyHeader = GameObject.Find("Canvas/InfoBox/InfoPredPreyHeader");
        infoPredPreyList = GameObject.Find("Canvas/InfoBox/InfoPredPreyList");
        infoPredPreyIcon = GameObject.Find("Canvas/InfoBox/InfoPredPreyIcon");
        infoPredPreyDivider = GameObject.Find("Canvas/InfoBox/InfoPredPreyDivider");
        infoLore = GameObject.Find("Canvas/InfoBox/InfoLore");
        infoCloseButton = GameObject.Find("Canvas/InfoBox/InfoCloseButton");
        // ...Info legend tooltip objects
        infoLegendTooltip = GameObject.Find("Canvas/InfoLegendTooltip");
        infoLegendName = GameObject.Find("Canvas/InfoLegendTooltip/InfoLegendName");
        infoLegendIcon = GameObject.Find("Canvas/InfoLegendTooltip/InfoLegendIcon");
        infoLegendLore = GameObject.Find("Canvas/InfoLegendTooltip/InfoLegendLore");
        // ...Other objects
        statsBoxBaseColor = statsBox.GetComponent<Image>().color;
        statsBoxBaseScale = statsBox.transform.localScale;
        statsBoxBaseRotation = statsBox.transform.localEulerAngles;
        infoPopupBaseColors = infoPopup.GetComponent<Button>().colors;
        infoPopupBaseNormalColor = infoPopup.GetComponent<Button>().colors.normalColor;
        infoPopupBaseHighlightColor = infoPopup.GetComponent<Button>().colors.highlightedColor;
        easeEnumerator = null;
        easeCoroutine = null;

        // Initialize resident
        resident = null;

        // Initialize pulse variables
        hasPulse = false;
        pulseTick = 0;
        pulseFactor = defaultPulseFactor;
        masterPulseFrequency = defaultPulseFrequency;
        rangeColor = new Color(40f/255f, 170f/255f, 220f/255f);

        // Initialize additional components
        center = this.GetComponent<Renderer>().bounds.center;
        currentColor = Color.white;

        // Grab all tiles
        allTiles = FindObjectsOfType<DemTile>();
    }

    /**
        Sets up the game for play once all variables have been initialized by Awake.
    */
    void Start ()
    {
        // Set initial active states
        nameStat.SetActive(true);
        iconStat.SetActive(true);
        trait1Stat.SetActive(false);
        traitIcon1.SetActive(false);
        trait2Stat.SetActive(false);
        traitIcon2.SetActive(false);
        infoBox.SetActive(false);
        infoSpeciesName.SetActive(true);
        infoSpeciesType.SetActive(true);
        infoSpeciesTypeIcon.SetActive(true);
        infoSpeciesTrait1.SetActive(true);
        infoSpeciesTrait2.SetActive(true);
        infoSpeciesTrait1Icon.SetActive(true);
        infoSpeciesTrait2Icon.SetActive(true);

        // Parse X and Y coords from name
        // Name format = "X,Y", so X is stored @ name[0] and Y @ name[2]
        // The char value for '0' starts at 0x30, subtract this to parse numeric value
        // NOTE: this assumes that X and Y values remain within the range [0,9]
        idX = this.name[0] - 0x30; 
        idY = this.name[2] - 0x30;

        // Set z-offset for center
        center.z = -1.5f;

        // Add OnPointerEnter and OnPointerExit events for all infoBox icons (for use with infoLegendTooltip)
        foreach (Image image in infoBox.GetComponentsInChildren<Image>())
        {
            // Add event triggers to show tooltip
            EventTrigger showTrigger = image.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry showEntry = new EventTrigger.Entry();
            showEntry.eventID = EventTriggerType.PointerEnter;
            showEntry.callback.AddListener((data) => { ShowInfoLegend((PointerEventData)data); });
            showTrigger.triggers.Add(showEntry);

            // Add event triggers to hide tooltip
            EventTrigger hideTrigger = image.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry hideEntry = new EventTrigger.Entry();
            hideEntry.eventID = EventTriggerType.PointerExit;
            hideEntry.callback.AddListener((data) => { HideInfoLegend((PointerEventData)data); });
            hideTrigger.triggers.Add(hideEntry);
        }

        // Set infoBox close button onClick
        infoCloseButton.GetComponent<Button>().onClick.AddListener(
        () =>
        {
            // Re-enable all tiles and buttons, and deactivate infoBox
            foreach (DemTile tile in allTiles)
                tile.enabled = true;
            foreach (Button button in FindObjectsOfType<Button>())
                button.enabled = true;
            infoBox.SetActive(false);
        });
        // Set infoPopup button onClick
        infoPopup.GetComponent<Button>().onClick.AddListener(
        () =>
        {
            // Deactivate current statsBox
            if (easeCoroutine != null) StopCoroutine(easeCoroutine);
            // Set all variable components inactive
            statsBox.SetActive(false);
            trait1Stat.SetActive(false);
            traitIcon1.SetActive(false);
            trait2Stat.SetActive(false);
            traitIcon2.SetActive(false);
            // Set location of infoBox
            infoBox.transform.position = new Vector3
            (
                Screen.width / 2 - infoBox.GetComponent<RectTransform>().rect.width / 2,
                Screen.height / 2 - infoBox.GetComponent<RectTransform>().rect.height / 2,
                0
            );
            // Disable all tiles and buttons, and activate infoBox
            foreach (DemTile tile in allTiles)
                tile.enabled = false;
            foreach (Button button in FindObjectsOfType<Button>())
                button.enabled = false;
            infoBox.SetActive(true);
            // Re-enable infoCloseButton
            infoCloseButton.GetComponent<Button>().enabled = true;
        });

        // Set up pulse
        ResetPulse();
        SetRestorePulse(Color.white, new Color(0.7f, 0.7f, 0.7f));

        // Enable master pulse if needed
        if (!masterPulseEnabled)
        {
            StartCoroutine(EnableMasterPulse());
            masterPulseEnabled = true;
        }
    }

    /**
        Static method for DemTile pulse synchronization.

        @return an IEnumerator
    */
    static IEnumerator EnableMasterPulse ()
    {
        float delayFactor = pulseFactor / 2.0f;
        while (true)
        {
            yield return new WaitForSeconds(delayFactor / masterPulseFrequency);
            pulseTick ++;
        }
    }

    /**
        Defines and returns a coroutine for pulsing between two different colors at a particular frequency.
        An optional delay to start the coroutine may be specified in seconds.
        Note that by default, the pulse is synced with the master clock defined by EnableMasterPulse.
        Also note that defining a non-positive pulse factor will set the tile to a solid color specified by 'color1'.

        @param  color1          (Color) starting color
        @param  color2          (Color) ending color
        @param  frequency       (float) specifies the pulse frequency (larger values => faster pulse)
        @param  pulseFactor     (float) a multiplier for adjusting the colors' Lerp increment length (maller values =
                                smoother transitions)
        @param  syncWithMaster  (bool) true to sync with the master pulse clock, false to pulse independent of master

        @return an IEnumerator
    */
    IEnumerator Pulse
    (
        Color color1, 
        Color color2, 
        float frequency = defaultPulseFrequency, 
        float pulseFactor = defaultPulseFactor,
        bool syncWithMaster = true
    )
    {
        if (syncWithMaster)
        {
            for (int i = pulseTick; true; i = pulseTick)
            {
                this.GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, Mathf.PingPong(pulseTick * pulseFactor, 1.0f));
                while (i == pulseTick)
                    yield return null;
            }
        }
        else
        {
            float delayFactor = 0.5f * pulseFactor;

            for (int i = 0; true; i++)
            {
                this.GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, Mathf.PingPong(i * pulseFactor, 1.0f));
                yield return new WaitForSeconds(delayFactor / frequency);
            }
        }
    }


    /**
        Sets the pulse parameters.
        Changes will affect both active and inactive pulses.

        @param  color1          (Color) starting color
        @param  color2          (Color) ending color
        @param  frequency       (float) specifies the pulse frequency (larger values => faster pulse)
        @param  pulseFactor     (float) a multiplier for adjusting the colors' Lerp increment length (maller values =
                                smoother transitions)
        @param  syncWithMaster  (bool) true to sync with the master pulse clock, false to pulse independent of master
    */
    public void SetPulse
    (
        Color color1, 
        Color color2, 
        float frequency = defaultPulseFrequency, 
        float pulseFactor = defaultPulseFactor,
        bool syncWithMaster = true
    )
    {
        if (hasPulse)
        {
            SignalPulse(false);
            pulse = Pulse(color1, color2, frequency, pulseFactor, syncWithMaster);
            SignalPulse(true);
        }
        else
            pulse = Pulse(color1, color2, frequency, pulseFactor, syncWithMaster);
    }

    /**
        Resets pulse to default values.
    */
    public void ResetPulse ()
    {
        pulse = Pulse(Color.white, new Color(0.7f, 0.7f, 0.7f));
    }

    /**
        Sets the pulse to its restore point.
    */
    void RestorePulse ()
    {
        if (hasPulse)
        {
            SignalPulse(false);
            pulse = GetRestorePulse();
            SignalPulse(true);
        }
        else
            pulse = GetRestorePulse();
    }

    /**
        Defines the restore pulse.

        @param  color1          (Color) starting color
        @param  color2          (Color) ending color
        @param  frequency       (float) specifies the pulse frequency (larger values => faster pulse)
    */
    public void SetRestorePulse (Color color1, Color color2, float frequency = defaultPulseFrequency)
    {
        restoreColor1 = color1;
        restoreColor2 = color2;
        restoreColorFrequency = frequency;
    }

    /**
        Returns a pulse instance as a restore.

        @return an IEnumerator 
    */
    IEnumerator GetRestorePulse ()
    {
        return Pulse(restoreColor1, restoreColor2, restoreColorFrequency);
    }

    /**
        Sends a signal to activate (signal = true) or deactivate (signal = false) the Pulse coroutine.

        Ignores activation signals if a pulse coroutine is currently active, preventing pulse overlap.
        Note that the pulse parameters must be defined with SetPulse prior to calling, otherwise the current values
        will be used.
    */
    public void SignalPulse (bool signal)
    {
        if (signal)
        {
            if (!hasPulse)
            {
                StartCoroutine(this.pulse);
                hasPulse = true;
            }
        }

        else if (hasPulse)
        {
            StopCoroutine(this.pulse);
            hasPulse = false;
        }
    }


    /**
        Callback method meant for infoBox tooltip OnPointerEnter event triggers.
        The function is called when the cursor hovers over a valid infoBox icon, causing a tooltip to appear.

        Note that this method adheres to the format as defined by EventTrigger.Entry.callback.

        @param  eventData   a PointerEventData object
        @see    EventTrigger.Entry.callback
    */
    void ShowInfoLegend (PointerEventData eventData)
    {
        // Parse eventData GameObject
        GameObject icon = eventData.pointerEnter;
        // Declare sprite and parse if available; abort and return if not
        Sprite sprite;
        try { sprite = icon.GetComponent<Image>().sprite; }
        catch (NullReferenceException e)
        {
            Debug.Log("Unable to load Sprite object for '" + icon.name + "'");
            return;
        }
        // Flag must be validated to show tooltip
        bool showTooltip = false;

        // Define tooltip components based on hovered icon
        switch (icon.name)
        {
            case "InfoSpeciesNameIcon":     infoLegendName.GetComponent<Text>().text = "Species Name";
                                            infoLegendLore.GetComponent<Text>().text = "Common species name.";
                                            infoLegendIcon.GetComponent<Image>().sprite = sprite;
                                            showTooltip = true;
                                            break;
            case "InfoSpeciesTypeIcon":     infoLegendName.GetComponent<Text>().text = "Species Type";
                                            infoLegendLore.GetComponent<Text>().text = "Types include Plant, Prey, or Predator.";
                                            infoLegendIcon.GetComponent<Image>().sprite = sprite;
                                            showTooltip = true;
                                            break;
            case "InfoBiomassIcon":         infoLegendName.GetComponent<Text>().text = "Biomass";
                                            infoLegendLore.GetComponent<Text>().text = "Tier 1 (plant), Tier 2 (prey), or Tier 3 (predator) biomass produced.";
                                            infoLegendIcon.GetComponent<Image>().sprite = sprite;
                                            showTooltip = true;
                                            break;
            case "InfoLoreIcon":            infoLegendName.GetComponent<Text>().text = "Species Lore";
                                            infoLegendLore.GetComponent<Text>().text = "Interesting facts about the species.";
                                            infoLegendIcon.GetComponent<Image>().sprite = sprite;
                                            showTooltip = true;
                                            break;
            case "InfoSpeciesTrait1Icon":   switch (sprite.name)
                                            {
                                                case "health_icon": infoLegendName.GetComponent<Text>().text = "Prey Health";
                                                                    infoLegendLore.GetComponent<Text>().text = "Each unit satisfies one predator hunger unit.";
                                                                    break;
                                                case "hunger_icon": infoLegendName.GetComponent<Text>().text = "Predator Hunger";
                                                                    infoLegendLore.GetComponent<Text>().text = "Nutrition (prey health) required to satisfy a predator.";
                                                                    break;
                                                default:            infoLegendName.GetComponent<Text>().text = "Species Trait";
                                                                    infoLegendLore.GetComponent<Text>().text = "Supplementary species trait.";
                                                                    break;
                                            }
                                            infoLegendIcon.GetComponent<Image>().sprite = sprite;
                                            showTooltip = true;
                                            break;
            case "InfoSpeciesTrait2Icon":   switch (sprite.name)
                                            {
                                                case "carnivore_teeth_icon":
                                                case "voracity_icon":   infoLegendName.GetComponent<Text>().text = "Predator Voracity";
                                                                        infoLegendLore.GetComponent<Text>().text = "Nutrition (prey health) consumed each turn.";
                                                                        break;
                                                default:                infoLegendName.GetComponent<Text>().text = "Species Trait";
                                                                        infoLegendLore.GetComponent<Text>().text = "Supplementary species trait.";
                                                                        break;
                                            }
                                            infoLegendIcon.GetComponent<Image>().sprite = sprite;
                                            showTooltip = true;
                                            break;
            case "InfoPredPreyIcon":        switch (sprite.name)
                                            {
                                                case "eat_you": infoLegendName.GetComponent<Text>().text = "Natural Predators";
                                                                infoLegendLore.GetComponent<Text>().text = "Current prey may be used to reduce these species' hunger levels.";
                                                                break;
                                                case "eat_me":  infoLegendName.GetComponent<Text>().text = "Local Prey";
                                                                infoLegendLore.GetComponent<Text>().text = "These species may be used to reduce current predator's hunger level.";
                                                                break;
                                                default:        infoLegendName.GetComponent<Text>().text = "Predator / Prey List";
                                                                infoLegendLore.GetComponent<Text>().text = "List of natural predators or prey.";
                                                                break;
                                            }
                                            infoLegendIcon.GetComponent<Image>().sprite = sprite;
                                            showTooltip = true;
                                            break;
            default:                        break;
        }
        // If hovered icon is valid, show tooltip
        if (showTooltip)
        {
            // Deactivate and set position
            infoLegendTooltip.SetActive(false);
            infoLegendTooltip.transform.position = icon.transform.position;

            // Define and begin easing coroutine
            easeEnumerator = EaseInInfoLegend();
            easeCoroutine = StartCoroutine(easeEnumerator);
        }
    }

    /**
        Callback method meant for infoBox tooltip OnPointerEnter event triggers.
        The function is called when the cursor moves away from a valid infoBox icon, causing any tooltip to vanish.

        Note that this method adheres to the format as defined by EventTrigger.Entry.callback.

        @param  eventData   a PointerEventData object
        @see    EventTrigger.Entry.callback
    */
    void HideInfoLegend (PointerEventData eventData)
    {
        // Deactivate current statsBox
        if (easeCoroutine != null) StopCoroutine(easeCoroutine);
        infoLegendTooltip.SetActive(false);
    }

    /**
        Eases in the infoLegendTooltip object.
        The color alpha channel and y-axis rotation are modified each iteration to create the effect.
        
        @param  easing  a floating point value denoting the step size of each iteration
    */
    IEnumerator EaseInInfoLegend (float easing = 0.15f)
    {
        // Delay start for one half second
        yield return new WaitForSeconds(0.5f);
        // Set statsBox active
        infoLegendTooltip.SetActive(true);
        // Set final states
        Vector3 finalRotation = statsBoxBaseRotation;
        float finalAlpha = statsBoxBaseColor.a;
        float finalIconAlpha = 0;
        // Initialize transitional variables
        Vector3 statRotation = statsBoxBaseRotation;
        Color statsColor = statsBox.GetComponent<Image>().color;
        float rotation = -45f;
        float alpha = 0;
        statRotation.y = rotation;
        statsColor.a = alpha;
        infoLegendTooltip.transform.localEulerAngles = statRotation;
        infoLegendTooltip.GetComponent<Image>().color = statsColor;

        // Ease until threshold reached
        while (rotation < finalRotation.y - 0.1f)
        {
            // Update transitional variables
            rotation += (finalRotation.y - rotation) * easing;
            statRotation.y = rotation;
            alpha += (finalAlpha - alpha) * easing;
            statsColor.a = alpha;
            // Apply changes to statsBox
            infoLegendTooltip.transform.localEulerAngles = statRotation;
            infoLegendTooltip.GetComponent<Image>().color = statsColor;
            // Stall for next iteration
            yield return new WaitForSeconds(0.01f);
        }
        // Post-threshold states set to base
        infoLegendTooltip.transform.localEulerAngles = statsBoxBaseRotation;
        infoLegendTooltip.GetComponent<Image>().color = statsBoxBaseColor;
    }

    /**
        Activates on mouse enter.
    */
    void OnMouseEnter ()
    {
        // Ignore if disabled
        if (!enabled) return;

        // Set tile pulse
        // If building...
        if (buildMenu.currentAnimalFactory != null)
        {
            if (available)
            {
                // As build origin, paint a solid color (no pulse)
                SetPulse(Color.cyan, Color.cyan, 0, 0);

                // Pulse the range tiles for a plant
                if (buildMenu.currentAnimalFactory.GetType() == 0)
                {
                    // Grab range offset data for the plant
                    int[][] range = SpeciesConstants.Range(buildMenu.currentAnimalFactory.GetName());
                    foreach (int[] coord in range)
                    {
                        // Skip tiles that are out of range
                        if (idX + coord[0] < 0 || idX + coord[0] > 8 || idY + coord[1] < 0 || idY + coord[1] > 4)
                            continue;

                        // If tile is free, set pulse
                        DemTile tile =  main.boardController.Tiles[idX + coord[0], idY + coord[1]].GetComponent<DemTile>();
                        if (!tile.resident)
                            tile.SetPulse(rangeColor, Color.white);
                    }
                }

            }

            // If not available, pulse red
	        else
            {
                SetPulse(Color.red, Color.Lerp(Color.red, Color.white, 0.25f), 0.25f, 0.05f, false);
                SignalPulse(true);
            }
        // If not building...
        }
        else
        {
            // Sublte white-to-gray for empty cursor
            SetPulse(Color.white, new Color(0.7f, 0.7f, 0.7f));
            SignalPulse(true);

            // If tile occupied, output some stats
            if (resident)
            {
                //Debug.Log(resident.name + " is here");
                // Parse resident info, set name
                BuildInfo info = resident.GetComponent<BuildInfo>();
                nameStat.GetComponent<Text>().text = resident.name;
                // Set infoBox common output data
                infoSpeciesName.GetComponent<Text>().text = resident.name;
                infoBiomass.GetComponent<Text>().text = SpeciesConstants.Biomass(resident.name).ToString();
                infoLore.GetComponent<Text>().text = SpeciesConstants.SpeciesLore(resident.name);
                // Set species type-specific statsBox and infoBox output data
                switch (SpeciesConstants.SpeciesType(resident.GetComponent<BuildInfo>().speciesId))
                {
                    // Plant: 
                    case 0 :    // STATSBOX //
                                //
                                // Set species type icon and color
                                iconStat.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/plant_icon");
                                iconStat.GetComponent<Image>().color = new Color32(46, 139, 87, 200);

                                // INFOBOX //
                                //
                                // Set species type and icon
                                infoSpeciesType.GetComponent<Text>().text = "Plant";
                                infoSpeciesTypeIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/plant_icon");
                                infoSpeciesTypeIcon.GetComponent<Image>().color = new Color32(46, 139, 87, 200);
                                // Set species traits and predator/prey objects as inactive
                                infoSpeciesTrait1.SetActive(false);
                                infoSpeciesTrait1Icon.SetActive(false);
                                infoSpeciesTrait2.SetActive(false);
                                infoSpeciesTrait2Icon.SetActive(false);
                                infoPredPreyHeader.SetActive(false);
                                infoPredPreyIcon.SetActive(false);
                                infoPredPreyList.SetActive(false);
                                infoPredPreyDivider.SetActive(false);

                                break;
                    // Prey:
                    case 1 :    // STATSBOX //
                                //
                                // Set species type icon and color
                                iconStat.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/prey_icon");
                                iconStat.GetComponent<Image>().color = new Color32(210, 105, 30, 200);
                                // Set primary trait as prey health
                                trait1Stat.GetComponent<Text>().text = resident.GetComponent<PreyInfo>().GetHealth() + 
                                    "/" + SpeciesConstants.Health(resident.name);
                                traitIcon1.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/health_icon");
                                traitIcon1.GetComponent<Image>().color = Color.Lerp
                                (
                                    new Color32(128, 128, 128, 150),
                                    new Color32(220, 20, 60, 150),
                                    resident.GetComponent<PreyInfo>().GetHealth() / SpeciesConstants.Health(resident.name)
                                );
                                trait1Stat.SetActive(true);
                                traitIcon1.SetActive(true);

                                // INFOBOX //
                                //
                                // Set species type and icon in infoBox
                                infoSpeciesType.GetComponent<Text>().text = "Prey";
                                infoSpeciesTypeIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/prey_icon");
                                infoSpeciesTypeIcon.GetComponent<Image>().color = new Color32(210, 105, 30, 200);
                                // Set species health in infoBox
                                infoSpeciesTrait1.GetComponent<Text>().text = trait1Stat.GetComponent<Text>().text;
                                infoSpeciesTrait1Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/health_icon");
                                infoSpeciesTrait1Icon.GetComponent<Image>().color = Color.Lerp
                                (
                                    new Color32(128, 128, 128, 150),
                                    new Color32(220, 20, 60, 150),
                                    resident.GetComponent<PreyInfo>().GetHealth() / SpeciesConstants.Health(resident.name)
                                );
                                //
                                // Set predator list
                                infoPredPreyIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/eat_you");
                                infoPredPreyIcon.GetComponent<Image>().color = new Color32(178, 34, 34, 150);
                                infoPredPreyHeader.GetComponent<Text>().text = "Natural Predators:";
                                // Destroy any pre-existing list items
                                foreach (Transform child in infoPredPreyList.transform) GameObject.DestroyObject(child.gameObject);
                                // Populate new list of predators
                                foreach (int id in SpeciesConstants.PredatorIDList(resident.name))
                                {
                                    // Create new PredPreyObject, set parent, set as active
                                    GameObject predObject = GameObject.Instantiate(GameObject.Find("Canvas/PredPreyObject"));
                                    predObject.GetComponentInChildren<Text>().text = SpeciesConstants.SpeciesName(id);
                                    predObject.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("DontEatMe/Sprites/" + SpeciesConstants.SpeciesName(id));
                                    //predObject.transform.parent = infoPredPreyList.transform;
                                    predObject.transform.SetParent(infoPredPreyList.transform, false);
                                    predObject.SetActive(true);
                                }
                                //
                                // Activate species trait 1, deactivate species trait2
                                infoSpeciesTrait1.SetActive(true);
                                infoSpeciesTrait1Icon.SetActive(true);
                                infoSpeciesTrait2.SetActive(false);
                                infoSpeciesTrait2Icon.SetActive(false);
                                // Activate infoPredPrey* objects
                                infoPredPreyHeader.SetActive(true);
                                infoPredPreyIcon.SetActive(true);
                                infoPredPreyList.SetActive(true);
                                infoPredPreyDivider.SetActive(true);

                                break;
                    // Preadator:
                    case 2 :    // STATSBOX //
                                //
                                // Set species type icon and color
                                iconStat.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/carnivore_icon");
                                iconStat.GetComponent<Image>().color = new Color32(178, 34, 34, 200);
                                // Set primary trait as predator hunger
                                trait1Stat.GetComponent<Text>().text = resident.GetComponent<PredatorInfo>().GetHunger() + 
                                    "/" + SpeciesConstants.Hunger(resident.name);
                                traitIcon1.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/hunger_icon");
                                traitIcon1.GetComponent<Image>().color = Color.Lerp
                                (
                                    new Color32(128, 128, 128, 150),
                                    new Color32(128, 128, 0, 150),
                                    resident.GetComponent<PredatorInfo>().GetHunger() / SpeciesConstants.Hunger(resident.name)
                                );
                                // Set secondary trait as predator voracity
                                trait2Stat.GetComponent<Text>().text = resident.GetComponent<PredatorInfo>().GetVoracity() + 
                                    "/" + SpeciesConstants.Voracity(resident.name);
                                traitIcon2.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/carnivore_teeth_icon");
                                traitIcon2.GetComponent<Image>().color = Color.Lerp
                                (
                                    new Color32(128, 128, 128, 150),
                                    new Color32(219, 112, 147, 150),
                                    resident.GetComponent<PredatorInfo>().GetVoracity() / SpeciesConstants.Voracity(resident.name)
                                );
                                trait1Stat.SetActive(true);
                                trait2Stat.SetActive(true);
                                traitIcon1.SetActive(true);
                                traitIcon2.SetActive(true);

                                // INFOBOX //
                                //
                                // Set species type and icon in infoBox
                                infoSpeciesType.GetComponent<Text>().text = "Predator";
                                infoSpeciesTypeIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/carnivore_icon");
                                infoSpeciesTypeIcon.GetComponent<Image>().color = new Color32(178, 34, 34, 200);
                                // Set species hunger as trait 1 in infoBox
                                infoSpeciesTrait1.GetComponent<Text>().text = trait1Stat.GetComponent<Text>().text;
                                infoSpeciesTrait1Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/hunger_icon");
                                infoSpeciesTrait1Icon.GetComponent<Image>().color = Color.Lerp
                                (
                                    new Color32(128, 128, 128, 150),
                                    new Color32(128, 128, 0, 150),
                                    resident.GetComponent<PredatorInfo>().GetHunger() / SpeciesConstants.Hunger(resident.name)
                                );
                                // Set secondary trait as predator voracity
                                infoSpeciesTrait2.GetComponent<Text>().text = resident.GetComponent<PredatorInfo>().GetVoracity() + 
                                    "/" + SpeciesConstants.Voracity(resident.name);
                                infoSpeciesTrait2Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/carnivore_teeth_icon");
                                infoSpeciesTrait2Icon.GetComponent<Image>().color = Color.Lerp
                                (
                                    new Color32(128, 128, 128, 150),
                                    new Color32(219, 112, 147, 150),
                                    resident.GetComponent<PredatorInfo>().GetVoracity() / SpeciesConstants.Voracity(resident.name)
                                );
                                //
                                // Set prey list
                                infoPredPreyIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/eat_me");
                                infoPredPreyIcon.GetComponent<Image>().color = new Color32(102, 205, 170, 150);
                                infoPredPreyHeader.GetComponent<Text>().text = "Local Prey:";
                                // Destroy any pre-existing list items
                                foreach (Transform child in infoPredPreyList.transform) GameObject.DestroyObject(child.gameObject);
                                // Populate new list ofprey
                                foreach (int id in SpeciesConstants.PreyIDList(resident.name))
                                {
                                    // Create new PredPreyObject, set parent, set as active
                                    GameObject preyObject = GameObject.Instantiate(GameObject.Find("Canvas/PredPreyObject"));
                                    preyObject.GetComponentInChildren<Text>().text = SpeciesConstants.SpeciesName(id);
                                    preyObject.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("DontEatMe/Sprites/" + SpeciesConstants.SpeciesName(id));
                                    //preyObject.transform.parent = infoPredPreyList.transform;
                                    preyObject.transform.SetParent(infoPredPreyList.transform, false);
                                    preyObject.SetActive(true);
                                }
                                //
                                // Activate species trait 1 and 2
                                infoSpeciesTrait1.SetActive(true);
                                infoSpeciesTrait1Icon.SetActive(true);
                                infoSpeciesTrait2.SetActive(true);
                                infoSpeciesTrait2Icon.SetActive(true);
                                // Activate infoPredPrey* objects
                                infoPredPreyHeader.SetActive(true);
                                infoPredPreyIcon.SetActive(true);
                                infoPredPreyList.SetActive(true);
                                infoPredPreyDivider.SetActive(true);

                                break;
                    // Unknown:
                    default :   //_type = "Unknown"; 
                                //statsBox.GetComponent<Image>().color = new Color32(255, 255, 255, 190);
                                iconStat.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/all_types_icon");
                                iconStat.GetComponent<Image>().color = Color.gray;
                                break;
                }

                // Position statsBox over current resident and deactivate
                statsBox.SetActive(false);
                statsBox.transform.position = Camera.main.WorldToScreenPoint(resident.gameObject.transform.position);
                // Define and begin easing coroutine
                easeEnumerator = EaseInStatsBox();
                easeCoroutine = StartCoroutine(easeEnumerator);
            }
        }
    }

    /**
        Eases in the statsBox object.
        The color alpha channel and y-axis rotation are modified each iteration to create the effect.
        The infoPopup also flashes from highlighted state to normal state (@ zero alpha) as a visual hint of its
        existence.
        
        @param  easing  a floating point value denoting the step size of each iteration
    */
    IEnumerator EaseInStatsBox (float easing = 0.15f)
    {
        // Delay one tenth second to start
        yield return new WaitForSeconds(0.1f);
        // Set statsBox active
        statsBox.SetActive(true);
        // Set final states
        Vector3 finalRotation = statsBoxBaseRotation;
        float finalAlpha = statsBoxBaseColor.a;
        float finalIconAlpha = 0;
        // Initialize transitional variables
        Vector3 statRotation = statsBoxBaseRotation;
        Color statsColor = statsBox.GetComponent<Image>().color;
        //Color iconColor = infoPopupBaseNormalColor;
        ColorBlock iconColors = infoPopupBaseColors;
        Color iconColor = iconColors.normalColor;
        float rotation = -45f;
        float alpha = 0;
        float iconAlpha = iconColors.highlightedColor.a;
        statRotation.y = rotation;
        statsColor.a = alpha;
        iconColor.a = iconAlpha;
        iconColors.normalColor = iconColor;
        statsBox.transform.localEulerAngles = statRotation;
        statsBox.GetComponent<Image>().color = statsColor;
        infoPopup.GetComponent<Button>().colors = iconColors;

        // Ease until threshold reached
        while (rotation < finalRotation.y - 0.1f)
        {
            // Update transitional variables
            rotation += (finalRotation.y - rotation) * easing;
            statRotation.y = rotation;
            alpha += (finalAlpha - alpha) * easing;
            statsColor.a = alpha;
            iconAlpha += (finalIconAlpha - iconAlpha) * easing;
            iconColor.a = iconAlpha;
            iconColors.normalColor = iconColor;
            // Apply changes to statsBox
            statsBox.transform.localEulerAngles = statRotation;
            statsBox.GetComponent<Image>().color = statsColor;
            infoPopup.GetComponent<Button>().colors = iconColors;
            // Stall for next iteration
            yield return new WaitForSeconds(0.01f);
        }
        // Post-threshold states set to base
        statsBox.transform.localEulerAngles = statsBoxBaseRotation;
        statsBox.GetComponent<Image>().color = statsBoxBaseColor;
        infoPopup.GetComponent<Button>().colors = infoPopupBaseColors;
    }

    /**
        Activates on mouse exit.
    */
    void OnMouseExit ()
    {
        // Ignore if disabled
        if (!enabled) return;

        // Reset highlight color
        if (buildMenu.currentAnimalFactory != null && available)
        {
            // ... for current tile
            RestorePulse();

            // ... for plant range tiles (if building)
            if (buildMenu.currentAnimalFactory.GetType() == 0)
            {
                int[][] range = SpeciesConstants.Range(buildMenu.currentAnimalFactory.GetName());
                foreach (int[] coord in range)
                {
                    // Skip tiles that are out of range
                    if (idX + coord[0] > 8 || idX + coord[0] < 0 || idY + coord[1] > 4 || idY + coord[1] < 0)
                        continue;

                    DemTile tile =  main.boardController.Tiles[idX + coord[0], idY + coord[1]].GetComponent<DemTile>();
                    // Tile must be free
                    if (!tile.resident)
                        // Set the pulse for each tile
                        tile.RestorePulse();
                    else tile.GetComponent<Renderer>().material.color = currentColor;
                }
            }
        }
        else
        {
            SignalPulse(false);
            this.GetComponent<Renderer>().material.color = currentColor;
        }

        // Stop any EaseInStatsBox coroutine in progress        
        //try { StopCoroutine(easeCoroutine); }
        //catch (NullReferenceException e) { Debug.Log("EaseInStatsBox not in progress --> " + e.Message); }
        if (easeCoroutine != null) StopCoroutine(easeCoroutine);
        // Set all variable components inactive
        statsBox.SetActive(false);
        trait1Stat.SetActive(false);
        traitIcon1.SetActive(false);
        trait2Stat.SetActive(false);
        traitIcon2.SetActive(false);
    }

    /**
        Activates on mouse click.
    */
    void OnMouseDown ()
    {
        // Ignore if disabled
        if (!enabled) return;

        // DEBUG
        Debug.Log("Tile (" + idX + ", " + idY + ") clicked, center @ (" + center.x + ", " + center.y + ", " + center.z + ")");

        // If a creature is flagged for building...
        if (buildMenu.currentAnimalFactory != null)
        {
            // If tile is empty...
            if (available)
            {
                DemAudioManager.audioUiLoad.Play ();
                //resident = DemMain.currentSelection;
                
                //resident.transform.position = center;
                AddAnimal(main.currentSelection);

                // If building a plant:
                if (buildMenu.currentAnimalFactory.isPlant ())
                {
                    int currentBiomass = buildMenu.GetTier1Biomass ();
                    int newBiomass = SpeciesConstants.Biomass (main.currentSelection.name);
                    buildMenu.UpdateTier1Biomass (currentBiomass - newBiomass);
                    buildMenu.AddTier2Biomass ((int)(newBiomass * 0.5));

                }
                // If building a prey:
                else
                {
                    int currentBiomass = buildMenu.GetTier2Biomass();
                    buildMenu.SubtractTier2Biomass (SpeciesConstants.Biomass (main.currentSelection.name));
                }

                // Set BuildMenu.currentlyBuilding to null after successful placement
                buildMenu.currentAnimalFactory = null;
                main.currentSelection = null;
                main.boardController.ClearAvailableTiles();
                turnSystem.PredatorTurn();

                // DEBUG 
                if (resident)
                    Debug.Log("Placed " + resident.name + " @ " + resident.GetComponent<Transform>().position);
           }
           // Play fail sound on invalid build attempt
           else
              DemAudioManager.audioFail2.Play ();
        }
    }


    /* TODO: 
        a) add required documentation omitted by original author/authors
        b) fix indentation per agreed coding conventions (4-space expanded tabs)
        c) clean up superfluous code
        d) fix naming inconsistencies (e.g. hasPlant vs ResidentIsPredator)
    */
    /*
  public bool hasPlant(){
    
    if (this.resident) {
		return this.resident.GetComponent<BuildInfo> ().isPlant();
    } else {
      return false;
    }

  }
  */
    public bool hasPlant ()
    {
        return this.resident && this.resident.GetComponent<BuildInfo>().isPlant();
    }

  public GameObject GetResident(){
    return resident;
  }

  public bool ResidentIsPredator(){
    
    if (resident) {
      return resident.GetComponent<BuildInfo> ().isPredator ();
    } else if(nextPredator){
      return true;
    }
    else
    {
      return false;
    }

  }

  public void SetResident(GameObject newResident){
    resident = newResident;
  }


  public void AddAnimal(GameObject animal){

    this.resident = animal;

    this.resident.GetComponent<BuildInfo> ().tile = this;

    this.resident.transform.position = this.center;

		//add statistic to tree place down, and prey place down
		if (this.resident.GetComponent<BuildInfo> ().isPlant ()) {
			buildMenu.statistic.setTreeDown (1);
		} else if (this.resident.GetComponent<BuildInfo> ().isPrey ()) {
			buildMenu.statistic.setPreyDown (1);
		}


  }


  public void AddNewPredator(GameObject animal){

    this.nextPredator = animal;

    this.nextPredator.GetComponent<BuildInfo> ().tile = this;

    Vector3 newPosition = new Vector3();
    newPosition.x = this.center.x+2;
    newPosition.y = this.center.y;
    newPosition.z = this.center.z;

    this.nextPredator.transform.position = newPosition;

  }

  public void UpdateNewPredator(){
    resident = nextPredator;
    nextPredator = null;
  }

  public void RemoveAnimal(){

		//add statistic to tree destroy, and prey eaten
		if (this.resident.GetComponent<BuildInfo> ().isPlant ()) {
			buildMenu.statistic.setTreeDestroy (1);
		} else if (this.resident.GetComponent<BuildInfo> ().isPrey ()) {
			buildMenu.statistic.setPreyEaten (1);
		}
    
    Destroy (resident);
    this.resident = null;
  
  }

  public int GetIdX(){
    return idX;
  }

  public int GetIdY(){
    return idY;
  }

  public Vector3 GetCenter(){
    return center;
  }

}

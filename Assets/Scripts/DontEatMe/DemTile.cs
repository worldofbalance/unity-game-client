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
    /** X-coord for DemTile */
  	public int idX; 
    /** Y-coord for DemTile */
  	public int idY;
    /** Current tile color */
    public Color currentColor;
    /** Color for plant range indicator pulse */
    public Color rangeColor;

    /** True if tile available for building */
    public bool available;

    private Vector3 center;

    /** Resident object (HerbivoreObject or PlantObject) placed on tile */
  	public GameObject resident; 

    private GameObject nextPredator;
    private BuildMenu buildMenu;
    public  GameObject mainObject;
    public GameObject panelObject;
    private DemMain main;
    private DemTurnSystem turnSystem;

    // StatsBox objects (static --> no two instances required)
    /** StatsBox parent object */
    public static GameObject statsBox;
    /** StatsBox species name */
    public static GameObject nameStat;
    /** StatsBox species icon */
    public static GameObject iconStat;
    /** Species trait (primary) */
    public static GameObject trait1Stat;    
    /** Species trait (secondary) */            
    public static GameObject trait2Stat; 
    /** Species trait icon (primary) */               
    public static GameObject traitIcon1;     
    /* Species trait icon (secondary) */
    public static GameObject traitIcon2;
    /** Info popup object */     
    public static GameObject infoPopup;    
    /** Popup with "remove build" feature */             
    public static GameObject removeBuildButton;   
    /** Base color for the statsBox object */      
    public static Color statsBoxBaseColor;
    /** Base localScale for the statsBox object */           
    public static Vector3 statsBoxBaseScale;
    /** Base localEulersAngles for the statsBox object */         
    public static Vector3 statsBoxBaseRotation;    
    /** Base ColorBlock for the infoPopup Button */      
    public static ColorBlock infoPopupBaseColors;  
    /** Base ColorBlock for the removeBuildButton */     
    public static ColorBlock removeButtonBaseColors;    

    // InfoBox objects
    /** InfoBox parent object */
    public static GameObject infoBox;      
    /** InfoBox species name */             
    public static GameObject infoSpeciesName;   
    /** InfoBox species type */                     
    public static GameObject infoSpeciesType;           
    /** InfoBox species icon */             
    public static GameObject infoSpeciesTypeIcon;       
    /** InfoBox species biomass */             
    public static GameObject infoBiomass;
    /** InfoBox species biomass tier level */             
    public static GameObject infoBiomassTier; 
    /** InfoBox species biomass icon */          
    public static GameObject infoBiomassIcon;   
    /** InfoBox species trait (primary) */        
    public static GameObject infoSpeciesTrait1;       
    /** InfoBox species trait (secondary) */  
    public static GameObject infoSpeciesTrait2;
    /** InfoBox species trait icon (primary) */
    public static GameObject infoSpeciesTrait1Icon;   
    /** InfoBox species trait icon (secondary) */  
    public static GameObject infoSpeciesTrait2Icon;     
    /** InfoBox predator / prey list header */
    public static GameObject infoPredPreyHeader;        
    /** InfoBox predator / prey list grid */
    public static GameObject infoPredPreyList;          
    /** InfoBox predator / prey list header icon */
    public static GameObject infoPredPreyIcon;          
    /** InfoBox predator / prey list header divider */
    public static GameObject infoPredPreyDivider;       
    /** InfoBox species lore */
    public static GameObject infoLore;                  
    /** InfoBox close button */
    public static GameObject infoCloseButton;           

    // InfoLegendTooltip objects
    /** InfoLegendTooltip parent object */
    public static GameObject infoLegendTooltip;         
    /** InfoLegendTooltip attribute name */
    public static GameObject infoLegendName;            
    /** InfoLegendTooltip attribute icon */
    public static GameObject infoLegendIcon;    
    /** InfoLegendTooltip attribute lore */        
    public static GameObject infoLegendLore;            
    /** Base Color for the infoPopup's Button normal state */
    public static Color infoPopupBaseNormalColor;    
    /** Base Color for the infoPopup's Button highlighted state */   
    public static Color infoPopupBaseHighlightColor;    

    // SimpleTooltip objects
    /** SimpleTooltip parent object */
    public static GameObject simpleTooltip; 
    /** Base color for the SimpleToolTip */            
    public static Color simpleTooltipBaseColor;         
    /** Base color for the SimpleTooltip icon frame */
    public static Color simpleTooltipIconFrameBaseColor;

    // RemoveBuildButton attributes
    private static Vector3 removeBuildButtonBaseScale;

    private IEnumerator easeEnumerator;     // Enumerator for statsBox easing routine
    private IEnumerator easeSimpleTooltip;  // Enumerator for simpleTooltip easing routine
    private Coroutine easeCoroutine;        // Easing coroutine for the statsBox object
    private Coroutine easeSimpleCoroutine;  // Easing coroutine for the statsBox object
    private bool easeFinished;              // True if easeCoroutine has finished


    private IEnumerator pulse; // Called as a coroutine to start/stop pulsing of tile colors
    private bool hasPulse; // Denotes if tile has currently active pulse
    private const float defaultPulseFrequency = 0.625f; // Default pulse frequency
    private const float defaultPulseFactor = 0.1f; // Default value for synchronizing pulse modulation with frequency
    /** Public accessor for defaultPulseFactor */
    public float DefaultPulseFrequency { get { return defaultPulseFrequency; } }

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
        removeBuildButton = GameObject.Find("Canvas/StatsBox/RemoveBuildButton");
        // ...InfoBox objects
        infoBox = GameObject.Find("Canvas/InfoBox");
        infoSpeciesName = GameObject.Find("Canvas/InfoBox/InfoSpeciesName");
        infoSpeciesType = GameObject.Find("Canvas/InfoBox/InfoSpeciesType");
        infoSpeciesTypeIcon= GameObject.Find("Canvas/InfoBox/InfoSpeciesTypeIcon");
        infoBiomass = GameObject.Find("Canvas/InfoBox/InfoBiomass");
        infoBiomassTier = GameObject.Find("Canvas/InfoBox/InfoBiomassTier");
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
        // ...SimpleTooltip object
        simpleTooltip = GameObject.Find("Canvas/SimpleTooltip");
        // ...Other objects
        statsBoxBaseColor = statsBox.GetComponent<Image>().color;
        statsBoxBaseScale = statsBox.transform.localScale;
        statsBoxBaseRotation = statsBox.transform.localEulerAngles;
        infoPopupBaseColors = infoPopup.GetComponent<Button>().colors;
        infoPopupBaseNormalColor = infoPopup.GetComponent<Button>().colors.normalColor;
        infoPopupBaseHighlightColor = infoPopup.GetComponent<Button>().colors.highlightedColor;
        simpleTooltipBaseColor = simpleTooltip.GetComponent<Image>().color;
        removeButtonBaseColors = removeBuildButton.GetComponent<Button>().colors;
        removeBuildButtonBaseScale = removeBuildButton.transform.localScale;
        easeEnumerator = null;
        easeSimpleTooltip = null;
        easeCoroutine = null;
        easeSimpleCoroutine = null;
        easeFinished = true;

        // Initialize resident
        resident = null;

        // Initialize pulse variables
        hasPulse = false;
        pulseTick = 0;
        pulseFactor = defaultPulseFactor;
        masterPulseFrequency = defaultPulseFrequency;
        rangeColor = new Color32(40, 170, 220, 255);

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
            if (image.gameObject.GetComponent<EventTrigger>() != null) break;
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

        if (removeBuildButton.GetComponent<EventTrigger>() == null)
        {
            // Define OnPointerEnter and OnPointerExti events for the StatsBox InfoPopup and RemoveBuildButton objects
            EventTrigger.Entry simpleTooltipShowEntry = new EventTrigger.Entry();
            EventTrigger.Entry simpleTooltipHideEntry = new EventTrigger.Entry();
            simpleTooltipShowEntry.eventID = EventTriggerType.PointerEnter;
            simpleTooltipHideEntry.eventID = EventTriggerType.PointerExit;
            // Add listeners
            simpleTooltipShowEntry.callback.AddListener((data) => { ShowSimpleToolTip(data as PointerEventData); });
            simpleTooltipHideEntry.callback.AddListener((data) => { HideSimpleToolTip(data as PointerEventData); });
            // Add trigger entry to objects
            removeBuildButton.AddComponent<EventTrigger>().triggers.Add(simpleTooltipShowEntry);
            removeBuildButton.GetComponent<EventTrigger>().triggers.Add(simpleTooltipHideEntry);
            infoPopup.AddComponent<EventTrigger>().triggers.Add(simpleTooltipShowEntry);
            infoPopup.AddComponent<EventTrigger>().triggers.Add(simpleTooltipHideEntry);
        }

        // Set infoBox close button onClick
        infoCloseButton.GetComponent<Button>().onClick.RemoveAllListeners();
        infoCloseButton.GetComponent<Button>().onClick.AddListener(
        () =>
        {
            // Re-enable all tiles and buttons, and deactivate infoBox
            foreach (DemTile tile in allTiles)
                tile.enabled = true;
            foreach (Button button in FindObjectsOfType<Button>())
                button.enabled = true;
            infoBox.SetActive(false);
            StartCoroutine(turnSystem.ResumeSpawn());
        });

        // Set infoPopup button onClick
        infoPopup.GetComponent<Button>().onClick.RemoveAllListeners();
        infoPopup.GetComponent<Button>().onClick.AddListener(
        () =>
        {
            StartCoroutine(turnSystem.PauseSpawn());
            // Deactivate current statsBox
            if (easeCoroutine != null) StopCoroutine(easeCoroutine);
            if (easeSimpleCoroutine != null) StopCoroutine(easeSimpleCoroutine);
            // Set all variable components inactive
            statsBox.SetActive(false);
            trait1Stat.SetActive(false);
            traitIcon1.SetActive(false);
            trait2Stat.SetActive(false);
            traitIcon2.SetActive(false);
            simpleTooltip.SetActive(false);
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

        // Set removeBuildButton button onClick
        removeBuildButton.GetComponent<Button>().onClick.RemoveAllListeners();
        removeBuildButton.GetComponent<Button>().onClick.AddListener(
        () =>
        {
            // If resident is prey, add Tier2 biomass back to the pool
            if(main.boardController.HoveredTile.resident.GetComponent<BuildInfo>().isPrey())
                buildMenu.AddTier2Biomass (SpeciesConstants.Biomass(main.boardController.HoveredTile.resident.name));
            // If resident is plant, update Tier1 and Tier2 biomass levels
            else if (main.boardController.HoveredTile.resident.GetComponent<BuildInfo>().isPlant())
            {
                buildMenu.AddTier1Biomass(SpeciesConstants.Biomass(main.boardController.HoveredTile.resident.name));
                buildMenu.SubtractTier2Biomass((int)(SpeciesConstants.Biomass(main.boardController.HoveredTile.resident.name) * 0.5));
            }
            // Pause predator spawn, deactivate statsBox, and invoke animated removal
            StartCoroutine(turnSystem.PauseSpawn());
            statsBox.SetActive(false);
            StartCoroutine(AnimatedRemove(main.boardController.HoveredTile, turnSystem));
        });

        // Start simpleTooltip icon frame pulse coroutine
        StartCoroutine(PulseSimpleTooltipIcon());

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
        Callback method meant for infoBox tooltip OnPointerExit event triggers.
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
        Callback method meant for the simpleTooltip OnPointerEnter event trigger.
        The function is called when the cursor hovers over a valid calling object, causing a tooltip to appear.

        Note that this method adheres to the format as defined by EventTrigger.Entry.callback.

        @param  eventData   a PointerEventData object
        @see    EventTrigger.Entry.callback
    */
    void ShowSimpleToolTip (PointerEventData eventData)
    {
        // Parse calling object
        GameObject callingObject = eventData.pointerEnter;

        // Set text
        // NOTE: for now, it will only activate on select items
        switch (callingObject.name)
        {
            // StatsBox RemoveBuildButton
            case "RemoveBuildButton":   simpleTooltip.GetComponentInChildren<Text>().text = "Destroy this build.";
                                        break;
            // StatsBox InfoPopup
            case "InfoPopup":           simpleTooltip.GetComponentInChildren<Text>().text = "Display this species' data.";
                                        break;
            // Default
            default:                    simpleTooltip.GetComponentInChildren<Text>().text = "Something-something plants and animals.";
                                        break;
        }

        // Deactivate and set position
        simpleTooltip.SetActive(false);
        simpleTooltip.transform.position = callingObject.transform.position;

        // Define and begin easing coroutine
        if (easeSimpleCoroutine != null) StopCoroutine(easeSimpleTooltip);
        easeSimpleTooltip = EaseInSimpleTooltip(0.15f);
        easeSimpleCoroutine = StartCoroutine(easeSimpleTooltip);
    }

    /**
        Callback method meant for the simpleTooltip OnPointerExit event trigger.
        The function is called when the cursor moves away from a valid calling object, causing any tooltip to vanish.

        Note that this method adheres to the format as defined by EventTrigger.Entry.callback.

        @param  eventData   a PointerEventData object
        @see    EventTrigger.Entry.callback
    */
    void HideSimpleToolTip (PointerEventData eventData)
    {
        // Deactivate current simpleTooltip
        if (easeSimpleCoroutine != null) StopCoroutine(easeSimpleCoroutine);
        simpleTooltip.SetActive(false);
        // Re-enable all tiles
        foreach (DemTile tile in allTiles) tile.enabled = true;
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
        Eases in the simpleTooltip object.
        The color alpha channel and y-axis rotation are modified each iteration to create the effect.
        
        @param  easing  a floating point value denoting the step size of each iteration
    */
    IEnumerator EaseInSimpleTooltip (float easing = 0.15f)
    {
        
        // Delay start for one second
        yield return new WaitForSeconds(1.0f);
        while (!easeFinished) yield return null;
        if (statsBox.activeSelf)
        {
            // Set simpleTooltip as active
            simpleTooltip.SetActive(true);
            // Set final states (use same attributes as infoLegendTooltip)
            Vector3 finalRotation = statsBoxBaseRotation;
            float finalAlpha = simpleTooltipBaseColor.a;
            float finalTextAlpha = 0;
            // Initialize transitional variables
            Vector3 tooltipRotation = statsBoxBaseRotation;
            Color tooltipColor = simpleTooltip.GetComponent<Image>().color;
            float rotation = -45f;
            float alpha = 0;
            tooltipRotation.y = rotation;
            tooltipColor.a = alpha;
            simpleTooltip.transform.localEulerAngles = tooltipRotation;
            simpleTooltip.GetComponent<Image>().color = tooltipColor;

            // Ease until threshold reached
            while (rotation < finalRotation.y - 0.1f && statsBox.activeSelf)
            {
                // Update transitional variables
                rotation += (finalRotation.y - rotation) * easing;
                tooltipRotation.y = rotation;
                alpha += (finalAlpha - alpha) * easing;
                tooltipColor.a = alpha;
                // Apply changes to simpleTooltip
                simpleTooltip.transform.localEulerAngles = tooltipRotation;
                simpleTooltip.GetComponent<Image>().color = tooltipColor;
                // Stall for next iteration
                yield return new WaitForSeconds(0.01f);
            }
            // Post-threshold states set to base
            simpleTooltip.transform.localEulerAngles = statsBoxBaseRotation;
            simpleTooltip.GetComponent<Image>().color = simpleTooltipBaseColor;
        }
    }

    /**
        Pulses the simpleTooltip icon frame, both alpha and local scale.
        The color alpha channel and x and y axes are modified each iteration to create the effect.
        
        @param  easing  a floating point value denoting the step size of each iteration
    */
    static IEnumerator PulseSimpleTooltipIcon (float easing = 0.025f)
    {
        Color baseColor = simpleTooltip.transform.GetChild(1).GetComponent<Image>().color;
        Color currentColor = baseColor;
        float alpha = 0.25f;

        int k = 0;
        while (true)
        {
            if (simpleTooltip.activeSelf)
            {
                // Define and set current alpha
                currentColor.a = alpha;
                simpleTooltip.transform.GetChild(1).GetComponent<Image>().color = currentColor;
                // Iterate next alpha value
                alpha = 0.25f + Mathf.PingPong(k * easing, 0.75f);
                k++;
            }
            // Otherwise reset alpha to base
            else alpha = 0.25f;

            yield return new WaitForSeconds(0.05f);
        }

    }

    /**
        Eases in the statsBox object.
        The color alpha channel and y-axis rotation are modified each iteration to create the effect.
        The infoPopup also flashes from highlighted state to normal state (@ zero alpha) as a visual hint of its
        existence.
        
        @param  easing  a floating point value denoting the step size of each iteration
    */
    IEnumerator EaseInStatsBox (float easing = 0.1f)
    {
        // Delay a fraction of a second to start
        yield return new WaitForSeconds(0.15f);
        // Set statsBox active
        statsBox.SetActive(true);
        // Set final states
        Vector3 finalRotation = statsBoxBaseRotation;
        float finalAlpha = statsBoxBaseColor.a;
        float finalIconAlpha = infoPopupBaseColors.normalColor.a; // Both infoPopup and removeBuildButton use the same values

        // Initialize transitional variables
        Vector3 statRotation = statsBoxBaseRotation;
        Color statsColor = statsBox.GetComponent<Image>().color;
        ColorBlock iconColors = infoPopupBaseColors;
        Color iconColor = iconColors.normalColor;
        ColorBlock removeBuildColors = removeButtonBaseColors;
        Color removeBuildColor = removeBuildColors.normalColor;
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
        if (removeBuildButton.activeSelf) 
        {
            removeBuildColor.a = iconAlpha;
            removeBuildColors.normalColor = removeBuildColor;
            removeBuildButton.GetComponent<Button>().colors = removeBuildColors;
        }

        // Ease until threshold reached
        while (rotation < finalRotation.y - 0.1f)
        {
            // Update transitional variables
            rotation += (finalRotation.y - rotation) * easing;
            statRotation.y = rotation;
            alpha += (finalAlpha - alpha) * easing;
            statsColor.a = alpha;
            // ..infoPopup
            iconAlpha += (finalIconAlpha - iconAlpha) * easing;
            iconColor.a = iconAlpha;
            iconColors.normalColor = iconColor;

            // Apply changes to statsBox
            statsBox.transform.localEulerAngles = statRotation;
            statsBox.GetComponent<Image>().color = statsColor;
            infoPopup.GetComponent<Button>().colors = iconColors;
            if (removeBuildButton.activeSelf)
            {
                removeBuildColor.a = iconAlpha;
                removeBuildColors.normalColor = removeBuildColor;
                removeBuildButton.GetComponent<Button>().colors = removeBuildColors;
            }

            // Stall for next iteration
            yield return new WaitForSeconds(0.01f);
        }
        // Post-threshold states set to base
        statsBox.transform.localEulerAngles = statsBoxBaseRotation;
        statsBox.GetComponent<Image>().color = statsBoxBaseColor;
        infoPopup.GetComponent<Button>().colors = infoPopupBaseColors;
        if (removeBuildButton.activeSelf) 
            removeBuildButton.GetComponent<Button>().colors = removeButtonBaseColors;
        easeFinished = true;
    }


    /**
        Coroutine invoked to animate a tile's current build (plant or prey).

        @param  tile        a DemTile object from which to remove a build
        @param  turnSystem  the responsible DemTurnSystem object
    */
    IEnumerator AnimatedRemove (DemTile tile, DemTurnSystem turnSystem)
    {
        // Find animation object, set position and scale, play audio clip
        GameObject ripSprite = GameObject.Find("Canvas/mainUI/RIPAnimation");
        DemAudioManager.audioSigh.Play();
        ripSprite.SetActive(false);
        ripSprite.GetComponent<RectTransform>().localScale = new Vector3(0.35f, 0.35f, 1f);
        Vector3 newpos = tile.resident.transform.position;
        ripSprite.GetComponent<RectTransform>().position = newpos;

        // Define transitional and final colors
        Color _color = Color.white;
        Color _baseColor = _color;
        ripSprite.GetComponent<SpriteRenderer>().color = _baseColor;

        // Remove build, replace with animation and fade its alpha
        tile.RemoveAnimal();
        ripSprite.SetActive(true);
        while (_color.a > 0)
        {
            _color.a -= 0.05f;
            _color.r -= 0.05f;
            _color.g -= 0.05f;
            _color.b -= 0.05f;
            ripSprite.GetComponent<SpriteRenderer>().color = _color;
            yield return new WaitForSeconds(0.05f);
        }
        // Deactivate animation and reset its color
        ripSprite.SetActive(false);
        ripSprite.GetComponent<SpriteRenderer>().color = _baseColor;
        // Resume predator spawn timer
        StartCoroutine(turnSystem.ResumeSpawn());
    }

    /**
        Public method to explicitly call OnMouseEnter.
        Used for updating tiles in realtime.
    */
    public void UpdateOnMouseEnter ()
    {
        this.OnMouseEnter();
    }

    /**
        Activates on mouse enter.
    */
    void OnMouseEnter ()
    {
        // Ignore if disabled
        if (!enabled) return;
        else main.boardController.HoveredTile = (DemTile)this;

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
                        if
                        (
                            idX + coord[0] < 0 || 
                            idX + coord[0] > main.boardController.numColumns - 1 || 
                            idY + coord[1] < 0 || 
                            idY + coord[1] > main.boardController.numRows - 1
                        )
                            continue;

                        // If tile is free, set pulse
                        DemTile tile =  main.boardController.Tiles[idX + coord[0], idY + coord[1]].GetComponent<DemTile>();
                        if (!tile.resident) tile.SetPulse(rangeColor, Color.white);
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
                // Disable all tiles
                foreach (DemTile tile in allTiles) tile.enabled = false;
                this.enabled = true;
                //Debug.Log(resident.name + " is here");
                // Parse resident info, set name
                BuildInfo info = resident.GetComponent<BuildInfo>();
                nameStat.GetComponent<Text>().text = resident.name;
                // Set infoBox common output data
                infoSpeciesName.GetComponent<Text>().text = resident.name;
                infoBiomass.GetComponent<Text>().text = SpeciesConstants.Biomass(resident.name).ToString();
                infoLore.GetComponent<Text>().text = SpeciesConstants.SpeciesLore(resident.name);
                // Set species type-specific statsBox and infoBox output data
                switch (SpeciesConstants.SpeciesType(resident.name))
                {
                    // Plant: 
                    case 0 :    // STATSBOX //
                                //
                                // Set species type icon and color
                                iconStat.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/plant_icon");
                                iconStat.GetComponent<Image>().color = buildMenu.plantIconColor;

                                // INFOBOX //
                                //
                                // Set species type and icon
                                infoSpeciesType.GetComponent<Text>().text = "Plant";
                                infoSpeciesTypeIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/plant_icon");
                                infoSpeciesTypeIcon.GetComponent<Image>().color = buildMenu.plantIconColor;
                                // Set biomass tier level and icon
                                infoBiomassTier.GetComponent<Text>().text = "1";
                                infoBiomassIcon.GetComponent<Image>().color = buildMenu.tier1BiomassIconColor;
                                // Set remove button icon as active
                                removeBuildButton.SetActive(true);
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
                                iconStat.GetComponent<Image>().color = buildMenu.preyIconColor;
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
                                infoSpeciesTypeIcon.GetComponent<Image>().color = buildMenu.preyIconColor;
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
                                // Set biomass tier level and icon
                                infoBiomassTier.GetComponent<Text>().text = "2";
                                infoBiomassIcon.GetComponent<Image>().color = buildMenu.tier2BiomassIconColor;
                                //
                                // Activate remove button icon, species trait 1; deactivate species trait2
                                removeBuildButton.SetActive(true);
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
                                iconStat.GetComponent<Image>().color = buildMenu.predatorIconColor;
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
                                infoSpeciesTypeIcon.GetComponent<Image>().color = buildMenu.predatorIconColor;
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
                                // Set biomass tier level and icon
                                infoBiomassTier.GetComponent<Text>().text = "3";
                                infoBiomassIcon.GetComponent<Image>().color = buildMenu.tier3BiomassIconColor;
                                //
                                // Activate species trait 1 and 2, deactivate remove button icon
                                removeBuildButton.SetActive(false);
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
                if (easeCoroutine != null) StopCoroutine(easeCoroutine);
                if (easeSimpleCoroutine != null) StopCoroutine(easeSimpleCoroutine);
                simpleTooltip.SetActive(false);
                easeEnumerator = EaseInStatsBox();
                easeFinished = false;
                easeCoroutine = StartCoroutine(easeEnumerator);
            }
        }
    }

    /**
        Activates on mouse exit.
    */
    void OnMouseExit ()
    {
        // Clear board controller's HoveredTile
        main.boardController.HoveredTile = null;
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
                    if
                    (
                        idX + coord[0] > main.boardController.numColumns - 1 || 
                        idX + coord[0] < 0 || 
                        idY + coord[1] > main.boardController.numRows - 1 || 
                        idY + coord[1] < 0
                    )
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
            // Re-enable all tiles
            foreach (DemTile tile in allTiles) tile.enabled = true;

            SignalPulse(false);
            this.GetComponent<Renderer>().material.color = currentColor;
        }

        // Stop any EaseInStatsBox coroutine in progress
        if (easeSimpleCoroutine != null) StopCoroutine(easeSimpleCoroutine);
        if (easeCoroutine != null) StopCoroutine(easeCoroutine);
        easeFinished = true;
        simpleTooltip.SetActive(false);
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
                DemAudioManager.audioUiLoad.Play();

                AddAnimal(main.currentSelection);

                // If building a plant:
                if (buildMenu.currentAnimalFactory.isPlant())
                {
                    int currentBiomass = buildMenu.GetTier1Biomass ();
                    int newBiomass = SpeciesConstants.Biomass (main.currentSelection.name);
                    //buildMenu.UpdateTier1Biomass (currentBiomass - newBiomass);
                    buildMenu.SubtractTier1Biomass(newBiomass);
                    buildMenu.AddTier2Biomass ((int)(newBiomass * 0.5));
                    buildMenu.UpdateTier1Biomass();
                    buildMenu.UpdateTier2Biomass();
                }
                // If building a prey:
                else if (buildMenu.currentAnimalFactory.isPrey())
                {
                    int currentBiomass = buildMenu.GetTier2Biomass();
                    buildMenu.SubtractTier2Biomass (SpeciesConstants.Biomass (main.currentSelection.name));
                }
                // Update build menu locks, set BuildMenu.currentlyBuilding to null after successful placement
                buildMenu.UpdateMenuLocks();
                buildMenu.currentAnimalFactory = null;
                main.currentSelection = null;
                main.boardController.ClearAvailableTiles();

                turnSystem.StallSpawn(1.0f);

                // DEBUG 
                //if (resident)
                //    Debug.Log("Placed " + resident.name + " @ " + resident.GetComponent<Transform>().position);
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
      return resident.GetComponent<BuildInfo>().isPredator ();
    } 
    else if(nextPredator){
      return true;
    }
    return false;
  }

    public void SetResident (GameObject newResident)
    {
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

    this.nextPredator.GetComponent<BuildInfo>().tile = this;

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

    public void RemoveAnimal ()
    {
        //add statistic to tree destroy, and prey eaten
        if (this.resident.GetComponent<BuildInfo> ().isPlant())
            buildMenu.statistic.setTreeDestroy (1);
        else if (this.resident.GetComponent<BuildInfo>().isPrey())
            buildMenu.statistic.setPreyEaten (1);
        Destroy (resident);
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

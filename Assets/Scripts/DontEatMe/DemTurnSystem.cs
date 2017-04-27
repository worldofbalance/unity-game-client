
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemTurnSystem : MonoBehaviour
{
    private  DemBoard board;
    private Dictionary<int, GameObject> activePredators;// = new Dictionary<int, GameObject>();
    private  Queue<DemTween> tweenList = new Queue<DemTween>();
    private  DemTile nextTile;
    private  DemTile currentTile;
    public  bool turnLock = false;
    //private  GameObject predator;
    private DemMain main;
    private GameObject mainObject;
    private DemTweenManager tweenManager;
    DemTile tile;
    private int lives;
    private BuildMenu buildMenu;
    private int credits;
    private int turnNumber;

    private bool paused; // Toggle for the PredatorSpawnRate coroutine
    /** Public accessor for paused */
    public bool Paused { get { return paused; } }
    private bool skip; // Circumvent PredatorSpawnRate delay for current iteration
    private bool resetTimer; // True if timer to be reset to countdownResetPoint
    private float countdown; // Current countdown in seconds
    private float countdownBaseResetPoint, countdownResetPoint; // Countdown reset values
    private bool timerMutex; // Mutex lock for predator turn / player build syncing
    private bool predatorTurnEnded; // True if the current predator move has ended, false otherwise
    /** Public accessor for predatorTurnEnded */
    public bool PredatorTurnEnded
    {
        get { return predatorTurnEnded; }
        set { predatorTurnEnded = value; }
    }

    private Image predatorTurnProgressBar;  // Countdown progress bar (til next predator turn)
    private Text predatorTurnProgressBarText; // Text component of ^
    private Image predatorTurnLevelBg; // Background of predator turn level widget
    private Text predatorTurnLevelText; // Text component of ^
    private int spawnRateShift; // Iterator for shifting the spawn rate (e.g. increasing after spawnRateShiftThreshold turns)
    private int spawnCountShift; // Iterator for shifting the spawn count
    private int spawnRateShiftThreshold; // Threshold for updating spawnRateShift
    private int spawnCountShiftThreshold; // Threshold for updating spawnCountShift
    private int spawnRate; // Current spawn rate factor --> how fast the next predator turn is invoked
    private int spawnCount; // Current spawn count factor --> how many predators to spawn on a turn

    /**
        Defines a simple linear integer boundary.
        The minimum value must be non-negative; the max value must be >= the min value.
        The magnitude is defined as the difference of max and min.
    */
    private struct LinearBounds
    {
        // Bound values
        private int min, max;
        // Min property modifiers
        public int Min
        { 
            get { return min; }
            set { min = Math.Max(0, value); }
        }
        // Max property modifiers
        public int Max
        {
            get { return max; }
            set { max = value >= min ? value : max; }
        }
        // Magnitude property accessor
        public int Magnitude { get { return max - min; } }

        /**
            Constructor.

            @param  min a non-negative integer value
            @param  max a non-negative integer value >= min
        */
        public LinearBounds (int min = 0, int max = 1)
        {
            Min = min;
            Max = max;
        }
    }

    private LinearBounds spawnRateBounds; // Spawn rate upper and lower bounds in [min, max] format 
    private LinearBounds spawnCountBounds; // Spawn count upper and lower bounds in [min, max] format

    private  Coroutine predatorSpawnCoroutine; // Handle for the PredatorSpawnScheduler

    // Progress bar color objects / variables
    private Color progressBarBaseColor, progressBarColor, progressBarDestinationColor;
    private float[] progressBarHSV;
    private float progressBarBaseHue, progressBarDestinationHue;
    private float progressBarAlpha;


    void Awake()
    {
        mainObject = GameObject.Find ("MainObject");
        board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
        main = mainObject.GetComponent<DemMain> ();
        tweenManager = mainObject.GetComponent<DemTweenManager> ();
        buildMenu = mainObject.GetComponent<BuildMenu> ();
        lives = 3;
        credits = 0;
        turnNumber = 0;

        // Initialize predator spawn timer variables / objects
        paused = false;
        skip = false;
        resetTimer = false;
        timerMutex = true;
        predatorTurnEnded = true;
        predatorTurnProgressBar = GameObject.Find("Canvas/mainUI/TurnWidget/TurnWidgetRadialBar/TurnWidgetRadialBarProgress").GetComponent<Image>();
        predatorTurnProgressBarText = GameObject.Find("Canvas/mainUI/TurnWidget/TurnWidgetRadialBar/TurnWidgetRadialBarText").GetComponent<Text>();
        predatorTurnLevelBg = GameObject.Find("Canvas/mainUI/TurnWidget/TurnWidgetRadialBar/TurnWidgetRadialBarLevel").GetComponent<Image>();
        predatorTurnLevelText = GameObject.Find("Canvas/mainUI/TurnWidget/TurnWidgetRadialBar/TurnWidgetRadialBarLevel/LevelText").GetComponent<Text>();

        spawnRateShift = spawnCountShift = 0; // Initialize spawn rate / count shift values
        spawnRateShiftThreshold = 10; // Initialize spawn rate shift value
        spawnCountShiftThreshold = 25; // Initialize spawn count shift value

        // Initialize spawn rate / count bounds
        spawnRateBounds = new LinearBounds(1, 10);
        spawnCountBounds = new LinearBounds(1, 5);

        // Initialize current spawn rate / count values
        spawnRate = spawnRateBounds.Min;
        spawnCount = spawnCountBounds.Min;

        // Set initial countdown /..ResetPoint values
        countdownBaseResetPoint = countdownResetPoint = countdown = 5f;

        // Initialize spawn coroutine
        predatorSpawnCoroutine = null;

        // Set progressBar colors / variables
        progressBarBaseColor = new Color32(0, 255, 183, 220);
        progressBarDestinationColor = new Color32(255, 45, 0, 220);

        float _H = 0, _S = 0, _V = 0;
        Color.RGBToHSV(progressBarDestinationColor, out _H, out _S, out _V);
        progressBarDestinationHue = _H;
        Color.RGBToHSV(progressBarBaseColor, out _H, out _S, out _V);
        progressBarBaseHue = _H;
        progressBarHSV = new float[3]{_H, _S, _V};
        progressBarAlpha = progressBarBaseColor.a;
        progressBarColor = progressBarBaseColor;
    }

    void Start()
    {
        // Update lives
        buildMenu.UpdateLives (lives);

        // Set progress bar and its components' color
        predatorTurnProgressBar.color = progressBarColor;
        predatorTurnLevelBg.color = progressBarColor;
        predatorTurnProgressBarText.color = progressBarColor;
        predatorTurnLevelText.color = progressBarColor;

        // Start spawn coroutine
        predatorSpawnCoroutine = StartCoroutine(StartSpawn(3f));
    }

    /**
        Coroutine for managing the predator spawn timer.

        @param  increment   time in seconds between updates
    */
    IEnumerator PredatorSpawnTimer (float increment = 0.01f)
    {
        // Initialize progress bar to full
        predatorTurnProgressBar.GetComponent<Image>().fillAmount = 1f;
        predatorTurnProgressBarText.text = countdownResetPoint.ToString("0.0");
        // Loop until StopCoroutine is invoked
        while (true)
        {
            // Stall for timerMutex
            yield return new WaitUntil(() => timerMutex);
            // Reset timer and progress bar if flagged
            if (resetTimer) 
            {
                countdown = countdownResetPoint;
                predatorTurnProgressBar.GetComponent<Image>().fillAmount = 1f;
                predatorTurnProgressBarText.text = countdownResetPoint.ToString("0.0");
                resetTimer = false;
            }
            // Begin countdown
            while (countdown > 0)
            {
                // Stall while paused
                if (paused) yield return null;
                // Drop countdown to zero if flagged
                else if (skip) 
                {
                    skip = false;
                    countdown = 0;
                }
                // Otherwise update countdown values and progress bar
                else
                {
                    predatorTurnProgressBar.GetComponent<Image>().fillAmount = countdown / countdownResetPoint;
                    predatorTurnProgressBarText.text = countdown.ToString("0.0");
                    yield return new WaitForSecondsRealtime(increment);
                    countdown -= increment;
                }
                // Update menu locks
                buildMenu.UpdateMenuLocks ();
            }
            // Flag reset timer, increment 
            resetTimer = true;
        }
    }

    /**
        Coroutine for scheduling time-based predator spawning.
        An accretion level may be specified for gradual 

        @param  spawnRateAccretion  coefficient determining the relative step size on spawn rate increase
    */
    IEnumerator PredatorSpawnScheduler (float spawnRateAccretion = 0.075f)
    {
        // Keep accretion in bounds
        spawnRateAccretion = Math.Max(0, Math.Min(1f - (float)1e-6, spawnRateAccretion));
        while (true)
        {
            // Wait on countdown and timer mutex
            yield return new WaitWhile(() => countdown > 0 && timerMutex);
            timerMutex = false;
            countdown = countdownResetPoint;
            // Increment spawnRateShift and spawnCountShift, update accordingly
            // TODO: implement spawnCount functionality to increase the number of predators spawned in a turn
            if (spawnRate < spawnRateBounds.Max && spawnRateAccretion > 0 && (spawnRate + 1) * spawnRateAccretion < 1f)
            {
                spawnRateShift = (spawnRateShift + 1) % spawnRateShiftThreshold;
                if (spawnRateShift == 0)
                {
                    spawnRate ++;
                    predatorTurnLevelText.text = spawnRate.ToString();
                    countdownResetPoint = countdownBaseResetPoint - (countdownBaseResetPoint * spawnRate * spawnRateAccretion);
                    // Calculate hue shift, redefine and apply progress bar color
                    float lerpRatio = ((float)spawnRate - (float)spawnRateBounds.Min) / (float)spawnRateBounds.Magnitude;
                    progressBarHSV[0] = Mathf.Lerp(progressBarBaseHue, progressBarDestinationHue, lerpRatio);

                    progressBarColor = Color.HSVToRGB(progressBarHSV[0], progressBarHSV[1], progressBarHSV[2]);
                    progressBarColor.a = progressBarAlpha;

                    predatorTurnProgressBar.color = progressBarColor;
                    predatorTurnProgressBarText.color = progressBarColor;
                    predatorTurnLevelBg.color = progressBarColor;
                    predatorTurnLevelText.color = progressBarColor;
                }
            }
            if (spawnCount < spawnCountBounds.Max)
            {
                spawnCountShift = (spawnCountShift + 1) % spawnCountShiftThreshold;
                if (spawnCountShift == 0) spawnCount ++;
            }
            // Invoke a new predator turn on current turn completion
            while (!predatorTurnEnded) yield return null;
            PredatorTurn();
        }
    }

    /**
        Coroutine for pausing further predator generation.
        An optional time offset in seconds may be specified to delay the pause.

        @param  timeOffset  a time offset in seconds (default = 0)
    */
    public IEnumerator PauseSpawn (float timeOffset = 0)
    {
        yield return (timeOffset <= 0 ? null : new WaitForSeconds(timeOffset));
        paused = true;
        // Gray out progress bar and its components
        Color pauseColor = Color.gray;
        predatorTurnProgressBar.color = pauseColor;
        predatorTurnLevelBg.color = pauseColor;
        predatorTurnProgressBarText.color = pauseColor;
        predatorTurnLevelText.color = pauseColor;
    }

    /**
        Coroutine for resuming paused predator generation.
        An optional time offset in seconds may be specified to delay the resume.

        @param  timeOffset  a time offset in seconds (default = 0)
    */
    public IEnumerator ResumeSpawn (float timeOffset = 0)
    {
        yield return (timeOffset <= 0 ? null : new WaitForSeconds(timeOffset));
        paused = false;
        // Restore color of progress bar and its components
        predatorTurnProgressBar.color = progressBarColor;
        predatorTurnLevelBg.color = progressBarColor;
        predatorTurnProgressBarText.color = progressBarColor;
        predatorTurnLevelText.color = progressBarColor;
    }

    /**
        Coroutine for stalling predator generation for a specified duration.
        An optional time offset in seconds may be specified to delay the resume.

        @param  delay   a delay in seconds (default = 0.5)
    */
    public void StallSpawn (float delay = 0.5f)
    {
        // Pause immmediately
        paused = true;
        // Gray out progress bar and its components
        Color pauseColor = Color.gray;
        predatorTurnProgressBar.color = pauseColor;
        predatorTurnLevelBg.color = pauseColor;
        predatorTurnProgressBarText.color = pauseColor;
        predatorTurnLevelText.color = pauseColor;
        // Schedule resume, yield null (keep delay in range)
        delay = Math.Max((float)1e-6, delay);
        StartCoroutine(ResumeSpawn(delay));
    }
    /**
        Coroutine for toggling predator generation paused state.
        An optional time offset in seconds may be specified to delay the toggle.

        @param  timeOffset  a time offset in seconds (default = 0)
    */
    public IEnumerator TogglePauseSpawn (float timeOffset = 0)
    {
        yield return (timeOffset <= 0 ? null : new WaitForSeconds(timeOffset));
        paused = !paused;
        // If paused, gray out progress bar and its components
        if (paused)
        {
            Color pauseColor = Color.gray;
            predatorTurnProgressBar.color = pauseColor;
            predatorTurnLevelBg.color = pauseColor;
            predatorTurnProgressBarText.color = pauseColor;
            predatorTurnLevelText.color = pauseColor;
        }
        // Else restore color of progress bar and its components
        else
        {
            predatorTurnProgressBar.color = progressBarColor;
            predatorTurnLevelBg.color = progressBarColor;
            predatorTurnProgressBarText.color = progressBarColor;
            predatorTurnLevelText.color = progressBarColor;
        }
    }

    /**
        Coroutine for stopping any further predator generation.
        An optional time offset in seconds may be specified to delay the stop.

        @param  timeOffset  a time offset in seconds (default = 0)
    */
    public IEnumerator StopSpawn (float timeOffset = 0)
    {
        yield return (timeOffset <= 0 ? null : new WaitForSeconds(timeOffset));
        if (predatorSpawnCoroutine != null) StopCoroutine(predatorSpawnCoroutine);
    }

    /**
        Coroutine for starting time-based predator generation.
        An optional time offset in seconds may be specified to delay the stop.

        @param  timeOffset  a time offset in seconds (default = 0)
    */
    public IEnumerator StartSpawn (float timeOffset = 0)
    {
        yield return (timeOffset <= 0 ? null : new WaitForSecondsRealtime(timeOffset));
        // Define and start predatorSpawnCoroutine
        predatorSpawnCoroutine = StartCoroutine(PredatorSpawnScheduler());
        StartCoroutine(PredatorSpawnTimer());
    }

    /**
        Skips to the next predator turn.
        An optional time offset in seconds may be specified to delay the stop.

        @param  timeOffset  a time offset in seconds (default = 0)
        @param  onComplete  an Action to execute on completion (default = null)
    */
    public IEnumerator Skip (float timeOffset = 0, Action onComplete = null)
    {
        yield return (timeOffset <= 0 ? null : new WaitForSeconds(timeOffset));
        yield return new WaitUntil(() => timerMutex);
        skip = true;
        if (onComplete != null) onComplete();
    }
	
    /*
        TODO:   consolidate PredatorTurn and PredatorFinishedMove into a single method;
                consider also consolidating PredatorExit.
                At present, the divorce of PredatorTurn and PredatorFinishedMove creates a convoluted process structure.

        TODO:   reformat to proper standards (tabs = 4-space expanded), eliminate superfluous newlines, add some annotations, etc.
    */
    public void PredatorTurn()
    {
        skip = timerMutex = predatorTurnEnded = false;
        turnNumber++;
        buildMenu.ToggleButtonLocks();
        activePredators = board.GetPredators();
        Debug.Log ("Total predators :" + activePredators.Count);
        foreach(KeyValuePair<int, GameObject> predatorEntry in activePredators)
        {
            PredatorInfo predator = predatorEntry.Value.GetComponent<PredatorInfo>();
            int x = predator.GetTile().GetIdX();
            int y = predator.GetTile().GetIdY();

            currentTile = board.Tiles [x, y].GetComponent<DemTile>();

            if (x + 1 == board.numColumns)
            {
                Debug.Log("arrived at final tile");
                tweenList.Enqueue (new DemPredatorExitTween(predator.gameObject, 500));
            } 
            else
            {
                nextTile = board.Tiles [x + 1, y].GetComponent<DemTile>();
                predator.SetNextTile(nextTile);
                tweenList.Enqueue(new DemTileTransitionTween(predator.gameObject, nextTile.GetCenter(), 500));
            }

        }

        buildMenu.statistic.setTurnCount(1);
        if (main.currentSelection != null) 
        {
            if (board.HoveredTile != null) board.HoveredTile.UpdateOnMouseEnter();
            board.SetAvailableTiles();
        }

        if (turnNumber % 2 == 1) GenerateNewPredators();
        ProcessTweens();

        timerMutex = true;
    }

    public void PredatorFinishedMove (GameObject finishedPredator)
    {
        PredatorInfo predator = finishedPredator.GetComponent<PredatorInfo>();
        bool markedForDeletion = false;

        if (predator != null && predator.GetNextTile().resident != null)
        {
            BuildInfo nextAnimal = predator.GetNextTile().resident.GetComponent<BuildInfo>();

            // TODO: check predator/prey relationship, ignore illegitimate prey
            // FIXME: also, this check is redundant since the next two conditionals check for the same thing
            //if(nextAnimal.isPrey() || nextAnimal.isPlant())
                

            // If predator collides with a prey
            if(nextAnimal.isPrey())
            {
                // Remove current prey
                predator.GetNextTile().RemoveAnimal();
                // Add Tier2 biomass back to the pool
                buildMenu.AddTier2Biomass (SpeciesConstants.Biomass(nextAnimal.name));

                // Check if prey is valid for the predator
                // NOTE: for now, the predator's hunger will be satisfied so long as the prey is valid
                if (predator.IsPredatorOf(nextAnimal))
                {
                    DemAudioManager.audioSelection.Play();
                    credits++;
                    buildMenu.UpdateCredits (credits);
                    markedForDeletion = true;

                    // If the prey was valid, Tier 1 biomass will increase (circle of life, carcass fertilizes the ground,
                    // some such something or other)
                    // The amount of Tier 1 biomass added is equal to HALF of the prey's biomass
                    buildMenu.AddTier1Biomass(SpeciesConstants.Biomass(nextAnimal.name) / 2);
                }
                // Otherwise, just play a failing noise for now; will update later...
                else DemAudioManager.audioFail.Play();
            }
            // If predator collides with a plant...
            // NOTE: for now, no matter the plant or predator, the plant gets destroyed... not realistic or ideal, but
            // this is the way things were programmed at first. Hopefully I'll have time to fix it.
            else if (nextAnimal.isPlant())
            {
                // Remove current plant
                predator.GetNextTile ().RemoveAnimal();
                // Update biomass levels
                buildMenu.AddTier1Biomass(SpeciesConstants.Biomass(nextAnimal.name));
                buildMenu.SubtractTier2Biomass((int)(SpeciesConstants.Biomass(nextAnimal.name) * 0.5));
            }
        }

        if (markedForDeletion)
        {
            buildMenu.SubtractTier3Biomass(SpeciesConstants.Biomass(predator.name));
            DemTile tempTile = predator.GetTile();
            activePredators.Remove(finishedPredator.GetInstanceID());
            tempTile.RemoveAnimal();
        }

        if (main.currentSelection != null) 
        {
            if (board.HoveredTile != null) board.HoveredTile.UpdateOnMouseEnter();
            board.SetAvailableTiles();
        }

        predator.AdvanceTile();
        ProcessTweens();
    }


    public void PredatorExit (GameObject finishedPredator)
    {
        PredatorInfo predator = finishedPredator.GetComponent<PredatorInfo>();
        predator.GetTile().RemoveAnimal();
        buildMenu.UpdateLives(lives--);

        if (lives == 0)
        {
            activePredators.Remove(finishedPredator.GetInstanceID());
            tweenList.Clear();
            GameOver();
        } 
        else
        {
            buildMenu.SubtractTier3Biomass(SpeciesConstants.Biomass(predator.name));
            activePredators.Remove(finishedPredator.GetInstanceID());
            ProcessTweens();
        }
    }

    public  void GenerateNewPredators()
    {
        //For Testing
        int random = UnityEngine.Random.Range (0, 5);
        int randomPredator = UnityEngine.Random.Range (0, main.predators.Length);
        GameObject newPredator = main.predators[randomPredator].Create();
        board.AddNewPredator(0, random, newPredator);

        tweenList.Enqueue(new DemPredatorEnterTween (newPredator, 700));
        buildMenu.AddTier3Biomass(SpeciesConstants.Biomass (newPredator.GetComponent<PredatorInfo>().name));
    }

    public void ProcessTweens()
    {
        if (tweenList.Count > 0) tweenManager.AddTween(tweenList.Dequeue());
        else
        {
            // Clear tween list, update button locks
            tweenList.Clear();
            buildMenu.ToggleButtonLocks();
            buildMenu.UpdateMenuLocks();
            // Update biomass levels
            buildMenu.UpdateTier1Biomass();
            buildMenu.UpdateTier2Biomass();
            buildMenu.UpdateTier3Biomass();
            // Flag predator turn as ended, re-enable skip turn button
            predatorTurnEnded = true;
            buildMenu.skipTurnButton.GetComponent<Button>().enabled = true;
        }
    }

    public int GetLives ()
    {
        return lives;
    }

    public  bool IsTurnLocked ()
    {
        return !turnLock;
    }

    public void GameOver ()
    {
        Debug.Log ("game Over");
        buildMenu.EndGame ();
    }
}

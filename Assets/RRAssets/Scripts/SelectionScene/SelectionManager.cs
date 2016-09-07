using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RR
{
    /// <summary>
    /// Manager for player selection screen
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {
        // Canvas container
        private GameObject selectionCanvas;

        // Main GameObject
        private GameObject mainObject;

        // Storage of changeable buttons
        private GameObject[] speciesButtons;

        // Storage of images on them
        private GameObject[] buttonImages;

        // Submit object that holds an submit button
        // for forwarding to the game
        private GameObject submitObject;

        // Canvas for species chart (who eats who)
        private GameObject chart;

        // Current index of selected species button
        private int currentSpeciesIndex;

        // Species intended to run throughout the game
        private int selectedSpecies;

        // Connection Manager
        private RR.RRConnectionManager rrcm;
    
        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods is called the first time.
        /// </summary>
        void Start ()
        {
            mainObject = GameObject.Find ("MainObject");
            rrcm = RR.RRConnectionManager.getInstance ();

            //Selection Canvas Initialization
            selectionCanvas = new GameObject ();
            selectionCanvas.name = "SelectionCanvas";

            // Main canvas
            Canvas canvas = selectionCanvas.AddComponent<Canvas> ();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Scaler
            CanvasScaler cs = selectionCanvas.AddComponent<CanvasScaler> ();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Raycaser (for selection)
            GraphicRaycaster gRay = selectionCanvas.AddComponent<GraphicRaycaster> ();

            // Set currently selected button with runnable species
            currentSpeciesIndex = 0;
    
            // Initialize Buttons
            initButtons ();

            //        NetworkRequestTable.init();
            //        NetworkResponseTable.init();

            RR.RRMessageQueue.getInstance ().AddCallback (RR.Constants.SMSG_RRSTARTGAME, ResponseRRStartGame);
            RR.RRMessageQueue.getInstance ().AddCallback (RR.Constants.SMSG_RRGETMAP, ResponseRRGetMap);

            RequestRRGetMap reqmap = new RequestRRGetMap ();
            reqmap.Send ();
            rrcm.Send (reqmap);

        }

        void OnDestroy ()
        {
            RRMessageQueue.getInstance ().RemoveCallback (RR.Constants.SMSG_RRSTARTGAME);
            RRMessageQueue.getInstance ().RemoveCallback (RR.Constants.SMSG_RRGETMAP);
        }
    
        void Update ()
        { 
            if (selectedSpecies != null) {
                for (int i = 0; i < speciesButtons.Length; i++) {
                    if (i != selectedSpecies) {
                        Button b = speciesButtons [i].GetComponent<Button> ();
                        ColorBlock cb = b.colors;
                        cb.normalColor = Color.white;
                        cb.highlightedColor = Color.white;
                        b.colors = cb;
                    }
                }
            }
        }

        /// <summary>
        /// Inits the buttons in GUI.
        /// </summary>
        private void initButtons ()
        {

            // Init the carousel
            initSpeciesButtons ();
            setButtonActive (currentSpeciesIndex);

            /** BACK BUTTON*/

            GameObject bBack = new GameObject ();
            bBack.name = "backButton";
            bBack.transform.parent = selectionCanvas.transform;
    
            RectTransform RT = bBack.AddComponent<RectTransform> ();
            RT.anchoredPosition = new Vector2 (0f, 0f);
            RT.sizeDelta = new Vector2 (25, 25);
            RT.localPosition = new Vector3 (-150f, -75f, 0f);
            RT.localScale = new Vector3 (3f, 3f, 1f);
    
            Button But = bBack.AddComponent<Button> ();
            But.transition = Selectable.Transition.ColorTint;
    
            Image img = bBack.AddComponent<Image> ();
            Sprite t = Resources.Load <Sprite> ("Prefabs/UI/backButton");
            img.sprite = t;
            But.targetGraphic = img;
            img.preserveAspect = true;
    
            But.onClick.AddListener (() => moveCarousel (false));
    
            /** NEXT BUTTON*/

            GameObject bNext = new GameObject ();
            bNext.name = "forwardButton";
            bNext.transform.parent = selectionCanvas.transform;
    
            RectTransform RT2 = bNext.AddComponent<RectTransform> ();
            RT2.anchoredPosition = new Vector2 (0f, 0f);
            RT2.sizeDelta = new Vector2 (25, 25);
            RT2.localPosition = new Vector3 (150f, -75f, 0f);
            RT2.localScale = new Vector3 (3f, 3f, 1f);
    
            Button But2 = bNext.AddComponent<Button> ();
            But2.transition = Selectable.Transition.ColorTint;
    
            Image img2 = bNext.AddComponent<Image> ();
            Sprite t2 = Resources.Load <Sprite> ("Prefabs/UI/forwardButton");
            img2.sprite = t2;
            But2.targetGraphic = img2;
            img2.preserveAspect = true;
    
            But2.onClick.AddListener (() => moveCarousel ());
    
            /** Submit Button*/

            submitObject = new GameObject ();
            submitObject.name = "forwardButton";
            submitObject.transform.parent = selectionCanvas.transform;
    
            RectTransform RT3 = submitObject.AddComponent<RectTransform> ();
            RT3.anchoredPosition = new Vector2 (0f, 0f);
            RT3.sizeDelta = new Vector2 (100, 25);
            RT3.localPosition = new Vector3 (0f, -225f, 0f);
            RT3.localScale = new Vector3 (3f, 3f, 1f);
    
            Button But3 = submitObject.AddComponent<Button> ();
            But3.transition = Selectable.Transition.ColorTint;
    
            Image img3 = submitObject.AddComponent<Image> ();
            Sprite t3 = Resources.Load<Sprite> ("Prefabs/UI/playButton");
            img3.sprite = t3;
            But3.targetGraphic = img3;
            img3.preserveAspect = true;
    
            But3.onClick.AddListener (() => goToRunnerScene ());
    
            submitObject.SetActive (false);
    
            /** Feeding Chart */
            
            chart = new GameObject ();
            chart.name = "PredatorPreyChart";
            chart.transform.parent = selectionCanvas.transform;
    
            RectTransform rtChart = chart.AddComponent<RectTransform> ();
            rtChart.anchoredPosition = new Vector2 (0f, 0f);
            rtChart.sizeDelta = new Vector2 (400, 75);
            rtChart.localPosition = new Vector3 (0f, 150f, 0f);
            rtChart.localScale = new Vector3 (3f, 3f, 1f);
    
            Image cImg = chart.AddComponent<Image> ();
            Sprite cSprite = Resources.Load<Sprite> ("Prefabs/UI/feedingChart");
            cImg.sprite = cSprite;
            cImg.preserveAspect = true;
    
        }

        /// <summary>
        /// Inits the species buttons "carousel".
        /// </summary>
        void initSpeciesButtons ()
        {
            speciesButtons = new GameObject[5];
            buttonImages = new GameObject[5];
    
            /** Species buttons*/
            for (int i = 0; i < 5; i++) {
                speciesButtons [i] = new GameObject ();
                buttonImages [i] = new GameObject ();
    
                // Naming button and setting as child of canvas
                speciesButtons [i].name = "Species " + i;
                buttonImages [i].name = "Avatar " + i;
                speciesButtons [i].transform.parent = selectionCanvas.transform;
                buttonImages [i].transform.parent = speciesButtons [i].transform;
    
                //Species button :: Placing the Button on the canvas
                RectTransform rectTrans = speciesButtons [i].AddComponent<RectTransform> ();
                rectTrans.anchoredPosition = new Vector2 (0f, 0f);
                rectTrans.sizeDelta = new Vector2 (100, 100);
                rectTrans.localScale = new Vector3 (2f, 2f, 1f);
    
                //Button Image :: Placing the image inside the  button
                RectTransform rT = buttonImages [i].AddComponent<RectTransform> ();
                rT.anchoredPosition = new Vector2 (0f, 0f);
                rT.sizeDelta = new Vector2 (37, 30);
                rT.localScale = new Vector3 (1f, 1f, 1f);
                
                //Setting a transition (ColorTint) on mouseClick
                Button s = speciesButtons [i].AddComponent<Button> ();
                s.transition = Selectable.Transition.ColorTint;
    
                //Species Button :: Setting the button image
                Image image = speciesButtons [i].AddComponent<Image> ();
                Sprite temporaryButtonPrefab = Resources.Load<Sprite> ("Prefabs/UI/speciesButton");
                image.preserveAspect = true;

                //Species Button :: Setting the button image
                Image img = buttonImages [i].AddComponent<Image> ();
                Sprite temporaryButtonAvatar = Resources.Load<Sprite> ("Art/Avatars/avatar" + i);
                img.preserveAspect = true;

                // Are all the files present?
                if (temporaryButtonPrefab != null && temporaryButtonAvatar != null) {
                    image.sprite = temporaryButtonPrefab;
                    s.targetGraphic = image;
    
                    img.sprite = temporaryButtonAvatar;

                    float xy = (i == 1)? 1.25f : 2f;
                    img.rectTransform.localScale = new Vector3 (xy, xy, 1f);
    
                } else {
                    Debug.LogError ("Temporary button prefab or avatar for id " + i + " does not exists.");
                }

                Debug.Log (i);

                // This weird construction is necessary due to the anonymous scope
                switch (i) {
                case 0:
                    s.onClick.AddListener (() => selectSpecies (speciesButtons [0], 0));
                    break;
                case 1:
                    s.onClick.AddListener (() => selectSpecies (speciesButtons [1], 1));
                    break;
                case 2:
                    s.onClick.AddListener (() => selectSpecies (speciesButtons [2], 2));
                    break;
                case 3:
                    s.onClick.AddListener (() => selectSpecies (speciesButtons [3], 3));
                    break;
                case 4:
                    s.onClick.AddListener (() => selectSpecies (speciesButtons [4], 4));
                    break;
                }
                speciesButtons [i].SetActive (false);
            }
        }

        /// <summary>
        /// Sets the button of the carousel active.
        /// Value 5 stands for a submit button. (Weird?)
        /// </summary>
        /// <param name="num">Button index.</param>
        void setButtonActive (int num)
        {
            // 5 stands for a submit button
            // TODO cope with this as this is certainly not a good pracitce
            if (num == 5) {
                submitObject.SetActive (true);
                return;
            }

            // Placing the Button on the canvas
            // Drawing one the proper coordinates
            RectTransform RectTrans = speciesButtons [num].GetComponent<RectTransform> ();
            RectTrans.localPosition = new Vector3 (0f, -75f, 0f);
            speciesButtons [num].SetActive (true);
        }

        /// <summary>
        /// Deactivates the button.
        /// Value 5 stands for a submit button. (Weird?)
        /// </summary>
        /// <param name="num">Button index.</param>
        void setButtonInactive (int num)
        {
            if (num == 5) {
                submitObject.SetActive (false);
            } else
            {
                speciesButtons [num].SetActive (false);
            }
        }

        /// <summary>
        /// Switches button of species in a carousel forwards
        /// </summary>
        /// <param name="forward">If set to <c>true</c> forward.</param>
        private void moveCarousel (bool forward = true) {

            int max = (forward) ? 4 : 0;
            int min = (forward) ? 0 : 4; 
            int step = (forward) ? 1 : -1;

            // Set current button inactive
            setButtonInactive (currentSpeciesIndex);

            // Set current button index
            currentSpeciesIndex = (currentSpeciesIndex == max)? min : currentSpeciesIndex += step;

            // Set current button active
            setButtonActive (currentSpeciesIndex);
        }

        void selectSpecies (GameObject species, int num)
        {
            Debug.Log (species);
            setButtonActive (5);
            selectedSpecies = num + 1;

            PlayerPrefs.SetInt ("species1", num + 1);    
    
            // green would be the following rgb value --> new Color (138,141,93,1);
            Button b = species.GetComponent<Button> ();
            ColorBlock cb = b.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            b.colors = cb;

            //Pass selected species information to the game manager
            //lets the game manager know which species to initialize
        }

        void goToRunnerScene ()
        {
            //        int temp = selectSpecies;
            //        PlayerPrefs.SetInt ("species1", temp);    

            RequestRRSpecies rs = new RequestRRSpecies ();
            rs.send (selectedSpecies);

            if (rrcm) {
                rrcm.Send (rs);
            }

            RequestRRStartGame request = new RequestRRStartGame ();
            Debug.Log ("Sending request for start game with player id " + RR.Constants.USER_ID);
            request.Send (RR.Constants.USER_ID);
            rrcm.Send (request);
//                    Application.LoadLevel("CountdownScene");

            // Give the client a message about waiting for the other player to finish selecting.  Hide the PLAY button so 
            // player can't send another RequestRRStartGame.  It is cruicial only one RequestRRstartGame is sent from each
            // player.
        }

        public void ResponseRRStartGame (ExtendedEventArgs eventArgs)
        {
            Debug.Log ("ResponseRRStartGame has been called from Selection Manager.cs");

            ResponseRRStartGameEventArgs args = eventArgs as ResponseRRStartGameEventArgs;

            if (args.status == 0) {
                Application.LoadLevel ("RRCountdownScene");
            } else {
//            Join();
            }
        }

        public void ResponseRRGetMap (ExtendedEventArgs eventArgs)
        {
            ResponseRRGetMapEventArgs args = eventArgs as ResponseRRGetMapEventArgs;
            GameManager.mapSeed = args.mapNumber;
            //Debug.Log("Get map seed from server! The number is " + args.mapNumber);
        }

    }
}
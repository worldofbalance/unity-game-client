using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RR
{
	public class SelectionManager : MonoBehaviour
	{
    
		private GameObject gObj;
		private GameObject[] speciesButtons;
		private GameObject[] buttonImages;
		private GameObject submit;
		private GameObject chart;
		private int spot1, spot2;
		private int selectedSpecies;
		private GameObject mainObject;
		private RRConnectionManager cManager;
    
		// Use this for initialization
		void Start ()
		{
			//Canvas Initialization
			gObj = new GameObject ();
			gObj.name = "SelectionCanvas";
			Canvas canvas = gObj.AddComponent<Canvas> ();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			CanvasScaler cs = gObj.AddComponent<CanvasScaler> ();
			cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			GraphicRaycaster gRay = gObj.AddComponent<GraphicRaycaster> ();
    
			//Button stored by position
			spot1 = 0;
			spot2 = 1;
    
			//Initialize Buttons
			initButtons ();
    
			mainObject = GameObject.Find ("MainObject");
    		
			cManager = RRConnectionManager.getInstance ();

    		
			//		NetworkRequestTable.init();
			//		NetworkResponseTable.init();

			RRMessageQueue.getInstance ().AddCallback (Constants.SMSG_RRSTARTGAME, ResponseRRStartGame);
			RRMessageQueue.getInstance ().AddCallback (Constants.SMSG_RRGETMAP, ResponseRRGetMap);

			RequestRRGetMap reqmap = new RequestRRGetMap ();
			reqmap.Send ();
			cManager.Send (reqmap);

		}

		void OnDestroy ()
		{
			RRMessageQueue.getInstance ().RemoveCallback (Constants.SMSG_RRSTARTGAME);
			RRMessageQueue.getInstance ().RemoveCallback (Constants.SMSG_RRGETMAP);
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
    
		private void initButtons ()
		{
    
			initSpeciesButtons ();
			setButtonActive (spot1);
			//setButtonActive(spot2);
    
			/*
    		 * BACK BUTTON
    		 */
			GameObject bBack = new GameObject ();
			bBack.name = "backButton";
			bBack.transform.parent = gObj.transform;
    
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
    
			But.onClick.AddListener (() => previous ());
    
			/*
    		 * NEXT BUTTON
    		 */
			GameObject bNext = new GameObject ();
			bNext.name = "forwardButton";
			bNext.transform.parent = gObj.transform;
    
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
    
			But2.onClick.AddListener (() => next ());
    
			/*
             * Submit Button
             */
			submit = new GameObject ();
			submit.name = "forwardButton";
			submit.transform.parent = gObj.transform;
    
			RectTransform RT3 = submit.AddComponent<RectTransform> ();
			RT3.anchoredPosition = new Vector2 (0f, 0f);
			RT3.sizeDelta = new Vector2 (100, 25);
			RT3.localPosition = new Vector3 (0f, -225f, 0f);
			RT3.localScale = new Vector3 (3f, 3f, 1f);
    
			Button But3 = submit.AddComponent<Button> ();
			But3.transition = Selectable.Transition.ColorTint;
    
			Image img3 = submit.AddComponent<Image> ();
			Sprite t3 = Resources.Load<Sprite> ("Prefabs/UI/playButton");
			img3.sprite = t3;
			But3.targetGraphic = img3;
			img3.preserveAspect = true;
    
			But3.onClick.AddListener (() => goToRunnerScene ());
    
			submit.SetActive (false);
    
			/*
             * Feeding Chart 
             */
			chart = new GameObject ();
			chart.name = "PredatorPreyChart";
			chart.transform.parent = gObj.transform;
    
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
    
		void initSpeciesButtons ()
		{
			speciesButtons = new GameObject[5];
			buttonImages = new GameObject[5];
    
			/*
             * Species buttons
             */
			for (int i = 0; i < 5; i++) {
				speciesButtons [i] = new GameObject ();
				buttonImages [i] = new GameObject ();
    
				// Naming button and setting as child of canvas
				speciesButtons [i].name = "Species " + i;
				buttonImages [i].name = "Avatar " + i;
				speciesButtons [i].transform.parent = gObj.transform;
				buttonImages [i].transform.parent = speciesButtons [i].transform;
    
				//Species button :: Placing the Button on the canvas
				RectTransform RectTrans = speciesButtons [i].AddComponent<RectTransform> ();
				RectTrans.anchoredPosition = new Vector2 (0f, 0f);
				RectTrans.sizeDelta = new Vector2 (100, 100);
				RectTrans.localScale = new Vector3 (2f, 2f, 1f);
    
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
				Sprite temp = Resources.Load<Sprite> ("Prefabs/UI/speciesButton");
				image.preserveAspect = true;
    
				//Species Button :: Setting the button image
				Image img = buttonImages [i].AddComponent<Image> ();
				Sprite t = Resources.Load<Sprite> ("Art/Avatars/avatar" + i);
				img.preserveAspect = true;
    			
				if (temp != null && t != null) {
					image.sprite = temp;
					s.targetGraphic = image;
    
					img.sprite = t;
					if (i == 1)
						img.rectTransform.localScale = new Vector3 (1.25f, 1.25f, 1f);
					else
						img.rectTransform.localScale = new Vector3 (2f, 2f, 1f);
    
				} else
					Debug.LogError ("Sprite is null");
    			
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
    
		void setButtonActive (int num)
		{
    
			if (num == 5) {
    
				submit.SetActive (true);
    
				return;
			}
    
			//Placing the Button on the canvas
			//Drawing one the proper coordinates
			RectTransform RectTrans = speciesButtons [num].GetComponent<RectTransform> ();
    
			RectTrans.localPosition = new Vector3 (0f, -75f, 0f);
    
			speciesButtons [num].SetActive (true);
		}
    
		void setButtonInactive (int num)
		{
			if (num == 5)
				submit.SetActive (false);
			else
				speciesButtons [num].SetActive (false);
		}
    
		void next ()
		{
            
			/*
            int temp1, temp2;
            temp1 = spot1;
            temp2 = spot2;
    
            Debug.Log("Next has been called");
            
            if (spot2 == 4)
            {
                spot2 = 0;
                spot1 = 4;
            }
            else if (spot1 == 4)
            {
                spot1 = 0;
                spot2 = 1;
            }
            else
            {
                spot1 += 1;
                spot2 += 1;
            }
    
            //Set the correct buttons to active
            setButtonInactive(temp1);
            setButtonInactive(temp2);
            setButtonActive(spot1);
            setButtonActive(spot2);
             */
    
			setButtonInactive (spot1);
    
			if (spot1 == 4) {
				spot1 = 0;
				setButtonActive (spot1);
			} else {
				spot1++;
				setButtonActive (spot1);
			}
		}
    
		void previous ()
		{
    
			/*
            int temp1, temp2;
            temp1 = spot1;
            temp2 = spot2;
    
            Debug.Log("Previous has been called");
    
            if (spot2 == 0)
            {
                spot2 = 4;
                spot1 = 3;
            }
            else if (spot1 == 0)
            {
                spot1 = 4;
                spot2 = 0;
            }
            else
            {
                spot1 -= 1;
                spot2 -= 1;
            }
    
            //Set the correct buttons to active
            setButtonInactive(temp1);
            setButtonInactive(temp2);
            setButtonActive(spot1);
            setButtonActive(spot2);
             */
    
			setButtonInactive (spot1);
    
			if (spot1 == 0) {
				spot1 = 4;
				setButtonActive (spot1);
			} else {
				spot1--;
				setButtonActive (spot1);
			}
		}
    
		void selectSpecies (GameObject species, int num)
		{
			Debug.Log (species);
			setButtonActive (5);
			selectedSpecies = num + 1;
    
			PlayerPrefs.SetInt ("species1", num + 1);	
    
			//green would be the following rgb value --> new Color (138,141,93,1);
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
			//		int temp = selectSpecies;
			//		PlayerPrefs.SetInt ("species1", temp);	
    		
			RequestRRSpecies rs = new RequestRRSpecies ();
			rs.send (selectedSpecies);
    		
    		
			if (cManager) {
				cManager.Send (rs);
			}
    		
    		
			RequestRRStartGame request = new RequestRRStartGame ();
			request.Send (Constants.USER_ID);
			cManager.Send (request);
			//		Application.LoadLevel("CountdownScene");
    		
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
				//			Join();
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
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RR {
	public class TestingGameManager : MonoBehaviour {
		
		public GameObject player1;
		public GameObject player2;
		public GameObject endFlag;
		public GameObject item1;
		public Canvas canvas;
	
	
		public ArrayList items;
		private GameCamera cam;
		private float raceTime;
		private static float startPoint = 0;
		private static float endPoint;
		
		// Use this for initialization
		void Start () {
	
			//Testing countdown
			Countdown ();
	
			cam = GetComponent<GameCamera> ();
			SpawnMap();
	
			SpawnPlayer ();
	
	
      player2 = Instantiate(player2, Vector3.zero, Quaternion.identity) as GameObject;
      player2.name = "Player_sprite_2(Clone)";
	
			raceTime = 0;
			//Debug.Log("Before!!!!!!!!!");
			
			items = new ArrayList();
			//repeat until none
			// item = new GO();
			// PlaItem(item, xy);
			// items.Add (items);
			
			GameObject.Find("GameLogic").GetComponent<Running>().RunOnce();
		}
		
		private void SpawnPlayer() {
      player1 = Instantiate (player1, new Vector3(-19f, 0f,0f), Quaternion.identity) as GameObject;
			cam.SetTarget(player1.transform);
      player1.name = "Player_sprite(Clone)";
		}
		
		private void SpawnMap()
		{
			GameObject[] MapVars = new GameObject[10];
			for (int i = 0; i < 2; i++) {
				MapVars[i] = Resources.Load("Prefabs/Maps/MapVariation_" + i) as GameObject;
			}
	
			float tempEnd = 20;
      int mapUnitLength = 1;
			for (int i = 0; i < mapUnitLength; i++)
			{
        tempEnd += 62.9f;
				Instantiate(MapVars[Random.Range (0,2)], new Vector3((float)(20 + (i * 62.9)), 0.507454f, 0), Quaternion.identity);
			}
			
			endPoint = tempEnd - 62.9f;
			Instantiate(endFlag, new Vector3(endPoint, 0.507454f, 0), Quaternion.identity);
		} 
		
		private void PlaceItem(GameObject itemType, Vector2 vect) {
			Instantiate (itemType, Vector3.zero, Quaternion.identity);
		}
	
		private void Countdown(){
			//Logic for game countdown
			GameObject temp = Resources.Load<GameObject> ("Art/Animations/Numbers/num0");
			temp.transform.localScale = new Vector3 (3f, 3f, 1f);
			Instantiate (temp, new Vector3(-19f, -2.5f,0f), Quaternion.identity);
		}
	}
}
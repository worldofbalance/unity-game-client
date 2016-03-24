using UnityEngine;
using System.Collections;

public class DemMain : MonoBehaviour {
  public float fieldOfView = 60;

	// Use this for initialization
	void Start () {


    Material grass1 = (Material)Resources.Load("DontEatMe/Materials/tile_1", typeof(Material));
    Material grass2 = (Material)Resources.Load("DontEatMe/Materials/tile_2", typeof(Material));




    //Setup Play board
    GameObject board = new GameObject("GameBoard");
  

    for(int x = 0; x < 9; x++){
      
      for (int y = 0; y < 5; y++) {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = board.transform;
        cube.transform.position = new Vector3(x-2, y-2, -1);
       
        cube.name = x + "," + y;

        if ((x % 2) == (y % 2)) {
          cube.GetComponent<Renderer> ().material = grass1;

        } else {
          cube.GetComponent<Renderer> ().material = grass2;
        }


      }
        
    }


    //Adjusting camera now

    //Get the highest object needed for game
    float bottom = GameObject.Find ("0,0").GetComponent<BoxCollider> ().bounds.min.y;

    //Get lowest object needed for game
    float top = GameObject.Find ("0,4").GetComponent<BoxCollider> ().bounds.max.y;

    //Horizontally allign the camera
    float horizontalCenter = (top + bottom) / 2;
    Vector3 newPosition = Camera.main.transform.position;
    newPosition.y = horizontalCenter;
    Camera.main.transform.position = newPosition;


    //Now aligning the background so that it is always in the view port, and scaled as best it can

    GameObject background = GameObject.Find ("DemBackGround");
    float bgRight= background.GetComponent<BoxCollider> ().bounds.max.x;
    float bgLeft= background.GetComponent<BoxCollider> ().bounds.min.x;
    float bgClose = background.GetComponent<BoxCollider> ().bounds.min.z;


    float bgDistance = Vector3.Distance(background.transform.position, Camera.main.transform.position);
    Vector3 viewTopMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, bgDistance));
    Vector3 viewBottomMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f,bgDistance));

    Vector3 viewLeftMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, bgDistance));
    Vector3 viewRightMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f,bgDistance));


    float bgRatio = 526f / 350f;
    Vector3 scale = viewTopMiddleBg - viewBottomMiddleBg;
    Vector3 scaleX = viewLeftMiddleBg - viewRightMiddleBg;
    //set a min for x scale
    if (-scaleX.x < 14f) {
      scaleX.x = -14f;
    }

    scale.z = background.transform.localScale.z;
    scale.x = -scaleX.x;
    scale.y = -scale.y;
    background.transform.localScale = scale;


    //Debug.Log(Camera.main.WorldToViewportPoint (new Vector3(1, 1, camera.nearClipPlane));

	
	}
	
	// Update is called once per frame
	void Update () {
   // Camera.main.fieldOfView = fieldOfView * (16f/9f) / ((float)Camera.main.pixelWidth / Camera.main.pixelHeight);
	
	}
}

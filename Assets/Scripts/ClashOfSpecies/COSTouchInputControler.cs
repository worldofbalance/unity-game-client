using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;



public class COSTouchInputControler:InputControllerBase
{
    

    public COSTouchInputControler () : base ()
    {
        walkableAreaMask = (int)Math.Pow (2, NavMesh.GetAreaFromName ("Walkable"));
    }

    //    COSTouchState eTouchRes;
    //
    //    public COSTouchState TouchState {
    //        get { return eTouchRes; }
    //        set { eTouchRes = value; }
    //    }

    public float moveSensitivityX = 1.0f;
    public float moveSensitivityY = 1.0f;
    public bool updateZoomSensitivity = true;
    public float zoomSpeed = 0.2f;
    public float minZoom = 1.0f;
    public float maxZoom = 20.0f;
    public bool invertMoveX = false;
    public bool invertMoveY = false;

    public float minFOV = 10f;
    public float maxFOV = 79.9f;

    float minPanDistance = 2;

    //    bool isPanning;

    public float inertiaDuration = 1.0f;

    //    private Camera _camera;

    public float terrainCameraPadding = 40;

    private float minX, maxX, minZ, maxZ;
    private float horizontalExtent, verticalExtent;

    private float scrollVelocity = 0.0f;
    private float timeTouchPhaseEnded;

    private Vector3 scrollDirection = Vector3.zero;
    private Vector3 oldTouchPos;

    public override void InputControllerAwake (Terrain t)
    {
        
    }

    public static GameObject SpawnAlly (RaycastHit hit, 
                                        ClashSpecies selected, Dictionary<int, int> remaining, ToggleGroup toggleGroup)
    {
        NavMeshHit placement;
//        if (remaining[selected.id] < 1)
//            return null;

        if (NavMesh.SamplePosition (hit.point, out placement, 100, walkableAreaMask) && hit.collider.CompareTag ("Terrain")) {
            Debug.Log ("mask is" + placement.mask);
            var allyResource = Resources.Load<GameObject> ("Prefabs/ClashOfSpecies/Units/" + selected.name);
            var allyObject = Instantiate (allyResource, placement.position, Quaternion.identity) as GameObject;
            allyObject.tag = "Ally";

            remaining [selected.id]--;
            var toggle = toggleGroup.ActiveToggles ().FirstOrDefault ();
            toggle.transform.parent.GetComponent<ClashUnitListItem> ().amountLabel.text = remaining [selected.id].ToString ();
            if (remaining [selected.id] == 0) {
                toggle.enabled = false;
                toggle.interactable = false;
//                selected = null;
            }
            return allyObject;
        }
        return null;
    }

    /// <summary>
    /// Updates enumVariable passed as an argument
    /// </summary>
    /// <returns>RaycastHit Position where it was tapped on terrain or null </returns>
    /// <param name="_camera">Camera.</param>
    /// <param name="eTouchRes">Current touch state</param>
    /// 
    public override RaycastHit InputUpdate (Camera _camera)
    {
        if (updateZoomSensitivity) {
            moveSensitivityX = _camera.orthographicSize / 5.0f;
            moveSensitivityY = _camera.orthographicSize / 5.0f;
        }

        Touch[] touches = Input.touches;

//        eTouchRes = COSTouchState.None;
        RaycastHit hit = new RaycastHit ();

        if (touches.Length < 1) {
            DetectTouchMovement.rotating = false;
            //if the camera is currently scrolling
            if (scrollVelocity != 0.0f) {
                //slow down over time
                float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
                float frameVelocity = Mathf.Lerp (scrollVelocity, 0.0f, t);
                Vector3 currTransform = -(Vector3)scrollDirection.normalized * (frameVelocity * 0.05f) * Time.deltaTime;
                currTransform.y = 0;
                _camera.transform.position += currTransform;

                if (t >= 1.0f)
                    scrollVelocity = 0.0f;
            }
            eTouchRes = COSTouchState.None;
        }

        if (touches.Length > 0) {
            //Single touch (move)
            if (touches.Length == 1) {
                DetectTouchMovement.rotating = false;
                Ray ray = Camera.main.ScreenPointToRay (touches [0].position);
//                RaycastHit hit;
                if (Physics.Raycast (ray, out hit, 1000)) {
                    //                    if (hit.collider.CompareTag("Terrain"))
                    NavMeshHit nmhPosition;
                    if (NavMesh.SamplePosition (hit.point, out nmhPosition, 10, NavMesh.AllAreas)) {
                        if (touches [0].phase == TouchPhase.Began) {
                            oldTouchPos = hit.point;
                            eTouchRes = COSTouchState.None;
                        } else if (touches [0].phase == TouchPhase.Moved && (hit.point - oldTouchPos).magnitude > minPanDistance) {
                            Vector3 delta = hit.point - oldTouchPos;

                            float positionX = delta.x * moveSensitivityX * Time.deltaTime;
                            positionX = invertMoveX ? positionX : positionX * -1;

                            float positionZ = delta.z * moveSensitivityY * Time.deltaTime;
                            positionZ = invertMoveY ? positionZ : positionZ * -1;

                            _camera.transform.position += new Vector3 (positionX, 0, positionZ);

                            scrollDirection = delta.normalized;
                            scrollVelocity = touches [0].deltaPosition.magnitude / touches [0].deltaTime;


                            if (scrollVelocity <= 100)
                                scrollVelocity = 0;

//                            isPanning = true;
                            eTouchRes = COSTouchState.IsPanning;
                        } else if (touches [0].phase == TouchPhase.Ended) {
                            timeTouchPhaseEnded = Time.time;
                            if (eTouchRes != COSTouchState.IsPanning) {
                                //here we handle simple tap on terrain
                                Debug.Log ("tapped");
                                eTouchRes = COSTouchState.TerrainTapped;
                            } else
                                eTouchRes = COSTouchState.None;
                        }
                    }
                }
            }

            //Double touch (zoom)
            if (touches.Length == 2) {
                Vector2 cameraViewsize = new Vector2 (_camera.pixelWidth, _camera.pixelHeight);

                Touch touchOne = touches [0];
                Touch touchTwo = touches [1];

                DetectTouchMovement.Calculate ();

                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

                float prevTouchDeltaMag = (touchOnePrevPos - touchTwoPrevPos).magnitude;
                float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;

                float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;

                if (Mathf.Abs (DetectTouchMovement.pinchDistanceDelta) > 0) {
                    _camera.fieldOfView += -DetectTouchMovement.pinchDistanceDelta * zoomSpeed;
                    // Clamp the field of view to make sure it's between minFOV and maxFOV.
                    _camera.fieldOfView = Mathf.Clamp (_camera.fieldOfView, minFOV, maxFOV);
                    eTouchRes = COSTouchState.IsZooming;
                } else {
                    _camera.transform.RotateAround (_camera.transform.position, Vector3.up, 
                        -DetectTouchMovement.turnAngleDelta);
                    eTouchRes = COSTouchState.IsRotating;
                }
            }
        }

        return hit;
    }


}
    
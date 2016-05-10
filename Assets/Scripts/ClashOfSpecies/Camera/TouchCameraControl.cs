using UnityEngine;
using System.Collections;

public class TouchCameraControl : MonoBehaviour
{
    public float moveSensitivityX = 1.0f;
    public float moveSensitivityY = 1.0f;
    public bool updateZoomSensitivity = true;
    public float zoomSpeed = 0.05f;
    public float minZoom = 1.0f;
    public float maxZoom = 20.0f;
    public bool invertMoveX = true;
    public bool invertMoveY = false;
    //    public float mapWidth = 60.0f;
    //    public float mapHeight = 40.0f;

    public float minFOV = 10f;
    public float maxFOV = 79.9f;

    float minPanDistance = 2;

    bool isPanning;

    public float inertiaDuration = 1.0f;

    private Camera _camera;

    public float terrainCameraPadding = 40;

    private float minX, maxX, minZ, maxZ;
    private float horizontalExtent, verticalExtent;

    private float scrollVelocity = 0.0f;
    private float timeTouchPhaseEnded;
    private Vector3 scrollDirection = Vector3.zero;
    private Vector3 oldTouchPos;

    UnityEngine.UI.Text txtPinchDistance, txtPinchDistanceDelta, txtTurnAngle, txtTurnAngleDelta, txtDeltaMagDiff;
    COSTouchInputControler cosInController;

    void Start()
    {
        Debug.Log(NavMesh.GetAreaFromName("Walkable") + " " + NavMesh.GetAreaFromName("Not Walkable") + " " +
            NavMesh.GetAreaFromName("Water"));
        _camera = Camera.main;
        minX = terrainCameraPadding;
        maxX = Terrain.activeTerrain.terrainData.size.x - terrainCameraPadding;
        minZ = terrainCameraPadding;
        maxZ = Terrain.activeTerrain.terrainData.size.z - terrainCameraPadding;

        var objTxtPinchDistance = GameObject.Find("txtPinchDistance");
        txtPinchDistance = objTxtPinchDistance.GetComponent<UnityEngine.UI.Text>();
        var q = GameObject.Find("txtPinchDistanceDelta");
        txtPinchDistanceDelta = q.GetComponent<UnityEngine.UI.Text>();
        var w = GameObject.Find("txtTurnAngle");
        txtTurnAngle = w.GetComponent<UnityEngine.UI.Text>();
        var e = GameObject.Find("txtTurnAngleDelta");
        txtTurnAngleDelta = e.GetComponent<UnityEngine.UI.Text>();
        var r = GameObject.Find("deltaMagDiff");
        txtDeltaMagDiff = r.GetComponent<UnityEngine.UI.Text>();

//        maxZoom = 0.5f * (mapWidth / _camera.aspect);
//
//        if (mapWidth > mapHeight)
//            maxZoom = 0.5f * mapHeight;

//        if (_camera.orthographicSize > maxZoom)
//            _camera.orthographicSize = maxZoom;

//        CalculateLevelBounds();
        cosInController = new COSTouchInputControler();
    }

    COSTouchInputControler.COSTouchState eTouchRes = COSTouchInputControler.COSTouchState.None;

    void Update()
    {
        RaycastHit hit = cosInController.TouchUpdate(_camera);

//        if (updateZoomSensitivity)
//        {
//            moveSensitivityX = _camera.orthographicSize / 5.0f;
//            moveSensitivityY = _camera.orthographicSize / 5.0f;
//        }
//
//        Touch[] touches = Input.touches;
//
//        if (touches.Length < 1)
//        {
//            //if the camera is currently scrolling
//            DetectTouchMovement.rotating = false;
//            if (scrollVelocity != 0.0f)
//            {
//                //slow down over time
//                float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
//                float frameVelocity = Mathf.Lerp(scrollVelocity, 0.0f, t);
//                Vector3 currTransform = -(Vector3)scrollDirection.normalized * (frameVelocity * 0.05f) * Time.deltaTime;
//                currTransform.y = 0;
//                _camera.transform.position += currTransform;
//
//                if (t >= 1.0f)
//                    scrollVelocity = 0.0f;
//            }
//        }
//
//        if (touches.Length > 0)
//        {
//            //Single touch (move)
//            if (touches.Length == 1)
//            {
//                DetectTouchMovement.rotating = false;
//                Ray ray = Camera.main.ScreenPointToRay(touches[0].position);
//
//                RaycastHit hit;
//
//                if (Physics.Raycast(ray, out hit, 1000))
//                {
//                    NavMeshHit target;
//                    NavMeshHit hit2;
//                    bool blocked = false;
//                    //
//                    NavMesh.SamplePosition(hit.point, out target, 1.0f, NavMesh.AllAreas);
//                    //
//                    blocked = NavMesh.Raycast(transform.position, target.position,
//                        out hit2, NavMesh.AllAreas);
//                    Debug.DrawLine(transform.position, target.position, blocked ? Color.red : Color.green, 5f);
//
//                    
//                    if (hit.collider.CompareTag("Terrain"))
//                    {
//                        if (touches[0].phase == TouchPhase.Began)
//                        {
//                            oldTouchPos = hit.point;
//                        }
//                        else if (touches[0].phase == TouchPhase.Moved && (hit.point - oldTouchPos).magnitude > minPanDistance)
//                        {
//                            Vector3 delta = hit.point - oldTouchPos;
//
//                            float positionX = delta.x * moveSensitivityX * Time.deltaTime;
//                            positionX = invertMoveX ? positionX : positionX * -1;
//
//                            float positionZ = delta.z * moveSensitivityY * Time.deltaTime;
//                            positionZ = invertMoveY ? positionZ : positionZ * -1;
//
//                            _camera.transform.position += new Vector3(positionX, 0, positionZ);
//
//                            scrollDirection = delta.normalized;
//                            scrollVelocity = touches[0].deltaPosition.magnitude / touches[0].deltaTime;
//
//
//                            if (scrollVelocity <= 100)
//                                scrollVelocity = 0;
//
//                            isPanning = true;
//                        }
//                        else if (touches[0].phase == TouchPhase.Ended)
//                        {
//                            timeTouchPhaseEnded = Time.time;
//                            if (!isPanning)
//                            {
////                                Debug.Log("clicked");
////                                NavMeshHit placement;
////
////                                NavMesh.Raycast(Camera.main.transform.position, ray.direction
////                                    , out placement, NavMesh.AllAreas);
////                                Debug.Log("clicked area raycats: " + placement.mask);
////                                if (NavMesh.SamplePosition(hit.point, out placement, 1.0f, NavMesh.AllAreas))
////                                {
////                                    Debug.Log("clicked area navmesh: " + placement.mask);
////
////                                }
//
//                                
//                            }
//                            isPanning = false;
//                        }
//                    }
//                }
//            }
//
//
//            //Double touch (zoom)
//            if (touches.Length == 2)
//            {
//                Vector2 cameraViewsize = new Vector2(_camera.pixelWidth, _camera.pixelHeight);
//
//                Touch touchOne = touches[0];
//                Touch touchTwo = touches[1];
//
//                DetectTouchMovement.Calculate();
//
//                txtPinchDistance.text = "pinchDistance:" + DetectTouchMovement.pinchDistance + "";
//                txtPinchDistanceDelta.text = "txtPinchDistanceDelta" + DetectTouchMovement.pinchDistanceDelta + "";
//                txtTurnAngle.text = "txtTurnAngle" + DetectTouchMovement.turnAngle + "";
//                txtTurnAngleDelta.text = "txtTurnAngleDelta" + DetectTouchMovement.turnAngleDelta + "";
//
//
//                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
//                Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;
//
//                float prevTouchDeltaMag = (touchOnePrevPos - touchTwoPrevPos).magnitude;
//                float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;
//
//                float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
//                txtDeltaMagDiff.text = "deltaMagDiff" + deltaMagDiff + "";
//
//                if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
//                {
//                    _camera.fieldOfView += -DetectTouchMovement.pinchDistanceDelta * zoomSpeed;
//                    // Clamp the field of view to make sure it's between 0 and 180.
//                    _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, minFOV, maxFOV);
////                    DetectTouchMovement.zooming = true;
////                    DetectTouchMovement.rotating = false;
//                }
//                else
//                {
//                    _camera.transform.RotateAround(_camera.transform.position, Vector3.up, 
//                        -DetectTouchMovement.turnAngleDelta);
//                }
//            }
//        }
    }

    //    void CalculateLevelBounds()
    //    {
    //        verticalExtent = _camera.orthographicSize;
    //        horizontalExtent = _camera.orthographicSize * Screen.width / Screen.height;
    //        minX = horizontalExtent - mapWidth / 2.0f;
    //        maxX = mapWidth / 2.0f - horizontalExtent;
    //        minZ = verticalExtent - mapHeight / 2.0f;
    //        maxZ = mapHeight / 2.0f - verticalExtent;
    //    }



    void LateUpdate()
    {
        Vector3 pos = new Vector3(
                          Mathf.Clamp(_camera.transform.position.x, minX, maxX),
                          _camera.transform.position.y, 
                          Mathf.Clamp(_camera.transform.position.z, minZ, maxZ));
        _camera.transform.position = pos;
    }

    //    void OnDrawGizmos()
    //    {
    //        Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapWidth, mapHeight, 0));
    //    }
}
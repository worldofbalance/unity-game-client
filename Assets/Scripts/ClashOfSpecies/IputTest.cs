using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IputTest : MonoBehaviour
{
    private COSAbstractInputController cosInController;

    public bool isRunningOnMobile;
    public Text angle;
    public Text pinch;

    // Use this for initialization
    void Start()
    {
	
        if (isRunningOnMobile)
            cosInController = ScriptableObject.CreateInstance<COSMobileInputControler>();
        else
            cosInController = ScriptableObject.CreateInstance<COSDesktopInputController>();
        cosInController.InputControllerAwake(Terrain.activeTerrain);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Application.CaptureScreenshot("/Users/dusan_cvetkovic/Documents/SFSUdocs/Semester2/CSC831MultGameD/Screenshot.png");
            Debug.Log("screenshot taken!!");
        }

        cosInController.InputUpdate(Camera.main);
        angle.text = "angle " + DetectTouchMovement.turnAngleDelta;
        pinch.text = "pinch " + DetectTouchMovement.pinchDistanceDelta;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TilePurchaseButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject tileUi;

    // via script
    public Button button1;
    public Text text1;

    // via editor
    public Button currentButton;
    public Text currentText;


    void Awake()
    {
        // setting up gameobject via script
        tileUi = GameObject.Find("Tilepurchasedialog");
       // button1 = tileUi.transform.GetChild(...).GetComponent<Button>();
      //  text1 = button1.transform.GetChild(0).GetComponent<Text>();


        // if script is attached to button in editor
        currentButton = this.gameObject.GetComponent<Button>();
        currentText = currentButton.transform.GetChild(0).GetComponent<Text>();
    }

    void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventdata)
    {
        if (currentText.text == "Cancel")
        {
            tileUi.SetActive(false);

        } else if (currentText.text == "Purchase")

        {
            Debug.Log("TilePurchase Successful");
        }
        // does what you need when you click stuff
        // code here will run for the button you attach this to
        // if buttons have different functions set up the logic to differentiate
        // functions
        // Suggestion: using button text to check for button and separating the functions
    }

}

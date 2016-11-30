using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
    Handles PointerEvent behavior for build buttons (plant and prey).
*/
public class BuildButtonPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panelObject; // Panel object for tooltips

    void Awake ()
    {
        panelObject = GameObject.Find("Canvas/Panel");
    }

    /**
        Called when the cursor enters the button's collision area.
        Activates a tooltip for displaying pertinent button-specific data (e.g. species data).

        @see    Selectable.OnPointerEnter (https://docs.unity3d.com/ScriptReference/UI.Selectable.OnPointerEnter.html)
    */
    public void OnPointerEnter (PointerEventData eventData)
    {
        // Activate and position tooltip panels
        panelObject.SetActive(true); // Main panel
        panelObject.transform.GetChild(1).gameObject.SetActive(true); // Biomass panel
        panelObject.transform.position = new Vector3(Input.mousePosition.x + 180, Input.mousePosition.y);

        // Determine whether active button is a plant or prey
        GameObject activeButton = gameObject.transform.GetChild(0).gameObject.activeSelf ? 
            gameObject.transform.GetChild(0).gameObject : gameObject.transform.GetChild(1).gameObject;

        Text nameField = panelObject.transform.GetChild(1).GetComponent<Text>(); // Species hover name field
        Text biomassField = panelObject.transform.GetChild(3).GetChild(0).GetComponent<Text>(); // Biomass text field

        // Set fields
        nameField.text = activeButton.name;
        biomassField.text = "Biomass : " + SpeciesConstants.Biomass(activeButton.name).ToString();
    }

    /**
        Called when the cursor exits the button's collision area.
        Deactivates panels and tooltips.

        @see    Selectable.OnPointerExit (https://docs.unity3d.com/ScriptReference/UI.Selectable.OnPointerExit.html)
    */
    public void OnPointerExit (PointerEventData eventData)
    {
        // Sets tooltip as inactive
        panelObject.SetActive(false);
    }
}
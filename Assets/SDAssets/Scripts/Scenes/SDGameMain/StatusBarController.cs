/*
 * File Name: StatusBarController.cs
 * Description: Update and redraw the health bar based on the player's current stamina.
 *              *Trying to combine with HealthBarController.cs
 */ 

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SD {
public class StatusBarController : MonoBehaviour {

    public GameController gameController;
    private RectTransform staminaBarTransform;
    private RectTransform healthBarTransform;
    private Image staminaBar;
    private Image healthBar;
    private Color defaultStaminaColor;
    private Color defaultHealthColor;


    private float maxY;
    private float currentStamina;
    private float currentHealth;


    // Use this for initialization
    void Start () {
        maxY = 40f;

        staminaBar = GameObject.Find ("StaminaBar").GetComponent<Image> ();
        staminaBarTransform = staminaBar.GetComponent<RectTransform> ();
        defaultStaminaColor = staminaBar.GetComponent<Image> ().color;

        healthBar = GameObject.Find ("HealthBar").GetComponent<Image> ();
        healthBarTransform = healthBar.GetComponent<RectTransform> ();
        defaultHealthColor = healthBar.GetComponent<Image> ().color;

        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        } else {
            Debug.Log ("Game Controller not found");
        }
    }

    // Update is called once per frame
    void Update () {
        currentStamina = gameController.GetStamina ();
        currentHealth = gameController.GetHealth ();

        if (currentStamina <= 10) {
            staminaBar.color = Color.red;
        } else {
            staminaBar.color = defaultStaminaColor;
        }

        if (currentHealth <= 10) {
            healthBar.color = Color.red;
        } else {
            healthBar.color = defaultHealthColor;
        }

        staminaBarTransform.sizeDelta = new Vector2 (currentStamina, maxY);
        healthBarTransform.sizeDelta = new Vector2 (currentHealth, maxY);
    }
}
}

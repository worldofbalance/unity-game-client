using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class ClashHealthBar : MonoBehaviour
{
    private Transform cameraTransform;
    private ClashBattleUnit unit;
    private ClashBattleController controller;
    private Slider slider;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        unit = GetComponentInParent<ClashBattleUnit>();
        slider = GetComponentInChildren<Slider>();
        controller = GameObject.Find ("Battle Menu").GetComponent<ClashBattleController> ();
        var colors = new ColorBlock();
        switch (unit.species.type)
        {
            case ClashSpecies.SpeciesType.CARNIVORE:
                colors.normalColor = Color.red;
                break;
            case ClashSpecies.SpeciesType.HERBIVORE:
                colors.normalColor = Color.blue;
                break;
            case ClashSpecies.SpeciesType.OMNIVORE:
                colors.normalColor = Color.yellow;
                break;
            case ClashSpecies.SpeciesType.PLANT:
                colors.normalColor = Color.green;
                break;
            default:
                break;
        }

        if (unit.gameObject.tag == "Enemy") {
            colors.normalColor = Color.red;
            controller.selectedSpeciesText.text = unit.species.name;
        }
        else {
           if (controller.activeHealthBar != null) {
             controller.activeHealthBar.hideBar();
           }
           colors.normalColor = Color.blue;
           controller.activeHealthBar = this;
           controller.selectedSpeciesText.text = unit.species.name;
        }
            
        colors.colorMultiplier = 1.0f;
        slider.colors = colors;
    }

    public void hideBar() {
        var colors = new ColorBlock();
        colors.normalColor = Color.blue;
        colors.colorMultiplier = 0f;
        slider.colors = colors;
    }
	
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);
        slider.value = (float)unit.currentHealth / (float)unit.species.hp;
    }

    public void dummyTest()
    {
        
    }
}

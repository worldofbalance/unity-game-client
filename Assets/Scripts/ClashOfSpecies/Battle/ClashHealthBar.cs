using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class ClashHealthBar : MonoBehaviour {
    private Transform camera;
    private ClashBattleUnit unit;
    private Slider slider;

	void Start () {
        camera = Camera.main.transform;
        unit = GetComponentInParent<ClashBattleUnit>();
        slider = GetComponentInChildren<Slider>();

        var colors = new ColorBlock();
        switch (unit.species.type) {
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
            default: break;
        }
        colors.colorMultiplier = 1.0f;
        slider.colors = colors;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(camera);
        slider.value = (float)unit.currentHealth / (float)unit.species.hp;
	}
}

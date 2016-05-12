using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace SD {
public class DropDownMenu : MonoBehaviour {

    public GameObject surrenderPanelCanvas;
    public GameObject foodChainPanelCanvas;
    public Dropdown dropdownMenu;
    private GameController gameController;
     

	// Use this for initialization
	void Start () {
            dropdownMenu.onValueChanged.AddListener (delegate {
                dropdownMenuValueChangedHandler (dropdownMenu);
            });
        GameController.getInstance ();
    }
    
        void Update(){
           
        }

        private void dropdownMenuValueChangedHandler(Dropdown target){
            switch (target.value) {
            case 0:
                break;
            case 1:
                showSurrenderPanel ();
                dropdownMenu.value = 0;
                break;
            case 2:
                showFoodChainPanel ();
                dropdownMenu.value = 0;
                break;
            default:
                break;
            }
        }

	// Update is called once per frame
        public void showSurrenderPanel(){
            surrenderPanelCanvas.SetActive (true);
        }

        public void showFoodChainPanel(){
            foodChainPanelCanvas.SetActive (true);
        }

        public void hideSurrenderPanel(){
            surrenderPanelCanvas.SetActive(false);
        }

}
}

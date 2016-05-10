using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace SD {
public class DropDownMenu : MonoBehaviour {

    public GameObject surrenderPanelCanvas;
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
            if (target.value == 1) {
                showSurrenderPanel ();
            }
        }

	// Update is called once per frame
        public void showSurrenderPanel(){
            surrenderPanelCanvas.SetActive (true);
        }

        public void hideSurrenderPanel(){
            surrenderPanelCanvas.SetActive(false);
        }

}
}

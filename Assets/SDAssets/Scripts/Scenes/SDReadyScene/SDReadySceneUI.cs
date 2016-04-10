using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SD {
    public class SDReadySceneUI : MonoBehaviour {

        private Button btnPlaySdv;

        void OnEnable () {
            btnPlaySdv = GameObject.Find ("BtnPlaySDV").GetComponent<Button> ();
        }

        public void BtnBackToLobbyClick() {
            Game.SwitchScene ("World");
        }

        public void BtnPlaySDVClick() {
            SDReadySceneManager.getInstance().StartGame ();
            btnPlaySdv.interactable = false;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SD {
    public class SDReadySceneUI : MonoBehaviour {

        private Button btnPlaySdv;
        private Text txtWaitingForOpponent;
        private static SDReadySceneUI sdReadySceneUI;

        void OnAwake() {
            sdReadySceneUI = this;
        }

        void OnEnable () {
            btnPlaySdv = GameObject.Find ("BtnPlaySDV").GetComponent<Button> ();
            txtWaitingForOpponent = GameObject.Find ("TxtWaiting").GetComponent<Text> ();
        }

        public void BtnBackToLobbyClick() {
            Game.SwitchScene ("World");
        }

        public void BtnPlaySDVClick() {
            SDReadySceneManager.getInstance().StartGame ();
            btnPlaySdv.interactable = false;
        }

        public static SDReadySceneUI getInstance() {
            return sdReadySceneUI;
        }

        public Text getWaitingText() {
            return txtWaitingForOpponent;
        }
    }
}

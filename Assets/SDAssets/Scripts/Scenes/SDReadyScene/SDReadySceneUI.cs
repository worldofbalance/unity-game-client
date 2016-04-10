using UnityEngine;
using UnityEngine.SceneManagement;

namespace SD {
    public class SDReadySceneUI : MonoBehaviour {

        public void BtnBackToLobbyClick() {
            Game.SwitchScene ("World");
        }

        public void BtnPlaySDVClick() {
            SDReadySceneManager.StartGame ();
        }
    }
}

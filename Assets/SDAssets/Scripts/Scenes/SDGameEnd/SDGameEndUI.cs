using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SD {
    public class SDGameEndUI : MonoBehaviour {

        private SDPersistentData persistentObject;

        public void BtnBackToLobbyClick() {
            SceneManager.LoadScene ("World");
        }

        public void BtnPlayAgainClick() {
            // Start another round of the game with the same players.
            persistentObject = SDPersistentData.getInstance();
            if (persistentObject) 
                persistentObject.initializeData ();
            SceneManager.LoadScene ("SDReadyScene");
        }

    }
}

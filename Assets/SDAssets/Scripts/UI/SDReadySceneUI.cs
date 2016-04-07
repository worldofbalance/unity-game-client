using UnityEngine;
using UnityEngine.SceneManagement;

public class SDReadySceneUI : MonoBehaviour {

    public void BtnBackToLobbyClick() {
        Game.SwitchScene ("World");
    }

    public void BtnPlaySDVClick() {
        SceneManager.LoadScene ("SDVGameMain");
    }
}

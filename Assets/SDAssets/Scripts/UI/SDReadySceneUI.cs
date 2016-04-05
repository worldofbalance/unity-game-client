using UnityEngine;

public class SDReadySceneUI : MonoBehaviour {

    public void BtnBackToLobbyClick() {
        Game.SwitchScene ("World");
    }

    public void BtnPlaySDVClick() {
        Game.SwitchScene ("SDVGameMain");
    }
}

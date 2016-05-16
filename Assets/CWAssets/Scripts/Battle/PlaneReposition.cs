using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace CW
{
    public class PlaneReposition : MonoBehaviour {

    private BattlePlayer player;

    public void init(BattlePlayer player1)
    {
        this.player = player1;
    }

    void OnMouseDown()
    {
        if(player.handCentered)
        {
            player.handPos = new Vector3(550, 10, -375);
            player.handCentered = false;
            player.reposition();
            Debug.Log("test");
        }
    }
}

}
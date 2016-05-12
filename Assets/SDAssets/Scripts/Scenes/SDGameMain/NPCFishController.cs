using System;
using UnityEngine;

/* Description: Class to control the behavior of an NPC fish in the game.
 */

namespace SD
{
    public class NPCFishController : MonoBehaviour
    {

        public NPCFish[] npcFish;

        NPCFishController() {
           
            Awake();
        }
        public NPCFish[] getNPCFishData() {
            return npcFish;
        }

         void Awake()
        {
        }
        public void setNPCFishData(NPCFish newFish )
        {

            npcFish[newFish.id] =newFish;
        }
    

        void Update()
        {
			         
        }

        public void SetTarget(NPCFish Fish)
        {
            Fish.target = new Vector2(Fish.xPosition + UnityEngine.Random.Range(-2.0f, 2.0f), Fish.yPosition + UnityEngine.Random.Range(-2.0f, 2.0f));
        }
    }
}


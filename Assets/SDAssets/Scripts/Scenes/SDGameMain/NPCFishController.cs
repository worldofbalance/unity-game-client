using System;
using UnityEngine;

/* Description: Class to control the behavior of an NPC fish in the game.
 */

namespace SD
{
    public class NPCFishController : MonoBehaviour
    {
        public NPCFish[] npcFish;


        public NPCFish[] getNPCFishData() {
            return npcFish;
        }

        public void setNPCFishData() {
         
        NPCFish= GameObject.FindGameObjectsWithTag("NpcFish");
        
        }

        void Update()
        {
			if(constants.playernumber=1)
            for(int i=0;i<npcFish.sizeof();i++){
            if(npcFish[i].isAlive==true) SetTarget(npcFish[i]);
            }
            
            
            
        }

        public void SetTarget(NPCFish Fish)
        {
            Fish.target = new Vector2(Fish.xPosition + UnityEngine.Random.Range(-2.0f, 2.0f), Fish.yPosition + UnityEngine.Random.Range(-2.0f, 2.0f));
        }

        
    }
}


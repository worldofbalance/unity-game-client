using System;
using UnityEngine;

/* Description: Class to associate a unity Prey GameObject with its metadata.
 */

namespace SD
{
    public class NPCFishController : MonoBehaviour
    {
        public NPCFish npcFish;
        private int counter=0;

        public NPCFish getNPCFishController() {
            return npcFish;
        }

        public void setNPCFishController(NPCFish fish) {
            npcFish = fish;
        }

        void Update()
        {

            npcFish.MoveToTarget();
            if(counter/5>1)npcFish.SetTarget();
            counter++;
            
        }
    }
}


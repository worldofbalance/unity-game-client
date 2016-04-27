using System;
using UnityEngine;

/* Description: Class to associate a unity Prey GameObject with its metadata.
 */

namespace SD
{
    public class NPCFishData : MonoBehaviour
    {
        public NPCFish npcFish;

        public NPCFish getNPCFishData() {
            return npcFish;
        }

        public void setNPCFishData(NPCFish fish) {
            npcFish = fish;
        }
    }
}


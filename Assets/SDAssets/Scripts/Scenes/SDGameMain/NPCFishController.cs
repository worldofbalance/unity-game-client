using System;
using UnityEngine;

/* Description: Class to control the behavior of an NPC fish in the game.
 */

namespace SD
{
    public class NPCFishController : MonoBehaviour
    {
        public NPCFish npcFish;


        public NPCFish getNPCFishData() {
            return npcFish;
        }

        public void setNPCFishData(NPCFish fish) {
            npcFish = fish;
        }

        void Update()
        {
            
            SetTarget(npcFish);
            for(int i = 0; i < 3; i++)
            { MoveToTarget(npcFish); }
            
            
            
        }

        public void SetTarget(NPCFish Fish)
        {

            Fish.target = new Vector2(Fish.xPosition + UnityEngine.Random.Range(-2.0f, 2.0f), Fish.yPosition + UnityEngine.Random.Range(-2.0f, 2.0f));
        }

        public void MoveToTarget(NPCFish Fish)
        {
            Fish.xRotationAngle = Vector2.Angle(Fish.current, Fish.target);
            Fish.current = Vector2.MoveTowards(Fish.current, Fish.target, 35 * Time.deltaTime);

        }
    }
}


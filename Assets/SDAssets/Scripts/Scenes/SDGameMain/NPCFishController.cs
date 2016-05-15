using System;
using UnityEngine;

/* Description: Class to control the behavior of an NPC fish in the game.
 */

namespace SD
{
    public class NPCFishController : MonoBehaviour
    {
        public NPCFish npcFish;
        Rigidbody rb;
        public Boundary boundary;
        public Vector2 current { get; set; }
        //public Vector2 target { get; set; }

        float fTime = 0;
        public NPCFish getNPCFishData() {
            return npcFish;
        }

        public void setNPCFishData(NPCFish fish) {
            npcFish = fish;
        }



        void Start() {
            rb = GetComponent<Rigidbody>();
            
            //target=new Vector2(UnityEngine.Random.Range(boundary.xMin, boundary.xMax), UnityEngine.Random.Range(boundary.yMin, boundary.yMax));
        }
        void Update()
        {

            MoveToTarget();
         
        }


        public void MoveToTarget()
        {

            //npcFish.current = Vector2.MoveTowards(npcFish.current, npcFish.target, 35 * Time.deltaTime);
            //  npcFish.xPosition = npcFish.current.x;
            // npcFish.yPosition = npcFish.current.y;
            /*if (Vector2.Distance(npcFish.current,npcFish.target)<5f) { SetTarget(); }
            npcFish.xRotationAngle = Vector2.Angle(npcFish.current, npcFish.target);
            transform.position = Vector2.MoveTowards(transform.position, npcFish.target, UnityEngine.Random.Range(10f, 100f) * Time.deltaTime);
            npcFish.UpdatePos(transform.position.x, transform.position.y);*/

                

            current = transform.position;
            transform.position = Vector2.MoveTowards(current, new Vector2(Mathf.PingPong(Time.time * 50, -50) + 10, transform.position.y), UnityEngine.Random.Range(10f, 100f) * Time.deltaTime);
            //current = transform.position;
            //UpdatePos(current.x, current.y);

        }
    }
}


using System;
using UnityEngine;
using System.Collections;


namespace SD
{
    public class NPCFish {
        public int id { get; set; }
        public int speciesId { get; set; }
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public bool isAlive { get; set; }
        public float xRotationAngle { get; set; }
        public Vector2 current { get; set; }
        public Vector2 target { get; set; }
    
        public NPCFish(int id)
        {
            this.id = id;
            this.speciesId = 0;
            this.xPosition = 0.0f;
            this.yPosition = 0.0f;
            this.xRotationAngle = 0.0f;
            this.isAlive = true;
            this.current = new Vector2(xPosition, yPosition);
        }
        public NPCFish(NPCFish newFish)
        {
            this.id = newFish.id;
            this.speciesId = newFish.speciesId;
            this.xPosition = newFish.xPosition;
            this.yPosition = newFish.yPosition;
            this.xRotationAngle = newFish.xRotationAngle;
            this.isAlive = newFish.isAlive;
            this.current = newFish.current;
            this.target = newFish.target;
        }
        void Awake() {
          
        }
        void Update() {
            if (SD.Constants.PLAYER_NUMBER == 1) {
                if(this.isAlive == true){ target = new Vector2(this.xPosition + UnityEngine.Random.Range(-2.0f, 2.0f), this.yPosition + UnityEngine.Random.Range(-2.0f, 2.0f)); }
                
                this.xRotationAngle = Vector2.Angle(this.current, this.target);
                this.current = Vector2.MoveTowards(this.current, this.target, 35 * Time.deltaTime);
            }
        }
        }
}

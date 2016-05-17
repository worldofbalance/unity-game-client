using System;
using UnityEngine;
using System.Collections;


namespace SD
{
    public class NPCFish {
        public int id { get; set; }
        public int speciesId { get; set;}
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public bool isAlive { get; set; }
        public float xRotationAngle { get; set; }
        public bool toBeCreated { get; set; }
        public Vector2 target { get; set; }
        public int targetOffset { get; set; }

        public NPCFish (int id)
        {
            this.id = id;
            this.speciesId = 0;
            this.xPosition = 0.0f;
            this.yPosition = 0.0f;
            this.xRotationAngle = 0.0f;
            this.isAlive = true;
            this.toBeCreated = false;
            this.target = new Vector2 (xPosition, yPosition);
            this.targetOffset = 20;
        }
        void Update() {


        }

    }
}

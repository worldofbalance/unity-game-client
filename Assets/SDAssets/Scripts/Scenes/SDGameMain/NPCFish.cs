using System;
using UnityEngine;
using System.Collections;


namespace SD
{
    public class NPCFish {
        public int id { get; set; }
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public bool isAlive { get; set; }
        public float xRotationAngle;
        public Vector2 current { get; set; }
        public Vector2 target { get; set; }

        public NPCFish (int id)
        {
            this.id = id;
            this.xPosition = 0.0f;
            this.yPosition = 0.0f;
            this.isAlive = true;
            this.current=new Vector2(xPosition,yPosition);

        }
        void Update() {
            current = new Vector2(xPosition, yPosition);
        }




    }

}


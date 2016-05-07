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
        private float xRotationAngle;
        private Vector2 current { get; set; }
        private Vector2 target;

        public NPCFish (int id)
        {
            this.id = id;
            this.xPosition = 0.0f;
            this.yPosition = 0.0f;
            this.isAlive = true;
        }



        public void SetTarget() {

            this.target = new Vector2(this.xPosition + UnityEngine.Random.Range(-2.0f, 2.0f), yPosition + UnityEngine.Random.Range(-2.0f, 2.0f));
        }

        public void MoveToTarget() {
            this.xRotationAngle = Vector2.Angle(current, target);
            this.current = Vector2.MoveTowards(this.current, this.target, 35 * Time.deltaTime);

        }

    }

}


using System;
using UnityEngine;
using System.Collections;


namespace SD
{
    public class NPCFish : MonoBehaviour    {
        public int id { get; set; }
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public bool isAlive { get; set; }
        public Vector3 target { get; set; }
        private float xRotationAngle;

        public NPCFish (int id)
        {
            this.id = id;
            this.xPosition = 0.0f;
            this.yPosition = 0.0f;
            this.isAlive = true;
        }

        void Update()
        {
            
            
            var angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            var yAngle = -90;
            xRotationAngle = angle;
            if (angle >= -90 || angle <= 90)
            {
                // invert the angle to avoid upside down movement.
                angle = 180 - angle;
                yAngle = 90;
            }

            if (target.x + 5 > target.x - xPosition || target.y + 5 > target.y - yPosition) { SetTarget(); }
        }

        void SetTarget() {

            target = new Vector3(xPosition + UnityEngine.Random.Range(-10.0f, 10.0f), yPosition + UnityEngine.Random.Range(-10.0f, 10.0f));
        }
    }

}


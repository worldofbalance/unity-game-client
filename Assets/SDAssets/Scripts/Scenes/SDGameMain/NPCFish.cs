using System;

namespace SD
{
    public class NPCFish
    {
        public int id { get; set; }
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public bool isAlive { get; set; }

        public NPCFish (int id)
        {
            this.id = id;
            this.xPosition = 0.0f;
            this.yPosition = 0.0f;
            this.isAlive = true;
        }
    }
}


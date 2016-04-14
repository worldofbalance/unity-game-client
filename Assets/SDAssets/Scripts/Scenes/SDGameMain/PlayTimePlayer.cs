using System;

namespace SD
{
    public class PlayTimePlayer
    {
        public float velocity { get; set; }
        public float speed { get; set; }
        public float speedUpFactor { get; set; }
        public float stamina {get; set; }
        public float health { get; set; }
        public float movementHorizontal { get; set; }
        public float movementVertical { get; set; }

        public PlayTimePlayer ()
        {
            speed = 20.0f;
            stamina = 100;
            health = 100;
            movementHorizontal = 0.0f;
            movementVertical = 0.0f;
        }
    }
}


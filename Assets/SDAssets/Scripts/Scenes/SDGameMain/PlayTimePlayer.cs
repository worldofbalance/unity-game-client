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
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public int score {get; set;}
        public float xRotation { get; set; }
        public bool isTurningLeft { get; set; }
        public bool isTurningRight {get; set; }

        public PlayTimePlayer ()
        {
            speed = 20.0f;
            stamina = 100;
            health = 100;
            movementHorizontal = 0.0f;
            movementVertical = 0.0f;
            xPosition = 0.0f;
            yPosition = 0.0f;
            score = 0;
            isTurningLeft = false;
            isTurningRight = false;
        }
    }
}


using System;

namespace SD
{
    public class PlayTimePlayer
    {
        public float velocity;
        public float speed;
        public float speedUpFactor;
        public float stamina;
        public float health;
        public float movementHorizontal;
        public float movementVertical;
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


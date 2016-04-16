using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace CW
{
    public class AbstractCard : MonoBehaviour
    {
        public int cardID, fieldIndex;
        public int maxHP, hp, dmg, naturalDmg, manaCost, level, dmgTimer = 0;
        public string dietChar;
        private Font font;
        private BattlePlayer player;
        public string name, type = " ", description = " ";
        public DIET diet;
        private bool canAttackNow, inMotion, moveBack;
        private Vector3 oriPosition;
        private Vector3 newPosition;
        private bool zoomed = false;
        private bool clicked = false, removeAfterDelay;
        private float raised = 50f;
        //VELOCITY
        private Vector3 targetPosition, startPosition;
        private float velocity, terminalVelocity, angle, distance;
        private float delayTimer, DELAY_CONSTANT = 1.5f;
        //Enum for Animal Type
        public enum DIET
        {
            OMNIVORE,
            CARNIVORE,
            HERBIVORE,
            WEATHER,
            FOOD
        }
        //byPedro
        private AudioSource audioSource;
        private Behaviour halo;
    
        public AbstractCardHandler handler;
    
        //Initialization for a card and sets it's position to (1000, 1000, 1000)
        public void init (BattlePlayer player, int cardID, string diet, int level, int attack, int health, string species_name, string type, string description)
        {
            this.player = player;
            this.cardID = cardID;
            this.manaCost = level;
            this.transform.position = new Vector3 (player.DeckPos.x, player.DeckPos.y, player.DeckPos.z);
            canAttackNow = true;
            velocity = 0;
            terminalVelocity = 6000;
            distance = 0;
            delayTimer = 0;
            name = species_name;
            this.diet = getDietType (diet);
            this.dietChar = diet;
            this.level = level;
            maxHP = hp = health;
            naturalDmg = dmg = attack;
            //this.type = type; //hide temporarily
            //this.description = description; //hide temporarily
        
            Debug.Log ("diet" + diet);
            //o-omnivore, c-carnivore, h-herbivore, f-food, w-weather
            Texture2D cardTexture = (Texture2D)Resources.Load ("Images/Battle/cardfront_" + this.dietChar, typeof(Texture2D));
            Texture2D speciesTexture = (Texture2D)Resources.Load ("Images/" + this.name, typeof(Texture2D));

            //Changing cardfront texture
            GetComponent<Renderer>().material.mainTexture = cardTexture;
            transform.Find ("CardArt").GetComponent<MeshRenderer> ().material.mainTexture = speciesTexture;

            //Changing card text 
//      Color gold = new Color (209f, 234f, 50f, 255f);
            transform.Find ("NameText").GetComponent<TextMesh> ().text = TextWrap (this.name, 16);
            transform.Find ("TypeText").GetComponent<TextMesh> ().text = this.type;
            transform.Find ("TypeText").GetComponent<MeshRenderer> ().material.color = Color.white;
            transform.Find ("DescriptionText").GetComponent<TextMesh> ().text = TextWrap (this.description, 26);
            transform.Find ("DescriptionText").GetComponent<MeshRenderer> ().material.color = Color.white;
            transform.Find ("LevelText").GetComponent<TextMesh> ().text = "" + this.level;
            transform.Find ("LevelText").GetComponent<MeshRenderer> ().material.color = Color.white;
            transform.Find ("DoneText").GetComponent<MeshRenderer> ().material.color = Color.red;
            transform.Find ("DamageText").GetComponent<TextMesh> ().text = "";
            transform.Find ("DamageText").GetComponent<MeshRenderer> ().material.color = Color.red;

            //Initializes off screen
            transform.position = new Vector3 (1000, 1000, 1000);

            //rotate facedown if player 2
            if (!player.player1 && !Constants.SINGLE_PLAYER) {
                transform.rotation = new Quaternion (180, 0, 0, 0); 
            }

            //by Pedro
            audioSource = gameObject.AddComponent<AudioSource> ();

            //halo = (Behaviour)gameObject.GetComponent("Halo");
            //halo.enabled = false;
        }
    
        //Returns the enum for the animal's diet. Herbivore, Omnivore, Carnivore
        DIET getDietType (string diet)
        {
            if (diet == "o") {
                return DIET.OMNIVORE;    
            } else if (diet == "c") {
                return DIET.CARNIVORE;    
            } else if (diet == "h") {
                return DIET.HERBIVORE;
            } else if (diet == "f") {
                return DIET.FOOD;
            } else 
                return DIET.WEATHER;
            //else diet == 2
        }
    
        //OnMouseDown also checks for touch events
        void OnMouseDown ()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (inMotion)
                    return;
            
                //keeping original position for Zoom
                if (!zoomed) {
                    oriPosition = this.transform.position;
                    zoomed = true;  

                }
            
                newPosition = oriPosition;
            
                //this.transform.localScale = new Vector3 (21, 2, 29); //About 1.4x size
                
            
                //if left-button clicked, set centered boolean true and move handpos
                if (Input.GetMouseButtonDown (0) && !player.handCentered) {
                    clicked = true;
                    player.handCentered = true;
                    player.handPos = new Vector3 (50, 400, -125);
                    player.reposition();
                    
                }//check if it is centered then do handler action
                else if (Input.GetMouseButtonDown (0) && player.handCentered) {
                    player.handCentered = false;
                    player.handPos = new Vector3 (550, 10, -375);
                    player.reposition();
                    if (handler != null) {
                        handler.clicked ();
                    }
                    
                }
                

                //if right-click is held down
                if (Input.GetMouseButton (1)) { 
                    /*if (player.player1) { //player 1
                        newPosition.z = oriPosition.z + 200; //Move up from bottom of screen
                    } else if (!player.player1) { //player 2
                        newPosition.z = oriPosition.z - 200; //Move down from top of screen
                    }*/
                    //this.transform.position = newPosition;
                    this.transform.localScale = new Vector3 (45, 10, 63); //3x size
                    this.transform.position = new Vector3 (newPosition.x, 50, newPosition.z + 200);
                }

                //if right-click is up
                //echan
                if (Input.GetMouseButtonUp (1)) { 
                    /*if (player.player1) { //player 1
                        newPosition.z = oriPosition.z + 200; //Move up from bottom of screen
                    } else if (!player.player1) { //player 2
                        newPosition.z = oriPosition.z - 200; //Move down from top of screen
                    }*/
                    //this.transform.position = newPosition;
                    this.transform.localScale = new Vector3 (15, 1, 21); //3x size
                    this.transform.position = newPosition;
                }
            }
        
        }
        //keeping for future use
        void OnMouseExit ()
        {
            //Normal scaling
            this.transform.localScale = new Vector3 (15, 1, 21);

            //By Pedro
            //this.halo.enabled = false;
        
            //Moves back to normal position if not clicked
            /*if (!clicked && !inMotion) {
                this.transform.position = oriPosition;
            }*/
            zoomed = false;
            clicked = false;
        }

        public int getDamage ()
        {
            return this.dmg;    
        }

        public int getManaCost ()
        {
            return manaCost;    
        }

        public void setCanAttack (bool canAttackNow)
        {
            this.canAttackNow = canAttackNow;   
        }
    
        public bool canAttack ()
        {
            return canAttackNow;    
        }
    
        public void attack (AbstractCard clicked, AbstractCard target, bool damageback)
        {
            calculateDirection (target.transform.position, true);

            //NetworkManager.Send (CardAttackProtocol.Prepare (GameManager.matchID, attack, fieldPosition), ProcessSummonCard);     
            target.receiveAttack (dmg);

            //by Pedro
            audioSource.clip = Resources.Load ("Sounds/attack") as AudioClip;
            //audioSource.PlayDelayed (1);
            audioSource.Play ();

            if (damageback) {

                clicked.receiveAttack (target.dmg);
            }
            canAttackNow = false;
        
        
        
        }
    
        public void attackTree (Trees tree)
        {
            tree.receiveAttack (dmg);

            //by Pedro
            audioSource.clip = Resources.Load ("Sounds/attack") as AudioClip;
            //audioSource.PlayDelayed (1);
            audioSource.Play ();

            setCanAttack (false);
            player.clickedCard = null;
            calculateDirection (tree.transform.position, true);

        
        }
    
        //Set the card so it can attack again
        public void endTurn ()
        {
            canAttackNow = true;
        }
    
        public void receiveAttack (int dmg)
        {
            dmgTimer = 120;
            transform.Find ("DamageText").GetComponent<TextMesh> ().text = "-" + dmg;
            hp -= dmg;
            Debug.Log ("Was dealt " + dmg + " damage and is now at " + hp + " hp");
        
            if (hp <= 0) {
                Debug.Log ("DEAD"); 
                //handler = new RemoveFromPlay (this, player);
                //handler.affect ();

                removeAfterDelay = true;
            }
        
        }
    
        public void calculateDirection (Vector3 targetPos, bool moveBack)
        {
            this.moveBack = moveBack;
            this.startPosition = transform.position;
            inMotion = true;

            this.targetPosition = targetPos;
            float deltaX = targetPos.x - transform.position.x;
            float deltaZ = targetPos.z - transform.position.z;
            velocity = 5;
            terminalVelocity = 5500;
        

        
            angle = Mathf.Atan2 (deltaZ, deltaX);
        
    
            distance = Mathf.Sqrt (deltaX * deltaX + deltaZ * deltaZ);
        

        
        }
    
        bool moving ()
        {
        
            //Moves as long as it is supposed to
            if (distance > 20) {
                velocity *= 1.3f;
                if (velocity > terminalVelocity)
                    velocity = terminalVelocity;
            
                float deltaX = Mathf.Cos (angle) * velocity * Time.deltaTime * 3;
                float deltaZ = Mathf.Sin (angle) * velocity * Time.deltaTime * 3;
            
            
                distance -= Mathf.Sqrt (deltaX * deltaX + deltaZ * deltaZ);
                transform.position = new Vector3 (transform.position.x + deltaX, transform.position.y, transform.position.z + deltaZ);
            
            
                return true;
            } else if (inMotion) {
            
                inMotion = false;
                transform.position = targetPosition;
                if (moveBack) {
                    calculateDirection (startPosition, false);
                    terminalVelocity = 2500;
                    velocity = 50;


                }
            }
        
            return false;
        }
    
        void Update ()
        {   
            if (removeAfterDelay) {
                delayTimer += Time.deltaTime;

                if (delayTimer > DELAY_CONSTANT) {

                    handler = new RemoveFromPlay (this, player);
                    handler.affect ();
                    removeAfterDelay = false;
                    delayTimer = 0;
                }

                Debug.Log (Time.deltaTime);

            }

            //Change text on card
            transform.Find ("AttackText").GetComponent<TextMesh> ().text = dmg.ToString ();
            transform.Find ("HealthText").GetComponent<TextMesh> ().text = hp.ToString ();
            if (hp < maxHP) {
                transform.Find ("HealthText").GetComponent<MeshRenderer> ().material.color = Color.red;
            } else if (hp > maxHP) {
                transform.Find ("HealthText").GetComponent<MeshRenderer> ().material.color = Color.green;
            } else if (hp == maxHP) {
                transform.Find ("HealthText").GetComponent<MeshRenderer> ().material.color = Color.white;
            }
            if (dmg < naturalDmg) {
                transform.Find ("AttackText").GetComponent<MeshRenderer> ().material.color = Color.red;
            } else if (dmg > naturalDmg) {
                transform.Find ("AttackText").GetComponent<MeshRenderer> ().material.color = Color.green;
            } else if (dmg == naturalDmg) {
                transform.Find ("AttackText").GetComponent<MeshRenderer> ().material.color = Color.white;
            }
            if (canAttackNow) {
                transform.Find ("DoneText").GetComponent<TextMesh> ().text = "";
            } else if (!canAttackNow) {
                transform.Find ("DoneText").GetComponent<TextMesh> ().text = "Done";
            }
            //If damaged
            if (dmgTimer > 0) {
                dmgTimer--;
            } else {
                transform.Find ("DamageText").GetComponent<TextMesh> ().text = "";
            }
            //Moving
            moving ();
        }

        //For wrapping long text
        public static string TextWrap (string originaltext, int chars_in_line)
        {
            string output = "";
            string[] words = originaltext.Split (' ');
            int line = 0;
            int numWords = 0;
            foreach (string word in words) {
                if ((line + word.Length + 1) <= chars_in_line) {
                    output += " " + word;
                    line += word.Length + 1;
                } else { 
                    output += " \n" + word;
                    line = word.Length;
                }
                if (++numWords == 20) {
                    output += "...";
                    break;
                }
            }

            return output;
        }
    }
}
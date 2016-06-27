using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ClashBattleUnit : MonoBehaviour
{

    public NavMeshAgent agent;
    private ClashBattleController controller;
    private Animator anim;

    public ClashBattleUnit target;
    Vector3 targetPoint = Vector3.zero;

    public Vector3 TargetPoint {
        get { return targetPoint; }
        set { targetPoint = value; }
    }

    public ClashSpecies species;
    public int currentHealth = 0;
    public int damage = 0;
    public float timeBetweenAttacks = 1.0f;
    // The time in seconds between each attack.
    float timer;
    // Timer for counting up to the next attack.
    float timeSinceSpawn = 0.0f;

    void Awake ()
    {
        agent = GetComponent<NavMeshAgent> ();
        anim = GetComponent<Animator> ();
        controller = GameObject.Find ("Battle Menu").GetComponent<ClashBattleController> ();
    }

    void Start ()
    {
        // Set current health depending on the species data.
        currentHealth += species.hp;
        timeBetweenAttacks = 100f / species.attackSpeed;
        damage += species.attack;
        if (agent != null) {
            agent.speed += species.moveSpeed / 20.0f;
        }
    }

    void Update ()
    {
        timer += Time.deltaTime;
        timeSinceSpawn += Time.deltaTime;
        if (!target && targetPoint == Vector3.zero) {
            Idle ();
        } else if (targetPoint != Vector3.zero) {
            anim.SetTrigger ("Walking");
            if (agent && agent.isActiveAndEnabled)
                agent.destination = targetPoint;
        } else if ((target.currentHealth > 0) && (timer >= timeBetweenAttacks) && (currentHealth >= 0.0f)) {
            Attack ();
        } else if (target.currentHealth <= 0) {
            target = null;		
        }

    }

    void Idle ()
    {
        //Added by Omar triggers eating animation
        if (anim != null) {
            anim.SetTrigger ("Eating");
        }
    }

    void Attack ()
    {
        timer = 0f;
        if (agent && agent.isActiveAndEnabled) {
            agent.destination = target.transform.position;
			
//			Debug.Log (tag + " " + species.name +
//			           " distance to " +
//			           target.tag + " " + target.species.name +
//			           " is " + agent.remainingDistance);
            if (agent.remainingDistance <= agent.stoppingDistance) {
                //Added by Omar triggers Attacking animation
//				Debug.Log(species.name + " attacking " + target.species.name);
                agent.gameObject.transform.LookAt (target.transform);
                if (anim != null) {
                    anim.SetTrigger ("Attacking");
                }
                target.TakeDamage (damage, this);
            } else {
                if (anim != null) {
                    anim.SetTrigger ("Walking");
                }
            }
        }
    }

    void Die ()
    {
        //Disable all functions here
        if (anim != null) {
            anim.SetTrigger ("Dead");
        }
        target = null;
        if (agent != null)
            agent.enabled = false;
        if (species.type == ClashSpecies.SpeciesType.PLANT) {
            RemoveBuffs (species, this.gameObject.tag);
            if (this.gameObject.tag == "Ally")
                controller.UpdateBuffPanel (species, false);
            this.gameObject.GetComponent<Renderer> ().enabled = false;
        }
    }

    void TakeDamage (int damage, ClashBattleUnit source = null)
    {
        if (timeSinceSpawn < 1.0)
            return; // Be invincible for the first second after being spawned
//		Debug.Log (tag + " " + species.name + " taking " + damage + " damage from " + source.tag + " " + source.species.name);

        currentHealth = Mathf.Max (0, currentHealth - damage);
        if (currentHealth == 0) {
            Die ();
        }
    }

    void RemoveBuffs (ClashSpecies cs, string tag)
    {
        var team = GameObject.FindGameObjectsWithTag (tag);
		
        foreach (var teammate in team) {
            //teammate != this.gameObject so it doesn't get a buff from itself
            if (teammate != this.gameObject) {
                var teammateAttribute = teammate.GetComponent<ClashBattleUnit> ();

                switch (cs.name) {
                case "Big Tree":	//hp buff
                    teammateAttribute.currentHealth -= 100;
                    //teammateAttribute.TakeDamage (100);
                    break;
                case "Baobab":	//damage buff
                    teammateAttribute.damage -= 8;
                    break;
                case "Trees and Shrubs":	//attack speed buff
                    if (teammateAttribute.agent != null)
                        teammateAttribute.agent.speed -= 5.0f;
                    break;
                default:
                    break;
                }
            }
        }
    }

    public void setSelected (bool isSelected)
    {
        foreach (Transform child in transform) {
            if (child.CompareTag (Constants.TAG_HEALTH_BAR))
                child.gameObject.SetActive (isSelected);
        }
    }
}

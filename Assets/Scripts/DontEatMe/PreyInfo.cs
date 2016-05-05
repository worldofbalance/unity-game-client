using System;           // Math
using System.Linq;      // AsEnumerable

/**
	Extends BuildInfo.cs to add prey-specific content.

	@author Jeremy Erickson
*/
public class PreyInfo : BuildInfo {
	private int currentHealth; // Current health of prey (<=0 implies prey is dead / consumed)
    
	/**
        Initializes prey data and returns this instance.

        This is necessary to synchronize initialization of supplementary data members that rely on these values
        specifically when initializing via a call to the parent's AddComponent method, as both the typical Start and
        Awake methods are not properly synced.

        A typical example of how to optimally attach a PreyInfo component to a parent object would be:
        PreyInfo preyInfo = parentObject.AddComponent<PreyInfo>().Initialize(speciesID);
	*/
    public PreyInfo Initialize (int _speciesID) {
        // Initialize critical species-dependent data
        this.speciesId = _speciesID;
		this.speciesType = 1;
		this.currentHealth = SpeciesConstants.Health(this.speciesId);

        // Return the instance reference once initialized 
        return this;
	}

	/**
		Return current health.
	*/
	public int GetHealth ()
	{
		return this.currentHealth;
	}

	/**
		Reduces current health and returns amount of health reduced.
	*/
    public int Injure (int damage)
    {
        // Return 0 if already consumed
        if (this.currentHealth <= 0)
            return 0;

        // Reduce health by damage amount
        this.currentHealth -= damage;

        // If damage reduces health below zero, return only what health was consumed
        if (this.currentHealth < 0)
            return this.currentHealth + damage;

        // Otherwise the damage amount == health consumed
        return damage;
    }

	/**
		Returns true if prey is consumed, false otherwise.
	*/
	public bool Consumed ()
	{
		return this.currentHealth <= 0;
	}

    /**
        Returns true if a species is a natural predator.
        Search by species name.
    */
    public bool IsPreyOf (string predName)
    {
        return SpeciesConstants.PredatorIDList(this.speciesId).AsEnumerable().Contains(SpeciesConstants.SpeciesID(predName));
    }

    /**
        Returns true if a species is a natural predator.
        Search by species ID.
    */
    public bool IsPreyOf (int predID)
    {
        return SpeciesConstants.PreyIDList(this.speciesId).AsEnumerable().Contains(predID);
    }


	// Methods for potential later use //
	/**
		Increase current health and return fully healed status
	*/
	public bool Heal (int health)
	{
		this.currentHealth = Math.Min(this.currentHealth + health, SpeciesConstants.Health(this.speciesId));
		return this.Healed();
	}

	/**
		Returns true if prey is at max health, false otherwise.
	*/
	public bool Healed ()
	{
		return this.currentHealth == SpeciesConstants.Health(this.speciesId);
	}
}

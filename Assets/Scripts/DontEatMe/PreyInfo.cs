using System;           // Math
using System.Linq;      // AsEnumerable

/**
	Extends BuildInfo.cs to add prey-specific content.

	@author Jeremy Erickson
    @see    BuildInfo.cs
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

        @param  _speciesID  a unique species identifier (should match database)

        @return the calling (PreyInfo) object
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
		Returns a prey's current health.

        @return an integer
	*/
	public int GetHealth ()
	{
		return this.currentHealth;
	}

	/**
		Reduces current health and returns amount of health reduced.

        @param  damage  the amount of damage to inflict (int)

        @return the amount of damage actually inflicted (int)
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

        @return a boolean value
	*/
	public bool Consumed ()
	{
		return this.currentHealth <= 0;
	}

    /**
        Returns true if a species is a natural predator.
        Search by species name.

        @param  predName    a species name (string)

        @return a boolean value
    */
    public bool IsPreyOf (string predName)
    {
        return SpeciesConstants.PredatorIDList(this.speciesId).AsEnumerable().Contains(SpeciesConstants.SpeciesID(predName));
    }

    /**
        Returns true if a species is a natural predator.
        Search by species ID.

        @param  predID  a species' unique ID (should match database)

        @return a boolean value
    */
    public bool IsPreyOf (int predID)
    {
        return SpeciesConstants.PredatorIDList(this.speciesId).AsEnumerable().Contains(predID);
    }

    /**
        Returns true if a species is a natural predator.
        Search by a predator's BuildInfo / PreyInfo component.

        @param  predator    a BuildInfo or child object
    */
    public bool IsPreyOf (BuildInfo predator)
    {
        return SpeciesConstants.PredatorIDList(this.speciesId).AsEnumerable().Contains(predator.speciesId);
    }

	/**
		Increase current health and return fully healed status

        @param  health  amount of health to restore as a non-negative integer

        @return true if fully healed after method call, false otherwise
	*/
	public bool Heal (int health)
	{
		this.currentHealth = Math.Min(this.currentHealth + Math.Max(health, 0), SpeciesConstants.Health(this.speciesId));
		return this.Healed();
	}

	/**
		Returns true if prey is at max health, false otherwise.

        @return a boolean value
	*/
	public bool Healed ()
	{
		return this.currentHealth == SpeciesConstants.Health(this.speciesId);
	}
}

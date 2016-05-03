using System;           // Math
using System.Linq;      // AsEnumerable

/**
	Extends BuildInfo.cs to add predator-specific content.

	@author Jeremy Erickson
*/
public class PredatorInfo : BuildInfo {
	private int currentHunger; // Current hunger of predator (<= 0 implies not hungry)
	private int currentVoracity; // Current voracity of predator (<= 0 implies unable to consume)

	/**
		Initializes predator data and returns this instance.

        This is necessary to synchronize initialization of supplementary data members that rely on these values
        specifically when initializing via a call to the parent's AddComponent method, as both the typical Start and
        Awake methods are not properly synced.

        A typical example of how to optimally attach a PredatorInfo component to a parent object would be:
        PredatorInfo predatorInfo = parentObject.AddComponent<PredatorInfo>().Initialize(speciesID);
	*/
    public PredatorInfo Initialize (int _speciesID) {
        // Initialize critical species-dependent data
        this.speciesId = _speciesID;
		this.speciesType = 2;
        this.currentHunger = SpeciesConstants.Hunger(this.speciesId);
		this.currentVoracity = SpeciesConstants.Voracity(this.speciesId);

        // Return the instance reference once initialized 
        return this;
	}

	/**
		Return current hunger.
	*/
	public int GetHunger ()
	{
		return this.currentHunger;
	}

	/**
		Returns current voracity.
		NOTE: For now, voracity will be treated as a fixed value; in future, voracity may change depending on certain
		factors, such as how full a predator is, etc.
	*/
	public int GetVoracity ()
	{
		return this.currentVoracity;
	}

	/**
        Reduces current hunger and returns nutritional amount consumed.
        Specifies an arbitrary amount of nutrition (positive integer only).
        If the "forced" parameter is set to true (default is false), then the predator's voracity is ignored altogether;
        otherwise, the voracity places an upper bound on the amount consumed this call.
	*/
	public int Consume (int nutrition, bool forced = false)
	{
        // Return 0 if nutrition is non-positive or if predator is not hungry
        if (nutrition <= 0 || this.currentHunger <= 0)
            return 0;

        int consumed = 0; // Set amount consumed to 0 initially
        
        // Check "force" flag, reduce hunger accordingly
        if (forced)
            consumed = this.currentHunger - nutrition;

        else
            consumed = this.currentHunger - Math.Min(this.currentVoracity, nutrition);

        // Return appropriate amount conusmed
        if (this.currentHunger < 0)
            return consumed + this.currentHunger;

        return consumed;
	}

    /**
        Reduces current hunger and returns nutritional amount consumed.
        Specifies a PreyInfo object to consume, reducing its health in the process.
        If the "forced" parameter is set to true (default is false), then the predator's prey list is ignored;
        otherwise, the predator will only consume valid prey.
    */
    public int Consume (PreyInfo prey, bool forced = false)
    {
        // Check if prey should be consumed
        if (this.IsPredatorOf(prey.speciesId) || forced)
        {
            int nutrition = prey.Injure(this.currentVoracity);
            this.currentHunger -= nutrition;
            return nutrition;
        }

        // Otherwise return 0
        return 0;
    }

	/**
		Returns true if predator is hungry, false otherwise.
	*/
	public bool Hungry ()
	{
		return this.currentHunger > 0;
	}

	/**
		Returns true if predator has satisfied its hunger, false otherwise.
		It is equivalent to !Hungry()
	*/
	public bool Satisfied ()
	{
		return this.currentHunger <= 0;
	}

    /**
        Returns true if a species is a natural prey.
        Search by species name.
    */
    public bool IsPredatorOf (string preyName)
    {
        return SpeciesConstants.PreyIDList(this.speciesId).AsEnumerable().Contains(SpeciesConstants.SpeciesID(preyName));
    }

    /**
        Returns true if a species is a natural prey.
        Search by species ID.
    */
    public bool IsPredatorOf (int preyID)
    {
        return SpeciesConstants.PreyIDList(this.speciesId).AsEnumerable().Contains(preyID);
    }

	// Methods for potential later use //

	/**
		Immediately reduces voracity to zero, rendering the predator unable to consume.
		The predator's hunger (i.e. nutritional need) is not affected.
		NOTE: possible future scenarios might include eating something poisonous and getting sick for a bit,
		or getting a mouthfull of porcupine quills, etc. 
	*/
	public void KillVoracity ()
	{
		this.currentVoracity = 0;
	}

	/**
		Immediately restores voracity to its max value.
	*/
	public void RestoreVoracity ()
	{
		this.currentVoracity = SpeciesConstants.Voracity(this.speciesId);
	}

	/**
		Sets the current voracity level, keeping it within legal bounds.
	*/
	public void SetVoracity (int voracity)
	{
		if (voracity < 0)
			voracity = 0;
		else if (voracity > SpeciesConstants.Voracity(this.speciesId))
			voracity = SpeciesConstants.Voracity(this.speciesId);

		this.currentVoracity = voracity;
	}

	/**
		Returns true if a predator can consume.
	*/
	public bool Voracious ()
	{
		return this.currentVoracity >= 0;
	}

	/**
		Increases current hunger.
	*/
	public void Regurgitate (int nutrition)
	{
		this.currentHunger = Math.Min(this.currentHunger + nutrition, SpeciesConstants.Hunger(this.speciesId));
	}
}

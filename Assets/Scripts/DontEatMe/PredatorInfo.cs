using System;

/**
	Extends BuildInfo.cs to add predator-specific content.

	@author Jeremy Erickson
*/
public class PredatorInfo : BuildInfo {
	private int currentHunger; // Current hunger of predator (<= 0 implies not hungry)
	private int currentVoracity; // Current voracity of predator (<= 0 implies unable to consume)

	/**
		Initialize predator data.
	*/
	void Start () {
		this.speciesType = 2;
		this.currentHunger = SpeciesConstants.Hunger(this.speciesId);
		this.currentVoracity = SpeciesConstants.Voracity(this.speciesId);
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
		Reduces current hunger and returns hungry status.
	*/
	public bool Consume (int nutrition)
	{
		this.currentHunger -= nutrition;
		return this.Hungry();
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

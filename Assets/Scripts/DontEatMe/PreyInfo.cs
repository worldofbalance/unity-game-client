using System;

/**
	Extends BuildInfo.cs to add prey-specific content.

	@author Jeremy Erickson
*/
public class PreyInfo : BuildInfo {
	private int currentHealth; // Current health of prey (<=0 implies prey is dead / consumed)

	/**
		Initialize prey data.
	*/
	void Start () {
		this.speciesType = 1;
		this.currentHealth = SpeciesConstants.Health(this.speciesId);
	}

	/**
		Return current health.
	*/
	public int GetHealth ()
	{
		return this.currentHealth;
	}

	/**
		Reduce current health and return consumed status.
	*/
	public bool Injure (int damage)
	{
		this.currentHealth -= damage;
		return this.Consumed();
	}

	/**
		Returns true if prey is consumed, false otherwise.
	*/
	public bool Consumed ()
	{
		return this.currentHealth <= 0;
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

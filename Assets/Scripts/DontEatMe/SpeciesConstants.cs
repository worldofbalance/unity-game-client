/**
	Static constants and methods for defining and returning species attributes for the Don't Eat Me minigame.

	@author: Jeremy Erickson
*/
public class SpeciesConstants
{
	/* PRIVATE CONSTANTS */
	// Changes to these will populate the necessary changes in all public constants and methods

	// Defines a prey
	private struct Prey
	{
		public string name;
		public int speciesID;
		public int health;

		public Prey (string n, int id, int h)
		{
			name = n;
			speciesID = id;
			health = h;
		}
	};

	// Defines a predator
	private struct Predator
	{
		public string name;
		public int speciesID;
		public int hunger;
		public int voracity;

		public Predator (string n, int id, int h, int v)
		{
			name = n;
			speciesID = id;
			hunger = h;
			voracity = v;
		}
	};

	// All available prey
	private static Prey[] PREY = {
		new Prey("Buffalo", 7, 100),
		new Prey("TreeMouse", 31, 5),
		new Prey("BushHyrax", 48, 15),
		new Prey("KoriBuskard", 65, 20),
		new Prey("CrestedPorcupine", 72, 25),
		new Prey("Oribi", 73, 50)
	}; 

	// All available predators
	private static Predator[] PREDATORS = {
		new Predator("BatEaredFox", 4, 10, 10),
		new Predator("BlackMamba", 6, 20, 5),
		new Predator("ServalCat", 69, 15, 10),
		new Predator("AfricanWildDog", 74, 55, 50),
		new Predator("Leopard", 80, 40, 30),
		new Predator("Lion", 86, 100, 40)
	};

	// Type constants
	private static short PREY_TYPE = 1;
	private static short PREDATOR_TYPE = 2;

	// Default values
	private static short DEFAULT_TYPE = 0;
	private static int DEFAULT_SPECIES_ID = 0;
	private static int DEFAULT_HEALTH = 10;
	private static int DEFAULT_HUNGER = 25;
	private static int DEFAULT_VORACITY = 25;

	/* PUBLIC CONSTANTS */
	public static int NUM_PREY = PREY.Length;
	public static int NUM_PREDATORS = PREDATORS.Length;
	public static int NUM_SPECIES = NUM_PREY + NUM_PREDATORS;

	/* PUBLIC METHODS */
	/**
		Returns a species ID associated with a species name.
	*/
	public static int SpeciesID (string name)
	{
		// Search prey
		foreach (Prey prey in PREY)
		{
			if (prey.name == name)
				return prey.speciesID;
		}
		// Search predators
		foreach (Predator predator in PREDATORS)
		{
			if (predator.name == name)
				return predator.speciesID;
		}
		// Otherwise return default
		return DEFAULT_SPECIES_ID;
	}

	/**
		Returns a name associated with a species ID.
	*/
	public static string SpeciesName (int id)
	{
		// Search prey
		foreach (Prey prey in PREY)
		{
			if (prey.speciesID == id)
				return prey.name;
		}
		// Search predators
		foreach (Predator predator in PREDATORS)
		{
			if (predator.speciesID == id)
				return predator.name;
		}
		// Otherwise return empty string
		return "";
	}

	/**
		Returns a prey's starting health.
		Search by species name.
	*/
	public static int Health (string name)
	{
		foreach (Prey prey in PREY)
		{
			if (prey.name == name)
				return prey.health;
		}
		return DEFAULT_HEALTH;
	}

	/**
		Returns a prey's starting health.
		Search by species ID.
	*/
	public static int Health (int id)
	{
		foreach (Prey prey in PREY)
		{
			if (prey.speciesID == id)
				return prey.health;
		}
		return DEFAULT_HEALTH;
	}

	/**
		Returns a predator's starting hunger.
		Search by species name.
	*/
	public static int Hunger (string name)
	{
		foreach (Predator predator in PREDATORS)
		{
			if (predator.name == name)
				return predator.hunger;
		}
		return DEFAULT_HUNGER;
	}

	/**
		Returns a predator's starting hunger.
		Search by species ID.
	*/
	public static int Hunger (int id)
	{
		foreach (Predator predator in PREDATORS)
		{
			if (predator.speciesID == id)
				return predator.hunger;
		}
		return DEFAULT_HUNGER;
	}

	/**
		Returns a predator's starting voracity.
		Search by species name.
	*/
	public static int Voracity (string name)
	{
		foreach (Predator predator in PREDATORS)
		{
			if (predator.name == name)
				return predator.voracity;
		}
		return DEFAULT_VORACITY;
	}

	/**
		Returns a predator's starting voracity.
		Search by species ID.
	*/
	public static int Voracity (int id)
	{
		foreach (Predator predator in PREDATORS)
		{
			if (predator.speciesID == id)
				return predator.voracity;
		}
		return DEFAULT_VORACITY;
	}

	/**
		Returns a species type.
		Search by species name.
	*/
	public static short SpeciesType (string name)
	{
		// Search prey
		foreach (Prey prey in PREY)
		{
			if (prey.name == name)
				return PREY_TYPE;
		}
		// Search predators
		foreach (Predator predator in PREDATORS)
		{
			if (predator.name == name)
				return PREDATOR_TYPE;
		}
		// Otherwise return defaul
		return DEFAULT_TYPE;
	}

	/**
		Returns a species type.
		Search by species ID.
	*/
	public static short SpeciesType (int id)
	{
		// Search prey
		foreach (Prey prey in PREY)
		{
			if (prey.speciesID == id)
				return PREY_TYPE;
		}
		// Search predators
		foreach (Predator predator in PREDATORS)
		{
			if (predator.speciesID == id)
				return PREDATOR_TYPE;
		}
		// Otherwise return defaul
		return DEFAULT_TYPE;
	}
}
using System.Collections.Generic;
using System.Linq;

/**
	Static constants and methods for defining and returning species attributes for the Don't Eat Me minigame.

	@author: Jeremy Erickson
*/
public class SpeciesConstants
{
	/* PRIVATE CONSTANTS */
	// Changes to these will populate the necessary changes in all public constants and methods

    // Reference for all species ID values (consistent with database)
    private enum SPECIES_ID
    {        
        AfricanWildDog      = 74,
        BatEaredFox         = 4,
        BlackMamba          = 6,
        Buffalo             = 7,
        BushHyrax           = 48,
        CrestedPorcupine    = 72,
        KoriBuskard         = 65,
        Leopard             = 80,
        Lion                = 86,
        Oribi               = 73,
        ServalCat           = 69,
        TreeMouse           = 31
    };

	// Defines a prey
	private struct Prey
	{
		public string name;
		public int speciesID;
		public int health;

        // public int[] preyIDLIst; // TODO: create prey list --> includes plants, but might include other prey (TBD)
        public int[] predatorIDList;

		public Prey (string _name, int _speciesID, int _health, int[] _predatorIDList)
		{
			name = _name;
			speciesID = _speciesID;
			health = _health;
            predatorIDList = _predatorIDList;
		}
	};

	// Defines a predator
	private struct Predator
	{
		public string name;
		public int speciesID;
		public int hunger;
		public int voracity;

        public int[] preyIDList;
        // public int[] predatorIDLIst; // TODO: create predator list --> includes larger predators (TBD)

		public Predator (string _name, int _speciesID, int _hunger, int _voracity, int[] _preyIDList)
		{
			name = _name;
			speciesID = _speciesID;
			hunger = _hunger;
			voracity = _voracity;
            preyIDList = _preyIDList;
		}
	};

	// All available prey
	private static Prey[] PREY = {
		new Prey
        (
            "Buffalo",                          // Name
            (int)SPECIES_ID.Buffalo,            // Species ID
            100,                                // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.Lion
            }
        ),
		new Prey
        (
            "TreeMouse",                        // Name
            (int)SPECIES_ID.TreeMouse,          // Species ID
            5,                                  // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.BatEaredFox,
                (int)SPECIES_ID.BlackMamba,
                (int)SPECIES_ID.ServalCat,
                (int)SPECIES_ID.AfricanWildDog,
                (int)SPECIES_ID.Leopard
            }
        ),
		new Prey
        (
            "BushHyrax",                        // Name
            (int)SPECIES_ID.BushHyrax,          // Species ID
            15,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.ServalCat,
                (int)SPECIES_ID.Leopard
            }
        ),
        new Prey
        (
            "KoriBuskard",                      // Name
            (int)SPECIES_ID.KoriBuskard,        // Species ID
            20,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.Leopard,
                (int)SPECIES_ID.Lion
            }
        ),
        new Prey
        (
            "CrestedPorcupine",                 // Name
            (int)SPECIES_ID.CrestedPorcupine,   // Species ID
            25,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.AfricanWildDog,
                (int)SPECIES_ID.Leopard
            }
        ),
        new Prey
        (
            "Oribi",                            // Name
            (int)SPECIES_ID.Oribi,              // Species ID
            50,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.AfricanWildDog,
                (int)SPECIES_ID.Leopard,
                (int)SPECIES_ID.Lion
            }
        )
	}; 

	// All available predators
	private static Predator[] PREDATORS = {
        new Predator
        (
            "BatEaredFox",                      // Name
            (int)SPECIES_ID.BatEaredFox,        // Species ID
            10,                                 // Hunger
            10,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse
            }
        ),
        new Predator
        (
            "BlackMamba",                       // Name
            (int)SPECIES_ID.BlackMamba,         // Species ID
            20,                                 // Hunger
            5,                                  // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax,
                (int)SPECIES_ID.CrestedPorcupine
            }
        ),
        new Predator
        (
            "ServalCat",                        // Name
            (int)SPECIES_ID.ServalCat,          // Species ID
            15,                                 // Hunger
            10,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax
            }
        ),
        new Predator
        (
            "AfricanWildDog",                   // Name
            (int)SPECIES_ID.AfricanWildDog,     // Species ID
            55,                                 // Hunger
            50,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.Oribi,
                (int)SPECIES_ID.CrestedPorcupine
            }
        ),new Predator
        (
            "Leopard",                          // Name
            (int)SPECIES_ID.Leopard,            // Species ID
            40,                                 // Hunger
            30,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax
            }
        ),
        new Predator
        (
            "Lion",                             // Name
            (int)SPECIES_ID.Lion,               // Species ID
            100,                                // Hunger
            40,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.Oribi,
                (int)SPECIES_ID.KoriBuskard,
                (int)SPECIES_ID.Buffalo
            }
        )
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

    public static IEnumerable<string> PREY_NAMES = PREY.AsEnumerable().Select(prey => prey.name);
    public static IEnumerable<string> PREDATOR_NAMES = PREDATORS.AsEnumerable().Select(predator => predator.name);

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

    /**
        Returns a predator list for a species as species ID values.
        Search by species name.
    */
    public static int[] PredatorIDList (string name)
    {
        // Search prey only for now
        foreach (Prey prey in PREY)
        {
            if (prey.name == name)
                return prey.predatorIDList;
        }
        // Otherwise return empty array
        return new int[]{};
    }

    /**
        Returns a predator list for a species as species ID values.
        Search by species ID.
    */
    public static int[] PredatorIDList (int id)
    {
        // Search prey only for now
        foreach (Prey prey in PREY)
        {
            if (prey.speciesID == id)
                return prey.predatorIDList;
        }
        // Otherwise return empty array
        return new int[]{};
    }

    /**
        Returns a prey list for a species as species ID values.
        Search by species name.
    */
    public static int[] PreyIDList (string name)
    {
        // Search predators only for now
        foreach (Predator predator in PREDATORS)
        {
            if (predator.name == name)
                return predator.preyIDList;
        }
        // Otherwise return emtpy array
        return new int[]{};
    }

    /**
        Returns a prey list for a species as species ID values.
        Search by species ID.
    */
    public static int[] PreyIDList (int id)
    {
        // Search predators only for now
        foreach (Predator predator in PREDATORS)
        {
            if (predator.speciesID == id)
                return predator.preyIDList;
        }
        // Otherwise return emtpy array
        return new int[]{};
    }
}
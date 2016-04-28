using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        public string lore; // Lore (i.e. description) taken from 'http://smurf.sfsu.edu/~wob/guide/species.php'
		public int speciesID;
		public int health;

        // public int[] preyIDLIst; // TODO: create prey list --> includes plants, but might include other prey (TBD)
        public int[] predatorIDList;

		public Prey (string _name, int _speciesID, int _health, int[] _predatorIDList, string _lore = "")
		{
			name = _name;
			speciesID = _speciesID;
			health = _health;
            predatorIDList = _predatorIDList;
            lore = _lore;
		}
	};

	// Defines a predator
	private struct Predator
	{
		public string name;
        public string lore; // Lore (i.e. description) taken from 'http://smurf.sfsu.edu/~wob/guide/species.php'
		public int speciesID;
		public int hunger;
		public int voracity;

        public int[] preyIDList;
        // public int[] predatorIDLIst; // TODO: create predator list --> includes larger predators (TBD)

		public Predator (string _name, int _speciesID, int _hunger, int _voracity, int[] _preyIDList, string _lore = "")
		{
			name = _name;
			speciesID = _speciesID;
			hunger = _hunger;
			voracity = _voracity;
            preyIDList = _preyIDList;
            lore = _lore;
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
            },
            // Lore
            "The African buffalo is a large African bovine. " +
            "It is not closely related to the slightly larger wild Asian water buffalo, but its ancestry remains unclear. " +
            "Owing to its unpredictable nature which makes it highly dangerous to humans, it has not been domesticated " +
            "unlike its Asian counterpart the domestic Asian water buffalo."
        ),
		new Prey
        (
            "Tree Mouse",                       // Name
            (int)SPECIES_ID.TreeMouse,          // Species ID
            5,                                  // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.BatEaredFox,
                (int)SPECIES_ID.BlackMamba,
                (int)SPECIES_ID.ServalCat,
                (int)SPECIES_ID.AfricanWildDog,
                (int)SPECIES_ID.Leopard
            },
            // Lore
            "Tree Mouse, Prionomys batesi, is a poorly understood climbing mouse from Central Africa. " +
            "It is unique enough that it has been placed in a genus of its own, Prionomys, since its discovery in 1910."
        ),
		new Prey
        (
            "Bush Hyrax",                       // Name
            (int)SPECIES_ID.BushHyrax,          // Species ID
            15,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.ServalCat,
                (int)SPECIES_ID.Leopard
            },
            // Lore
            "The yellow-spotted rock hyrax or bush hyrax is a species of mammal in the family Procaviidae."
        ),
        new Prey
        (
            "Kori Buskard",                     // Name
            (int)SPECIES_ID.KoriBuskard,        // Species ID
            20,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.Leopard,
                (int)SPECIES_ID.Lion
            },
            // Lore
            "The Kori Bustard is a large bird native to Africa. It is a member of the bustard family. " +
            "It may be the heaviest bird capable of flight."
        ),
        new Prey
        (
            "Crested Porcupine",                // Name
            (int)SPECIES_ID.CrestedPorcupine,   // Species ID
            25,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.AfricanWildDog,
                (int)SPECIES_ID.Leopard
            },
            // Lore
            "The crested porcupine is a species of rodent in the Hystricidae family."
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
            },
            // Lore
            "Oribi are graceful slender-legged, long-necked small antelope found in grassland almost throughout Sub-Saharan Africa."
        )
	}; 

	// All available predators
	private static Predator[] PREDATORS = {
        new Predator
        (
            "Bat-Eared Fox",                    // Name
            (int)SPECIES_ID.BatEaredFox,        // Species ID
            10,                                 // Hunger
            10,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse
            },
            // Lore
            "The bat-eared fox is a canid of the African savanna, named for its large ears. " +
            "Fossil records show this canid to first appear during the middle Pleistocene, about 800,000 years ago."
        ),
        new Predator
        (
            "Black Mamba",                      // Name
            (int)SPECIES_ID.BlackMamba,         // Species ID
            20,                                 // Hunger
            5,                                  // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax,
                (int)SPECIES_ID.CrestedPorcupine
            },
            // Lore
            "The black mamba, also called the common black mamba or black-mouthed mamba, is the longest venomous snake in Africa, " +
            "averaging around 2.5 to 3.2 m in length, and sometimes growing to lengths of 4.45 m."
        ),
        new Predator
        (
            "Serval Cat",                       // Name
            (int)SPECIES_ID.ServalCat,          // Species ID
            15,                                 // Hunger
            10,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax
            },
            // Lore
            "The serval, Leptailurus serval or Caracal serval, known in Afrikaans as Tierboskat, \"tiger-forest-cat\", " +
            "is a medium-sized African wild cat. DNA studies have shown that the serval is closely related to the " +
            "African golden cat and the caracal."
        ),
        new Predator
        (
            "African Wild Dog",                 // Name
            (int)SPECIES_ID.AfricanWildDog,     // Species ID
            55,                                 // Hunger
            50,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.Oribi,
                (int)SPECIES_ID.CrestedPorcupine
            },
            // Lore
            "African Wild Dog is a canid found only in Africa, especially in savannas and lightly wooded areas."
        ),
        new Predator
        (
            "Leopard",                          // Name
            (int)SPECIES_ID.Leopard,            // Species ID
            40,                                 // Hunger
            30,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax
            },
            // Lore
            "The leopard, Panthera pardus, is a member of the Felidae family and the smallest of the four \"big cats\" " +
            "in the genus Panthera, the other three being the tiger, lion, and jaguar."
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
            },
            // Lore
            "The lion is one of the four big cats in the genus Panthera, and a member of the family Felidae. " +
            "With some males exceeding 250 kg in weight, it is the second-largest living cat after the tiger."
        )
	};

	// Type constants (not consistent with database!)
	private static short PREY_TYPE = 1;
	private static short PREDATOR_TYPE = 2;

	// Default values (self-descriptive)
	private static short DEFAULT_TYPE = 0;

    private static int DEFAULT_SPECIES_ID = 0;
    private static string DEFAULT_SPECIES_NAME = "[ No name ]";
	private static int DEFAULT_HEALTH = 10;
	private static int DEFAULT_HUNGER = 25;
	private static int DEFAULT_VORACITY = 25;

    private static string DEFAULT_LORE = "[ No description available ]";

    // Contains (species name, species ID) pairs
    private static Dictionary<string, int> NAME_TO_ID =
        (from prey in PREY select new KeyValuePair<string, int>(prey.name, prey.speciesID))
        .Concat
        (from predator in PREDATORS select new KeyValuePair<string, int>(predator.name, predator.speciesID))
        .ToDictionary(pair => pair.Key, pair => pair.Value);

    /* PRIVATE METHODS */
    /**
        Internal method for removing whitespace and hyphen chars from a string.
    */
    private static string Compact (string s)
    {
        return s.Replace('\x20', '\x01').Replace('\x2d', '\x01');
    }

	/* PUBLIC CONSTANTS */

    // Number of available prey, predators, and total species, respectively
	public static readonly int NUM_PREY = PREY.Length;
	public static readonly int NUM_PREDATORS = PREDATORS.Length;
	public static readonly int NUM_SPECIES = NUM_PREY + NUM_PREDATORS;

    // Names of all available prey, predators, and total species, respectively
    public static readonly string[] PREY_NAMES = (from prey in PREY select prey.name).ToArray();
    public static readonly string[] PREDATOR_NAMES = (from predator in PREDATORS select predator.name).ToArray();
    public static readonly string[] SPECIES_NAMES = PREY_NAMES.Concat(PREDATOR_NAMES).ToArray();

	/* PUBLIC METHODS */
	/**
		Returns a species ID associated with a species name.
	*/
	public static int SpeciesID (string name)
	{
        return NAME_TO_ID.ContainsKey(name) ? NAME_TO_ID[name] : DEFAULT_SPECIES_ID;
	}

	/**
		Returns a name associated with a species ID.
	*/
	public static string SpeciesName (int id)
	{
        return NAME_TO_ID.ContainsValue(id) ? NAME_TO_ID.First(pair => pair.Value == id).Key : DEFAULT_SPECIES_NAME;
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

    /**
        Returns the lore for a species.
        Search by species name.
    */
    public static string SpeciesLore (string name)
    {
        // Search prey
        foreach (Prey prey in PREY)
        {
            if (prey.name == name)
                return prey.lore;
        }
        // Search predators
        foreach (Predator predator in PREDATORS)
        {
            if (predator.name == name)
                return predator.lore;
        }
        // Otherwise return default
        return DEFAULT_LORE;
    }

    /**
        Returns the lore for a species.
        Search by species name.
    */
    public static string SpeciesLore (int id)
    {
        // Search prey
        foreach (Prey prey in PREY)
        {
            if (prey.speciesID == id)
                return prey.lore;
        }
        // Search predators
        foreach (Predator predator in PREDATORS)
        {
            if (predator.speciesID == id)
                return predator.lore;
        }
        // Otherwise return default
        return DEFAULT_LORE;
    }
}
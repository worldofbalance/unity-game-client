using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

/**
	Static constants and methods for defining and returning species attributes for the Don't Eat Me minigame.


	@author     Jeremy Erickson
    @version    2.2
    @see        http://smurf.sfsu.edu/~wob/guide/species.php

    NOTE:   this script is intended as a stand-in for the remote database to provide local functionality for debugging and
            local-system database access in the event the remote database is not usable.
            The bulk of the data (with a few necessary exceptions) were parsed directly from the official game website, meaning
            most data will be consistent with the remote database when used.
            Refer to the official site for more information on specific data.

    NOTE:   the original script was modified by others after its initial creation; thus, the original author cannot 
            guarantee data consistency and accuracy, and certain supplementary methods/code may lack proper annotations,
            comments, documentation, or formatting.

    UPDATE: The script has been modified to act as a modular "local database" intended to populate species data directly
            from the remote database.
            This should be done by re-implementing the PRIVATE CONSTANTS (enums and Dictionary objects) to shift from
            the initial hard-coded values to comparable values dynamically parsed from the remote database.
            Changes to these constants will populate the remainder of the script with the correct data values, so any
            existing code that invokes SpeciesConstants methods or accesses auxillary data members will remain valid.
            The PRIVATE CONSTANTS segment is labeled accordingly with start and end tags, starting from the beginning of
            the script definition.
*/
public class SpeciesConstants
{
/*  PRIVATE CONSTANTS : 
    Changes to these will populate the necessary changes in all public constants and methods
*/
    /**
         Static list of plant names.
         This should only be used for initial population of data; the dynamic readonly PLANT_NAMES array should be
         used for coding purposes external to this script.
    */
    private static string[] STATIC_PLANT_NAMES = new string[6]
    {
        "Acacia",
        "Baobab",
        "Big Tree",
        "Fruits And Nectar",
        "Grass And Herbs",
        "Trees And Shrubs"
    };

    /**
         Static list of prey names.
         This should only be used for initial population of data; the dynamic readonly PREY_NAMES array should be
         used for coding purposes external to this script.
    */
    private static string[] STATIC_PREY_NAMES = new string[6]
    {
        "Buffalo",
        "Bush Hyrax",
        "Crested Porcupine",
        "Kori Bustard",
        "Oribi",
        "Tree Mouse"
    };

    /**
         Static list of predator names.
         This should only be used for initial population of data; the dynamic readonly PREDATOR_NAMES array should be
         used for coding purposes external to this script.
    */
    private static string[] STATIC_PREDATOR_NAMES = new string[6]
    {
        "African Wild Dog",
        "Bat-Eared Fox",
        "Black Mamba",
        "Leopard",
        "Lion",
        "Serval Cat"
    };

    /** Reference for all species ID values (consistent with database) */
    private enum SPECIES_ID
    {
        // Plants
        Acacia              = 1007,
        Baobab              = 1009,
        BigTree             = 1008,
        FruitsAndNectar     = 1003,
        GrassAndHerbs       = 1005,
        TreesAndShrubs      = 1001,
        // Prey
        Buffalo             = 7,
        BushHyrax           = 48,
        CrestedPorcupine    = 72,
        KoriBustard         = 65,
        Oribi               = 73,
        TreeMouse           = 31,
        // Predators        
        AfricanWildDog      = 74,
        BatEaredFox         = 4,
        BlackMamba          = 6,
        Leopard             = 80,
        Lion                = 86,
        ServalCat           = 69
    };

    /** Reference for all species biomass levels (consistent with database) */
    private enum SPECIES_BIOMASS
    {
        // Plants
        Acacia              = 2400,
        Baobab              = 4400,
        BigTree             = 3200,
        FruitsAndNectar     = 20,
        GrassAndHerbs       = 40,
        TreesAndShrubs      = 40,
        // Prey
        Buffalo             = 50000,
        BushHyrax           = 2000,
        CrestedPorcupine    = 15000,
        KoriBustard         = 2000,
        Oribi               = 25000,
        TreeMouse           = 800,
        // Predators        
        AfricanWildDog      = 25000,
        BatEaredFox         = 10000,
        BlackMamba          = 8000,
        Leopard             = 30000,
        Lion                = 2500,
        ServalCat           = 15000
    };

    /** Reference for all species prices (consistent with database) */
    private enum SPECIES_PRICE
    {
        // Plants
        Acacia              = 30,
        Baobab              = 75,
        BigTree             = 50,
        FruitsAndNectar     = 5,
        GrassAndHerbs       = 9,
        TreesAndShrubs      = 5,
        // Prey
        Buffalo             = 50,
        BushHyrax           = 25,
        CrestedPorcupine    = 50,
        KoriBustard         = 60,
        Oribi               = 50,
        TreeMouse           = 50,
        // Predators        
        AfricanWildDog      = 50,
        BatEaredFox         = 50,
        BlackMamba          = 50,
        Leopard             = 50,
        Lion                = 50,
        ServalCat           = 50
    };

    /** Reference for all species trophic levels as <SPECIES_ID, TROPHIC_LEVEL> KeyValue pairs */
    private static Dictionary<int, float> TROPHIC_LEVEL = new Dictionary<int, float>()
    {
        // Plants
        { (int)SPECIES_ID.Acacia,            1f },
        { (int)SPECIES_ID.Baobab,            1f }, 
        { (int)SPECIES_ID.BigTree,           1f }, 
        { (int)SPECIES_ID.FruitsAndNectar,   1f },
        { (int)SPECIES_ID.GrassAndHerbs,     1f },
        { (int)SPECIES_ID.TreesAndShrubs,    1f },
        // Prey
        { (int)SPECIES_ID.Buffalo,           2f },
        { (int)SPECIES_ID.BushHyrax,         2f },
        { (int)SPECIES_ID.CrestedPorcupine,  2f },
        { (int)SPECIES_ID.KoriBustard,       2.61792f },
        { (int)SPECIES_ID.Oribi,             2f },
        { (int)SPECIES_ID.TreeMouse,         2.38095f },
        // Predators
        { (int)SPECIES_ID.AfricanWildDog,    3.30203f },
        { (int)SPECIES_ID.BatEaredFox,       3.55142f },
        { (int)SPECIES_ID.BlackMamba,        3.57098f },
        { (int)SPECIES_ID.Leopard,           3.42426f },
        { (int)SPECIES_ID.Lion,              3.22409f },
        { (int)SPECIES_ID.ServalCat,         3.43754f }
    };

    /** Reference for prey / predator metabolism levels as <SPECIES_ID, METABOLISM> KeyValue pairs */
    private static Dictionary<int, float> METABOLISM = new Dictionary<int, float>()
    {
        // Prey
        { (int)SPECIES_ID.Buffalo,           0.26f },
        { (int)SPECIES_ID.BushHyrax,         0.68f },
        { (int)SPECIES_ID.CrestedPorcupine,  0.58f },
        { (int)SPECIES_ID.KoriBustard,       0.66f },
        { (int)SPECIES_ID.Oribi,             0.56f },
        { (int)SPECIES_ID.TreeMouse,         0.79f },
        // Predators
        { (int)SPECIES_ID.AfricanWildDog,    0.5f },
        { (int)SPECIES_ID.BatEaredFox,       0.69f },
        { (int)SPECIES_ID.BlackMamba,        0.48f },
        { (int)SPECIES_ID.Leopard,           0.44f },
        { (int)SPECIES_ID.Lion,              0.35f },
        { (int)SPECIES_ID.ServalCat,         0.61f }
    };

    /** Reference for all species trophic levels as <SPECIES_ID, CLASS> KeyValue pairs */
    private static Dictionary<int, string> SPECIES_CLASS = new Dictionary<int, string>()
    {
        // Plants
        { (int)SPECIES_ID.Acacia,           "Producer" },
        { (int)SPECIES_ID.Baobab,           "Producer" }, 
        { (int)SPECIES_ID.BigTree,          "Producer" }, 
        { (int)SPECIES_ID.FruitsAndNectar,  "Producer" },
        { (int)SPECIES_ID.GrassAndHerbs,    "Producer" },
        { (int)SPECIES_ID.TreesAndShrubs,   "Producer" },
        // Prey
        { (int)SPECIES_ID.Buffalo,          "Herbivore" },
        { (int)SPECIES_ID.BushHyrax,        "Herbivore" },
        { (int)SPECIES_ID.CrestedPorcupine, "Herbivore" },
        { (int)SPECIES_ID.KoriBustard,      "Omnivore" },
        { (int)SPECIES_ID.Oribi,            "Herbivore" },
        { (int)SPECIES_ID.TreeMouse,        "Omnivore" },
        // Predators
        { (int)SPECIES_ID.AfricanWildDog,   "Carnivore" },
        { (int)SPECIES_ID.BatEaredFox,      "Carnivore" },
        { (int)SPECIES_ID.BlackMamba,       "Carnivore" },
        { (int)SPECIES_ID.Leopard,          "Carnivore" },
        { (int)SPECIES_ID.Lion,             "Carnivore" },
        { (int)SPECIES_ID.ServalCat,        "Carnivore" }
    };

    /** Reference for all species trophic levels as <SPECIES_ID, CATEGORY> KeyValue pairs */
    private static Dictionary<int, string> SPECIES_CATEGORY = new Dictionary<int, string>()
    {
        // Plants
        { (int)SPECIES_ID.Acacia,           "Plant" },
        { (int)SPECIES_ID.Baobab,           "Plant" }, 
        { (int)SPECIES_ID.BigTree,          "Plant" }, 
        { (int)SPECIES_ID.FruitsAndNectar,  "Plant" },
        { (int)SPECIES_ID.GrassAndHerbs,    "Plant" },
        { (int)SPECIES_ID.TreesAndShrubs,   "Plant" },
        // Prey
        { (int)SPECIES_ID.Buffalo,          "Large Animal" },
        { (int)SPECIES_ID.BushHyrax,        "Small Animal" },
        { (int)SPECIES_ID.CrestedPorcupine, "Large Animal" },
        { (int)SPECIES_ID.KoriBustard,      "Bird" },
        { (int)SPECIES_ID.Oribi,            "Large Animal" },
        { (int)SPECIES_ID.TreeMouse,        "Small Animal" },
        // Predators
        { (int)SPECIES_ID.AfricanWildDog,   "Large Animal" },
        { (int)SPECIES_ID.BatEaredFox,      "Large Animal" },
        { (int)SPECIES_ID.BlackMamba,       "Small Animal" },
        { (int)SPECIES_ID.Leopard,          "Large Animal" },
        { (int)SPECIES_ID.Lion,             "Large Animal" },
        { (int)SPECIES_ID.ServalCat,        "Large Animal" }
    };

    /** Reference for all species trophic levels as <SPECIES_ID, LORE> KeyValue pairs */
    private static Dictionary<int, string> SPECIES_LORE = new Dictionary<int, string>()
    {
        // Plants
        { (int)SPECIES_ID.Acacia,           "Acacia is a genus of shrubs and trees belonging to the subfamily Mimosoideae of the family Fabaceae, " +
                                            "first described in Africa by the Swedish botanist Carl Linnaeus in 1773." },
        { (int)SPECIES_ID.Baobab,           "Baobab is a genus of eight species of tree, six native to Madagascar, one native to mainland Africa " +
                                            "and the Arabian Peninsula and one to Australia. The mainland African species also occurs on Madagascar, " +
                                            "but it is not a native of that island." }, 
        { (int)SPECIES_ID.BigTree,          "Trees are an important component of the natural landscape because of their prevention of erosion " +
                                            "and the provision of a weather-sheltered ecosystem in and under their foliage. " +
                                            "They also play an important role in producing oxygen and reducing carbon dioxide in the atmosphere, " +
                                            "as well as moderating ground temperatures." }, 
        { (int)SPECIES_ID.FruitsAndNectar,  "Special" },
        { (int)SPECIES_ID.GrassAndHerbs,    "Grasses are among the most versatile life forms. They became widespread toward the end of the " +
                                            "Cretaceous period, and fossilized dinosaur dung have been found containing phytoliths of a variety of " +
                                            "grasses that include grasses that are related to modern rice and bamboo." },
        { (int)SPECIES_ID.TreesAndShrubs,   "Special" },
        // Prey
        { (int)SPECIES_ID.Buffalo,          "The African buffalo is a large African bovine. " +
                                            "It is not closely related to the slightly larger wild Asian water buffalo, but its ancestry remains unclear. " +
                                            "Owing to its unpredictable nature which makes it highly dangerous to humans, it has not been domesticated " +
                                            "unlike its Asian counterpart the domestic Asian water buffalo." },
        { (int)SPECIES_ID.BushHyrax,        "The yellow-spotted rock hyrax or bush hyrax is a species of mammal in the family Procaviidae." },
        { (int)SPECIES_ID.CrestedPorcupine, "The crested porcupine is a species of rodent in the Hystricidae family." },
        { (int)SPECIES_ID.KoriBustard,      "The Kori Bustard is a large bird native to Africa. It is a member of the bustard family. " +
                                            "It may be the heaviest bird capable of flight." },
        { (int)SPECIES_ID.Oribi,            "Oribi are graceful slender-legged, long-necked small antelope found in grassland almost throughout Sub-Saharan Africa." },
        { (int)SPECIES_ID.TreeMouse,        "Tree Mouse, Prionomys batesi, is a poorly understood climbing mouse from Central Africa. " +
                                            "It is unique enough that it has been placed in a genus of its own, Prionomys, since its discovery in 1910." },
        // Predators
        { (int)SPECIES_ID.AfricanWildDog,   "African Wild Dog is a canid found only in Africa, especially in savannas and lightly wooded areas." },
        { (int)SPECIES_ID.BatEaredFox,      "The bat-eared fox is a canid of the African savanna, named for its large ears. " +
                                            "Fossil records show this canid to first appear during the middle Pleistocene, about 800,000 years ago." },
        { (int)SPECIES_ID.BlackMamba,       "The black mamba, also called the common black mamba or black-mouthed mamba, is the longest venomous snake in Africa, " +
                                            "averaging around 2.5 to 3.2 m in length, and sometimes growing to lengths of 4.45 m." },
        { (int)SPECIES_ID.Leopard,          "The leopard, Panthera pardus, is a member of the Felidae family and the smallest of the four \"big cats\" " +
                                            "in the genus Panthera, the other three being the tiger, lion, and jaguar." },
        { (int)SPECIES_ID.Lion,             "The lion is one of the four big cats in the genus Panthera, and a member of the family Felidae. " +
                                            "With some males exceeding 250 kg in weight, it is the second-largest living cat after the tiger." },
        { (int)SPECIES_ID.ServalCat,        "The serval, Leptailurus serval or Caracal serval, known in Afrikaans as Tierboskat, \"tiger-forest-cat\", " +
                                            "is a medium-sized African wild cat. DNA studies have shown that the serval is closely related to the " +
                                            "African golden cat and the caracal." }
    };

    /** Reference plant effect ranges */
    private static Dictionary<string, int[][]> PLANT_RANGES = new Dictionary<string, int[][]>()
    {
        {
            "Acacia",
            new int[4][]
            {
                // . * .
                // * O *
                // . * .
                new int[2]{0, 1},
                new int[2]{1, 0},
                new int[2]{0, -1},
                new int[2]{-1, 0}
            }
        },
        {
            "Baobab",
            new int[8][]
            {
                // * * *
                // * O *
                // * * *
                new int[2]{0, 1},
                new int[2]{1, 0},
                new int[2]{0, -1},
                new int[2]{-1, 0},
                new int[2]{-1, -1},
                new int[2]{1, 1},
                new int[2]{1, -1},
                new int[2]{-1, 1}
            }
        }, 
        {
            "Big Tree",
            new int[4][]
            {
                // * . *
                // . O .
                // * . *
                new int[2]{-1, -1},
                new int[2]{1, 1},
                new int[2]{1, -1},
                new int[2]{-1, 1}
            }
        }, 
        {
            "Fruits And Nectar",
            new int[2][]
            {
              // . . . .
              // . O * * 
              // . . . .
              new int[2]{-1, 0},
              new int[2]{-2, 0},
          }
        },
        {
            "Grass And Herbs",
            new int[2][]
            {
                // . * .
                // . O .
                // . * .
                new int[2]{0, 1},
                new int[2]{0, -1}
            }
        },
        {
            "Trees And Shrubs",
            new int[3][]
            {
                // . . *
                // . O * 
                // . . *
                new int[2]{-1, 1},
                new int[2]{-1, 0},
                new int[2]{-1, -1}
            }
        }
    };
/* END PRIVATE CONSTANTS */

    /**
        Defines pertinent plant data for Plant object creation.
    */
    private struct Plant
    {
        // Minigame-specific variables
        public int[][] range;       // Array of [x,y] offset pairs with [0,0] @ plant origin, denoting relative effect range
        // Database-consistent variables
        public string name;         // Species name
        public string speciesClass; // Species class (e.g. "Producer")
        public string category;     // Species category (e.g. "Plant")
        public float trophicLevel;  // Species trophic level
        public string lore;         // Lore (i.e. description) taken from 'http://smurf.sfsu.edu/~wob/guide/species.php'
        public int speciesID;       // Unique species ID
        public int biomass;         // Biomass level (Plant -> Tier 1)
        public int price;           // Price in credits

        /**
            Constructor.

            NOTE:   all parameters should be consistent with the remote database whenever possible.

            UPDATE: additional data has been added to the SpeciesConstants script which now reflects in the Plant struct;
                    all Plant data is now dynamically parsed from various data structures and requires only the species
                    name to generate all other variables.

            @param  _name       species name (string)
        */
        public Plant (string _name)
        {
            // Set parametized and game-specific values
            name = _name;
            range = PLANT_RANGES[name];
            // Set database-consistent values
            speciesID = (int)Enum.Parse(typeof(SPECIES_ID), new Regex("[ -]").Replace(name, ""));
            speciesClass = SPECIES_CLASS[speciesID];
            category = SPECIES_CATEGORY[speciesID];
            lore = SPECIES_LORE[speciesID];
            trophicLevel = TROPHIC_LEVEL[speciesID];
            biomass = (int)Enum.Parse(typeof(SPECIES_BIOMASS), new Regex("[ -]").Replace(name, ""));
            price = (int)Enum.Parse(typeof(SPECIES_PRICE), new Regex("[ -]").Replace(name, ""));
        }
    };

    /**
        Defines all available prey data in the script.
    */
	private struct Prey
	{
		public string name;         // Species name
        public string speciesClass; // Species class (e.g. "Small Animal")
        public string category;     // Species category (e.g. "Herbivore")
        public float trophicLevel;  // Species trophic level
        public string lore;         // Lore (i.e. description) taken from 'http://smurf.sfsu.edu/~wob/guide/species.php'
        public int speciesID;       // Unique species ID
        public int health;          // Base health
        public int biomass;         // Biomass level (Prey -> Tier 2)
        public int price;           // Price in credits


        // public int[] preyIDLIst; // TODO: create prey list --> includes plants, but might include other prey (TBD)
        public int[] predatorIDList;

        /**
            Constructor.
            NOTE: all parameters should be consistent with the remote database whenever possible.

            @param  _name           species name (string)
            @param  _speciesID      a unique species ID (int)
            @param  _health         a Prey's starting health (int)
            @param  _predatorIDList list of a prey's natural predators defined by speciesID (int[])
            @param  _lore           species lore (i.e. small factoids) (string)
            @param  _biomass        amount of prey (Tier 2) biomass a prey produces (int)
        */
        public Prey (string _name, int _speciesID, int _health, int[] _predatorIDList, string _lore, int _biomass)
        {
            // Set parametized and game-specific values
            name = _name;
            predatorIDList = _predatorIDList;
            // Set database-consistent values
            speciesID = (int)Enum.Parse(typeof(SPECIES_ID), new Regex("[ -]").Replace(name, ""));
            speciesClass = SPECIES_CLASS[speciesID];
            category = SPECIES_CATEGORY[speciesID];
            biomass = (int)Enum.Parse(typeof(SPECIES_BIOMASS), new Regex("[ -]").Replace(name, ""));
            trophicLevel = TROPHIC_LEVEL[speciesID];
            lore = SPECIES_LORE[speciesID];
            price = (int)Enum.Parse(typeof(SPECIES_PRICE), new Regex("[ -]").Replace(name, ""));
            // Calculate health
            health = (int)Math.Round((biomass/trophicLevel) * (1 - METABOLISM[speciesID]) * 0.1);
        }
	};

    /**
        Defines pertinent predator data for Predator object creation.
    */
	private struct Predator
	{
        public string name;
        public string lore; // Lore (i.e. description) taken from 'http://smurf.sfsu.edu/~wob/guide/species.php'
        public int speciesID;
        public int hunger;
        public int voracity;
        public int biomass;

        public int[] preyIDList;
        // public int[] predatorIDLIst; // TODO: create predator list --> includes larger predators (TBD)

        /**
            Constructor.
            NOTE: all parameters should be consistent with the remote database whenever possible.

            @param  _name           species name (string)
            @param  _speciesID      a unique species ID (int)
            @param  _hunger         a Predator's starting hunger required to satisfy (int)
            @param  _voracity       a Predator's starting voracity, or max nutrition comsumed per turn (int)
            @param  _preyIDList     list of a predator's natural prey defined by speciesID (int[])
            @param  _lore           species lore (i.e. small factoids) (string)
            @param  _biomass        amount of predator (Tier 3) biomass a predator produces (int)
        */
        public Predator (string _name, int _speciesID, int _hunger, int _voracity, int[] _preyIDList, string _lore, int _biomass)
        {
            name = _name;
            speciesID = _speciesID;
            hunger = _hunger;
            voracity = _voracity;
            preyIDList = _preyIDList;
            lore = _lore;
            biomass = _biomass;
        }
	};

    /**
        Dynamically spawns all plant objects found in the script.
        Data is parsed from the species names found in SpeciesConstants.STATIC_PLANT_NAMES.

        @return a Plant[] object
    */
    private static Plant[] CreatePlants ()
    {
        // Create temporary Plant array
        Plant[] _plants = new Plant[SpeciesConstants.STATIC_PLANT_NAMES.Length];
        // Iterate species names, create and add plants
        for (int i = 0; i < SpeciesConstants.STATIC_PLANT_NAMES.Length; i++)
            _plants[i] = new Plant(SpeciesConstants.STATIC_PLANT_NAMES[i]);
        // Return populated array
        return _plants;
    }

    /** Define the PLANTS array */
    private static Plant[] PLANTS = SpeciesConstants.CreatePlants();


    /**
        Defines all available prey found in the script.

        NOTE: as with the Plant array above, subsequent changes not made by the original author may exist. Data consistency
        cannot not be guaranteed.
    */
	private static Prey[] PREY = 
    {
		new Prey
        (
            "Buffalo",                          // Name
            (int)SPECIES_ID.Buffalo,            // Species ID
            500,                                // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.Lion
            },
            // Lore
            "The African buffalo is a large African bovine. " +
            "It is not closely related to the slightly larger wild Asian water buffalo, but its ancestry remains unclear. " +
            "Owing to its unpredictable nature which makes it highly dangerous to humans, it has not been domesticated " +
            "unlike its Asian counterpart the domestic Asian water buffalo.",
            50000
        ),
		new Prey
        (
            "Tree Mouse",                       // Name
            (int)SPECIES_ID.TreeMouse,          // Species ID
            8,                                  // Health
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
            "It is unique enough that it has been placed in a genus of its own, Prionomys, since its discovery in 1910.",
            800
        ),
		new Prey
        (
            "Bush Hyrax",                       // Name
            (int)SPECIES_ID.BushHyrax,          // Species ID
            20,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.ServalCat,
                (int)SPECIES_ID.Leopard
            },
            // Lore
            "The yellow-spotted rock hyrax or bush hyrax is a species of mammal in the family Procaviidae.",
            2000
        ),
        new Prey
        (
            "Kori Bustard",                     // Name
            (int)SPECIES_ID.KoriBustard,        // Species ID
            20,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.Leopard,
                (int)SPECIES_ID.Lion
            },
            // Lore
            "The Kori Bustard is a large bird native to Africa. It is a member of the bustard family. " +
            "It may be the heaviest bird capable of flight.",
            2000
        ),
        new Prey
        (
            "Crested Porcupine",                // Name
            (int)SPECIES_ID.CrestedPorcupine,   // Species ID
            15,                                 // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.AfricanWildDog,
                (int)SPECIES_ID.BlackMamba,
                (int)SPECIES_ID.Leopard
            },
            // Lore
            "The crested porcupine is a species of rodent in the Hystricidae family.",
            1500
        ),
        new Prey
        (
            "Oribi",                            // Name
            (int)SPECIES_ID.Oribi,              // Species ID
            250,                                // Health
            new int[]                           // Predators
            {
                (int)SPECIES_ID.AfricanWildDog,
                (int)SPECIES_ID.Leopard,
                (int)SPECIES_ID.Lion
            },
            // Lore
            "Oribi are graceful slender-legged, long-necked small antelope found in grassland almost throughout Sub-Saharan Africa.",
            25000
        )
	}; 

    /**
        Defines all available predators found in the script.

        NOTE: as with the Plant and Prey arrays above, subsequent changes not made by the original author may exist.
        Data consistency cannot not be guaranteed.
    */
	private static Predator[] PREDATORS = 
    {
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
            "Fossil records show this canid to first appear during the middle Pleistocene, about 800,000 years ago.",
            10000
        ),
        new Predator
        (
            "Black Mamba",                      // Name
            (int)SPECIES_ID.BlackMamba,         // Species ID
            80,                                 // Hunger
            5,                                  // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax,
                (int)SPECIES_ID.CrestedPorcupine
            },
            // Lore
            "The black mamba, also called the common black mamba or black-mouthed mamba, is the longest venomous snake in Africa, " +
            "averaging around 2.5 to 3.2 m in length, and sometimes growing to lengths of 4.45 m.",
            8000
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
            "African golden cat and the caracal.",
            1500
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
            "African Wild Dog is a canid found only in Africa, especially in savannas and lightly wooded areas.",
            25000
        ),
        new Predator
        (
            "Leopard",                          // Name
            (int)SPECIES_ID.Leopard,            // Species ID
            30,                                 // Hunger
            15,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.TreeMouse,
                (int)SPECIES_ID.BushHyrax
            },
            // Lore
            "The leopard, Panthera pardus, is a member of the Felidae family and the smallest of the four \"big cats\" " +
            "in the genus Panthera, the other three being the tiger, lion, and jaguar.",
            30000
        ),
        new Predator
        (
            "Lion",                             // Name
            (int)SPECIES_ID.Lion,               // Species ID
            100,                                // Hunger
            50,                                 // Voracity
            new int[]                           // Prey
            {
                (int)SPECIES_ID.Oribi,
                (int)SPECIES_ID.KoriBustard,
                (int)SPECIES_ID.Buffalo
            },
            // Lore
            "The lion is one of the four big cats in the genus Panthera, and a member of the family Felidae. " +
            "With some males exceeding 250 kg in weight, it is the second-largest living cat after the tiger.",
            2500
        )
	};

	// Type constants (not consistent with database!)
    private static short PLANT_TYPE = 0;
	private static short PREY_TYPE = 1;
	private static short PREDATOR_TYPE = 2;

	// Default values (self-descriptive)
	private static short DEFAULT_TYPE = -1;
    private static int DEFAULT_SPECIES_ID = 0;
    private static string DEFAULT_SPECIES_NAME = "[ No name ]";
    private static int[][] DEFAULT_RANGE = new int[1][]{ new int[2]{-1, 0} };
	private static int DEFAULT_HEALTH = 10;
	private static int DEFAULT_HUNGER = 25;
	private static int DEFAULT_VORACITY = 25;
    private static float DEFAULT_METABOLISM = 0.5f;
    private static string DEFAULT_LORE = "[ No description available ]";

    /**
        Contains all <SPECIES_NAME, SPECIES_ID> pairs
    */
    private static Dictionary<string, int> NAME_TO_ID =
        (from plant in PLANTS select new KeyValuePair<string, int>(plant.name, plant.speciesID))
        .Concat
        (from prey in PREY select new KeyValuePair<string, int>(prey.name, prey.speciesID))
        .Concat
        (from predator in PREDATORS select new KeyValuePair<string, int>(predator.name, predator.speciesID))
        .ToDictionary(pair => pair.Key, pair => pair.Value);

    /* PRIVATE METHODS */
    /**
        Internal method for removing whitespace and hyphen chars from a string.

        @param  s   a string to modify
        @return a new string object
    */
    private static string Compact (string s)
    {
        return s.Replace('\x20', '\x01').Replace('\x2d', '\x01');
    }

	/* PUBLIC CONSTANTS */

    // Number of available plants, prey, predators, and total species, respectively
    /** Number of species in the PLANTS array */
    public static readonly int NUM_PLANTS = PLANTS.Length;
    /** Number of species in the PREY array */
	public static readonly int NUM_PREY = PREY.Length;
    /** Number of species in the PREDATORS array */
	public static readonly int NUM_PREDATORS = PREDATORS.Length;
    /** Total number of species in the PLANTS, PREY, and PREDATORS arrays */
	public static readonly int NUM_SPECIES = NUM_PLANTS + NUM_PREY + NUM_PREDATORS;

    // Names of all available plants, prey, predators, and total species, respectively
    /** Array containing all Plant names in the PLANTS array */
    public static readonly string[] PLANT_NAMES = (from plant in PLANTS select plant.name).ToArray();
    /** Array containing all Prey names in the PREY array */
    public static readonly string[] PREY_NAMES = (from prey in PREY select prey.name).ToArray();
    /** Array containing all Predator names in the PREDATORS array */
    public static readonly string[] PREDATOR_NAMES = (from predator in PREDATORS select predator.name).ToArray();
    /** Array containing all species names in the PLANTS, PREY, and PREDATORS arrays */
    public static readonly string[] SPECIES_NAMES = PLANT_NAMES.Concat(PREY_NAMES).Concat(PREDATOR_NAMES).ToArray();

	/* PUBLIC METHODS */
	/**
		Returns a species ID associated with a species name.

        @param  name    a species name (string)
        @return a unique species ID (int)
	*/
	public static int SpeciesID (string name)
	{
        return NAME_TO_ID.ContainsKey(name) ? NAME_TO_ID[name] : DEFAULT_SPECIES_ID;
	}

	/**
		Returns a name associated with a species ID.

        @param  id  a unique species ID (int)
        @return a species name (string)
	*/
	public static string SpeciesName (int id)
	{
        return NAME_TO_ID.ContainsValue(id) ? NAME_TO_ID.First(pair => pair.Value == id).Key : DEFAULT_SPECIES_NAME;
	}

    /**
        Returns a plant's effect range.
        Search by species name.

        @param  name    a species name (string)
        @return a plant's effect range as a 2D array (int[][])
    */
    public static int[][] Range (string name)
    {
        foreach (Plant plant in PLANTS)
        {
            if (plant.name == name)
                return plant.range;
        }
        return DEFAULT_RANGE;
    }

    /**
        Returns a plant's effect range.
        Search by species ID.

        @param  id  a unique species id (int)
        @return a plant's effect range as a 2D array (int[][])
    */
    public static int[][] Range (int id)
    {
        foreach (Plant plant in PLANTS)
        {
            if (plant.speciesID == id)
                return plant.range;
        }
        return DEFAULT_RANGE;
    }

	/**
		Returns a prey's starting health.
		Search by species name.

        @param  name    a species name (string)
        @return a prey's starting health (int)
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

        @param  id  a unique species id (int)
        @return a prey's starting health (int)
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

        @param  name    a species name (string)
        @return a predator's starting hunger (int)
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

        @param  id  a unique species id (int)
        @return a predator's starting hunger (int)
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

        @param  name    a species name (string)
        @return a predator's starting voracity (int)
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

        @param  id  a unique species id (int)
        @return a predator's starting voracity (int)
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

        NOTE: this is NOT currently consistent with the database.

        @param  name    a species name (string)
        @return a species' type: 0 => plant, 1 => prey, 2 => predator (int)
	*/
	public static short SpeciesType (string name)
	{
        // Search plants
        foreach (Plant plant in PLANTS) if (plant.name == name) return PLANT_TYPE;
        // Search prey
        foreach (Prey prey in PREY) if (prey.name == name) return PREY_TYPE;
        // Search predators
        foreach (Predator predator in PREDATORS) if (predator.name == name) return PREDATOR_TYPE;
        // Otherwise return default
        return DEFAULT_TYPE;
	}

	/**
		Returns a species type.
		Search by species ID.

        NOTE: this is NOT currently consistent with the database.

        @param  id  a unique species id (int)
        @return a species' type: 0 => plant, 1 => prey, 2 => predator (int)
	*/
	public static short SpeciesType (int id)
	{
        // Search plants
        foreach (Plant plant in PLANTS) if (plant.speciesID == id) return PLANT_TYPE;
        // Search prey
        foreach (Prey prey in PREY) if (prey.speciesID == id) return PREY_TYPE;
        // Search predators
        foreach (Predator predator in PREDATORS) if (predator.speciesID == id) return PREDATOR_TYPE;
        // Otherwise return default
        return DEFAULT_TYPE;
	}

    /**
        Returns a predator list for a species as species ID values.
        Search by species name.

        @param  name    a species name (string)
        @return a species ID list (int[]{})
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

        @param  id  a unique species id (int)
        @return a species ID list (int[]{})
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

        @param  name    a species name (string)
        @return a species ID list (int[]{})
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

        @param  id  a unique species id (int)
        @return a species ID list (int[]{})
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

        @param  name    a species name (string)
        @return a species' lore (string)
    */
    public static string SpeciesLore (string name)
    {
        // Search plants
        foreach (Plant plant in PLANTS)
            if (plant.name == name) return plant.lore;
        // Search prey
        foreach (Prey prey in PREY)
            if (prey.name == name) return prey.lore;
        // Search predators
        foreach (Predator predator in PREDATORS)
            if (predator.name == name) return predator.lore;
        // Otherwise return default
        return DEFAULT_LORE;
    }

    /**
        Returns the lore for a species.
        Search by species ID.

        @param  id  a unique species id (int)
        @return a species' lore (string)
    */
    public static string SpeciesLore (int id)
    {
        // Search plants
        foreach (Plant plant in PLANTS)
            if (plant.speciesID == id) return plant.lore;
        // Search prey
        foreach (Prey prey in PREY)
            if (prey.speciesID == id) return prey.lore;
        // Search predators
        foreach (Predator predator in PREDATORS)
            if (predator.speciesID == id) return predator.lore;
        // Otherwise return default
        return DEFAULT_LORE;
    }

    /**
        Returns the metabolism for a prey or predator species.
        Search by species name.

        @param  name    a species name (string)
        @return a species' metabolism (float)
    */
    public static float Metabolism (string name)
    {
        if (METABOLISM.ContainsKey(SpeciesID(name)))
            return METABOLISM[SpeciesID(name)];
        return DEFAULT_METABOLISM;
    }

    /**
        Returns the metabolism for a prey or predator species.
        Search by species ID.

        @param  id  a unique species id (int)
        @return a species' metabolism (float)
    */
    public static float Metabolism (int id)
    {
        if (METABOLISM.ContainsKey(id))
            return METABOLISM[id];
        return DEFAULT_METABOLISM;
    }

/*
    SUPPLEMENTARY METHODS (unknown author/s)
*/

  public static int Biomass(string name){

    foreach (Plant plant in PLANTS)
    {
      if (plant.name == name)
        return plant.biomass;
    }

    foreach (Prey prey in PREY)
    {
      if (prey.name == name)
        return prey.biomass;
    }
    foreach (Predator predator in PREDATORS)
    {
      if (predator.name == name)
        return predator.biomass;
    }
    return 0;
  }

  public bool EatenBy(string _prey, string _predator)
  {
    int predatorID = SpeciesID ( _predator);
    foreach (Prey prey in PREY)
    {
      
      if (prey.name == _prey) {

        for(int i = 0; i< prey.predatorIDList.Count(); i++){
          if (prey.predatorIDList [i] == predatorID)
            return true;
        }
      
      }
        
        
    }

    return false;
  }

  public bool Eats(string _predator, string _prey){
    
    int predatorID = SpeciesID ( _predator);
    foreach (Prey prey in PREY)
    {

      if (prey.name == _prey) {

        for(int i = 0; i< prey.predatorIDList.Count(); i++){
          if (prey.predatorIDList [i] == predatorID)
            return true;
        }

      }


    }

    return false;
  }

}
using UnityEngine;

using System.Collections.Generic;

/**
	Constants defining the scaling factor for MoveObjects within Don't Eat Me
	NOTE:	This may be later modified once data may be parsed from the database via the server.
			For now, this serves as a temporary solution.

	@author: Jeremy Erickson
*/
public class SpeciesConstants {
	// TEMPORARY ARRAY FOR PARSING SCALE FACTOR BY SPECIES ID
	// NOTE: species ID's are consistent with those found in the database @ smurf.sfsu.edu :: deBuggerWeb
	public static float ScaleBySpeciesId (int speciesId) {
		switch (speciesId) {
			/* SMALL ANIMALS */
			// TreeMouse = 31
			case (int)SpeciesID.TreeMouse: return 0.3f;
			// SmithsRedRockHare = 52
			case (int)SpeciesID.SmithsRedRockHare: return 0.35f;;
			// GreaterBushbaby = 55
			case (int)SpeciesID.GreaterBushbaby: return 0.4f;

			/* MEDIUM ANIMALS */
			// BushHyrax = 48
			case (int)SpeciesID.BushHyrax: return 0.45f;

			/* LARGE ANIMALS */
			// BohorReedback = 82
			case (int)SpeciesID.BohorReedback: return 0.5f;

			/* DEFAULT */
			default: return 1.0f;
		}
	}

	/**
		Counterpart to ScaleBySpeciesId: accepts species name as parameter
	*/
	public static float ScaleBySpeciesName (string name) {
		return ScaleBySpeciesId(SpeciesIdByName(name));
	}

	/**
		Returns a species ID for a given species name, or -1 if not found
	*/
	public static int SpeciesIdByName (string name) {
		switch (name) {
			case "TreeMouse": 			return (int) SpeciesID.TreeMouse;
			case "BushHyrax":			return (int) SpeciesID.BushHyrax;
			case "SmithsRedRockHare":	return (int) SpeciesID.SmithsRedRockHare;
			case "GreaterBushbaby":		return (int) SpeciesID.GreaterBushbaby;
			case "BohorReedback":		return (int) SpeciesID.BohorReedback;
			default: 					return (int) SpeciesID.NONE;
		}
	}

	// SpeciesID enum
	public enum SpeciesID {
		TreeMouse			= 	31,
		BushHyrax			= 	48,
		SmithsRedRockHare 	=	52,
		GreaterBushbaby		=	55,
		BohorReedback 		=	82,
		NONE				=	-1
	};
}
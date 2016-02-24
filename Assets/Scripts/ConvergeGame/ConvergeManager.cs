using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ConvergeManager holds shared data on the elements being presented by the
//Converge Game
public class ConvergeManager
	
{
	public string selected { get; set; }
	public Vector3 lastMousePosition  { get; set; }
	public List<string> mouseOverLabels  { get; set; }
	public string lastSeriesToDraw  { get; set; }
	public List<string> seriesLabels  { get; set; }
	public Dictionary<string, Color> seriesColors { get; set; }
	public Dictionary<string, string> seriesColorsHex  { get; set; }
	public List<string> excludeList { get; set; }
	public List<int> seriesNodes  { get; set; }

	public ConvergeManager ()
	{
		lastMousePosition = Vector3.zero;
		mouseOverLabels = new List<string>();
		seriesLabels = new List<string> ();
		seriesNodes = new List<int> ();
		seriesColors = new Dictionary<string, Color> ();
		seriesColorsHex = new Dictionary<string, string> ();
		excludeList = new List<string>();
	}

	//populate new list of labels with information sorted by node Id
	public void SortLabelsAndNodes ()
	{
		seriesLabels.Sort (SortByTrophicLevels); 

		//add nodes in sorted order
		seriesNodes = new List<int> ();
		foreach (string label in seriesLabels) {
			int nodeStart = label.IndexOf ("[");
			int nodeEnd = label.IndexOf ("]");
			string nodeStr = label.Substring (nodeStart + 1, nodeEnd - nodeStart - 1);
			seriesNodes.Add (int.Parse (nodeStr));
		}

	}

	private int SortByTrophicLevels (string x, string y)
	{
		SpeciesData spX = SpeciesTable.GetSpecies (x);
		SpeciesData spY = SpeciesTable.GetSpecies (y);
		return spY.trophic_level.CompareTo (spX.trophic_level);
	}

	//call SetColors AFTER sorting
	public void SetColors ()
	{
		//count number of animals (sorted such that animal nodes come first)
		int seriesCount = seriesNodes.Count;
		int plantMax = 7;
		int animalCount = 0;
		while (animalCount < seriesCount && seriesNodes [animalCount] > plantMax) {
			animalCount ++;
		}
		int plantCount = seriesCount - animalCount;

		//get color range from LABColor for best spectrum (converts RGB to linear)
		//vary color from red for animals and from green for plants
		LABColor firstColor = 		new LABColor (new Color (1.00f, 0.00f, 0.00f));
		LABColor lastAnimalColor =	new LABColor (new Color (0.50f, 0.50f, 1.00f)); //incorporate blue
		LABColor firstPlantColor = 	new LABColor (new Color (0.50f, 0.50f, 0.00f));
		LABColor lastColor = 		new LABColor (new Color (0.00f, 1.00f, 0.00f));
		float[] deltaAnimalColor = LABColor.Diff (lastAnimalColor, firstColor);
		for (int j=0; j < deltaAnimalColor.Length; j++) {
			deltaAnimalColor[j] = deltaAnimalColor[j] / (Mathf.Max(1.0f, (float)animalCount - 1.0f));
		}
		if (plantCount == 1) {
			firstPlantColor = lastColor;
		}
		float[] deltaPlantColor = LABColor.Diff (lastColor, firstPlantColor);
		for (int j=0; j < deltaPlantColor.Length; j++) {
			deltaPlantColor[j] = deltaPlantColor[j] / (Mathf.Max(1.0f, (float)plantCount - 1.0f));
		}
		LABColor currColor = firstColor;

		//loop through series to set series' colors
		for (int i = 0; i < seriesCount; i ++) {
			if (i == animalCount ) {  //first plant
				currColor = firstPlantColor;
			} 
			Color color = LABColor.ToColor (currColor);

			string name = seriesLabels [i];
			seriesColors [name] = color;
			seriesColorsHex [name] = Functions.ColorToHex (color);

			currColor.Increment(i < animalCount ? deltaAnimalColor : deltaPlantColor);
		}
	}

	public string MatchSeriesLabel (string labelIn)
	{
		//need to add opening bracket to prevent matches of "Leopard" to "Leopard Tortoise"
		labelIn = labelIn + " ["; 
		return seriesLabels.Find (rec => rec.StartsWith (labelIn));
	}

}


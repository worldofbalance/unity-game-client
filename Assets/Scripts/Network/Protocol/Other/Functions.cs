using UnityEngine;

using System;
using System.Collections.Generic;

public class Functions
{

	public static string ColorToHex (Color32 color)
	{
		return color.r.ToString ("X2") + color.g.ToString ("X2") + color.b.ToString ("X2");
	}
	
	public static Color HexToColor (string hex)
	{
		byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);

		return new Color32 (r, g, b, 255);
	}

	public static Texture2D CreateTexture2D (Color color)
	{
		return Functions.CreateTexture2D (color, 1, 1);
	}

	public static Texture2D CreateTexture2D (Color color, int width, int height)
	{
		Texture2D texture = new Texture2D (width, height);

		for (int x = 0; x < texture.height; x++) {
			for (int y = 0; y < texture.width; y++) {
				texture.SetPixel (x, y, color);
			}
		}

		texture.Apply ();

		return texture;
	}
	
	public static void DrawBackground (Rect position, Texture image, float scale = 0.15f)
	{
		float texWidth = image.width * scale;

		GUI.BeginGroup (position);
		// Top Left Region
		GUI.DrawTextureWithTexCoords (new Rect (0, 0, texWidth, texWidth), image, new Rect (0, 1 - scale, scale, scale));
		// Top Center Region
		GUI.DrawTextureWithTexCoords (new Rect (texWidth, 0, position.width - texWidth * 2, texWidth), image, new Rect (1 - scale, 1 - scale, 0, scale));
		// Top Right Region
		GUI.DrawTextureWithTexCoords (new Rect (position.width - texWidth, 0, texWidth, texWidth), image, new Rect (1 - scale, 1 - scale, scale, scale));
		// Middle Left Region
		GUI.DrawTextureWithTexCoords (new Rect (0, texWidth, texWidth, position.height - texWidth * 2), image, new Rect (0, 1 - scale, scale, 0));
		// Middle Center Region
		GUI.DrawTextureWithTexCoords (new Rect (texWidth, texWidth, position.width - texWidth * 2, position.height - texWidth * 2), image, new Rect (0.5f, 0.5f, 0, 0));
		// Middle Right Region
		GUI.DrawTextureWithTexCoords (new Rect (position.width - texWidth, texWidth, texWidth, position.height - texWidth * 2), image, new Rect (1 - scale, 1 - scale, scale, 0));
		// Bottom Left Region
		GUI.DrawTextureWithTexCoords (new Rect (0, position.height - texWidth, texWidth, texWidth), image, new Rect (0, 0, scale, scale));
		// Bottom Center Region
		GUI.DrawTextureWithTexCoords (new Rect (texWidth, position.height - texWidth, position.width - texWidth * 2, texWidth), image, new Rect (1 - scale, 0, 0, scale));
		// Bottom Right Region
		GUI.DrawTextureWithTexCoords (new Rect (position.width - texWidth, position.height - texWidth, texWidth, texWidth), image, new Rect (1 - scale, 0, scale, scale));
		GUI.EndGroup ();
	}

	public static void DrawBackground (Rect position, Texture image, Color color)
	{
		Color temp = GUI.color;

		GUI.color = color;
		Functions.DrawBackground (position, image);
		GUI.color = temp;
	}

	public static bool CanBeSeen (Camera camera, Vector3 position)
	{
		Vector3 point = camera.WorldToViewportPoint (position);
		return point.x >= 0 && point.x <= 1 && point.y >= 0 && point.y <= 1;
	}
	
	public static CSVObject ParseCSV (string csv)
	{
		if (csv == null) {
			return null;
		}
		CSVObject csvObject = new CSVObject ();
		int cnt = 0;

		List<string> xAxisLabels = new List<string> (); // i.e. Months
		string[] rowList = csv.Split ('\n');

		for (int i = 0; i < rowList.Length; i++) {
			if (rowList [i].Length <= 1) {
				continue;
			}

			string[] elements = rowList [i].Split (',');

			if (i == 0) { // Labels
				cnt = elements.Length;  //set definite count of elements
				for (int j = 1; j < elements.Length; j++) {
					xAxisLabels.Add (elements [j]);
				}

				csvObject.xLabels = xAxisLabels;
			} else {
				string seriesLabel = "Untitled"; // i.e. Species
				List<string> values = new List<string> ();

				for (int j = 0; j < elements.Length; j++) {
					if (j == 0) {
						//seriesLabel = elements[0].Trim();
						seriesLabel = NormalizeSpeciesName (elements[0]);
					} else {
						values.Add (elements [j].Trim ());
					}
				}
				//add zeros if any elements are missing (i.e. species extinct)
				for (int j = elements.Length; j < cnt; j++) {
					values.Add ("0");
				}

				csvObject.csvList [seriesLabel] = values;
			}
		}

		return csvObject;
	}

	//after this process, CSV names and speciestable names will still differ in
	//that CSV names have node info attached.
	public static string NormalizeSpeciesName (string str)
	{
		string normStr = str.Trim ();

		//simulation server (as found in CSV files) returns slightly
		//different names than species table; correct for converge_game
		//which matches based on name
		int nodeIdx = normStr.IndexOf ("[");
		if (nodeIdx != Constants.ID_NOT_SET) {
			if (normStr.Equals ("Fat or tree mouse [31]")) {
				normStr = "Tree mouse [31]";
			} else if (normStr.Equals ("Bushpig [83]")) {
				normStr = "Bush pig [83]";
			} else if (normStr.Equals ("Greater bush baby [55]")) {
				normStr = "Greater bushbaby [55]";
			} else if (normStr.Equals ("Grains  seeds [4]")) {
				normStr = "Grains and seeds [4]";
			} else if (normStr.Equals ("Black-and-white colobus monkey [66]")) {
				normStr = "Black and white columbus monkey [66]";
			}else if (normStr.Equals ("Millipedes [21]")) {
				normStr = "Millipede [21]";
			} else if (normStr.Equals ("Cockroaches [19]")) {
				normStr = "Cockroach [19]";
			} else if (normStr.Equals ("Harvester termites [12]")) {
				normStr = "Harvester termite [12]";
			} else if (normStr.Equals ("Harvester termites [12]")) {
				normStr = "Harvester termite [12]";
			} else if (normStr.Equals ("Kirk's dikdik [71]")) {
				normStr = "Kirk's dik-dik [71]";
				//note: upper case 'A'
			} else if (normStr.Equals ("Four-toed African hedgehog [45]")) {
				normStr = "Four toed african hedgehog [45]";
			} else if (normStr.Equals ("Rove- and ground beetles [9]")) {
				normStr = "Rove and ground beetles [9]";
			} else if (normStr.Equals ("Aquatic crustaceans [35]")) {
				normStr = "Crab [35]";
			} else if (normStr.Equals ("Centipedes [18]")) {
				normStr = "Centipede [18]";
			}
		}

		//force to titlecase
		normStr = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase (
			normStr.ToLower ());

		return normStr;
	}

	//Players will randomly be allowed to/prevented from receiving one
	//hint per attempt - once set, true for all further game play
	public static bool RandomBoolean ()
	{
		return (UnityEngine.Random.Range (-1f, +1f) > 0);
	}
}
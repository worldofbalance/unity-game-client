using UnityEngine;

using System;
using System.Collections;

public sealed class GUIExtended {

	// Window Properties
	private float left;
	private float top;
	private float width = 200;
	private float height = 200;
	private Rect windowRect;
	
	public static void Label(Rect position, string text, GUIStyle style, Color outColor, Color inColor) {
		GUIStyle backupStyle = style;
		style.normal.textColor = outColor;
		position.x--;
		GUI.Label(position, text, style);
		position.x += 2;
		GUI.Label(position, text, style);
		position.x--;
		position.y--;
		GUI.Label(position, text, style);
		position.y += 2;
		GUI.Label(position, text, style);
		position.y--;
		style.normal.textColor = inColor;
		GUI.Label(position, text, style);
		style = backupStyle;
	}

	public static int DropDownList(Rect position, int selected, string[] texts, ref bool active) {
		GUIStyle style = new GUIStyle(GUI.skin.box);
//		Texture2D tex = Resources.Load<Texture2D>("white");
//		style.normal.background = tex;

		if (active) {
			position.height = (texts.Length + 1) * (30 + 4) + 9;
		} else {
			position.height = 30 + 11;
		}

		GUI.BeginGroup(position, style);
			Rect buttonRect = new Rect(0, 0, position.width, 30);

			if (GUI.Button(buttonRect, texts[selected])) {
				active = !active;
			}

			if (active) {
				for (int i = 0; i < texts.Length; i++) {
					string text = texts[i];

					if (selected == i) {
						GUI.color = new Color(0.5f, 0.5f, 0.5f, 2);
					}

					if (GUI.Button(new Rect(buttonRect.x, buttonRect.y + (i + 1) * (buttonRect.height + 4), buttonRect.width, buttonRect.height), text)) {
						selected = i;
					}

					GUI.color = Color.white;
				}
//				selected = GUI.SelectionGrid(new Rect(buttonRect.x, buttonRect.y + buttonRect.height + 4, buttonRect.width, 100), selected, texts, 1);
			}
		GUI.EndGroup();

		return selected;
	}
}

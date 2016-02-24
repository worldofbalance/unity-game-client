using UnityEngine;

using System.Collections.Generic;

public class Page {

	public string title { get; set; }
	public List<string> contents { get; set; }
	public Vector2 scrollPos { get; set; }
	
	public Page(string title, List<string> contents) {
		this.title = title;
		this.contents = contents;

		scrollPos = Vector2.zero;
	}
}

using UnityEngine;
using System.Collections;

public class DynamicRect  {

	private Rect rect;
	public float left { get { return rect.left; } }
	public float top { get { return rect.top; } }
	public float width { get { return rect.width; } set { rect.width = value; } }
	public float height { get { return rect.height; } set { rect.height = value; } }
	public float x { get { return rect.x; } set { rect.x = value; } }
	public float y { get { return rect.y; } set { rect.y = value; } }
	public float deltaTime { get; set; }
	// Memory
	public float left_old { get; private set; }
	public float top_old { get; private set; }
	public float width_old { get; private set; }
	public float height_old { get; private set; }

	public DynamicRect(float left, float top, float width, float height) {
		rect = new Rect(left_old = left, top_old = top, width_old = width, height_old = height);
	}

	public Rect GetRect() {
		return rect;
	}
}

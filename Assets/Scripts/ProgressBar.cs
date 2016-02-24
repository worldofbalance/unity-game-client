using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	private float progress = 0.0f;
	private Vector2 pos = new Vector2(20,40);
	private Vector2 size = new Vector2(60,20);
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		progress = Time.time * 0.05f;
	}

	void OnGUI() {
		GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), progressBarEmpty);
    	GUI.DrawTexture(new Rect(pos.x, pos.y, size.x * Mathf.Clamp01(progress), size.y), progressBarFull);
	}
}

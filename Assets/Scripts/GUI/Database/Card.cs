using UnityEngine;

public class Card {

	public string name { get; set; }
	public Texture2D image { get; set; }
	public SpeciesData species { get; set; }
	private Rect rect;
	public float width { get { return rect.width; } }
	public float height { get { return rect.height; } }
	private bool isActive = false;
	public float x { get { return rect.x; } set { rect.x = startPos.x = currentPos.x = value; } }
	public float y { get { return rect.y; } set { rect.y = startPos.y = currentPos.y = value; } }
	public Color color { get; set; }
	public Texture2D cardTexture { get; set; }
	public Texture2D statTexture { get; set; }
	public Font font { get; set; }
	public Texture2D imgTexture { get; set; }
	public Rect imageRect { get; set; }
	private Texture2D frameTexture;
	// Other
	private float deltaTime;
	private Vector2 startPos;
	private Vector2 currentPos;
	private float speed = 1f;
	public bool isMoving { get; set; }
	
	public Card(string name, Texture2D image, SpeciesData species, Rect rect, Color color) {
		this.name = name;
		this.image = image;
		this.species = species;
		this.rect = rect;
		this.color = color;

		cardTexture = Resources.Load<Texture2D>("card_bg");
		statTexture = Resources.Load<Texture2D>("chart_dot");
		font = Resources.Load<Font>("Fonts/" + "Chalkboard");

		frameTexture = Resources.Load<Texture2D>("card_highlight");

		float imageWidth = rect.width * 0.8f, imageHeight = rect.width * 0.5f;
		imageRect = new Rect((rect.width - imageWidth) / 2, 35, imageWidth, imageHeight);
		imgTexture = Functions.CreateTexture2D(Color.white);

		startPos.x = currentPos.x = rect.x;
		startPos.y = currentPos.y = rect.y;
	}

	public Card(string name, Texture2D image, SpeciesData species, Rect rect) : this(name, image, species, rect, Color.black) {}

	public void Draw() {
		if (isMoving = Vector2.Distance(new Vector2(rect.x, rect.y), currentPos) > 0.05f) {
			deltaTime += Time.deltaTime * speed;
			rect.x = Mathf.Lerp(startPos.x, currentPos.x, deltaTime);
			rect.y = Mathf.Lerp(startPos.y, currentPos.y, deltaTime);
		}

		GUI.BeginGroup(rect);
			Functions.DrawBackground(new Rect(0, 0, rect.width, rect.height), cardTexture);

			float nameWidth = rect.width * 0.8f;
			GUI.BeginGroup(new Rect((rect.width - nameWidth) / 2, 10, nameWidth, 30));
				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.alignment = TextAnchor.UpperCenter;
				style.font = font;
				style.fontSize = 14;
				style.normal.textColor = color;

				GUI.Label(new Rect(0, 0, nameWidth, 24), name, style);
			GUI.EndGroup();

			GUI.BeginGroup(imageRect);
				GUI.DrawTexture(new Rect(0, 0, imageRect.width, imageRect.height), imgTexture);
				GUI.DrawTexture(new Rect((imageRect.width - imageRect.height) / 2, 0, imageRect.height, imageRect.height), image);
			GUI.EndGroup();

			GUI.color = Color.green;
			GUI.DrawTexture(new Rect(imageRect.x, 130, imageRect.width, 10), imgTexture);
			GUI.color = Color.white;

			GUI.BeginGroup(new Rect(25, 150, 150, 30));
				style.normal.textColor = Color.white;

				GUI.color = Color.red;
				GUI.DrawTexture(new Rect(0, 0, 30, 30), statTexture);
				GUI.color = Color.white;
				GUI.Label(new Rect(-1, 2, 30, 30), "", style);
				GUI.color = Color.green;
				GUI.DrawTexture(new Rect(40, 0, 30, 30), statTexture);
				GUI.color = Color.white;
				GUI.Label(new Rect(39, 2, 30, 30), "", style);
				GUI.color = Color.blue;
				GUI.DrawTexture(new Rect(80, 0, 30, 30), statTexture);
				GUI.color = Color.white;
				GUI.Label(new Rect(79, 2, 30, 30), "", style);
         	GUI.EndGroup();

			if (color != Color.black) {
				Functions.DrawBackground(new Rect(0, 0, rect.width, rect.height), frameTexture, color);
			}
		GUI.EndGroup();
	}

	public Rect GetRect() {
		return rect;
	}

	public void Translate(float x, float y, float speed = 1f) {		
		deltaTime = 0;

		startPos.x = rect.x;
		startPos.y = rect.y;
		
		currentPos.x = x;
		currentPos.y = y;

		this.speed = speed;
	}
}
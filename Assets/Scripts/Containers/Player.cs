using UnityEngine;

public class Player {
	
	private int player_id;
	public string name { get; set; }
	public short level { get; set; }
	public int xp { get; set; }
	public int credits { get; set; }
	public Color color { get; set; }
	public string last_played { get; set; }
	
	public Player(int player_id) {
		this.player_id = player_id;
	}
	
	public int GetID() {
		return player_id;
	}
	public string GetName() {
		return name;
	}
}

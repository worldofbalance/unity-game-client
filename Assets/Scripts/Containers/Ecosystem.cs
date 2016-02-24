using UnityEngine;

using System.Collections.Generic;

public class Ecosystem {

	public int eco_id { get; set; }
	public short type { get; set; }
	public int score { get; set; }
	public Player player { get; set; }
	public List<Zone> zones = new List<Zone>();

	public Ecosystem(int eco_id) {
		this.eco_id = eco_id;
	}
}

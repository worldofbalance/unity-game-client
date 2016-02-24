using UnityEngine;

public class Zone : MonoBehaviour {

	public int zone_id { get; set; }
	public short row { get; set; }
	public short column { get; set; }
	public short terrain_type { get; set; }
	public int v_capacity { get; set; }
	public int player_id { get; set; }

	public Zone(int zone_id) {
		this.zone_id = zone_id;
	}

	public Zone(int zone_id, short row, short column, short terrain_type) {
		this.zone_id = zone_id;
		this.row = row;
		this.column = column;
		this.terrain_type = terrain_type;
	}

	public int GetID() {
		return zone_id;
	}
}

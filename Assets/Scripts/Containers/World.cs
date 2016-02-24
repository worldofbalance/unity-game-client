public class World {
	
	public int world_id { get; set; }
	public string name { get; set; }
	public short type { get; set; }
	public int credits { get; set; }
	public bool isNew { get; set; }
	public short year { get; set; }
	public short day { get; set; }
	public int play_time { get; set; }
	public int score { get; set; }
	
	public World(int world_id) {
		this.world_id = world_id;
	}
}

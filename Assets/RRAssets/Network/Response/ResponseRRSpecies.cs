using UnityEngine;
using System.Collections;


namespace RR{
public class ResponseRRSpeciesEventArgs : ExtendedEventArgs {
	public int  id { get; set; }
	
	
	public ResponseRRSpeciesEventArgs() {
		event_id = Constants.SMSG_RRSPECIES;
	}
}
}

namespace RR {
public class ResponseRRSpecies : NetworkResponse {
	
	private int id;
	
	
	
	
	public override void parse() {
		Debug.Log ("species parse() -------------------------");
		id = DataReader.ReadInt(dataStream);
		
	}
	
	public override ExtendedEventArgs process() {
		
		Debug.Log ("species choose for 2nd player : " + id);
		ResponseRRSpeciesEventArgs args = new ResponseRRSpeciesEventArgs ();
		args.id = id;

		GameManager.species2 = id;
		
		return args;
	}
	
	
}
}

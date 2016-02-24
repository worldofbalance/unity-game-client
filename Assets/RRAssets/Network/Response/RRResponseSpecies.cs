using UnityEngine;
using System.Collections;


namespace RR{
public class RRResponseSpeciesEventArgs : ExtendedEventArgs {
	public int  id { get; set; }

	
	public RRResponseSpeciesEventArgs() {
		event_id = Constants.SMSG_RRSPECIES;
	}
}
}

namespace RR {
public class RRResponseSpecies : NetworkResponse {

	private int id;




	public override void parse() {
		///Debug.Log ("species parse() -------------------------");
		id = DataReader.ReadInt(dataStream);

	}

	public override ExtendedEventArgs process() {

		//Debug.Log ("species choose for 2nd player : " + id);
		RRResponseSpeciesEventArgs args = new RRResponseSpeciesEventArgs ();
		args.id = id;


		return args;
	}


}
}
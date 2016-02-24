using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ConvergePriorAttemptProtocol {
	
	public static NetworkRequest Prepare(int player_id, int ecosystem_id, int attemptOffset) {
		NetworkRequest request = new NetworkRequest(NetworkCode.CONVERGE_PRIOR_ATTEMPT);
		request.AddInt32(player_id);
		request.AddInt32(ecosystem_id);
		request.AddInt32(attemptOffset);

		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseConvergePriorAttempt response = new ResponseConvergePriorAttempt();
		
		using (BinaryReader br = new BinaryReader(dataStream, Encoding.UTF8)) {
			int playerId = br.ReadInt32 ();
			int ecosystemId = br.ReadInt32 ();
			int attemptId = br.ReadInt32 ();
			bool allowHints = br.ReadBoolean ();
			int hintId = br.ReadInt32 ();
			short fldSize = br.ReadInt16 ();
			String config = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
			fldSize = br.ReadInt16 ();
			String csv = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));

			ConvergeAttempt attempt = new ConvergeAttempt (playerId, 
			                                               ecosystemId, 
			                                               attemptId,
			                                               allowHints,
			                                               hintId,
			                                               config,
			                                               csv
			                                               //null
			                                               );

			response.attempt = attempt;
		}
		
		return response;
	}
}

public class ResponseConvergePriorAttempt : NetworkResponse {
	
	public ConvergeAttempt attempt { get; set; }
	
	public ResponseConvergePriorAttempt() {
		protocol_id = NetworkCode.CONVERGE_PRIOR_ATTEMPT;
	}
}

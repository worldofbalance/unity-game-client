using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

public class ConvergeNewAttemptProtocol
{

	public static NetworkRequest Prepare (
		int playerId, 
		int ecosystemId, 
		int attemptId, 
		bool allowHints,
		int hintId,
		int timesteps, 
		string config
		)
	{
		NetworkRequest request = new NetworkRequest (NetworkCode.CONVERGE_NEW_ATTEMPT);
		request.AddInt32 (playerId);
		request.AddInt32 (ecosystemId);
		request.AddInt32 (attemptId);
		request.AddBool (allowHints);
		request.AddInt32 (hintId);
		request.AddInt32 (timesteps);
		request.AddString (config);

		return request;
	}

	public static NetworkResponse Parse (MemoryStream dataStream)
	{
		ResponseConvergeNewAttempt response = new ResponseConvergeNewAttempt ();

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
			//Debug.Log ("csv = " + csv);

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

public class ResponseConvergeNewAttempt : NetworkResponse
{
		
	public ConvergeAttempt attempt { get; set; }
		
	public ResponseConvergeNewAttempt ()
	{
		protocol_id = NetworkCode.CONVERGE_NEW_ATTEMPT;
	}
}



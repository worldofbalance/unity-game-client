using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Used to end the battle
/// </summary>
public class ClashEndBattleProtocol {

	public enum BattleResult{
		WIN = 0,
		LOSS,
		DRAW
	}

	/// <summary>
	/// Creates a request containing battle results
	/// </summary>
	/// <param name="res">The result</param>
	public static NetworkRequest Prepare(BattleResult res) {
		NetworkRequest request = new NetworkRequest(NetworkCode.CLASH_END_BATTLE);
		request.AddInt32((int)res);

		
		return request;
	}

	/// <summary>
	/// Fills the response object with new credit balance
	/// </summary>
	/// <param name="dataStream">the input stream</param>
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseClashEndBattle response = new ResponseClashEndBattle();
		response.credits = DataReader.ReadInt(dataStream);
		return response;
	}
}

/// <summary>
/// Stores the new credit balance (changes depending on outcome of battle)
/// </summary>
public class ResponseClashEndBattle : NetworkResponse {
	/// <summary>
	/// Gets or sets the new credit balance
	/// </summary>
	/// <value>The balance.</value>
	public int credits {get; set;}

	public ResponseClashEndBattle() {
		protocol_id = NetworkCode.CLASH_END_BATTLE;
	}


}

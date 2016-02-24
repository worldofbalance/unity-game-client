public class NetworkRequest {

	private short protocol_id;
	private GamePacketStream buffer = new GamePacketStream();

	public NetworkRequest(short protocol_id) {
		AddShort16(this.protocol_id = protocol_id);
	}

	public short GetID() {
		return protocol_id;
	}

	public void AddShort16(short val) {
		buffer.Add(val);
	}

	public void AddInt32(int val) {
		buffer.Add(val);
	}

	public void AddLong64(long val) {
		buffer.Add(val);
	}

	public void AddBool(bool val) {
		buffer.Add(val);
	}

	public void AddBytes(byte[] bytes) {
		buffer.Add(bytes);
	}

	public void AddString(string val) {
		buffer.Add(val);
	}
	
	public void AddFloat(float val) {
		buffer.Add(val);
	}
	
	public int Size() {
		return buffer.Size();
	}
	
	public byte[] GetBytes() {
		return buffer.ToByteArray();
	}
}

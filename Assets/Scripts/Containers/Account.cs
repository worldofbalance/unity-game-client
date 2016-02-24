public class Account {
	
	public int account_id { get; set; }
	public string username { get; set; }
	public string last_logout { get; set; }
	
	public Account(int account_id) {
		this.account_id = account_id;
	}
}

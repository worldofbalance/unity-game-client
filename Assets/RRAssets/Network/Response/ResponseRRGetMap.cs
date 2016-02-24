using UnityEngine;
using System.Collections;

namespace RR{
	public class ResponseRRGetMapEventArgs : ExtendedEventArgs {
		public int mapNumber { get; set; }

		public ResponseRRGetMapEventArgs() {
			event_id = Constants.SMSG_RRGETMAP;
		}
	}
	
	public class ResponseRRGetMap : NetworkResponse {
		
		private int mapNumber;
		
		public ResponseRRGetMap() {
		}
		
		public override void parse() {
			mapNumber = DataReader.ReadInt (dataStream);
		}
		
		public override ExtendedEventArgs process() {

			ResponseRRGetMapEventArgs args = new ResponseRRGetMapEventArgs();
			
			args.mapNumber = this.mapNumber;

			return args;
		}
	}
}
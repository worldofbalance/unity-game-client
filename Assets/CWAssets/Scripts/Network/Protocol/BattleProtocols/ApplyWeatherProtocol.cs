using System.IO;

namespace CW
{
    public class ApplyWeatherProtocol
    {
        
        public static NetworkRequest Prepare (int playerID, int card_id)
        {
            
            NetworkRequest request = new NetworkRequest (NetworkCode.APPLY_WEATHER);
            request.AddInt32 (playerID);
            request.AddInt32 (card_id);
            return request;
        }
        
        public static NetworkResponse Parse (MemoryStream dataStream)
        {
            ResponseWeatherCard response = new ResponseWeatherCard ();
            
            response.status = DataReader.ReadShort (dataStream);
            
            return response;
        }
        
        
    }
    
    public class ResponseWeatherCard : NetworkResponse
    {
        
        public short status { get ; set; }
        
        public ResponseWeatherCard ()
        {
            protocol_id = NetworkCode.APPLY_WEATHER;
        }
    }
}
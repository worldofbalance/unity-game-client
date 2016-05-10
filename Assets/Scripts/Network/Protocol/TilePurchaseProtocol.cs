using UnityEngine;
using System.Collections;
using System.IO;

public class TilePurchaseProtocol {


    public static NetworkRequest Prepare(int zone_id)
    {
        NetworkRequest request = new NetworkRequest(NetworkCode.WORLD);
        request.AddInt32(zone_id);
        return request;

    }

    public static NetworkResponse Parse(MemoryStream dataStream)
    {
        TilePurchase response = new TilePurchase();
        response.status = DataReader.ReadShort(dataStream);

        if (response.status == 0)
        {
            response.zone_id = DataReader.ReadInt(dataStream);
            response.price = DataReader.ReadInt(dataStream);
        }

        return response;
    }
}

public class TilePurchase : NetworkResponse
{

    public int zone_id { get; set; }
    public int price { get; set; }
    public short status { get; set; }

    public TilePurchase()
    {
        protocol_id = NetworkCode.TILE_PURCHASE;
    }
}

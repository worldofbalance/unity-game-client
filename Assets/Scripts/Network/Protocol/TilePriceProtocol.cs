using UnityEngine;
using System.Collections;
using System.IO;

public class TilePriceProtocol
{


    public static NetworkRequest Prepare(int zone_id)
    {
        NetworkRequest request = new NetworkRequest(NetworkCode.TILE_PRICE);
        request.AddInt32(zone_id);
        return request;

    }

    public static NetworkResponse Parse(MemoryStream dataStream)
    {
        TilePrice response = new TilePrice();
        response.status = DataReader.ReadShort(dataStream);

        if (response.status == 0)
        {
            response.zone_id = DataReader.ReadInt(dataStream);
            response.price = DataReader.ReadInt(dataStream);
            response.canBuy = DataReader.ReadBool(dataStream);
        }

        return response;
    }
}

public class TilePrice : NetworkResponse
{

    public int zone_id { get; set; }
    public int price { get; set; }
    public bool canBuy { get; set; }
    public short status { get; set; }

    public TilePrice()
    {
        protocol_id = NetworkCode.TILE_PRICE;
    }
}


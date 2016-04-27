using UnityEngine;

using System;
namespace SD
{
    public class ResponsePlayInitEventArgs : ExtendedEventArgs
    {
        public short status { get; set; }
        public int playerId { get; set; }
        public int playerNumber { get; set; }
        public string playerName { get; set;}

        public ResponsePlayInitEventArgs()
        {
            event_id = Constants.SMSG_RACE_INIT;
        }
    }
}

namespace SD
{
    public class ResponsePlayInit : NetworkResponse
    {

        private short status;
        private int playerId;
        private int playerNumber;
        private string playerName;

        public ResponsePlayInit()
        {
        }

        public override void parse()
        {
            status = DataReader.ReadShort(dataStream);
            playerId = DataReader.ReadInt (dataStream);
            playerNumber = DataReader.ReadInt (dataStream);
            playerName = DataReader.ReadString (dataStream);
        }

        public override ExtendedEventArgs process()
        {
            ResponsePlayInitEventArgs args = null;

            args = new ResponsePlayInitEventArgs();
            args.status = status;
            args.playerId = playerId;
            args.playerNumber = playerNumber;
            args.playerName = playerName;
            return args;
        }
    }
}
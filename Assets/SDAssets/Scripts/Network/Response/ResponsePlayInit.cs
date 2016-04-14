//author: Ye
using UnityEngine;

using System;
namespace SD
{
    public class ResponsePlayInitEventArgs : ExtendedEventArgs
    {
        public short status { get; set; }

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

        private short status;//start a battle: 0; wait for a battle: 1

        public ResponsePlayInit()
        {
        }

        public override void parse()
        {
            status = DataReader.ReadShort(dataStream);
            //		Battle.stopSendRequest();
            if (status == 0)
            {
                //Debug.Log("Battle Preparation Started");
                //change to battle scene
                //when the battle is ended, change stopSendRequest to true;
            }
            else if (status == 1)
            {
                Debug.Log("request received by server, wait for a race");
            }
        }

        public override ExtendedEventArgs process()
        {
            ResponsePlayInitEventArgs args = null;

            args = new ResponsePlayInitEventArgs();
            args.status = status;

            if (status == 0)
            {
                //battle start, stop sending battle request
                Debug.Log("response received from server");
            }
            else if (status == 1)
            {
                //battle not start, continue sending battle request

            }
            return args;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace SD
{
    public class SDNpcPositionProtocol {

        public static NetworkRequest Prepare (int numNpc, List<NPCFish> npcFish)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_NPCPOSITION);
            request.AddString (numNpc.ToString()); // TODO: Have the server the change these into int/float.
            foreach (NPCFish item in npcFish) {
                request.AddString (item.id.ToString());
                request.AddString(item.speciesId.ToString());
                request.AddString (item.xPosition.ToString());
                request.AddString (item.yPosition.ToString());
                request.AddString (item.xRotationAngle.ToString());
            }
            Debug.Log ("Returning the request");
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDNpcPosition response = new ResponseSDNpcPosition ();
            response.numFish = DataReader.ReadInt (dataStream);
            for (int i = 0; i < response.numFish; i++) {
                response.preyId = DataReader.ReadInt (dataStream);
                response.speciesId = DataReader.ReadInt (dataStream);
                response.xPosition = DataReader.ReadFloat (dataStream);
                response.yPosition = DataReader.ReadFloat (dataStream);
                response.rotation = DataReader.ReadFloat (dataStream);

                // Set the required parameters to the NPCFish objects.
                if (GameController.getInstance ().getNpcFishes ().ContainsKey (response.preyId)) {
                    // The species already exists, simply adjust the position/rotation.
                    NPCFish npcFish = GameController.getInstance ().getNpcFishes ()[response.preyId];
                    npcFish.xPosition = response.xPosition;
                    npcFish.yPosition = response.yPosition;
                    npcFish.xRotationAngle = response.rotation;
                } else {
                    // This object needs to be created.
                    NPCFish npcFish = new NPCFish(response.preyId);
                    npcFish.xPosition = response.xPosition;
                    npcFish.yPosition = response.yPosition;
                    npcFish.xRotationAngle = response.rotation;
                    npcFish.speciesId = response.speciesId;
                    npcFish.toBeCreated = true;
                }

            }
            return response;
        }
    }

    public class ResponseSDNpcPosition : NetworkResponse
    {
        public int numFish {get; set;}
        public int preyId { get; set; }
        public int speciesId {get; set;}
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public float rotation { get; set; }

        public ResponseSDNpcPosition ()
        {
            protocol_id = NetworkCode.SD_NPCPOSITION;
        }
    }
}

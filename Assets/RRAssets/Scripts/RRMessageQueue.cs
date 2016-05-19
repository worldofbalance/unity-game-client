using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RR
{
    /// <summary>
    /// RR queue of callbacks to process based on server responses
    /// </summary>
    public class RRMessageQueue : MonoBehaviour
    {
        // Placement for a callback method
        public delegate void Callback (ExtendedEventArgs eventArgs);

        // List of callback functions
        public Dictionary<int, Callback> callbackList { get; set; }

        // The queue of args waiting for processing
        public Queue<ExtendedEventArgs> msgQueue { get; set; }

        // Instance of a singleton
        static private RRMessageQueue sInstance;

        // Instance retrieval for a singleton
        public static RRMessageQueue getInstance() {
            return sInstance;
        }

        /// <summary>
        /// Awake is called only once during the lifetime
        /// of the script instance when the script instance is being loaded.
        /// </summary>
        void Awake ()
        {
            sInstance = this;
            callbackList = new Dictionary<int, Callback> ();
            msgQueue = new Queue<ExtendedEventArgs> ();
        }
    
        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods is called the first time.
        /// </summary>
        void Start ()
        {
        }
    
        /// <summary>
        /// Update is called once per frame
        /// 
        /// It is calling callbacks as long as they are in a queue
        /// </summary>
        void Update ()
        {
            while (msgQueue.Count > 0) {

                // Pop one args from the message queue
                ExtendedEventArgs args = msgQueue.Dequeue ();

                if (callbackList.ContainsKey (args.event_id)) {
                    // Call the one callback which is requiered by the args
                    callbackList [args.event_id] (args);

                    // TODO debug, report
                    Debug.Log("Processed event handler for event No. " + args.event_id + " [" + args.GetType() + "]");
                } else {
                    // TODO debug, report
                    Debug.Log("Missing handler for event No. " + args.event_id + " [" + args.GetType() + "]");
                }
            }
        }

        /// <summary>
        /// Adds the callback to the callback list.
        /// </summary>
        /// <param name="event_id">Event identifier.</param>
        /// <param name="callback">Callback.</param>
        public void AddCallback (int event_id, Callback callback)
        {
            callbackList.Add (event_id, callback);
        }

        /// <summary>
        /// Removes the callback from the callback list.
        /// </summary>
        /// <param name="event_id">Event identifier.</param>
        public void RemoveCallback (int event_id)
        {
            if (callbackList.ContainsKey (event_id)) {
                callbackList.Remove (event_id);
            }
        }

        /// <summary>
        /// Adds the message to the message queue..
        /// </summary>
        /// <param name="event_id">Event identifier</param>
        /// <param name="args">ExtendedEventArgs</param>
        public void AddMessage (int event_id, ExtendedEventArgs args)
        {
            msgQueue.Enqueue (args);
        }
    }
}
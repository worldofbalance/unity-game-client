using System;
using UnityEngine;

<<<<<<< HEAD
public class Config
{
    //public static readonly string REMOTE_HOST = "localhost";
    //public static readonly string REMOTE_HOST = "52.25.138.18";
        public static readonly string REMOTE_HOST = "thecity.sfsu.edu";
    //public static readonly string REMOTE_HOST = "smurf.sfsu.edu";
    //public static readonly string REMOTE_HOST = "130.212.93.116";
    //public static readonly string REMOTE_HOST = "52.32.228.220";
=======
public class Config {
    //public static readonly string REMOTE_HOST = "localhost";
	  public static readonly string REMOTE_HOST = "thecity.sfsu.edu";
	//public static readonly string REMOTE_HOST = "smurf.sfsu.edu";
	//public static readonly string REMOTE_HOST = "130.212.93.116";
	//public static readonly string REMOTE_HOST = "52.32.228.220";
>>>>>>> dd7ea056b7e17912f863b69f73556ddc5564f21d

    public static string GetHost()
    {
        string envHost = Environment.GetEnvironmentVariable("WOB_HOST");
        return String.IsNullOrEmpty(envHost) ? REMOTE_HOST : envHost;
    }
}

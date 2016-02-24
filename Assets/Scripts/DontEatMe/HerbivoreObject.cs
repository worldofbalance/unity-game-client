using UnityEngine;
using System.Collections;

public class HerbivoreObject : MoveObject
{

	// Use this for initialization
	public override void Start ()
	{
		this.SetObjectType ((int)GAME_TYPE.Herbivore);
		base.Start();
	}
	
	// Update is called once per frame
		public override void Update ()
	{
		base.Update();
	}
}


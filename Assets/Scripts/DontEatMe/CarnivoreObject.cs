using UnityEngine;
using System.Collections;

public class CarnivoreObject : MoveObject
{

	// Use this for initialization
	public override void Start ()
	{
		this.SetObjectType ((int)GAME_TYPE.Carnivore);
		base.Start();
	}
	
	// Update is called once per frame
		public override void Update ()
	{
		base.Update();
	}
}


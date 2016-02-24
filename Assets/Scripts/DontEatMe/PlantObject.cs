using UnityEngine;
using System.Collections;

public class PlantObject : MoveObject
{

	// Use this for initialization
	public override void Start ()
	{
		this.SetObjectType ((int)GAME_TYPE.Plant);
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();
	}
}


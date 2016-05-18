/****************************************		
	Copyright 2015 Unluck Software	
	www.chemicalbliss.com

	Changelog
	v1.2
	IMPORTANT!	_posBuffer is now used for flock position, not controller position.
	Changed from _roamers Array To List
	Code cleanup
	v1.21
	Added grouping
	Fixed gizmo bug
	v1.3																																																																																																																		
*****************************************/
#pragma strict
import System.Collections.Generic;
public var _childPrefab:SchoolChild[];			// Assign prefab with SchoolChild script attached
public var _groupChildToNewTransform:boolean;	// Parents fish transform to school transform
public var _groupTransform:Transform;			// New game object created for group
public var _groupName:String = "";				// Name of group (if no name, the school name will be used)
public var _groupChildToSchool:boolean;			// Parents fish transform to school transform
public var _childAmount:int = 250;				// Number of objects
public var _spawnSphere:float = 3;				// Range around the spawner waypoints will created //changed to box
public var _spawnSphereDepth:float = 3;			
public var _spawnSphereHeight:float = 1.5;		
public var _childSpeedMultipler:float = 2;		// Adjust speed of entire school
public var _minSpeed:float = 6;					// minimum random speed
public var _maxSpeed:float = 10;				// maximum random speed
public var _speedCurveMultiplier:AnimationCurve = new AnimationCurve(Keyframe(0, 1), Keyframe(1, 1));
public var _minScale:float = .7;				// minimum random size
public var _maxScale:float = 1;					// maximum random size
public var _minDamping:float = 1;				// Rotation tween damping, lower number = smooth/slow rotation (if this get stuck in a loop, increase this value)
public var _maxDamping:float = 2;
public var _waypointDistance:float = 1;			// How close this can get to waypoint before creating a new waypoint (also fixes stuck in a loop)
public var _minAnimationSpeed:float = 2;
public var _maxAnimationSpeed:float = 4;		
public var _randomPositionTimerMax:float = 10;	// When _autoRandomPosition is enabled
public var _randomPositionTimerMin:float = 4;	
public var _acceleration:float = .025;			// How fast child speeds up
public var _brake:float = .01;					// How fast child slows down 
public var _positionSphere:float = 25;			// If _randomPositionTimer is bigger than zero the controller will be moved to a random position within this sphere
public var _positionSphereDepth:float = 5;		// Overides height of sphere for more controll
public var _positionSphereHeight:float = 5;		// Overides height of sphere for more controll
public var _childTriggerPos:boolean;			// Runs the random position function when a child reaches the controller
public var _forceChildWaypoints:boolean;		// Forces all children to change waypoints when this changes position
public var _autoRandomPosition:boolean;			// Automaticly positions waypoint based on random values (_randomPositionTimerMin, _randomPositionTimerMax)
public var _forcedRandomDelay:float = 1.5;		// Random delay added before forcing new waypoint
public var _schoolSpeed:float;					// Value multiplied to child speed
public var _roamers:List.<SchoolChild>;
public var _posBuffer:Vector3;
public var _posOffset:Vector3;

//AVOIDANCE
public var _avoidance:boolean;				//Enable/disable avoidance
public var _avoidAngle:float = 0.35; 		//Angle of the rays used to avoid obstacles left and right
public var _avoidDistance:float = 1;		//How far avoid rays travel
public var _avoidSpeed:float = 75;			//How fast this turns around when avoiding	
public var _stopDistance:float	= .5;		//How close this can be to objects directly in front of it before stopping and backing up. This will also rotate slightly, to avoid "robotic" behaviour
public var _stopSpeedMultiplier:float = 2;	//How fast to stop when within stopping distance
public var _avoidanceMask:LayerMask = -1;

//PUSH
public var _push:boolean;					//Enable/disable push
public var _pushDistance:float;				//How far away obstacles can be before starting to push away	
public var _pushForce:float = 5;			//How fast/hard to push away

//BUBBLES
public var _bubbles:SchoolBubbles;

//FRAME SKIP
public var _updateDivisor:int = 1;				//Skip update every N frames (Higher numbers might give choppy results, 3 - 4 on 60fps , 2 - 3 on 30 fps)
public var _newDelta:float;
public var _updateCounter:int;
public var _activeChildren:int;

function Start () {
	_posBuffer = transform.position + _posOffset;
	_schoolSpeed = Random.Range(1 , _childSpeedMultipler);
	AddFish(_childAmount);
	Invoke("AutoRandomWaypointPosition", RandomWaypointTime());
}

function Update () {
	if(_activeChildren > 0){
		if(_updateDivisor > 1){
			_updateCounter++;
			_updateCounter = _updateCounter % _updateDivisor;	
			_newDelta = Time.deltaTime*_updateDivisor;	
		}else{
			_newDelta = Time.deltaTime;
		}
		UpdateFishAmount();
	}
}

function InstantiateGroup(){
	if(_groupTransform) return;
	var g:GameObject = new GameObject();
	_groupTransform = g.transform;
	_groupTransform.position = transform.position;
	if(_groupName != ""){
		g.name = _groupName;
		return;
	}	
	g.name = transform.name + " Fish Container";
}

function AddFish(amount:int){
	if(_groupChildToNewTransform)InstantiateGroup();	
	for(var i:int=0;i<amount;i++){
		var child:int = Random.Range(0,_childPrefab.length);
		var obj : SchoolChild = Instantiate(_childPrefab[child]);		
		obj._spawner = this;
		_roamers.Add(obj);
		AddChildToParent(obj.transform);
	}	
}

function AddChildToParent(obj:Transform){	
	if(_groupChildToSchool){
		obj.parent = transform;
		return;
	}
	if(_groupChildToNewTransform){
		obj.parent = _groupTransform;
		return;
	}
}

function RemoveFish(amount:int){
	var dObj:SchoolChild = _roamers[_roamers.Count-1];
	_roamers.RemoveAt(_roamers.Count-1);
	Destroy(dObj.gameObject);
}

function UpdateFishAmount(){
	if(_childAmount>= 0 && _childAmount < _roamers.Count){
		RemoveFish(1);
		return;
	}
	if (_childAmount > _roamers.Count){	
		AddFish(1);
		return;
	}
}

//Set waypoint randomly inside box
function SetRandomWaypointPosition() {
	_schoolSpeed = Random.Range(1 , _childSpeedMultipler);
	var t:Vector3;
	t.x = Random.Range(-_positionSphere, _positionSphere) + transform.position.x;
	t.z = Random.Range(-_positionSphereDepth, _positionSphereDepth) + transform.position.z;
	t.y = Random.Range(-_positionSphereHeight, _positionSphereHeight) + transform.position.y;
	_posBuffer = t;	
	if(_forceChildWaypoints){
		for (var i:int = 0; i < _roamers.Count; i++) {
			(_roamers[i] as SchoolChild).Wander(Random.value*_forcedRandomDelay);
		}	
	}
}

function AutoRandomWaypointPosition () {
	if(_autoRandomPosition && _activeChildren > 0){
		SetRandomWaypointPosition();
	}
	CancelInvoke("AutoRandomWaypointPosition");
	Invoke("AutoRandomWaypointPosition", RandomWaypointTime());
}

function RandomWaypointTime():float{
	return Random.Range(_randomPositionTimerMin, _randomPositionTimerMax);
}

function OnDrawGizmos () {
	if(!Application.isPlaying && _posBuffer != transform.position + _posOffset) _posBuffer = transform.position + _posOffset;
	Gizmos.color = Color.blue;
	Gizmos.DrawWireCube (_posBuffer, Vector3(_spawnSphere*2, _spawnSphereHeight*2 ,_spawnSphereDepth*2));
	Gizmos.color = Color.cyan;
	Gizmos.DrawWireCube (transform.position, Vector3((_positionSphere*2)+_spawnSphere*2, (_positionSphereHeight*2)+_spawnSphereHeight*2 ,(_positionSphereDepth*2)+_spawnSphereDepth*2));
}
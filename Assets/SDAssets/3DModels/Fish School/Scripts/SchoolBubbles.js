#pragma strict

public var _bubbleParticles:ParticleSystem;
public var _emitEverySecond:float = 0.01;
public var _speedEmitMultiplier:float = 0.25;
public var _minBubbles:int = 0;
public var _maxBubbles:int = 5;

function Start () {
	if(!_bubbleParticles) transform.GetComponent(ParticleSystem);
}

function EmitBubbles (pos:Vector3, amount:float) {
	var f:float = amount*_speedEmitMultiplier;
	if(f < 1) return;
	_bubbleParticles.transform.position = pos;
	_bubbleParticles.Emit(Mathf.Clamp(amount*_speedEmitMultiplier, _minBubbles, _maxBubbles));	
}
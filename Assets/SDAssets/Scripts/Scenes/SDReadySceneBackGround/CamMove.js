
var mRoot : GameObject;

var mRotSinAmp : float;
var mRotSinSpeed : float;

private var mRotSinTime : float;
private var mRotYDefault : float;


var mPosSinAmp : float;
var mPosSinSpeed : float;

private var mPosSinTime : float;
private var mPosYDefault : float;


function Start()
{
	if ( !mRoot )
		mRoot = gameObject;

	SetDefaults( mRoot );
}

function SetDefaults( aCamGO : GameObject )
{
	mRotYDefault = aCamGO.transform.rotation.eulerAngles.y;
	mPosYDefault = aCamGO.transform.position.y;

}

function LateUpdate()
{
	mRotSinTime += (mRotSinSpeed * Time.deltaTime);	
	var jumpy : float = Mathf.Sin( mRotSinTime );	
	mRoot.transform.rotation.eulerAngles.y = mRotYDefault + (jumpy * mRotSinAmp );	
	
	mPosSinTime += (mPosSinSpeed * Time.deltaTime);	
	jumpy = Mathf.Sin( mPosSinTime );
	mRoot.transform.position.y = mPosYDefault + (jumpy * mPosSinAmp );	
}

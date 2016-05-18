/****************************************
    Copyright 2015 Unluck Software
    www.chemicalbliss.com
*****************************************/
@CustomEditor(SchoolController)
class SchoolControllerEditor extends Editor
{
    var myProperty: SerializedProperty;
    var bubbles: SerializedProperty;
    var avoidanceMask: SerializedProperty;

    function OnEnable()
    {
        avoidanceMask= serializedObject.FindProperty("_avoidanceMask");
        if(target._bubbles == null)
        {
            target._bubbles = Transform.FindObjectOfType(SchoolBubbles);
        }
        myProperty = serializedObject.FindProperty("_childPrefab");
        bubbles = serializedObject.FindProperty("_bubbles");
    }

    function OnInspectorGUI()
    {
        var warningColor: Color = Color32(255, 174, 0, 255);
        var warningColor2: Color = Color.yellow;
        var dColor: Color = Color32(175, 175, 175, 255);
        var aColor: Color = Color.white;
        var warningStyle = new GUIStyle(GUI.skin.label);
        warningStyle.normal.textColor = warningColor;
        warningStyle.fontStyle = FontStyle.Bold;
        var warningStyle2 = new GUIStyle(GUI.skin.label);
        warningStyle2.normal.textColor = warningColor2;
        warningStyle2.fontStyle = FontStyle.Bold;
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        if(UnityEditor.EditorApplication.isPlaying)
        {
            GUI.enabled = false;
        }
        target._updateDivisor = EditorGUILayout.Slider("Frame Skipping", target._updateDivisor, 1, 10);
        GUI.enabled = true;
        if(target._updateDivisor > 4)
        {
            EditorGUILayout.LabelField("Will cause choppy movement", warningStyle);
        }
        else if(target._updateDivisor > 2)
        {
            EditorGUILayout.LabelField("Can cause choppy movement	", warningStyle2);
        }
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        serializedObject.Update();
        EditorGUILayout.PropertyField(myProperty, new GUIContent("Fish Prefabs"), true);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.LabelField("Prefabs must have SchoolChild component", EditorStyles.miniLabel);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Grouping", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Move fish into a parent transform", EditorStyles.miniLabel);
        target._groupChildToSchool = EditorGUILayout.Toggle("Group to School", target._groupChildToSchool);
        if(target._groupChildToSchool)
        {
            GUI.enabled = false;
        }
        target._groupChildToNewTransform = EditorGUILayout.Toggle("Group to New GameObject", target._groupChildToNewTransform);
        target._groupName = EditorGUILayout.TextField("Group Name", target._groupName);
        GUI.enabled = true;
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Bubbles", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(bubbles, new GUIContent("Bubbles Object"), true);
        if(target._bubbles)
        {
            target._bubbles._emitEverySecond = EditorGUILayout.FloatField("Emit Every Second", target._bubbles._emitEverySecond);
            target._bubbles._speedEmitMultiplier = EditorGUILayout.FloatField("Fish Speed Emit Multiplier", target._bubbles._speedEmitMultiplier);
            target._bubbles._minBubbles = EditorGUILayout.IntField("Minimum Bubbles Emitted", target._bubbles._minBubbles);
            target._bubbles._maxBubbles = EditorGUILayout.IntField("Maximum Bubbles Emitted", target._bubbles._maxBubbles);
            if(GUI.changed)
            {
            EditorUtility.SetDirty(target._bubbles);
            }
        }
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Area Size", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Size of area the school roams within", EditorStyles.miniLabel);
        target._positionSphere = EditorGUILayout.FloatField("Roaming Area Width", target._positionSphere);
        target._positionSphereDepth = EditorGUILayout.FloatField("Roaming Area Depth", target._positionSphereDepth);
        target._positionSphereHeight = EditorGUILayout.FloatField("Roaming Area Height", target._positionSphereHeight);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Size of the school", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Size of area the Fish swim towards", EditorStyles.miniLabel);
        target._childAmount = EditorGUILayout.Slider("Fish Amount", target._childAmount, 1, 500);
        target._spawnSphere = EditorGUILayout.FloatField("School Width", target._spawnSphere);
        target._spawnSphereDepth = EditorGUILayout.FloatField("School Depth", target._spawnSphereDepth);
        target._spawnSphereHeight = EditorGUILayout.FloatField("School Height", target._spawnSphereHeight);
        target._posOffset = EditorGUILayout.Vector3Field("Start Position Offset", target._posOffset);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Speed and Movement ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Change Fish speed, rotation and movement behaviors", EditorStyles.miniLabel);
        target._childSpeedMultipler = EditorGUILayout.FloatField("Random Speed Multiplier", target._childSpeedMultipler);
        target._speedCurveMultiplier = EditorGUILayout.CurveField("Speed Curve Multiplier", target._speedCurveMultiplier);
        if(target._childSpeedMultipler < 0.01) target._childSpeedMultipler = 0.01;
        target._minSpeed = EditorGUILayout.FloatField("Min Speed", target._minSpeed);
        target._maxSpeed = EditorGUILayout.FloatField("Max Speed", target._maxSpeed);
        target._acceleration = EditorGUILayout.Slider("Fish Acceleration", target._acceleration, .001, 0.07);
        target._brake = EditorGUILayout.Slider("Fish Brake Power", target._brake, .001, 0.025);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Turn Speed", EditorStyles.boldLabel);
        target._minDamping = EditorGUILayout.FloatField("Min Turn Speed", target._minDamping);
        target._maxDamping = EditorGUILayout.FloatField("Max Turn Speed", target._maxDamping);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Randomize Fish Size ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Change scale of Fish when they are added to the stage", EditorStyles.miniLabel);
        target._minScale = EditorGUILayout.FloatField("Min Scale", target._minScale);
        target._maxScale = EditorGUILayout.FloatField("Max Scale", target._maxScale);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Fish Random Animation Speeds", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Animation speeds are also increased by movement speed", EditorStyles.miniLabel);
        target._minAnimationSpeed = EditorGUILayout.FloatField("Min Animation Speed", target._minAnimationSpeed);
        target._maxAnimationSpeed = EditorGUILayout.FloatField("Max Animation Speed", target._maxAnimationSpeed);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Fish Waypoint Distance", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Waypoints inside small sphere", EditorStyles.miniLabel);
        target._waypointDistance = EditorGUILayout.FloatField("Distance To Waypoint", target._waypointDistance);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Fish Triggers School Waypoint", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Fish waypoint triggers a new School waypoint", EditorStyles.miniLabel);
        target._childTriggerPos = EditorGUILayout.Toggle("Fish Trigger Waypoint", target._childTriggerPos);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Automatically New Waypoint", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Automatically trigger new school waypoint", EditorStyles.miniLabel);
        target._autoRandomPosition = EditorGUILayout.Toggle("Auto School Waypoint", target._autoRandomPosition);
        if(target._autoRandomPosition)
        {
            target._randomPositionTimerMin = EditorGUILayout.FloatField("Min Delay", target._randomPositionTimerMin);
            target._randomPositionTimerMax = EditorGUILayout.FloatField("Max Delay", target._randomPositionTimerMax);
            if(target._randomPositionTimerMin < 1)
            {
                target._randomPositionTimerMin = 1;
            }
            if(target._randomPositionTimerMax < 1)
            {
                target._randomPositionTimerMax = 1;
            }
        }
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Fish Force School Waypoint", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Force all Fish to change waypoints when school changes waypoint", EditorStyles.miniLabel);
        target._forceChildWaypoints = EditorGUILayout.Toggle("Force Fish Waypoints", target._forceChildWaypoints);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Force New Waypoint Delay", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("How many seconds until the Fish in school will change waypoint", EditorStyles.miniLabel);
        target._forcedRandomDelay = EditorGUILayout.FloatField("Waypoint Delay", target._forcedRandomDelay);
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        EditorGUILayout.LabelField("Obstacle Avoidance", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Steer and push away from obstacles (uses more CPU)", EditorStyles.miniLabel);
        EditorGUILayout.PropertyField(avoidanceMask, new GUIContent("Collider Mask"));
        target._avoidance = EditorGUILayout.Toggle("Avoidance (enable/disable)", target._avoidance);
        if(target._avoidance)
        {
            target._avoidAngle = EditorGUILayout.Slider("Avoid Angle", target._avoidAngle, .05, .95);
            target._avoidDistance = EditorGUILayout.FloatField("Avoid Distance", target._avoidDistance);
            if(target._avoidDistance <= 0.1) target._avoidDistance = 0.1;
            target._avoidSpeed = EditorGUILayout.FloatField("Avoid Speed", target._avoidSpeed);
            target._stopDistance = EditorGUILayout.FloatField("Stop Distance", target._stopDistance);
            target._stopSpeedMultiplier = EditorGUILayout.FloatField("Stop Speed Multiplier", target._stopSpeedMultiplier);
            if(target._stopDistance <= 0.1) target._stopDistance = 0.1;
        }
        EditorGUILayout.EndVertical();
        GUI.color = dColor;
        EditorGUILayout.BeginVertical("Box");
        GUI.color = Color.white;
        target._push = EditorGUILayout.Toggle("Push (enable/disable)", target._push);
        if(target._push)
        {
            target._pushDistance = EditorGUILayout.FloatField("Push Distance", target._pushDistance);
            if(target._pushDistance <= 0.1) target._pushDistance = 0.1;
            target._pushForce = EditorGUILayout.FloatField("Push Force", target._pushForce);
            if(target._pushForce <= 0.01) target._pushForce = 0.01;
        }
        EditorGUILayout.EndVertical();
        if(GUI.changed) EditorUtility.SetDirty(target);
    }
}
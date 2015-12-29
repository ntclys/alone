var target : Transform;
var distance = 7.0;

var xSpeed = 250.0;
var ySpeed = 120.0;
var zoomSpeed : float = 100.0;

var yMinLimit = -20;//-20;
var yMaxLimit = 80;

private var x = 0.0;
private var y = 0.0;

@script AddComponentMenu("Camera-Control/Mouse Orbit")

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;

	// Make the rigid body not change rotation
   //	if (rigidbody)
	//	rigidbody.freezeRotation = true;
}

function LateUpdate () {
    if (target) {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.03;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.03;
 		
 		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		
		distance += -(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomSpeed * 10.0 ; 
		
		if(distance >=7.0 )
			distance =7.0;
		 if(distance <=1.0)
			distance = 1.0;
			
		
      
        var rotation = Quaternion.Euler(y, x, 0);
        var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
        
        transform.rotation = rotation;
        transform.position = position;
    }
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}
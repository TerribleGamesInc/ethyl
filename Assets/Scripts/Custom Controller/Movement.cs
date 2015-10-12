using System;
using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	//Component References.

    private  MouseLook 		mouseLook;			//A reference to the mouse look script.
	private Controller 		controller;			//A reference to the controller script of the character.
	private new Rigidbody 	rigidbody;			//A reference to the rigidbody of the character.
	private CapsuleCollider capsuleCollider;	//A reference to the collider attached to the character used for collision detection.
	private new Camera 		camera;				//A reference to the main camera that is a child of the character.

	public  bool 	airControl 			  = false;	//If the character can control their motion in the air.
	private bool 	_isGrounded;					//If the character is currently grounded.
	private bool 	wasGrounded;					//If the character was previously grounded.
	private bool 	_jumping;						//If the character is currently jumping.
	private float 	yRotation;					 	//Rotation along the y-axis.
	private float 	groundCheckDistance   = 0.01f; 	//Distance used to check if the controller is grounded.
	private float 	stickToGroundDistance = 0.5f;  	//The offset distance threshold to help keep the player on the ground when they are not jumping.
	private Vector3 groundContractNormal;

	private AnimationCurve slopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));



	// Use this for initialization
	void Start () {
		mouseLook 		= GetComponent<MouseLook>();
		controller 		= GetComponent<Controller>();
		rigidbody 		= GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		camera 	   		= GetComponentInChildren<Camera>();
		mouseLook.Init (this.transform, camera.transform);
	}
	
	// Update is called once per frame
	void Update() {
		RotateView();
	}
	void FixedUpdate() {
		//Check if the character is on the ground.
		GroundCheck();

		if ((Mathf.Abs (controller.movement.x) > float.Epsilon || Mathf.Abs (controller.movement.y) > float.Epsilon) && (airControl || _isGrounded)) {
			Vector3 targetPosition = camera.transform.forward * controller.movement.y + camera.transform.right * controller.movement.x;
			targetPosition = Vector3.ProjectOnPlane (targetPosition, groundContractNormal).normalized;

			targetPosition.x *= controller.speed;
			targetPosition.y *= controller.speed;
			targetPosition.z *= controller.speed;

			if (rigidbody.velocity.sqrMagnitude < (controller.speed * controller.speed)) {
				rigidbody.AddForce(targetPosition * SlopeMultiplyer(), ForceMode.Impulse);
			}
		}

		if (_isGrounded) {
			rigidbody.drag = 5f;
			
			if (controller.Jump) {
				Jump();
			}
			if (!_jumping && Mathf.Abs(controller.movement.x) < float.Epsilon && Mathf.Abs(controller.movement.y) < float.Epsilon && rigidbody.velocity.magnitude < 1f) {
				rigidbody.Sleep();
			}
		}
		else {
			rigidbody.drag = 0f;
			if (wasGrounded && !_jumping) {
				StickToGroundHelper();
			}
		}
	}
	/*ACCESSOR METHODS*/

	public bool Grounded {
		get {return _isGrounded;}
	}
	public bool Jumping {
		get {return _jumping;}
	}
	public bool Sprinting {
		get {return controller.Sprint;}
	}
	public Vector3 Velocity {
		get {return rigidbody.velocity;}
	}
	/*MUTATOR METHODS*/
	private void GroundCheck() {
		float distance = ((capsuleCollider.height / 2.0f) - capsuleCollider.radius) + groundCheckDistance;

		wasGrounded = _isGrounded;

		RaycastHit hit;
		if(Physics.SphereCast(this.transform.position, capsuleCollider.radius, Vector3.down, out hit, distance)) {
			_isGrounded = true;
			groundContractNormal = hit.normal;
		}
		else {
			_isGrounded = false;
			groundContractNormal = Vector3.up;
		}
		if(!wasGrounded && _isGrounded && _jumping) {
			_jumping = false;
		}
	}
	private void Jump() {
		rigidbody.drag = 0f;
		rigidbody.velocity = new Vector3 (rigidbody.velocity.x, 0f, rigidbody.velocity.z);
		rigidbody.AddForce(new Vector3(0f, controller.JumpForce, 0f), ForceMode.Impulse);
		_jumping = true;
	}
	private void RotateView() {
		if (Mathf.Abs (Time.timeScale) < float.Epsilon) {
			return;
		}

		float oldYRotation = this.transform.eulerAngles.y;
		mouseLook.LookRotation(this.transform, camera.transform);

		if (_isGrounded || airControl) {
			//Rotate the rigidbody velocity to match the new direction that the character is looking.
			Quaternion velocityRotation = Quaternion.AngleAxis(this.transform.eulerAngles.y - oldYRotation, Vector3.up);
			rigidbody.velocity = velocityRotation * rigidbody.velocity;
		}
	}
	private float SlopeMultiplyer() {
		float angle = Vector3.Angle(groundContractNormal, Vector3.up);
		return slopeCurveModifier.Evaluate(angle);
	}
	private void StickToGroundHelper() {
		RaycastHit hit;
		float distance = ((capsuleCollider.height / 2.0f) - capsuleCollider.radius) + stickToGroundDistance;
		if(Physics.SphereCast(this.transform.position, capsuleCollider.radius, Vector3.down, out hit, distance)) {
			if(Mathf.Abs (Vector3.Angle(hit.normal, Vector3.up)) < 85f) {
				rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, hit.normal);
			}
		}
	}
}
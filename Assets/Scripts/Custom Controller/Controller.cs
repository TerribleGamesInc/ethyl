using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public Vector2 movement;

	public float speed;

	//Declare private script level variables.
	[SerializeField][Range(1.0f, 010.0f)] private float forwardSpeed  	 = 008.0f;	//The speed of the character when moving forward.
	[SerializeField][Range(1.0f, 010.0f)] private float backwardSpeed 	 = 004.0f; 	//The speed of the character when moving backward.
	[SerializeField][Range(1.0f, 010.0f)] private float strafeSpeed   	 = 004.0f;	//The speed of the character when strafing.
	[SerializeField][Range(1.0f, 005.0f)] private float sprintMultiplyer = 002.0f; 	//The speed multiplyer when the character is sprinting.
	[SerializeField][Range(0.0f, 100.0f)] private float _jumpForce		 = 50f;	//The force of a jump.
										  private bool  _sprint;
										  private bool  _jump;
	
	// Use this for initialization
	void Start () {
		movement = new Vector2();
	}

	void Update() {
		GetInput();
		UpdateSpeed();
	}

	public bool Sprint {
		get {return _sprint;}
		set {_sprint = value;}
	}
	public bool Jump {
		get {return _jump;}
		set {_jump = value;}
	}
	public float JumpForce {
		get {return _jumpForce;}
	}
	private void UpdateSpeed() { 
		//If there is no movement...
		if(movement == Vector2.zero) {
			//...return.
			return;
		}
		//If the character is strafing...
		if(movement.x != 0) {
			//...Assign strafe speed to speed.
			speed = strafeSpeed;
		}
		//If the character is moving forward....
		if(movement.y > 0) {
			//...Assign forward speed to speed.
			speed = forwardSpeed;
		}
		//If the character is moving backward.
		if(movement.y < 0) {
			//...Assign backward speed to speed.
			speed = backwardSpeed;
		}
		if(_sprint) {
			speed *= sprintMultiplyer;
		}
	}

	private void GetInput() {
		if (Input.GetButton("Left")) {
			movement.x = -1;
		}
		if(Input.GetButton("Right")) {
			movement.x = 1;
		}
		if (Input.GetButton("Forward")) {
			movement.y = 1;
		}
		if(Input.GetButton("Backward")) {
			movement.y = -1;
		}
		if(!Input.GetButton("Left") && !Input.GetButton("Right")) {
			movement.x = 0;
		}
		if(!Input.GetButton("Forward") && !Input.GetButton("Backward")) {
			movement.y = 0;
		}
		_sprint = Input.GetButton("Sprint");
		_jump 	= Input.GetButtonDown("Jump") && !_jump;
	}
}
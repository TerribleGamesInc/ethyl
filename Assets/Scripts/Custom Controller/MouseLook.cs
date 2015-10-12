using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public float MinimumX 	  = -90F;
	public float MaximumX 	  = 90F;
	public float smoothTime   = 5f;
	public bool  smooth;
	public bool  clampVerticalRotation = true;

	
	private Quaternion characterTargetRotation;
	private Quaternion cameraTargetRotation;
	
	
	public void Init(Transform character, Transform camera) {
		characterTargetRotation = character.localRotation;
		cameraTargetRotation    = camera.localRotation;
	}
	
	
	public void LookRotation(Transform character, Transform camera) {
		float yRotation = Input.GetAxis("Mouse X") * XSensitivity;
		float xRotation = Input.GetAxis("Mouse Y") * YSensitivity;
		
		characterTargetRotation *= Quaternion.Euler(0f, yRotation, 0f);
		cameraTargetRotation    *= Quaternion.Euler(-xRotation, 0f, 0f);
		
		if(clampVerticalRotation) {
			cameraTargetRotation = ClampRotationAroundXAxis (cameraTargetRotation);
		}
		if(smooth) {
			character.localRotation = Quaternion.Slerp (character.localRotation, characterTargetRotation, smoothTime * Time.deltaTime);
			camera.localRotation    = Quaternion.Slerp (camera.localRotation, cameraTargetRotation, smoothTime * Time.deltaTime);
		}
		else {
			character.localRotation = characterTargetRotation;
			camera.localRotation    = cameraTargetRotation;
		}
	}
	
	Quaternion ClampRotationAroundXAxis(Quaternion q) {
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;
		
		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
		
		angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
		
		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
		
		return q;
	}
}
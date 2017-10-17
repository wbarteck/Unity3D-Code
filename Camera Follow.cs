using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	[HideInInspector]
	public Camera cam;
	[Header("General Movement")]
	public GameObject follow;
	public float speed = 30f;
	public float zoomSpeed = 500f;
	public float rotateSpeed = 90f;
	public float dampening = 5f;
	public float minHeight = 3f;
	public float maxHeight = 30f;

	private float rotateAmount;
	private float zoomAmount;

	Vector3 currentPosition;
	Vector3 wantedPosition;
	Quaternion currentRotation;
	Quaternion wantedRotation;

	// Use this for initialization
	void Start () {
		//get cam
		cam = GetComponent<Camera>();
		//initialize position and rotation
		currentPosition = transform.position;
		wantedPosition = currentPosition;
		currentRotation = transform.rotation;
		wantedRotation = currentRotation;
	}
	
	// Update is called once per frame
	void Update () {
		//establish variables
		currentPosition = transform.position;
		currentRotation = transform.rotation;
		//get direction
		Vector3 fwd = transform.forward;
		fwd.y = 0; //limit to the x-z plane
		fwd = fwd.normalized; //keep it clean
		Vector3 right = transform.right;
		right.y = 0;
		right = right.normalized;

		if (rotateAmount != 0) {
			Vector3 rotatePoint = transform.position + fwd * transform.position.y;
			RotateAround (rotatePoint, Vector3.up, rotateSpeed * Time.deltaTime * rotateAmount);
		}

		//calculate the direction towards the object
		Vector3 offset = fwd * wantedPosition.y;

		//get position
		Vector3 pos = follow.transform.position - offset;
		pos.y = wantedPosition.y;

		//set new position
		wantedPosition = pos;

		//zoom
		wantedPosition += zoomAmount * transform.forward * zoomSpeed * Time.deltaTime;

		//calculate move
		currentPosition = Vector3.Lerp(currentPosition, wantedPosition, dampening * Time.deltaTime);
		currentRotation = Quaternion.Lerp (currentRotation, wantedRotation, dampening * Time.deltaTime);
		//move
		transform.position = currentPosition;
		transform.rotation = currentRotation;
	}
	void LateUpdate() {
		rotateAmount = 0;
		zoomAmount = 0;
	}

	void RotateAround(Vector3 center, Vector3 axis, float angle) {
		Vector3 pos = wantedPosition;
		Quaternion rot = Quaternion.AngleAxis (angle, axis); //desired rotation
		Vector3 dir = pos - center;
		dir = rot * dir;
		dir.y = 0;
		wantedPosition = center +  dir;
		Quaternion newRot = transform.rotation;
		wantedRotation *= Quaternion.Inverse (newRot) * rot * newRot;
	}
}

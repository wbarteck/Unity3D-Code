using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Greyman;

public class MultiObjectFollow : MonoBehaviour {

	public List<GameObject> trackers = new List<GameObject>();// All the targets the camera needs to encompass.

	public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
	public float m_ScreenEdgeBuffer = 20f; //10     // Space between the top/bottom most target and the screen edge.
	public float m_forward_offset = 2f;

	public float maxZoomDistance = 20f;				// Max distance allowed for Camera to zoom out
	public float minZoomDistance = 50f;
	private float zoom = .5f;
	public float zoomSpeed = -5f;
	private float rotSpeed = 140f;
	///public float maxTrackerDisplacement = 12f;	// Max istance a tracker can be away from the camera

	public Camera m_Camera;                        // Used for referencing the camera.
	public Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
	private Vector3 m_AveragePosition;              // The position the camera is moving towards.

	private float maxDistance; 						// The max distance between tracked objects
	private Vector3 dir;							// Direction the Camera is facing
	private Vector3 desiredPosition;

	// UI Camera
	public UICamera uiCamera;

	public bool AlwaysLook = false;

	private void Awake ()
	{
		m_Camera = GetComponentInChildren<Camera> ();
		//InvokeRepeating("SnapRotate", 5f ,5f);
	}


	private void Update ()
	{
		// Move the camera towards a desired position.
		Move ();

		// Change the size of the camera based.
		Zoom ();

		//make sure targets are al in view
		//FindObjectsOutsideView();

		if (AlwaysLook) {
			FaceForward ();
		}


	}


	private void Move ()
	{
		// Find the average position of the targets.
		FindAveragePosition ();

		if (float.IsNaN(m_MoveVelocity.x) || float.IsNaN(m_MoveVelocity.y) || float.IsNaN(m_MoveVelocity.z)) {
			m_MoveVelocity = new Vector3 ();
		}

		// Smoothly transition to that position.
		float dist = Mathf.Clamp(maxDistance / 2f + m_ScreenEdgeBuffer, 0, maxZoomDistance);
		dist = Mathf.Lerp (minZoomDistance, dist, zoom);
		desiredPosition = m_AveragePosition - transform.forward * (dist);
		Vector3 fwd = transform.forward;
		fwd.y = 0;
		fwd.Normalize ();
		desiredPosition += fwd * m_forward_offset;
		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_MoveVelocity, m_DampTime);
	}


	private void FindAveragePosition ()
	{
		maxDistance = 0;
		Vector3 averagePos = new Vector3 (0f, 0f, 0f);

		// Go through all the targets and add their positions together.
		foreach (GameObject a in trackers)
		{
			// If the target isn't active, go on to the next one.
			if (!a.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += a.transform.position;

			// Find max distance
			foreach (GameObject b in trackers) {
				float dist = (b.transform.position - a.transform.position).magnitude;
				if (dist > maxDistance)
					maxDistance = dist;
			}
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		averagePos /= trackers.Count;

		// Keep the same y value.
		//averagePos.y = transform.position.y;

		// The desired position is the average position;
		m_AveragePosition = averagePos;
	}


	public void Zoom (float amount = 0f)
	{
		// FOR ORTHOGRAPHIC
		// Find the required size based on the desired position and smoothly transition to that size.
		//float requiredSize = FindRequiredSize();
		//m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);

		// FOR PERSPECTIVe
		zoom -= amount * zoomSpeed * Time.deltaTime;
		zoom = Mathf.Clamp (zoom, 0f, 1f);
		dir = (m_Camera.transform.rotation.eulerAngles).normalized;

	}
	public void Rotate(float amount = 0f) {
		RotateAround(m_AveragePosition, Vector3.up, amount * Time.deltaTime * rotSpeed);
	}

	public void SnapRotate() {
		StopAllCoroutines ();
		StartCoroutine (SnapForward (trackers [0].transform.forward));
	}

	IEnumerator SnapForward( Vector3 dir = new Vector3()) {
		Vector3 camFwd = transform.forward;
		camFwd.y = 0;
		camFwd.Normalize ();

		Vector3 eulerDir = trackers [0].transform.eulerAngles;
		Vector3 eulerCam = transform.eulerAngles;

		float y1 = Vector3.Angle (dir, camFwd);
		float y2 = eulerDir.y - eulerCam.y;
		y2 = (y2 + 360) % 360;

		Debug.DrawRay (trackers [0].transform.position, dir * 3, Color.red, 1f);
		Debug.DrawRay (trackers [0].transform.position, camFwd * 3, Color.blue, 1f);

		while (-10f > y1 || y1 > 10f) {
			camFwd = transform.forward;
			camFwd.y = 0;
			camFwd.Normalize ();

			eulerCam = transform.eulerAngles;

			if (y2 > 0f && y2 < 180f) {
				this.RotateAround (m_AveragePosition, Vector3.up, y2/15);
			} else {
				this.RotateAround (m_AveragePosition, Vector3.up, -(360 - y2)/15);
			}

			y1 = Vector3.Angle (dir, camFwd);
			y2 = eulerDir.y - eulerCam.y;
			y2 = (y2 + 360) % 360;

			yield return null;
		}
			
		StopAllCoroutines ();
	}

	public void FaceForward() {
		Vector3 camFwd = transform.forward;
		camFwd.y = 0;
		camFwd.Normalize ();

		Vector3 eulerDir = trackers [0].transform.eulerAngles;
		Vector3 eulerCam = transform.eulerAngles;

		float y1 = Vector3.Angle (dir, camFwd);
		float y2 = eulerDir.y - eulerCam.y;
		y2 = (y2 + 360) % 360;

		if (-1f > y1 || y1 > 1f) {
			camFwd = transform.forward;
			camFwd.y = 0;
			camFwd.Normalize ();

			eulerCam = transform.eulerAngles;

			if (y2 > 0f && y2 < 180f) {
				this.RotateAround (m_AveragePosition, Vector3.up, y2 / 15);
			} else {
				this.RotateAround (m_AveragePosition, Vector3.up, -(360 - y2) / 15);
			}

			y1 = Vector3.Angle (dir, camFwd);
			y2 = eulerDir.y - eulerCam.y;
			y2 = (y2 + 360) % 360;
		}
	}

	private float FindRequiredSize ()
	{
		// Find the position the camera rig is moving towards in its local space.
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_AveragePosition);

		// Start the camera's size calculation at zero.
		float size = 0f;

		// Go through all the targets...
		foreach (GameObject a in trackers)
		{
			// ... and if they aren't active continue on to the next target.
			if (!a.activeSelf)
				continue;

			// Otherwise, find the position of the target in the camera's local space.
			Vector3 targetLocalPos = transform.InverseTransformPoint(a.transform.position);

			// Find the position of the target from the desired position of the camera's local space.
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			// Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

			// Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
		}

		// Add the edge buffer to the size.
		size += m_ScreenEdgeBuffer;

		// Make sure the camera's size isn't below the minimum.
		//size = Mathf.Max (size, m_MinSize);

		return size;
	}


	public void SetStartPositionAndSize ()
	{
		// Find the desired position.
		FindAveragePosition ();

		// Set the camera's position to the desired position without damping.
		transform.position = m_AveragePosition;

		// Find and set the required size of the camera.
		m_Camera.orthographicSize = FindRequiredSize ();
	}

	void RotateAround(Vector3 center, Vector3 axis, float angle) {
		Vector3 pos = desiredPosition;
		Quaternion rot = Quaternion.AngleAxis (angle, axis); //desired rotation
		Vector3 dir = pos - center;
		dir = rot * dir;
		dir.y = 0;
		desiredPosition = center +  dir;
		Quaternion newRot = transform.rotation;
		gameObject.transform.rotation *= Quaternion.Inverse (newRot) * rot * newRot;
	}

}

[System.Serializable]
public class UICamera { 
	public Camera UICam;
	public Canvas canvas;
    //public OffScreenIndicator osi;
    //public OffScreenIndicatorManagerCanvas osimc;
	// TODO canvas elements

}
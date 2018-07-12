using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public float OpenTime = 1;
	private float closeTime = 1;

	public float yOffset = 1;

	private Vector3 DownPosition;
	private Vector3 UpPosition;

	private bool isMoving = false;

	// Use this for initialization
	void Start () {
	
		DownPosition = transform.position;

		UpPosition = DownPosition + (Vector3.up * yOffset);

		closeTime = OpenTime / 3.0f;

	}

	public void ToggleDoor(bool open)
	{

		if (isMoving == true) {
			StopAllCoroutines ();
		}

		if (open == true) {
			StartCoroutine (MoveDoor (UpPosition, OpenTime));
		} else {
			StartCoroutine (MoveDoor (DownPosition, closeTime));
		}
	}


	IEnumerator MoveDoor(Vector3 newPosition, float moveTime)
	{
		float t = 0;

		Vector3 currentPosition = transform.position;

		isMoving = true;


		while (t < moveTime) {

			t += Time.smoothDeltaTime;

			transform.position = Vector3.Slerp (currentPosition, newPosition, t / moveTime);

			yield return new WaitForFixedUpdate ();

		}

		isMoving = false;


	}
	

}

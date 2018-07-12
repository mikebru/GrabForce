using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour {

	public float MassRequired = 1;

	public float Yoffset;

	public UnityEvent OnPressedEvent;
	public UnityEvent OnReleaseEvent;

	private Rigidbody currentBody;
	private Vector3 UpTransform;
	private Vector3 DownTransform;

	private bool isMoving = false;

	// Use this for initialization
	void Start () {
		UpTransform = this.transform.position;
		DownTransform = new Vector3 (UpTransform.x, UpTransform.y - Yoffset, UpTransform.z);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Rigidbody> () != null) {
			
			if (other.gameObject.GetComponent<Rigidbody> ().mass >= MassRequired) {

				currentBody = other.gameObject.GetComponent<Rigidbody> ();
				OnPressedEvent.Invoke ();

				StartCoroutine (MovePlate (DownTransform));
			}
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (currentBody != null && other.gameObject.GetComponent<Rigidbody> () == currentBody) {
			OnReleaseEvent.Invoke ();
			StartCoroutine (MovePlate (UpTransform));
		}
	}

	IEnumerator MovePlate(Vector3 newPosition)
	{
		float t = 0;
		isMoving = true;

		Vector3 currentPosition = transform.position;

		while (t < .5f) {
			t += Time.smoothDeltaTime;

			transform.position = Vector3.Lerp (currentPosition, newPosition, t / .5f);

			yield return new WaitForFixedUpdate ();

		}

		isMoving = false;

	}
}

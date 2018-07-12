using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ForceLineGrab : MonoBehaviour {

	private SpringJoint spring;

	public Rigidbody ConnectedBody;

	public Transform Player;

	private int curveCount = 1;	
	private int layerOrder = 0;
	private int SEGMENT_COUNT = 50;

	private LineRenderer lineRenderer;

	public Vector3 startPosition {get; set;}
	public Vector3 midPosition {get; set;}
	public Vector3 endPosition {get; set;}

	private bool LineDrawn { get; set;}

	// Use this for initialization
	void Start () {

		if (!lineRenderer)
		{
			lineRenderer = GetComponent<LineRenderer>();
		}


		spring = GetComponent<SpringJoint> ();

	}

	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);


		//if we click when we have an object, drop it 
		if (Input.GetMouseButtonDown (0) && ConnectedBody != null) {
			ConnectedBody.useGravity = true;
			spring.connectedBody = null;
			ConnectedBody = null;
			ToggleLine (false);

			return;
		}

		//check for what we are clicking on
		if (Physics.Raycast (ray, out hit)) {

			if (hit.transform.tag == "Ground") {
				transform.position = new Vector3 (hit.point.x, hit.point.y + 1.5f, hit.point.z);
			}
			else if (hit.transform.tag == "Object") {

				if (Input.GetMouseButtonDown (0) && ConnectedBody == null) {
					ConnectedBody = hit.transform.gameObject.GetComponent<Rigidbody> ();

					ToggleLine (true);
				}
			}
		}

		//continually draw the line
		if (ConnectedBody != null) {
			SetDrawValues (Player.position, this.transform.position, ConnectedBody.transform.position);
			DrawCurve ();
		}
	}


	public void ToggleLine(bool isOn)
	{
		if (isOn == true) {
			lineRenderer.enabled = isOn;

			StartCoroutine (AnimateLine (isOn));
		} else {
			LineDrawn = false;
			lineRenderer.enabled = false;
			//StartCoroutine (AnimateOut ());
		}
	}


	void ActivateGrab()
	{
		ConnectedBody.useGravity = false;
		spring.connectedBody = ConnectedBody;

		spring.massScale = ConnectedBody.mass;
	}


	public void SetDrawValues(Vector3 start, Vector3 mid, Vector3 end)
	{
		startPosition = start;
		midPosition = mid;
		endPosition = end;
	}


	IEnumerator AnimateOut()
	{
		for (int j = 0; j <curveCount; j++)
		{
			for (int i = 1; i < SEGMENT_COUNT; i++)
			{
				float t = i / (float)SEGMENT_COUNT;
				int nodeIndex = j * 3;

				lineRenderer.SetPosition (SEGMENT_COUNT - (i), lineRenderer.GetPosition (SEGMENT_COUNT - (i - 1)));
				yield return null;
			}
		}

		lineRenderer.enabled = false;
	}


	IEnumerator AnimateLine(bool isOn)
	{
		for (int j = 0; j <curveCount; j++)
		{
			for (int i = 1; i < SEGMENT_COUNT; i++)
			{
				float t = i / (float)SEGMENT_COUNT;
				int nodeIndex = j * 3;

				Vector3 pixel = CalculateQuadBezierPoint(t, startPosition, midPosition, endPosition);
				lineRenderer.SetVertexCount(((j * SEGMENT_COUNT) + i));
				lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);

				yield return null;
			}
		}

		if (isOn == true) {
			ActivateGrab ();
			LineDrawn = true;
		}
	}


	public void DrawCurve()
	{
		if (LineDrawn == true) {

			for (int j = 0; j < curveCount; j++) {
				for (int i = 1; i < SEGMENT_COUNT; i++) {
					float t = i / (float)SEGMENT_COUNT;
					int nodeIndex = j * 3;

					Vector3 pixel = CalculateQuadBezierPoint (t, startPosition, midPosition, endPosition);
					lineRenderer.SetVertexCount (((j * SEGMENT_COUNT) + i));
					lineRenderer.SetPosition ((j * SEGMENT_COUNT) + (i - 1), pixel);
				}
			}
		}
	}

	public Vector3 CalculateQuadBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;

		Vector3 p = uu * p0; 
		p += 3 * u * t * p1; 
		p += tt * p2; 

		return p;
	}



}

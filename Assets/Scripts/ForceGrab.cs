using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGrab : MonoBehaviour {


	private SpringJoint spring;

	public Rigidbody ConnectedBody;

	public LineDraw lineRender;
	public Transform Player;

	// Use this for initialization
	void Start () {

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
			lineRender.ToggleLine (false);

			return;
		}

		//check for what we are clicking on
		if (Physics.Raycast (ray, out hit)) {

			if (hit.transform.tag == "Ground") {
				transform.position = new Vector3 (hit.point.x, hit.point.y + 2.0f, hit.point.z);
			}
			else if (hit.transform.tag == "Object") {

				if (Input.GetMouseButtonDown (0) && ConnectedBody == null) {
					ConnectedBody = hit.transform.gameObject.GetComponent<Rigidbody> ();
					ConnectedBody.useGravity = false;
					spring.connectedBody = ConnectedBody;

					spring.massScale = ConnectedBody.mass;

					lineRender.ToggleLine (true);
				}
			}
		}

		//continually draw the line
		if (ConnectedBody != null && lineRender != null) {
			lineRender.SetDrawValues (Player.position, this.transform.position, ConnectedBody.transform.position);
			lineRender.DrawCurve ();
		}


	}






}

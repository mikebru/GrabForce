using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusGrab: MonoBehaviour {


	public SpringJoint spring;

	public LineDraw lineRender;

	public Rigidbody ConnectedBody { get; set;}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	
		//move spring farther and cloaser to the player
		if (OVRInput.Get(OVRInput.Button.DpadUp))
		{
			spring.gameObject.transform.position = spring.gameObject.transform.position + (transform.forward * .1f);
		}

		if (OVRInput.Get(OVRInput.Button.DpadDown))
		{
			spring.gameObject.transform.position = spring.gameObject.transform.position - (transform.forward * .1f);
		}


		if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
		{
			//if we click when we have an object, drop it 
			if (ConnectedBody != null) {
				ConnectedBody.useGravity = true;
				spring.connectedBody = null;
				ConnectedBody = null;
				lineRender.ToggleLine (false);

				return;
			}

			RaycastHit hit;
			Ray ray = new Ray (transform.position, transform.forward);

			//check for what we are clicking on
			if (Physics.Raycast (ray, out hit)) {
				
				if (hit.transform.tag == "Object" && ConnectedBody == null) {
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
			lineRender.SetDrawValues (this.transform.position, spring.transform.position, ConnectedBody.transform.position);
			lineRender.DrawCurve ();
		}
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGrab : MonoBehaviour {

    public Transform cursorObject;

    public GameObject ConnectedBody;

    public GameObject lineRender;

    private Vector3 lastCursorPosition;

    public float MoveSpeed= 1;

    [Range(2,4)]
    public int GridSize = 2;

    private float groundLevel;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if we click when we have an object, drop it 
        if (Input.GetMouseButtonDown(0) && ConnectedBody != null)
        {
            //release object and place it
            ConnectedBody = null;
            lineRender.SetActive(false);

            return;
        }

        //check for what we are clicking on
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Ground")
            {
                groundLevel = hit.transform.position.y + .2f;

                //Move cursorObject position, Only by fixed movement amounts
                cursorObject.position = new Vector3((int)hit.point.x + ((int)hit.point.x % 2), (int)hit.point.y, (int)hit.point.z + ((int)hit.point.z % 2));

            }

            if (hit.transform.tag == "Object")
            {
                if (Input.GetMouseButtonDown(0) && ConnectedBody == null)
                {
                    ConnectedBody = hit.transform.gameObject;
                    lineRender.SetActive(true);
                }
            }
        }



        //if we have moved to a new place, move the connected object
        if (cursorObject.position != lastCursorPosition && ConnectedBody != null)
        {
            //move connected object to this position
            StopCoroutine("MoveConnectedBody");

            Vector3 moveLocation = new Vector3(cursorObject.position.x, ConnectedBody.transform.position.y + groundLevel, cursorObject.position.z);

            StartCoroutine(MoveConnectedBody(moveLocation, MoveSpeed - ConnectedBody.GetComponent<Rigidbody>().mass / 5.0f));

            lastCursorPosition = cursorObject.position;
        }

    }

    IEnumerator MoveConnectedBody(Vector3 newPosition, float moveSpeed)
    {
        float moveTime;
        float t = 0;

        Vector3 currentPosition = ConnectedBody.transform.position;

        float distance = Vector3.Distance(currentPosition, newPosition);

        moveTime = distance/moveSpeed;

        while (t < moveTime )
         {
            if (ConnectedBody == null)
            {
                t = moveTime;

                yield return new WaitForFixedUpdate();
            }
            else
            {
                t += Time.smoothDeltaTime;

                Vector3 currentHeading = (newPosition - currentPosition).normalized;

                //potentially move with velocity
                //ConnectedBody.GetComponent<Rigidbody>().velocity = currentHeading * moveSpeed * Mathf.Sin((Mathf.PI / 2) * (t)/moveTime);

                ConnectedBody.transform.position = Vector3.Lerp(currentPosition, newPosition, Mathf.Sin((Mathf.PI / 2) * (t) / moveTime));

                yield return new WaitForFixedUpdate();
            }

         }
    }
}

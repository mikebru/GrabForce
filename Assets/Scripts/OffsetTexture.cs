using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetTexture : MonoBehaviour {

	public Renderer render;
	public Vector2 Offset;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		render.material.mainTextureOffset = new Vector2 (render.material.mainTextureOffset.x + Offset.x, render.material.mainTextureOffset.y + Offset.y);


	}
}

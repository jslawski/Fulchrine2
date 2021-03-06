﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public static CameraFollow instance;

	private Camera camera;
	private GameObject pointOfInterest;
	private Transform cameraTransform;
	private float cameraDistance;

	void Awake()
	{
		CameraFollow.instance = this;
		this.camera = this.gameObject.GetComponent<Camera>();
		this.cameraTransform = this.gameObject.transform;
		this.cameraDistance = this.cameraTransform.position.y;
	}
	
	void LateUpdate () 
	{
		if (this.pointOfInterest != null)
		{

			this.UpdateCameraPosition();
		}
	}

	public void SetPointOfInterest(GameObject pointOfInterest)
	{
		this.pointOfInterest = pointOfInterest;	
	}

	private void UpdateCameraPosition()
	{
		Vector3 worldSpaceCenterOfScreen = this.camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, this.cameraDistance));
		Vector3 shiftVector = this.pointOfInterest.transform.position - worldSpaceCenterOfScreen;
		this.cameraTransform.position = this.cameraTransform.position + shiftVector;
	}
}

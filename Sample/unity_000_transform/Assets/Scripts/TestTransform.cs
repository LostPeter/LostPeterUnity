using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TestTransform : MonoBehaviour
{
    public Vector3 vCameraPos = new Vector3(0.0f, 2.0f, -1.0f);
    public Vector3 vCameraLookAt = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 vCameraUp = new Vector3(0.0f, 1.0f, 0.0f);
    
    public Camera camera;


	void Awake()
	{
		this.camera = GetComponent<Camera>();
		if (this.camera == null)
		{
			Debug.LogError("TestTransform.Awake: Can not get Camera component !");
			return;
		}
		transform.localPosition = this.vCameraPos;
		transform.LookAt(this.vCameraLookAt, this.vCameraUp);

		Matrix4x4 matView = this.camera.worldToCameraMatrix;
		logMatrix44("Camera View: ", matView);

		Matrix4x4 matProjection = this.camera.projectionMatrix;
		logMatrix44("Camera Projection: ", matProjection);

        Matrix4x4 matViewProjection = matProjection * matView;
		logMatrix44("Camera View Projection: ", matViewProjection);
    }
    private void logMatrix44(string name, Matrix4x4 mat44)
    {
        Debug.Log(name + ": [" +
                  mat44.m00 + " " + mat44.m01 + " " + mat44.m02 + " " + mat44.m03 + "] [" +
                  mat44.m10 + " " + mat44.m11 + " " + mat44.m12 + " " + mat44.m13 + "] [" +
                  mat44.m20 + " " + mat44.m21 + " " + mat44.m22 + " " + mat44.m23 + "] [" +
                  mat44.m30 + " " + mat44.m31 + " " + mat44.m32 + " " + mat44.m33 + "]");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
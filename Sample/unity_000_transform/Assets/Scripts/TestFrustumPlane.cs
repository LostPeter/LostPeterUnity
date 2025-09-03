using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TestFrustumPlane : MonoBehaviour
{
    private static GameObject CreateCube(Vector3 cubePos, Color color, Vector3 scale)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = cubePos;
        cube.transform.localScale = scale;
        cube.GetComponent<Renderer>().sharedMaterial.color = color;

        return cube;
    }

    void calculateProjectionParameters(Camera cameraMain, ref float fLeft, ref float fRight, ref float fTop, ref float fBottom)
	{
        float rad = cameraMain.fieldOfView;
        

        float tanThetaY = Mathf.Tan(Mathf.Deg2Rad *rad * 0.5f);
        float tanThetaX = tanThetaY * cameraMain.aspect;

        float half_w = tanThetaX * cameraMain.nearClipPlane;
        float half_h = tanThetaY * cameraMain.nearClipPlane;

        fLeft = -half_w;
        fRight = +half_w;
        fTop = +half_h;
        fBottom = -half_h;
    }

    void calculteCorners(Camera cameraMain)
	{
        float fNearLeft = 0.0f;
        float fNearRight = 0.0f;
        float fNearTop = 0.0f;
        float fNearBottom = 0.0f;
        calculateProjectionParameters(cameraMain, ref fNearLeft, ref fNearRight, ref fNearTop, ref fNearBottom);

        float fNearZ = cameraMain.nearClipPlane;
        float fFarZ = (cameraMain.farClipPlane == 0) ? 100000 : cameraMain.farClipPlane;

        //Calculate far palne corners
        float fRadio = fFarZ / fNearZ;
        float fFarLeft = fNearLeft * fRadio;
        float fFarRight = fNearRight * fRadio;
        float fFarTop = fNearTop * fRadio;
        float fFarBottom = fNearBottom * fRadio;

        Vector4 v0 = cameraMain.cameraToWorldMatrix * new Vector4(fNearRight, fNearTop, -fNearZ, 1.0f);
        Vector4 v1 = cameraMain.cameraToWorldMatrix * new Vector4(fNearLeft, fNearTop, -fNearZ, 1.0f);
        Vector4 v2 = cameraMain.cameraToWorldMatrix * new Vector4(fNearLeft, fNearBottom, -fNearZ, 1.0f);
        Vector4 v3 = cameraMain.cameraToWorldMatrix * new Vector4(fNearRight, fNearBottom, -fNearZ, 1.0f);

        Vector4 v4 = cameraMain.cameraToWorldMatrix * new Vector4(fFarRight, fFarTop, -fFarZ, 1.0f);
        Vector4 v5 = cameraMain.cameraToWorldMatrix * new Vector4(fFarLeft, fFarTop, -fFarZ, 1.0f);
        Vector4 v6 = cameraMain.cameraToWorldMatrix * new Vector4(fFarLeft, fFarBottom, -fFarZ, 1.0f);
        Vector4 v7 = cameraMain.cameraToWorldMatrix * new Vector4(fFarRight, fFarBottom, -fFarZ, 1.0f);

        Vector3 vScale = new Vector3(0.01f, 0.01f, 0.01f);
		CreateCube(v0, Color.red, vScale);
		CreateCube(v1, Color.red, vScale);
		CreateCube(v2, Color.red, vScale);
		CreateCube(v3, Color.red, vScale);

        vScale = Vector3.one;
        CreateCube(v4, Color.green, vScale);
		CreateCube(v5, Color.green, vScale);
		CreateCube(v6, Color.green, vScale);
		CreateCube(v7, Color.green, vScale);

		Debug.Log("v0: " + v0);
        Debug.Log("v1: " + v1);
        Debug.Log("v2: " + v2);
        Debug.Log("v3: " + v3);

        Debug.Log("v4: " + v4);
        Debug.Log("v5: " + v5);
        Debug.Log("v6: " + v6);
        Debug.Log("v7: " + v7);
    }


    enum FFrustumPlaneType
    {
        F_FrustumPlane_Left = 0,                        //0: Left
        F_FrustumPlane_Right,                           //1: Right
        F_FrustumPlane_Down,                            //2: Down
        F_FrustumPlane_Up,                              //3: Up
        F_FrustumPlane_Near,                            //4: Near
        F_FrustumPlane_Far,                             //5: Far
        
        F_FrustumPlane_Count
    };

    void GetWorldFrustumPlanes(Camera cameraMain, Plane[] aWorldFrustumPlanes)
    {
        Matrix4x4 matVP = cameraMain.projectionMatrix * cameraMain.worldToCameraMatrix;

        //F_FrustumPlane_Left/F_FrustumPlane_Right
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Left].normal = new Vector3(matVP.m30 + matVP.m00,
                                                                                             matVP.m31 + matVP.m01,
                                                                                             matVP.m32 + matVP.m02);
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Left].distance = matVP.m33 + matVP.m03;

        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Right].normal = new Vector3(matVP.m30 - matVP.m00,
                                                                                              matVP.m31 - matVP.m01,
                                                                                              matVP.m32 - matVP.m02);
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Right].distance = matVP.m33 - matVP.m03;

        //F_FrustumPlane_Down/F_FrustumPlane_Up
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Down].normal = new Vector3(matVP.m30 + matVP.m10,
                                                                                             matVP.m31 + matVP.m11,
                                                                                             matVP.m32 + matVP.m12);
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Down].distance = matVP.m33 + matVP.m13;

        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Up].normal = new Vector3(matVP.m30 - matVP.m10,
                                                                                           matVP.m31 - matVP.m11,
                                                                                           matVP.m32 - matVP.m12);
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Up].distance = matVP.m33 - matVP.m13;

        //F_FrustumPlane_Near/F_FrustumPlane_Far
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Near].normal = new Vector3(matVP.m30 + matVP.m20,
                                                                                             matVP.m31 + matVP.m21,
                                                                                             matVP.m32 + matVP.m22);
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Near].distance = matVP.m33 + matVP.m23;

        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Far].normal = new Vector3(matVP.m30 - matVP.m20,
                                                                                            matVP.m31 - matVP.m21,
                                                                                            matVP.m32 - matVP.m22);
        aWorldFrustumPlanes[(int)FFrustumPlaneType.F_FrustumPlane_Far].distance = matVP.m33 - matVP.m23;


        for (int i = 0; i < 6; i++)
        {
            float length = aWorldFrustumPlanes[i].normal.magnitude;
            aWorldFrustumPlanes[i].distance /= length;
            aWorldFrustumPlanes[i].normal = Vector3.Normalize(aWorldFrustumPlanes[i].normal);

            FFrustumPlaneType type = (FFrustumPlaneType)i;
            Debug.Log("Type: " + type + " - " + aWorldFrustumPlanes[i]);
        }
    }
    
    void Start()
    {
        Camera cameraMain = GetComponent<Camera>();

        Debug.Log("worldToCameraMatrix: ");
        Debug.Log(cameraMain.worldToCameraMatrix);
        Debug.Log(cameraMain.worldToCameraMatrix.transpose);

        Debug.Log("cameraToWorldMatrix: ");
        Debug.Log(cameraMain.cameraToWorldMatrix);
        Debug.Log(cameraMain.cameraToWorldMatrix.transpose);

        calculteCorners(cameraMain);

        Plane[] aWorldFrustumPlanes = new Plane[(int)FFrustumPlaneType.F_FrustumPlane_Count]
        {
            new Plane(), 
            new Plane(),
            new Plane(),
            new Plane(),
            new Plane(),
            new Plane(),
        };
        GetWorldFrustumPlanes(cameraMain, aWorldFrustumPlanes);

        Plane[] aPlanes = GeometryUtility.CalculateFrustumPlanes(cameraMain);
        for (int i = 0; i < aPlanes.Length; i++)
		{
            Plane plane = aPlanes[i];
            Debug.Log(plane);
        }
    }

    void Update()
    {
        
    }
}

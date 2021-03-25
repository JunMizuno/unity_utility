using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject targetObject;

    public void Start()
    {
        if (mainCamera)
        {
            if (targetObject)
            {
                mainCamera.transform.LookAt(targetObject.transform);
            }
        }
    }

    public void Update()
    {
        if (!CheckCameraObject())
        {
            Debug.Log("<color=red>" + "Error:Camera not found" + "</color>");
            return;
        }

        float x = Mathf.Sin(Time.time * 0.5f) * 4.0f;
        float y = Mathf.Cos(Time.time * 0.2f) * 1.0f;
        float z = Mathf.Cos(Time.time * 0.5f) * -4.0f;
        var pos = mainCamera.gameObject.transform.position;
        pos = new Vector3(x, y, z);
        mainCamera.gameObject.transform.position = pos;
        mainCamera.transform.LookAt(targetObject.transform);
    }

    // カメラでオブジェクトなどを追従するときに使用する。(他オブジェクトのUpfate()の最後に処理される。)
    public void LateUpdate()
    {
        
    }

    public bool CheckCameraObject()
    {
        return (mainCamera != null);
    }

    public bool CheckTargetObject()
    {
        return (targetObject != null);
    }

    public void SetCameraPos(Vector3 pos)
    {
        mainCamera.gameObject.transform.position = pos;
    }

    public void MoveCameraPos(Vector3 vec)
    {
        var pos = mainCamera.gameObject.transform.position;
        pos += vec;
        mainCamera.gameObject.transform.position = pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 5;
        mainCamera.orthographicSize -= scroll;
        mainCamera.orthographicSize = Mathf.Max(0, mainCamera.orthographicSize);
    }
}

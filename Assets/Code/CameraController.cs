using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    private void Awake() => cam = GetComponent<Camera>();

    private void Update()
    {
        var movementSpeed = 0.025f / (cam.orthographicSize + 1.0f);

        var p = transform.position;
        var x = p.x + movementSpeed * Input.GetAxis("Horizontal");
        var y = p.y + movementSpeed * Input.GetAxis("Vertical");

        transform.position = new Vector3(
            Mathf.Clamp(x, -4.0f,4.0f),
            Mathf.Clamp(y, -4.0f,4.0f), 
            -10.0f);
        
        cam.orthographicSize = Math.Clamp(cam.orthographicSize - 0.1f * Input.mouseScrollDelta.y, 0.1f, 5.0f);
    }
}
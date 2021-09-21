using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public float orthoSizeMin, orthoSizeMax, zoomRate, zoomSpeed;
    public float currentCamSize;

    private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minX, maxX, minY, maxY;

    private void Awake()
    {
        cam = this.GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        currentCamSize = cam.orthographicSize;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), smoothSpeed * Time.deltaTime);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX), 
            Mathf.Clamp(transform.position.y, minY, maxY), 
            transform.position.z);


        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentCamSize += zoomRate;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentCamSize -= zoomRate;
        }
        currentCamSize = Mathf.Clamp(currentCamSize, orthoSizeMin, orthoSizeMax);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, currentCamSize, Time.deltaTime * zoomSpeed);

    }
}

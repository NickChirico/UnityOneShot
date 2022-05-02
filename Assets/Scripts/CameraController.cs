using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public float orthoSizeMin, orthoSizeMax, zoomRate, zoomSpeed;
    public float currentCamSize;

    public Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minX, maxX, minY, maxY;

    private void Awake()
    {
        cam = this.GetComponent<Camera>();
        if (currentCamSize <= 1)
            currentCamSize = cam.orthographicSize;
        else if (currentCamSize > 1)
            cam.orthographicSize = currentCamSize;
        cam.backgroundColor = GameObject.Find("Path Manager").GetComponent<PathManager>().myBackgroundColor;
    }

    private void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    private void LateUpdate()
    {
        if (target != null && !jumping)
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

        if (jumping)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    bool jumping;
    public void JumpToPlayer()
    {
        if (target != null)
        {
            StartCoroutine(JumpScreen());
            //UI_Manager.GetUIManager.ScreenTransition();
        }

    }

    IEnumerator JumpScreen()
    {
        jumping = true;
        yield return new WaitForSeconds(0.15f);
        jumping = false;
    }
}

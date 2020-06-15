using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothSpeed;

    [SerializeField]
    private float magnitude;

    [SerializeField]
    private float duration;
    private bool cameraShaking;
    private float elapsed;

    Vector3 originalPosition;

    void Start()
    {
        cameraShaking = false;
        elapsed = 0f;
        originalPosition = transform.position;
    }

    private void LateUpdate()
    {
        CameraPositionUpdate();
        /*transform.position = original_position;

        if (Input.GetKeyDown("space") && !camera_shaking)
            camera_shaking = true;

        if (camera_shaking && elapsed < duration)
        {
            original_position = transform.position;
            CameraShaking();
        }
        else
        {
            camera_shaking = false;
            elapsed = 0f;
            transform.position = original_position;
        }*/
            
    }

    private void CameraPositionUpdate()
    {
        Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);

        Vector3 smoothPostion = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothPostion;

        transform.LookAt(target);
    }

    private void CameraShaking()
    {
        Vector3 originalPosition = transform.localPosition;

        float x = Random.Range(-1f, 1f) * magnitude + transform.localPosition.x;
        float y = Random.Range(-1f, 1f) * magnitude + transform.localPosition.y;

        transform.position = new Vector3(x, y , originalPosition.z);

        elapsed += Time.deltaTime;
    }
}

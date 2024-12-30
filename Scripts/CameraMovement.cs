using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private const float minBoundX = -27, maxBoundX = 25, minBoundY = -12, maxBoundY = 13;
    float minX, maxX, minY, maxY;

    private void Start()
    {
        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;
        minX = minBoundX + cameraHalfWidth;
        maxX = maxBoundX - cameraHalfWidth;
        minY = minBoundY + cameraHalfHeight;
        maxY = maxBoundY - cameraHalfHeight;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new(moveX, moveY, 0);
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
        transform.position = new(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), -10);
    }
}

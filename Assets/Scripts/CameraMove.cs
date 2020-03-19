using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0f;
    [SerializeField] float minZoomIn = 100f;
    [SerializeField] float maxZoomIn = 600f;

    const float fovChangeSpeed = 1;
    Bounds cameraBound;
    float minX;
    float maxX;
    float minZ;
    float maxZ;

    private void Start()
    {
        Application.targetFrameRate = 58;
        cameraBound = GameObject.Find("CameraBound").GetComponent<BoxCollider>().bounds;
        SizeChanged();
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.tapCount > 0)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        break;
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Ended:
                        break;
                }
            }
        }
        else
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            Vector3 moveDir = new Vector3(h, v, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(moveDir);

            if (Input.GetKey(KeyCode.Q))
            {
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - fovChangeSpeed * Time.deltaTime,
                    minZoomIn,
                    maxZoomIn);
                SizeChanged();
            }

            if (Input.GetKey(KeyCode.E))
            {
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + fovChangeSpeed * Time.deltaTime,
                    minZoomIn,
                    maxZoomIn);
                SizeChanged();
            }
        }

        Vector3 pos = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
            10f,
            Mathf.Clamp(transform.position.z, minZ, maxZ));

        transform.position = pos;
    }

    void SizeChanged()
    {
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Camera.main.aspect;

        minX = cameraBound.min.x + horzExtent;
        maxX = cameraBound.max.x - horzExtent;
        minZ = cameraBound.min.z + vertExtent;
        maxZ = cameraBound.max.z - vertExtent;
    }
}

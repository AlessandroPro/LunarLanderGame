using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

// Cotains functions that allow the camera to zoom in and focus on an object
public class CameraController : MonoBehaviour
{
    public GameObject lunarLander;
    public bool focus;
    public float moveSpeed;
    public float zoomSpeed;
    public float zoomLimit;

    private Vector3 defaultPos;
    private float defaultSize;

    // Start is called before the first frame update
    void Start()
    {
        focus = false;

        defaultPos = transform.position;
        defaultSize = GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(focus)
        {
            UpdateFocus();
        }
    }

    // Zooms in and increases the orthographic camera size for one frame
    private void UpdateFocus()
    {
        float size = GetComponent<Camera>().orthographicSize;
        float sizeDiff = size - zoomLimit;

        Vector3 landerOffset = lunarLander.transform.position - transform.position;
        landerOffset.Set(landerOffset.x, landerOffset.y, 0f);

        float time = sizeDiff / zoomSpeed;
        if(time > 0)
        {
            moveSpeed = landerOffset.magnitude / time;
        }
        

        if (size > zoomLimit)
        {
            GetComponent<Camera>().orthographicSize -= zoomSpeed * Time.deltaTime;
        }


        if(landerOffset.magnitude < moveSpeed * Time.deltaTime)
        {
            transform.position += landerOffset;
        }
        else
        {
            landerOffset.Normalize();
            transform.position += landerOffset * moveSpeed * Time.deltaTime;
        }
    }

    public void StartFocusing()
    {
        focus = true;
    }

    // Sets the camera back to it's original position and orthgraphic size
    public void ResetFocus()
    {
        focus = false;
        transform.position = defaultPos;
        GetComponent<Camera>().orthographicSize = defaultSize;
    }
}

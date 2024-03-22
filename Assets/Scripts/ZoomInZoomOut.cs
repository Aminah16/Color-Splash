using UnityEngine;

public class ZoomInZoomOut : MonoBehaviour
{
    Vector3 touchStart;

    public float zoomOutMin = 1f;
    public float zoomOutMax = 2f;
    public float panSpeed = 0.5f;
    public Rect panBounds;
    public bool isZoomingOrPanning = false;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            HandlePinchZoom();
            isZoomingOrPanning = true; // Set flag to true when zooming
        }
        else if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isZoomingOrPanning = false; // Reset flag when panning starts
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - touchStart;
            Vector3 newPosition = gameObject.transform.position + direction * panSpeed * gameObject.transform.localScale.x * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, panBounds.xMin, panBounds.xMax);
            newPosition.y = Mathf.Clamp(newPosition.y, panBounds.yMin, panBounds.yMax);

            gameObject.transform.position = newPosition;

            isZoomingOrPanning = true; // Set flag to true when panning
        }
        else
        {
            isZoomingOrPanning = false; // Reset flag when neither zooming nor panning
        }

        HandleZoom();
    }

    void HandleZoom()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        float factor = Mathf.Clamp(gameObject.transform.localScale.x + zoomInput, zoomOutMin, zoomOutMax);
        gameObject.transform.localScale = new Vector3(factor, factor, 0);
    }

    void HandlePinchZoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        Zoom(difference * 0.01f);
    }

    void Zoom(float increment)
    {
        float factor = Mathf.Clamp(gameObject.transform.localScale.x + increment, zoomOutMin, zoomOutMax);
        gameObject.transform.localScale = new Vector3(factor, factor, 0);
    }
}
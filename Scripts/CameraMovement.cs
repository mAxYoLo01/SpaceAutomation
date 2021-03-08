using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 touchStart;
    public float minimumZoom;
    public float maximumZoom;
    public bool gamePaused = false;
    public bool draggingBlock = false;
    public bool inMenu = false;
    public GameObject selectedObject;

    public static CameraMovement instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of CameraMovement found!");
            return;
        }
        instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
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
        else if (Input.GetMouseButton(0) && !draggingBlock && !inMenu)
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float cameraX = Mathf.Clamp(Camera.main.transform.position.x + direction.x, 0, MapGenerator.instance.width);
            float cameraY = Mathf.Clamp(Camera.main.transform.position.y + direction.y, 0, MapGenerator.instance.height);
            Camera.main.transform.position = new Vector3(cameraX, cameraY, Camera.main.transform.position.z);
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, minimumZoom, maximumZoom);
    }
}
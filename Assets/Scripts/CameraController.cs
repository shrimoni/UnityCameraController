using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target; // Target transform for the camera to rotate
    [SerializeField] float speed; // Speed of the camera around player

    [SerializeField] float pitch;
    [SerializeField] float yaw;

    float hDistance; //Horizontal Distance(i.e.Adjacent Side)
    float vDistance; //Vertical Distance(i.e.Opposite Side)

    [SerializeField] private float distanceFromPlayer; //Basically, radius
    [SerializeField] private float angleAroundPlayer;

    private const float cameraPitchClampMin = 0f;
    private const float cameraPitchClampMax = 90f;


    private void Update()
    {
        CalculateZoom();
        CalculatePitch();
        CalculateAngleAroundPlayer();
        float horizontalDistance = CalculateHorizontalDistance();
        float verticalDistance = CalculateVerticalDistance();
        CalculateCameraPosition(horizontalDistance, verticalDistance);
    }

    // Calculates the camera position
    // based on the horizontal distance & vertical distance
    // from the player
    void CalculateCameraPosition(float hDistance, float vDistance)
    {
        // New angle change and players angle gives the total angle changed
        float totalAngleChanged = target.transform.eulerAngles.y + angleAroundPlayer;

        // From top view, we are finding the horizontal distance & vertical distance
        float offsetX = (Mathf.Sin(totalAngleChanged * Mathf.Deg2Rad)) * hDistance;
        float offsetZ = (Mathf.Cos(totalAngleChanged * Mathf.Deg2Rad)) * hDistance;

        var pos = transform.position;
        pos.x = target.transform.position.x - offsetX;
        pos.y = target.transform.position.y + vDistance;
        pos.z = target.transform.position.z - offsetZ;

        yaw = target.transform.eulerAngles.y + angleAroundPlayer; // according to trigonometry
        transform.position = pos;
        transform.eulerAngles = new Vector3(pitch, yaw, transform.eulerAngles.z);
    }

    // Calculates the horizontal distance
    // Since we know the pitch & distance from the player
    // Using trigonometry concept we are getting the horizontal distance
    // This is from front view
    float CalculateHorizontalDistance()
    {
        var distance = (Mathf.Cos(pitch * Mathf.Deg2Rad)) * distanceFromPlayer;
        return distance;
    }

    // Calculates the vertical distance
    // Since we know the pitch & distance from the player
    // Using trigonometry concept we are getting the horizontal distance
    // This is from front view
    float CalculateVerticalDistance()
    {
        var distance = (Mathf.Sin(pitch * Mathf.Deg2Rad)) * distanceFromPlayer;
        return distance;
    }

    // Calculates the zoom level, when you
    // scroll the mouse wheel
    // distance from player decreases / increases based on zoomLevel value
    public void CalculateZoom()
    {
        float zoomLevel = Input.GetAxis("Mouse ScrollWheel") * speed;
        distanceFromPlayer -= zoomLevel;
    }

    // Calculates the pitch of the camera when you
    // press the right mouse button and move up / down
    public void CalculatePitch()
    {
        if (Input.GetMouseButton(0))
        {
            float pitchChange = Input.GetAxis("Mouse Y") * speed;
            pitch -= pitchChange;
            pitch = Mathf.Clamp(pitch, cameraPitchClampMin, cameraPitchClampMax);
        }
    }

    // Calculates the angle change when you press the left mouse button
    // and drag it along the 'X' direction
    // angle change is nothing button how far the camera is rotated
    // from the player.
    public void CalculateAngleAroundPlayer()
    {
        if (Input.GetMouseButton(0))
        {
            float angleChange = Input.GetAxis("Mouse X") * speed;
            angleAroundPlayer += angleChange;
        }
    }
}

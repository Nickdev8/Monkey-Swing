using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public bool slowmo = false;
    public string playerTag = "Player"; // The tag of the player GameObject
    public float minYLevel = -10f; // The minimum Y level to check

    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
    public float m_MaxSize = 8.5f;                  // The smallest orthographic size the camera can be.
    public Transform[] m_Targets; // All the targets the camera needs to encompass.


    private Camera m_Camera;                        // Used for referencing the camera.
    private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 m_DesiredPosition;              // The position the camera is moving towards.


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        // Find the player GameObject with the specified tag
        GameObject player = GameObject.FindWithTag(playerTag);

        if (player != null)
        {
            // Get the player's current position
            Vector3 playerPosition = player.transform.position;

            // Check if the player's Y level is less than or equal to minYLevel
            if (playerPosition.y <= minYLevel)
            {
                // The player is below the specified Y level
                //slowmo = true;
                // You can add your desired actions here, such as game over or teleporting the player.
            }
            else
            {
                //slowmo = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (slowmo == true)
        Time.timeScale = 0.5f;
        if (slowmo == false)
        Time.timeScale = 1f;

        // Move the camera towards a desired position.
        Move();

        // Change the size of the camera based.
        Zoom();
    }


    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        // Smoothly transition to that position.
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindAveragePosition()
    {
        // Find all game objects with the tag "player" and store their transforms in m_Targets array.
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        m_Targets = new Transform[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            m_Targets[i] = players[i].transform;
        }


        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += m_Targets[i].position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        averagePos.y = transform.position.y;

        // The desired position is the average position;
        m_DesiredPosition = new Vector3(averagePos.x, averagePos.y, -10);
    }


    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // ... and if they aren't active continue on to the next target.
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        // Add the edge buffer to the size.
        size += m_ScreenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, m_MinSize);

        // Make sure the camera's size isn't above the maximum.
        size = Mathf.Min(size, m_MaxSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        transform.position = m_DesiredPosition;

        // Find and set the required size of the camera.
        m_Camera.orthographicSize = FindRequiredSize();
    }
}
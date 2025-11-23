using System.Collections.Generic;
using UnityEngine;

public class RotateSwitch : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 3f; // The radius which player can interact with the switch
    [SerializeField] private GameObject interactText;
    [SerializeField] private List<GameObject> platforms;
    [SerializeField] private List<GameObject> platformsColliders;
    [SerializeField] private List<GameObject> switchList;
    [Header("Target to Rotate")]
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 5f;

    private bool isRotating = false;
    private Quaternion targetRotation;
    private void Start()
    {
        interactText.SetActive(false);

        foreach(GameObject col in platformsColliders)
            col.SetActive(false);
    }
    void Update()
    {
        interactText.SetActive(true);
        if (IsNearSwitch())
        {
            // Player presses e to interact
            if (Input.GetKeyDown(KeyCode.E) && !isRotating)
            {
                // Set all platforms to false
                foreach (GameObject obj in platforms)
                    obj.SetActive(false);
                RotateTarget();
            }
        }
        else
        {
            interactText.SetActive(false);
        }

        // Smooth rotation
        if (isRotating)
        {
            // Disable colliders only after rotation finished
            foreach (GameObject obj in platformsColliders)
                obj.SetActive(true);
            // Rotate towards target rotation
            objectToRotate.rotation = Quaternion.RotateTowards(
                objectToRotate.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Check if rotation finished
            if (Quaternion.Angle(objectToRotate.rotation, targetRotation) < 0.01f)
            {
                // Snap exactly to target
                objectToRotate.rotation = targetRotation;
                isRotating = false;

                // Show platforms now that rotation is done
                foreach (GameObject obj in platforms)
                    obj.SetActive(true);

                // Disable colliders only after rotation finished
                foreach (GameObject obj in platformsColliders)
                    obj.SetActive(false);
            }
        }
    }

    void RotateTarget()
    {
        targetRotation = objectToRotate.rotation * Quaternion.Euler(0, rotationAngle, 0);
        isRotating = true;
    }

    bool IsNearSwitch()
    {
        foreach (GameObject s in switchList)
        {
            // Check distance between player and switch position
            float distance = Vector3.Distance(player.position, s.transform.position);

            if (distance <= detectionRadius)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize detection area in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

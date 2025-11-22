using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RotaryDial : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform dialTransform;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float returnSpeed = 5f; // Speed of return animation

    private bool isDragging = false; // Boolean for when dragging the rotary dial
    private Vector2 centerPoint; // Center of the screen space
    private float previousAngle; // The initial angle from which the mouse down position is pressed
    private float currentRotation;
    private float originalRotation;



    void Start()
    {
        if (dialTransform == null)
            dialTransform = GetComponent<RectTransform>();

        // Store the original rotation
        originalRotation = dialTransform.rotation.eulerAngles.z;
        currentRotation = originalRotation;

       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;

        // Stops the coroutine if pointer down on the dial
        StopCoroutine(ReturnToOriginalRotation());

        // Get center point in screen space
        centerPoint = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, dialTransform.position);

        // Calculate initial angle
        Vector2 direction = eventData.position - centerPoint;
        previousAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // Calculate current angle from center to mouse position
        Vector2 direction = eventData.position - centerPoint;
        float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate angle difference
        float angleDelta = Mathf.DeltaAngle(previousAngle, currentAngle);

        // Apply rotation
        currentRotation += angleDelta * rotationSpeed;


        // Update the rotation visually
        dialTransform.rotation = Quaternion.Euler(0, 0, currentRotation);

        // Store current angle for next frame
        previousAngle = currentAngle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // Start coroutine to return to original rotation
        StartCoroutine(ReturnToOriginalRotation());
    }

    private IEnumerator ReturnToOriginalRotation()
    {
        while (Mathf.Abs(Mathf.DeltaAngle(currentRotation, originalRotation)) > 0.1f)
        {
            // Smoothly lerp back to original rotation
            currentRotation = Mathf.LerpAngle(currentRotation, originalRotation, Time.deltaTime * returnSpeed);
            dialTransform.rotation = Quaternion.Euler(0, 0, currentRotation);
            yield return null;
        }

        // Snap to exact original rotation just incase
        currentRotation = originalRotation;
        dialTransform.rotation = Quaternion.Euler(0, 0, currentRotation);
        
    }

    /*
    // Public method to get current rotation value
    public float GetRotation()
    {
        return currentRotation;
    }

    // Public method to set rotation programmatically
    public void SetRotation(float angle)
    {
        currentRotation = angle;
        if (limitRotation)
        {
            currentRotation = Mathf.Clamp(currentRotation, minAngle, maxAngle);
        }
        dialTransform.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
    */
}
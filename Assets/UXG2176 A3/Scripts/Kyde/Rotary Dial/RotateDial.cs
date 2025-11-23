using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class RotaryDial : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform dialTransform;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float returnSpeed = 5f; // Speed of return animation

    private bool isDragging = false;
    private Vector2 centerPoint;
    private float previousAngle;
    private float currentRotation = 0f;
    private float originalRotation = 0f;
    private Coroutine returnCoroutine;
    private Canvas canvas;



    void Start()
    {
        if (dialTransform == null)
            dialTransform = GetComponent<RectTransform>();

        // Store the original rotation
        originalRotation = dialTransform.rotation.eulerAngles.z;
        currentRotation = originalRotation;

        // Get the canvas for proper screen point conversion
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (isDragging)
        {
            UpdateRotation();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;

        // Stop any ongoing return animation
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }

        // Get center point in screen space
        Camera cam = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
        centerPoint = RectTransformUtility.WorldToScreenPoint(cam, dialTransform.position);

        // Calculate initial angle
        Vector2 direction = eventData.position - centerPoint;
        previousAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void UpdateRotation()
    {
        // Get current mouse position
        Vector2 mousePos = Input.mousePosition;

        // Calculate current angle from center to mouse position
        Vector2 direction = mousePos - centerPoint;
        float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate angle difference
        float angleDelta = Mathf.DeltaAngle(previousAngle, currentAngle);

        // Apply rotation
        currentRotation += angleDelta * rotationSpeed;

  

        // Update the rotation visually
        dialTransform.rotation = Quaternion.Euler(0, 0, currentRotation);

        // Store current angle for next frame of the rotation
        previousAngle = currentAngle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // Start returning to original rotation
        returnCoroutine = StartCoroutine(ReturnToOriginalRotation());
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
        returnCoroutine = null;
    }


}
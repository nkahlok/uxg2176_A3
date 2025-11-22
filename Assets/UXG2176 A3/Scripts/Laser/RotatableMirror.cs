using UnityEngine;

public class RotatableMirror : Interactable
{
    [Header("Mirror Settings")]
    [SerializeField] private float rotationAmount = 45f; // How much to rotate per interaction
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Axis to rotate around (Y-axis by default)
    [SerializeField] private float rotationSpeed = 5f; // Speed of rotation animation
    
    private Quaternion targetRotation;
    private bool isRotating = false;

    private void Start()
    {
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        // Smoothly rotate to target rotation
        if (isRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            
            // Check if we're close enough to target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        RotateMirror();
    }

    private void RotateMirror()
    {
        // Calculate new target rotation
        targetRotation *= Quaternion.AngleAxis(rotationAmount, rotationAxis);
        isRotating = true;
    }
    public Vector3 GetReflectionDirection(Vector3 incomingDirection, Vector3 hitPoint)
    {
        Vector3 scale = transform.localScale;
        Vector3 mirrorNormal;
        
        if (scale.x < scale.y && scale.x < scale.z)
        {
            mirrorNormal = transform.right;
        }
        else if (scale.y < scale.x && scale.y < scale.z)
        {
            mirrorNormal = transform.up;
        }
        else
        {
            mirrorNormal = transform.forward;
        }
        
        // Calculate reflection using Vector3.Reflect
        return Vector3.Reflect(incomingDirection, mirrorNormal);
    }

    // Get the mirror's normal for laser calculations
    public Vector3 GetMirrorNormal()
    {
        Vector3 scale = transform.localScale;
        
        if (scale.x < scale.y && scale.x < scale.z)
            return transform.right;
        else if (scale.y < scale.x && scale.y < scale.z)
            return transform.up;
        else
            return transform.forward;
    }
}
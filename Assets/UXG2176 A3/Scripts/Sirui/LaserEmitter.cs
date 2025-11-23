using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private float laserRange = 100f;
    [SerializeField] private int maxReflections = 10; // Maximum number of mirror bounces
    [SerializeField] private LayerMask hitLayers; // What layers the laser can hit
    
    [Header("Direction Settings")]
    [SerializeField] private bool useCustomDirection = false;
    [SerializeField] private Vector3 customDirection = Vector3.forward; // Custom direction if enabled
    
    [Header("Visual Settings")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Color laserColor = Color.red;
    [SerializeField] private float laserWidth = 0.1f;

    private LaserReceiver currentReceiver = null;

    private void Start()
    {
        SetupLineRenderer();
    }

    private void Update()
    {
        UpdateLaser();
    }

    private void SetupLineRenderer()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
        lineRenderer.sortingOrder = 10;
    }

    private void UpdateLaser()
    {
        // Start position and direction
        Vector3 currentPosition = transform.position;
        Vector3 currentDirection = useCustomDirection ? customDirection.normalized : transform.forward;
        
        // Store all laser points for LineRenderer
        int pointCount = 1;
        Vector3[] laserPoints = new Vector3[maxReflections + 2]; // +2 for start and potential end point
        laserPoints[0] = currentPosition;

        bool hitReceiver = false;
        LaserReceiver hitReceiverComponent = null;

        // Trace the laser through reflections
        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(currentPosition, currentDirection, out hit, laserRange, hitLayers))
            {
                // Add hit point
                laserPoints[pointCount] = hit.point;
                pointCount++;

                LaserReceiver receiver = hit.collider.GetComponent<LaserReceiver>();
                if (receiver != null)
                {
                    hitReceiver = true;
                    hitReceiverComponent = receiver;
                    break;
                }

                RotatableMirror mirror = hit.collider.GetComponent<RotatableMirror>();
                if (mirror != null)
                {

                    Vector3 mirrorNormal = mirror.GetMirrorNormal();
                    currentDirection = Vector3.Reflect(currentDirection, mirrorNormal);
                    
                    currentPosition = hit.point + mirrorNormal * 0.02f;
                }
                else
                {
                    break;
                }
            }
            else
            {
                laserPoints[pointCount] = currentPosition + currentDirection * laserRange;
                pointCount++;
                break;
            }
        }

        // Update LineRenderer
        lineRenderer.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            lineRenderer.SetPosition(i, laserPoints[i]);
        }

        // Notify receivers
        if (hitReceiver && hitReceiverComponent != null)
        {
            if (currentReceiver != hitReceiverComponent)
            {
                // New receiver hit
                if (currentReceiver != null)
                {
                    currentReceiver.OnLaserExit();
                }
                currentReceiver = hitReceiverComponent;
                currentReceiver.OnLaserHit();
            }
        }
        else
        {
            // No receiver hit
            if (currentReceiver != null)
            {
                currentReceiver.OnLaserExit();
                currentReceiver = null;
            }
        }
    }
}
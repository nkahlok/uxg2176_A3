using UnityEngine;

public class InteractCube : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 3f;

    [Header("Materials")]
    [SerializeField] private Material[] materials;

    [SerializeField] private int cubeIndex; // ID of this cube (0,1,2)

    private Renderer rend;
    private int currentIndex = 0;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = materials[currentIndex];

        // Send initial color index
        CubePuzzleManager.instance.UpdateCubeColor(cubeIndex, currentIndex);
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= detectionRadius)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleMaterial();
            }
        }
    }

    void ToggleMaterial()
    {
        currentIndex = (currentIndex + 1) % materials.Length;

        rend.material = materials[currentIndex];

        // Send updated color index
        CubePuzzleManager.instance.UpdateCubeColor(cubeIndex, currentIndex);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

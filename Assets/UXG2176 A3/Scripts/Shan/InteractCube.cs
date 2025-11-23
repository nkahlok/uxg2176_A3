using UnityEngine;

public class InteractCube : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 3f;

    [Header("Materials")]
    [SerializeField] private Material[] materials;

    [SerializeField] private int cubeIndex; // id of cubes in order is 0,1,2
    [SerializeField] private AudioSource interactSfx;
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
        // If within distance radius and e is pressed
        if (Vector3.Distance(player.position, transform.position) <= detectionRadius)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Play sfx
                interactSfx.Play();

                // Toggle materials of the cube
                ToggleMaterial();
            }
        }
    }

    void ToggleMaterial()
    {
        // Switch between 3 of the materials
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

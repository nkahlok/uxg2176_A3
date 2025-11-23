using UnityEngine;

public class InteractCube : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 3f;

    [Header("Materials")]
    [SerializeField] private Material[] materials; 

    private Renderer rend;
    private int currentIndex = 0;

    [SerializeField] private int cubeIndex;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = materials[currentIndex];

        CubePuzzleManager.instance.UpdateCubeColor(cubeIndex,rend.material.color);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRadius)
        {
            if (Input.GetKeyDown(KeyCode.E))
                ToggleMaterial();
        }
    }

    void ToggleMaterial()
    {
        currentIndex++;

        if (currentIndex >= materials.Length)
            currentIndex = 0;

        rend.material = materials[currentIndex];

        CubePuzzleManager.instance.UpdateCubeColor(cubeIndex, rend.material.color);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    CinemachineCamera cam;
    CinemachineOrbitalFollow orbital;

    private void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        orbital = GetComponent<CinemachineOrbitalFollow>();
    }
}

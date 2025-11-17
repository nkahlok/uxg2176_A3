using UnityEngine;
using UnityEngine.UI;

public class DroneShooting : MonoBehaviour
{
    GameInput gameInput;

    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int maxBullets = 10;
    int numBullets = 0;
    [SerializeField] float reloadDuration = 1f;
    float reloadTimer = 0f;
    bool isReloading = false;
    [SerializeField] float maxRange = 10f;
    float cameraRange;
    Vector3 targetDir;
    [SerializeField] LayerMask hitLayers;

    [SerializeField] Image[] bulletWheelImages;
    [SerializeField] Image crosshair;

    private void Start()
    {
        gameInput = Player.Instance.GetComponent<GameInput>();
        gameInput.OnDroneShootAction += GameInput_OnDroneShootAction;
        gameInput.OnDroneReloadAction += GameInput_OnDroneReloadAction;

        numBullets = maxBullets;
    }

    private void Update()
    {
        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
        }
        else
        {
            if (isReloading)
            {
                Reload();
            }
        }
    }

    private void LateUpdate()
    {
        UpdateCrosshair();
    }

    private void GameInput_OnDroneShootAction(object sender, System.EventArgs e)
    {
        if (numBullets > 0)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(targetDir));
            bulletWheelImages[maxBullets - numBullets].enabled = false;
            numBullets--;

            if (numBullets == 0)
            {
                reloadTimer = reloadDuration;
                isReloading = true;
            }
        }
    }

    private void GameInput_OnDroneReloadAction(object sender, System.EventArgs e)
    {
        reloadTimer = reloadDuration;
        isReloading = true;
    }

    private void Reload()
    {
        numBullets = maxBullets;
        isReloading = false;

        foreach (Image image in bulletWheelImages)
        {
            image.enabled = true;
        }
    }

    private Vector3 GetCameraTargetPoint()
    {
        // shoots a ray from centre of screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        cameraRange = maxRange + Vector3.Distance(Camera.main.transform.position, bulletSpawnPoint.position);

        if (Physics.Raycast(ray, out RaycastHit hit, cameraRange, hitLayers))
        {
            return hit.point;
        }
        else
        {
            return ray.origin + ray.direction * cameraRange;
        }
    }

    private void UpdateCrosshair()
    {
        Vector3 targetPos = GetCameraTargetPoint();
        targetDir = (targetPos - bulletSpawnPoint.position).normalized;

        if (Physics.Raycast(bulletSpawnPoint.position, targetDir, out RaycastHit hit, maxRange, hitLayers))
        {
            targetPos = hit.point;
        }
        else
        {
            targetPos = bulletSpawnPoint.position + targetDir * maxRange;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);
        crosshair.rectTransform.position = screenPos;
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 targetPos = GetCameraTargetPoint();
    //    targetDir = targetPos - bulletSpawnPoint.position;
    //    targetDir.Normalize();

    //    Gizmos.color = Color.yellow;
    //    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    //    Gizmos.DrawRay(ray.origin, ray.direction * cameraRange);

    //    if (Physics.Raycast(bulletSpawnPoint.position, targetDir, out RaycastHit hit, maxRange, hitLayers))
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawRay(bulletSpawnPoint.position, targetDir * hit.distance);
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawRay(bulletSpawnPoint.position, targetDir * maxRange);
    //    }
    //}
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameInput gameInput;

    Rigidbody rb;
    [SerializeField] float moveSpeed;
    Vector3 smoothedInput, smoothedVelocity;
    [SerializeField] float smoothTime;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        //if (Player.Instance.playerState == Player.PlayerState.DRONE)
        //{
        //    // get player input
        //    Vector3 input = gameInput.GetMovementVector();

        //    // get flattened cam vectors and normalise
        //    Vector3 camForward = droneCamera.transform.forward;
        //    Vector3 camRight = droneCamera.transform.right;
        //    camForward.y = 0f;
        //    camRight.y = 0f;
        //    camForward.Normalize();
        //    camRight.Normalize();

        //    Vector3 moveDir = camForward * input.z + camRight * input.x;
        //    moveDir.Normalize();

        //    // calculate target velocity
        //    Vector3 targetVelocity = moveDir * maxHorizontalSpeed;
        //    targetVelocity.y = input.y * maxVerticalSpeed;

        //    // smooth damp curr velocity
        //    currVelocity = Vector3.SmoothDamp(currVelocity, targetVelocity, ref smoothedVelocity, smoothTime);

        //    // update position with calculated velocity
        //    rb.linearVelocity = currVelocity;
        //}
        //else
        //{
        //    // smooth damp curr velocity
        //    currVelocity = Vector3.SmoothDamp(currVelocity, Vector3.zero, ref smoothedVelocity, smoothTime);

        //    // update position with calculated velocity
        //    rb.linearVelocity = currVelocity;
        //}
    }
}

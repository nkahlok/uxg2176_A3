using UnityEngine;

public class OpenPhoneCanvas : MonoBehaviour
{
    public CallingID callingID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponentInParent<Player>() != null)
        {
            // If puzzle has been solved, return and dont open canvas
            if (callingID.GetPuzzleSolved())
                return;

            transform.GetChild(0).gameObject.SetActive(true);

            // Turns the cursor on 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(false);

        // Brings back the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

}

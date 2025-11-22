using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClueSwitch : MonoBehaviour
{
    public string code; // The number for the telephone dial
    public string order; // The order of what the code belongs to
    public bool isCode; // Boolean to indicate whether the text is showing the code or not

    TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isCode)
            text.text = code;
        else if(!isCode)
            text.text = order;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the drone bullet hits the text
        if (other.CompareTag("Projectile"))
        {
            if(!isCode)
            {
                text.text = code;  // Text becomes the code if not showing code
                isCode = !isCode; // Switch the bool
            }
            else
            {
                text.text = order; // Text becomes the order if not showing order
                isCode = !isCode; // Switch the bool
            }
        }
    }
}

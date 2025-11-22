using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NumberDial : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    string number; // The number of the dial this script is attached to

    bool dialThisNumber; // Boolean for when this number is selected
    bool numberReleased; // Boolean for when releasing the number

    float intervalBetweenNumbers; // Time between being able to key in a new number, prevent stray numbers from being keyed in

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        number = this.gameObject.name;  
    }

    // Update is called once per frame
    void Update()
    {
        intervalBetweenNumbers -= Time.deltaTime;

        // Keeps the object upright and prevents the numbers from rotating
        transform.rotation = Quaternion.identity;

        // Basically when the number is not being held, the interval float will decrease
        if(intervalBetweenNumbers < 0 )
        {
            dialThisNumber = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(number);

        // Calls the PointerDown event of the Rotary Dial when pressing down on a number
        this.gameObject.GetComponentInParent<RotaryDial>().OnPointerDown(eventData);
        dialThisNumber = true;

        // Reset the cooldown as we are constantly holding onto it
        intervalBetweenNumbers = 2f;

        // Play SFX
        KydeSoundManager.instance.PlayDialSound();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Calls the PointerUp event of the Rotary Dial when releasing the number
        this.gameObject.GetComponentInParent<RotaryDial>().OnPointerUp(eventData);
        numberReleased = true;

        // Play SFX
        KydeSoundManager.instance.PlayDialSound();

    }

    public void OnDrag(PointerEventData eventData)
    {
        // Calls the OnDrag event of the Rotary Dial when dragging the number
        this.gameObject.GetComponentInParent<RotaryDial>().OnDrag(eventData);


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // When the number that is being held down is released and exits from collision of the Dial hand

        if(collision.gameObject.name == "Dial hand" && numberReleased && dialThisNumber && intervalBetweenNumbers > 0)
        {
            // Adds this number into the call ID
            CallingID.instance.AddDialNumber(number);

            // Turns off the booleans
            dialThisNumber = false;
            numberReleased = false;

        }
    }
}

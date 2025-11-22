using System;
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] string unlockCode;
    string enteredCode;

    [SerializeField] TMP_Text displayText;

    [Serializable]
    public enum ButtonType
    {
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        ZERO,
        CLEAR,
        ENTER
    };

    private void Start()
    {
        displayText.text = string.Empty;
    }

    public void ButtonClick(KeypadButtons button)
    {
        if (button.buttonType == ButtonType.CLEAR)
        {
            enteredCode = string.Empty;
            displayText.text = enteredCode;
            return;
        }

        if (enteredCode.Length < unlockCode.Length)
        {
            switch (button.buttonType)
            {
                case ButtonType.ONE:
                    enteredCode += "1";
                    break;

                case ButtonType.TWO:
                    enteredCode += "2";
                    break;

                case ButtonType.THREE:
                    enteredCode += "3";
                    break;

                case ButtonType.FOUR:
                    enteredCode += "4";
                    break;

                case ButtonType.FIVE:
                    enteredCode += "5";
                    break;

                case ButtonType.SIX:
                    enteredCode += "6";
                    break;

                case ButtonType.SEVEN:
                    enteredCode += "7";
                    break;

                case ButtonType.EIGHT:
                    enteredCode += "8";
                    break;

                case ButtonType.NINE:
                    enteredCode += "9";
                    break;

                case ButtonType.ZERO:
                    enteredCode += "0";
                    break;

                default:
                    break;
            }

            displayText.text = enteredCode;
        }
        else if (enteredCode.Length == unlockCode.Length)
        {
            if (button.buttonType == ButtonType.ENTER && enteredCode == unlockCode)
            {
                UnlockPuzzle();
            }
        }
    }

    private void UnlockPuzzle()
    {

    }
}

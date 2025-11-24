using System;
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] string unlockCode;
    string enteredCode;
    [HideInInspector] public bool isUnlocked = false;

    [SerializeField] TMP_Text displayText;
    [SerializeField] ObjectiveText objectiveText;

    [SerializeField] GameObject door;

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
        enteredCode = string.Empty;
    }

    public void ButtonClick(KeypadButtons button)
    {
        if (button.buttonType == ButtonType.CLEAR)
        {
            ClearInput();
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

                case ButtonType.ENTER:
                    ClearInput();
                    break;

                default:
                    break;
            }

            displayText.text = enteredCode;
        }
        else if (enteredCode.Length == unlockCode.Length)
        {
            if (button.buttonType == ButtonType.ENTER)
            {
                if (enteredCode == unlockCode)
                {
                    UnlockPuzzle();
                }
                else
                {
                    ClearInput();
                }
            }
        }
    }

    private void UnlockPuzzle()
    {
        door.SetActive(false);
        ClearInput();
        isUnlocked = true;

        objectiveText.UpdateObjText(ObjectiveText.ObjText.NEXTLEVEL);

        Player.Instance.SwitchMode(Player.PlayerState.PLAYER);
    }

    public void ClearInput()
    {
        enteredCode = string.Empty;
        displayText.text = enteredCode;
    }
}

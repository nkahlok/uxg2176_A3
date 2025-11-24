using TMPro;
using UnityEngine;

public class CCTVPanel : Interactable
{
    GameInput gameInput;
    int cctvVirtualCamIndex = 0;
    [SerializeField] TMP_Text camText;
    [SerializeField] ObjectiveText objectiveText;
    [SerializeField] AudioClip buttonClick1;
    [SerializeField] AudioClip buttonClick2;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;

    private void Start()
    {
        gameInput = Player.Instance.GetComponent<GameInput>();
        gameInput.OnCCTVCycleLeftAction += GameInput_OnCCTVCycleLeftAction;
        gameInput.OnCCTVCycleRightAction += GameInput_OnCCTVCycleRightAction;
        gameInput.OnCCTVReturnAction += GameInput_OnCCTVReturnAction;

        objectiveText.UpdateObjText(ObjectiveText.ObjText.FIXEDCAM_CAMPANEL);

        audioSource2.volume = 0f;
        audioSource2.Play();
    }

    private void GameInput_OnCCTVCycleLeftAction(object sender, System.EventArgs e)
    {
        CycleCCTVCamera(false);
    }

    private void GameInput_OnCCTVCycleRightAction(object sender, System.EventArgs e)
    {
        CycleCCTVCamera(true);
    }

    private void GameInput_OnCCTVReturnAction(object sender, System.EventArgs e)
    {
        audioSource2.volume = 0f;
        Player.Instance.SwitchMode(Player.PlayerState.PLAYER);
    }

    public override void Activate()
    {
        objectiveText.UpdateObjText(ObjectiveText.ObjText.FIXEDCAM_KEYPAD);
        audioSource.PlayOneShot(buttonClick1);
        audioSource2.volume = 1f;
        Player.Instance.SwitchMode(Player.PlayerState.CCTV);
    }

    private void CycleCCTVCamera(bool cycleRight)
    {
        audioSource.PlayOneShot(buttonClick2);
        if (cycleRight)
        {
            cctvVirtualCamIndex++;
            cctvVirtualCamIndex %= CameraManager.Instance.GetCCTVCameraCount();
        }
        else
        {
            cctvVirtualCamIndex--;
            if (cctvVirtualCamIndex < 0)
            {
                cctvVirtualCamIndex += CameraManager.Instance.GetCCTVCameraCount();
            }
        }

        camText.text = $"Cam {cctvVirtualCamIndex + 1}";
        CameraManager.Instance.SetCameraMode(cctvVirtualCamIndex);
    }
}

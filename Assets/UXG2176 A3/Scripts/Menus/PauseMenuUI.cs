using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeClicked);
        mainMenuButton.onClick.AddListener(MainMenuClicked);
    }

    private void ResumeClicked()
    {
        Player.Instance.ResumeGame();
    }

    private void MainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

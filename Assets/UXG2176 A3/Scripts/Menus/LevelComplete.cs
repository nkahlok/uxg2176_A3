using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(MainMenuClicked);
    }

    private void MainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

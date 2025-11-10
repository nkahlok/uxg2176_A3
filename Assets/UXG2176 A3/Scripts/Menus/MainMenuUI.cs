using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayClicked);
        quitButton.onClick.AddListener(QuitClicked);
    }

    private void PlayClicked()
    {
        // Make sure demo is scene 1 else need change number
        SceneManager.LoadScene(1);
    }

    private void QuitClicked()
    {
        Application.Quit();
    }
}

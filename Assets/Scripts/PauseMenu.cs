using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    AudioManager audioManager;

    public void Pause()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.PlaySFX(audioManager.tap);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.PlaySFX(audioManager.tap);
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.PlaySFX(audioManager.tap);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.PlaySFX(audioManager.tap);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] bool goNextLevel = true; // Default dicentang
    [SerializeField] string levelName;

    AudioManager audioManager;

    private void Awake()
    {
        // Pastikan AudioManager tidak null agar tidak error jika tidak ada objek Audio
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null)
        {
            audioManager = audioObj.GetComponent<AudioManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.win);
            }

            UnlockNewLevel();

            // --- CEK KONDISI PERPINDAHAN LEVEL DI SINI ---
            if (goNextLevel)
            {
                // Pindah pakai sistem urutan (buildIndex + 1)
                SceneController.instance.NextLevel();
            }
            else
            {
                // Pindah pakai nama scene spesifik (menggunakan fungsi yang kita buat sebelumnya)
                SceneController.instance.LoadSceneWithTransition(levelName);
            }
        }
    }

    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex", 1))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
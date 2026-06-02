using UnityEngine;
using UnityEngine.SceneManagement;

public class fallRestart : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.death);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

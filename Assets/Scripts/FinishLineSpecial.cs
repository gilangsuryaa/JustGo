using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ada untuk pindah scene

public class FinishLineSpecial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playermovement player = collision.GetComponent<playermovement>();

            SceneManager.LoadScene("Level 5");
        }
    }
}
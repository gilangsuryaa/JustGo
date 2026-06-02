using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah yang menyentuh adalah Player
        if (collision.CompareTag("Player"))
        {
            // Akses script Player dan ubah status kuncinya
            playermovement player = collision.GetComponent<playermovement>();
            if (player != null)
            {
                player.hasKey = true;
                Debug.Log("Kunci didapat!");
                Destroy(gameObject); // Menghapus kunci dari layar
            }
        }
    }
}
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] string targetSceneName = "Level4-Dungeon";
    [SerializeField] private GameObject warningText; // Slot untuk objek tulisan peringatan

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
            playermovement player = collision.GetComponent<playermovement>();

            // Cek apakah player punya kunci
            if (player != null && player.hasKey == true)
            {
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.win);
                }
                Debug.Log("Pintu Terbuka! Memicu Transisi...");

                // Panggil SceneController untuk transisi dan pindah scene berdasarkan nama
                if (SceneController.instance != null)
                {
                    SceneController.instance.LoadSceneWithTransition(targetSceneName);
                }
                else
                {
                    // Fail-safe: Jika SceneController tidak ditemukan, tetap pindah scene tanpa transisi agar game tidak macet
                    UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
                }
            }
            else
            {
                Debug.Log("Pintu terkunci! Cari kunci dulu.");

                // --- LOGIKA BARU UNTUK UI PERINGATAN ---
                if (warningText != null)
                {
                    if (audioManager != null)
                    {
                        audioManager.PlaySFX(audioManager.nokey);
                    }
                    warningText.SetActive(true); // 1. Tampilkan tulisan peringatan

                    // 2. Batalkan Invoke lama (Fungsinya: kalau player nabrak pintunya berkali-kali, 
                    // durasi 3 detiknya bakal direset ulang dari awal, gak bakal kepotong)
                    CancelInvoke("HideWarning");

                    // 3. Panggil fungsi HideWarning setelah 3 detik (bisa kamu ganti jadi 5f kalau mau 5 detik)
                    Invoke("HideWarning", 3f);
                }
            }
        }
    }

    void HideWarning()
    {
        if (warningText != null)
        {
            warningText.SetActive(false); // Sembunyikan tulisan
        }
    }
}
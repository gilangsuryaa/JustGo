using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk sistem pindah scene

public class MainMenu : MonoBehaviour
{
    [Header("Pengaturan Scene")]
    [SerializeField] private string gameplaySceneName = "Level1"; // Ganti dengan nama scene game kamu

    void Start()
    {
        // --- SOLUSI BUG MUSIK ILANG ---
        // Mencari semua AudioSource di game (termasuk AudioManager kamu yang kemarin ke-stop)
        AudioSource[] semuaAudio = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in semuaAudio)
        {
            // Jika musiknya tidak sedang berputar, kita perintahkan untuk Play kembali
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
    }

    // FUNGSI UNTUK TOMBOL "KELUAR" / "QUIT"
    public void KeluarGame()
    {
        Debug.Log("Game Ditutup!");
        Application.Quit(); // Menutup game saat sudah di-build (.exe)

        // Biar pas kamu tes di Unity Editor, tombol keluar ini tetep keliatan efeknya (game stop play)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
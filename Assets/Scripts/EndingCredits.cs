using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredits : MonoBehaviour
{
    [Header("Pengaturan Gerakan")]
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float targetY = 1200f;

    [Header("Pengaturan Scene")]
    [SerializeField] private string menuSceneName = "MainMenu";

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        AudioSource[] semuaSuaraLama = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource suara in semuaSuaraLama)
        {
            // Matikan suara lama JIKA objeknya bukan milik CreditMusic di scene ini
            if (suara.gameObject.name != "CreditMusic")
            {
                suara.Stop();
            }
        }
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if (scrollSpeed > 0 && rectTransform.anchoredPosition.y >= targetY)
        {
            BackToMenu();
        }
        else if (scrollSpeed < 0 && rectTransform.anchoredPosition.y <= targetY)
        {
            BackToMenu();
        }
    }

    // --- TAMBAHKAN FUNGSI PUBLIC INI ---
    public void SkipCredits()
    {
        // Saat tombol ditekan, langsung panggil fungsi pindah scene
        BackToMenu();
    }

    void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
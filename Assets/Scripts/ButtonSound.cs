using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    void Start()
    {
        // Otomatis nambahin fungsi klik tanpa perlu drag-drag di inspector
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayTapSound();
        }
    }
}
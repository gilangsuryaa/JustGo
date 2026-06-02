using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            // Jika belum ada, maka ini adalah yang pertama
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jika sudah ada instance lain, hancurkan yang baru lahir ini
            Destroy(gameObject);
        }
    }
}
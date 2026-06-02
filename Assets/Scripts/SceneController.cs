using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    // Variabel internal untuk menyimpan instance
    private static SceneController _instance;

    // Properti global yang dipanggil oleh script lain (seperti FinishLine)
    public static SceneController instance
    {
        get
        {
            if (_instance == null)
            {
                // 1. Cari dulu apakah objeknya sudah ada di Scene saat ini
                _instance = Object.FindFirstObjectByType<SceneController>();

                // 2. Jika BENAR-BENAR TIDAK ADA (karena kamu play langsung di Level tinggi)
                if (_instance == null)
                {
                    // Ambil otomatis secara gaib dari folder Assets/Resources
                    GameObject prefab = Resources.Load<GameObject>("GameManager");

                    if (prefab != null)
                    {
                        GameObject clone = Instantiate(prefab);
                        _instance = clone.GetComponent<SceneController>();
                    }
                    else
                    {
                        // Peringatan jika kamu lupa membuat Prefab-nya
                        Debug.LogError("Cuy! Prefab bernama 'GameManager' tidak ditemukan di folder Resources!");
                    }
                }
            }
            return _instance;
        }
    }

    [SerializeField] Animator transitionAnim; //

    private void Awake()
    {
        // Logika Singleton yang disempurnakan agar sinkron dengan fungsi otomatis di atas
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); //
        }
        else if (_instance != this)
        {
            Destroy(gameObject); //
        }
    }

    public void NextLevel() //
    {
        StartCoroutine(LoadLevel()); //
    }

    public void LoadSceneWithTransition(string sceneName) //
    {
        StartCoroutine(LoadLevelByName(sceneName)); //
    }

    public void LoadScene(string sceneName) //
    {
        SceneManager.LoadScene(sceneName); //
    }

    IEnumerator LoadLevel() //
    {
        transitionAnim.SetTrigger("End"); //
        yield return new WaitForSeconds(1); //
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1); //
        transitionAnim.SetTrigger("Start"); //
    }

    IEnumerator LoadLevelByName(string sceneName) //
    {
        transitionAnim.SetTrigger("End"); //
        yield return new WaitForSeconds(1); //
        SceneManager.LoadSceneAsync(sceneName); //
        transitionAnim.SetTrigger("Start"); //
    }
}
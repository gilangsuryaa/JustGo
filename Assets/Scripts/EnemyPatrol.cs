using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Wajib ada untuk menggunakan IEnumerator (Jeda Waktu)

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed = 2f;

    private Vector3 targetPos;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D mainCollider;
    private bool isDead = false; // Menandai apakah slime sudah mati

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

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();

        pointA.parent = null;
        pointB.parent = null;
        targetPos = pointA.position;
    }

    void Update()
    {
        // Jika sudah mati, slime diam, tidak usah mengeksekusi kode patroli di bawah
        if (isDead) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            if (targetPos == pointA.position)
            {
                targetPos = pointB.position;
                Flip(-1);
            }
            else
            {
                targetPos = pointA.position;
                Flip(1);
            }
        }
    }

    void Flip(float scaleX)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * scaleX;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek apakah yang ditabrak adalah Player dan Slime masih hidup
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            // Ambil komponen PlayerController dari objek yang ditabrak
            playermovement player = collision.gameObject.GetComponent<playermovement>();

            if (player != null)
            {
                // Panggil fungsi PlayerDied yang akan kita buat di script Player
                audioManager.PlaySFX(audioManager.death);
                player.PlayerDied();
            }
}
    }

    // --- FUNGSI BARU: DIUTUS OLEH WEAKSPOT SAAT DIINJAK ---
    public void SlimeDied()
    {
        if (!isDead)
        {
            StartCoroutine(DeathSequence());
        }
    }

    // --- ALUR ANIMASI KEMATIAN SLIME ---
    IEnumerator DeathSequence()
    {
        isDead = true; // Menghentikan patroli

        // Matikan fungsi fisika & collider agar tidak mengganggu player lagi
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        mainCollider.enabled = false;
        audioManager.PlaySFX(audioManager.slimedead);

        // 1. Putar animasi "Hit"
        anim.Play("Hit");
        yield return new WaitForSeconds(0.4f); // Tunggu animasi Hit selesai (sesuaikan detiknya jika kurang pas)

        // 2. Putar animasi "Particles"
        anim.Play("Particles");
        yield return new WaitForSeconds(0.5f); // Tunggu efek partikel poof-nya selesai

        // 3. Hancurkan objek Slime sepenuhnya dari game
        Destroy(gameObject);
    }
}
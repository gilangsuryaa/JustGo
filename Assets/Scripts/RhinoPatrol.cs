using UnityEngine;
using System.Collections;

public class RhinoPatrol : MonoBehaviour
{
    [SerializeField] Transform pointA; //
    [SerializeField] Transform pointB; //
    [SerializeField] float speed = 3f; // Diatur agak cepat karena badak biasanya menyeruduk

    private Vector3 targetPos; //
    private Animator anim; //
    private Rigidbody2D rb; //
    private Collider2D mainCollider; //
    private bool isDead = false; // Menandai apakah badak sudah menabrak dinding
    private AudioSource audioSource;

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
        anim = GetComponent<Animator>(); //
        rb = GetComponent<Rigidbody2D>(); //
        mainCollider = GetComponent<Collider2D>(); //

        audioSource = GetComponent<AudioSource>();

        if (pointA != null) pointA.parent = null; //
        if (pointB != null) pointB.parent = null; //
        targetPos = pointA.position; //
    }

    void Update()
    {
        // Jika badak sudah menabrak dinding, stop gerakan patroli
        if (isDead) return; //

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); //

        if (Vector2.Distance(transform.position, targetPos) < 0.1f) //
        {
            if (targetPos == pointA.position) //
            {
                targetPos = pointB.position; //
                Flip(-1); //
            }
            else
            {
                targetPos = pointA.position; //
                Flip(1); //
            }
        }
    }

    void Flip(float scaleX) //
    {
        Vector3 localScale = transform.localScale; //
        localScale.x = Mathf.Abs(localScale.x) * scaleX; //
        transform.localScale = localScale; //
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. LOGIKA NABRAK PLAYER (Kondisi badak masih hidup/patroli)
        if (collision.gameObject.CompareTag("Player") && !isDead) //
        {
            playermovement player = collision.gameObject.GetComponent<playermovement>(); //

            if (player != null) //
            {
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.death);
                }
                player.PlayerDied(); // Kill player
            }
        }

        // 2. LOGIKA NABRAK TEMBOK (Kondisi objek punya tag "Wall")
        else if (collision.gameObject.CompareTag("Wall") && !isDead)
        {
            StartCoroutine(HitWallSequence());
        }
    }

    // --- ALUR SAAT BADAK MENABRAK TEMBOK VORP! ---
    IEnumerator HitWallSequence()
    {
        isDead = true; // Kita pinjam variabel ini sebagai status "Pause Gerakan" sementara

        // 1. Hentikan kecepatan badak saat menabrak dinding
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; //
                                              // Catatan: KITA HAPUS baris rb.isKinematic = true agar badak tidak melayang/menembus lantai
        }

        // PENTING: KITA HAPUS BARIS mainCollider.enabled = false; 
        // Biarkan collider tetap aktif agar badak bisa menabrak dinding lagi nanti!

        // 2. Putar animasi "hitwall" saat menabrak dinding
        if (anim != null)
        {
            anim.Play("hitwall"); //
        }

        // --- JALANKAN SUARA 3D SAAT NABRAK TEMBOK ---
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 3. Tunggu selama 0.5 detik (badak ceritanya sedang pusing sebelum balik arah)
        yield return new WaitForSeconds(0.5f); //

        // --- LOGIKA BARU: MEMBALIKKAN ARAH PATROLI ---
        // Di sini kita cek, kalau target sebelumnya adalah Point A, kita balikkan ke Point B, dan sebaliknya
        if (targetPos == pointA.position) //
        {
            targetPos = pointB.position; //
            Flip(-1); // Balik arah visual ke kiri
        }
        else
        {
            targetPos = pointA.position; //
            Flip(1); // Balik arah visual ke kanan
        }

        // 4. Setelah arah dibalik, aktifkan kembali gerakan badak
        isDead = false;

        if (anim != null)
        {
            anim.Play("run"); // Perintahkan badak untuk memutar animasi lari lagi
        }

        // PENTING: KITA HAPUS BARIS Destroy(gameObject); yang ada di paling bawah sebelumnya!
    }
}
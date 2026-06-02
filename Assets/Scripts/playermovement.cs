using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
public class playermovement : MonoBehaviour
{
    private bool isDead = false;
    public float speed;
    public float jf;
    public bool jump;
    Rigidbody2D rb;
    Animator anim;
    [Header("Sistem Skor")]
    public int scoreValue = 0;
    public TextMeshProUGUI scoreDisplay;

    [Header("Sistem Kunci")]
    public TextMeshProUGUI keyDisplay;

    [Header("Jump settings")]
    public float jumpForce = 10f;
    public float jumpTime = 0.3f;
    private float jumpTimeCounter;
    private bool isJumping;

    [Header("Better jump(Snappines)")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [SerializeField] private bool isGrounded;

    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D coll;

    private bool facingRight = true;

    private bool isFinished = false;
    private bool canMove = false;

    public bool hasKey = false; // Status awal tidak punya kunci

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        audioManager.PlaySFX(audioManager.spawn);

        GetComponent<Animator>().SetBool("isSpawning", true);
        Invoke("FinishSpawn", 1.0f);
        Invoke("EnableMovement", 1f);

        if (keyDisplay != null)
        {
            keyDisplay.text = ": ?";
        }
    }

    void FinishSpawn()
    {
        GetComponent<Animator>().SetBool("isSpawning", false);
    }

    void Update()
    {
        if (!canMove || isFinished) return; //

        // 1. Ambil status tanah dari BoxCast yang akurat
        bool diTanah = IsGrounded(); //
        isGrounded = diTanah; // Kita samakan agar variabel global di Inspector ikut akurat

        if (diTanah && Input.GetKeyDown(KeyCode.Space)) //
        {
            isJumping = true; //
            jumpTimeCounter = jumpTime; //
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //
        }

        if (Input.GetKey(KeyCode.Space) && isJumping) //
        {
            if (jumpTimeCounter > 0) //
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //
                jumpTimeCounter -= Time.deltaTime; //
            }
            else //
            {
                isJumping = false; //
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) //
        {
            isJumping = false; //
        }

        // 2. PERBAIKAN UTAMA: Gunakan variabel 'diTanah' (BoxCast), bukan 'isGrounded' lama
        anim.SetBool("jump", !diTanah);

        // 3. Kirim kecepatan vertikal terbaru ke parameter yVelocity di Animator
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (!canMove || isFinished) return;

        float horiz = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horiz * speed, rb.linearVelocity.y);

        anim.SetFloat("Speed", Mathf.Abs(horiz));

        if (horiz > 0 && !facingRight)
        {
            Flip();
        }
        else if (horiz < 0 && facingRight)
        {
            Flip();
        }

        if(rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if(rb.linearVelocityY > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Cek apakah objek yang disentuh adalah lantai
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Apple"))
        {
            audioManager.PlaySFX(audioManager.collect);
            scoreValue += 1;
            scoreDisplay.text = ": " + scoreValue;

            Animator appleAnim = collision.GetComponent<Animator>();

            if (appleAnim != null)
            {
                appleAnim.SetTrigger("isCollected");
            }

            collision.enabled = false;

            Destroy(collision.gameObject, 0.5f);

            Debug.Log("Skor sekarang: " + scoreValue);
        }
        else if (collision.CompareTag("Orange"))
        {
            audioManager.PlaySFX(audioManager.collect);
            scoreValue += 5;
            scoreDisplay.text = ": " + scoreValue;

            Animator appleAnim = collision.GetComponent<Animator>();

            if (appleAnim != null)
            {
                appleAnim.SetTrigger("isCollected");
            }

            collision.enabled = false;

            Destroy(collision.gameObject, 0.5f);

            Debug.Log("Skor sekarang: " + scoreValue);
        }
        else if (collision.CompareTag("Key"))
        {
            // 1. Mainkan suara collect seperti buah
            audioManager.PlaySFX(audioManager.collect); //

            // 2. Ubah status hasKey menjadi true
            hasKey = true; //

            // 3. Ubah tampilan teks UI dari "?" menjadi "1"
            if (keyDisplay != null)
            {
                keyDisplay.text = ": 1";
            }

            // 4. Matikan collider kunci agar tidak tertabrak 2 kali
            collision.enabled = false; //

            // 5. Hancurkan objek kunci dari map
            Destroy(collision.gameObject); //
        }
        else if(collision.CompareTag("Trap")){
            audioManager.PlaySFX(audioManager.death);
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    public void ProsesFinishPlayer()
    {
        if (isFinished) return;

        isFinished = true;

        this.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.Play("dissapear");
        }

        Destroy(gameObject, 1.5f);
    }

    // --- FUNGSI UTAMA SAAT PLAYER DIKANDANDKAN OLEH MUSUH ---
    public void PlayerDied()
    {
        if (!isDead)
        {
            StartCoroutine(PlayerDeathSequence());
        }
    }

    // --- ALUR ANIMASI KEMATIAN PLAYER ---
    IEnumerator PlayerDeathSequence()
    {
        isDead = true;

        // 1. Matikan fungsi fisika agar player diam di tempat dan tidak jatuh menembus tanah
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // 2. Matikan Collider agar player tidak bisa menabrak musuh lain selama proses menghilang
        Collider2D cd = GetComponent<Collider2D>();
        if (cd != null)
        {
            cd.enabled = false;
        }

        // 3. Matikan script input gerakan player ini sendiri agar tidak bisa digerakkan oleh tombol keyboard
        this.enabled = false;

        // 4. Mainkan animasi "Disappear" di Animator
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("disappear");
        }

        // 5. Tunggu sampai animasi menghilang selesai (Aset Pixel Frog biasanya sekitar 0.5 sampai 0.6 detik)
        yield return new WaitForSeconds(0.7f);

        // 6. Selesai animasi, baru restart level dari awal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
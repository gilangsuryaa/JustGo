using UnityEngine;
using System.Collections;

public class trapspatrol : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed = 2f;

    private Vector3 targetPos;

    void Start()
    {
        // Melepas parent agar titik koordinat patroli tidak ikut bergerak mengikuti trap
        if (pointA != null) pointA.parent = null;
        if (pointB != null) pointB.parent = null;

        targetPos = pointA.position;
    }

    void Update()
    {
        // LOGIKA GERAKAN MONDAR-MANDIR (Abadi tanpa ngecek isDead)
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
        // Membalik arah hadap gambar trap jika diperlukan (misal jebakan berwajah/punya arah)
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * scaleX;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // LOGIKA NGEKILL PLAYER
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ambil komponen playermovement dari Player yang ditabrak
            playermovement player = collision.gameObject.GetComponent<playermovement>();

            if (player != null)
            {
                // Panggil fungsi PlayerDied agar player memutar animasi mati yang estetik sebelum restart
                player.PlayerDied();
            }
        }
    }
}
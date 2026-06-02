using UnityEngine;

public class EnemyWeakSpot : MonoBehaviour
{
    [SerializeField] float bounceForce = 12f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Membuat player memantul ke atas
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
            }

            // Cari script EnemyPatrol di objek induk (Parent), lalu panggil fungsi kematiannya
            EnemyPatrol slimeParent = transform.parent.GetComponent<EnemyPatrol>();
            if (slimeParent != null)
            {
                slimeParent.SlimeDied();
            }

            // Hancurkan collider kepala ini sendiri agar tidak bisa diinjak dua kali berkali-calli
            Destroy(gameObject);
        }
    }
}
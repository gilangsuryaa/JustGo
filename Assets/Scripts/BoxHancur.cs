using UnityEngine;

public class BoxHancur:MonoBehaviour
{
    public GameObject itemPrefab;
    private bool isBroken = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.break_box);
            if (collision.transform.position.y > transform.position.y + 0.4f)
            {
                isBroken = true;

                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 8f);
                }

                MunculkanItem();
            }
        }
    }

    void MunculkanItem()
    {
        if(itemPrefab != null)
        {
            GameObject buah = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            Collider2D colBuah = buah.GetComponent<Collider2D>();
            if (colBuah != null)
            {
                StartCoroutine(AktifkanColliderLagi(colBuah));
            }
        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    System.Collections.IEnumerator AktifkanColliderLagi(Collider2D col)
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.2f);

        if (col != null)
        {
            col.enabled = true;
        }

        Destroy(gameObject);
    }
}

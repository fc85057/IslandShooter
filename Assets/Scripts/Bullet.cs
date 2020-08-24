using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public LayerMask layermask;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Gunshot");
        rb.velocity = (transform.right * -1) * speed;
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Enemy").GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layermask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            /*Debug.Log("Hit " + collision.name);
            if (collision.tag == "Enemy")
            {
                Debug.Log("Hit skeleton");
                // how to call die here?
            }*/
            if (collision.tag != "Player" && collision.tag != "Enemy")
            {
                Destroy(gameObject);
            }
        }
        
        if (collision.tag == "Enemy" && (collision.GetType() != typeof(CircleCollider2D)))
        {
            collision.GetComponent<Enemy>().Die();
            GameManager.instance.ChangeScore(1);
            Destroy(gameObject);
        }
 
    }
}

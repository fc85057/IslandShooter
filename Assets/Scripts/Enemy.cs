using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float moveSpeed = 5f;
    public int maxHealth = 10;
    public float attackRate = .2f;
    public LayerMask groundLayerMask;
    public LayerMask playerLayer;


    public enum EnemyState { chase, patrol, dead }

    private Rigidbody2D rb;
    [SerializeField]
    public Transform attackPoint;
    [SerializeField]
    private Animator animator;
    private EnemyState currentState;
    private Collider2D boundsCheck;
    
    private bool isMovingRight;
    private float speed;
    private float nextAttackTime = 0f;
    private Vector3 lastposition;

    public Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.patrol;
        isMovingRight = true;
        boundsCheck = GetComponent<BoxCollider2D>();
        lastposition = new Vector3(0f, 0f);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // speed = rb.velocity.magnitude;
        if (currentState == EnemyState.dead)
        {
            Dead();
        }
        else if (currentState == EnemyState.chase)
        {
            Chase();
        }
        else if (currentState == EnemyState.patrol)
        {
            Patrol();
        }

        if (lastposition == transform.position)
        {
            animator.SetFloat("MovementSpeed", 0f);
        }
        else
        {
            animator.SetFloat("MovementSpeed", 1f);
        }

        lastposition = transform.position;
        // speed = rb.velocity.magnitude;
        Debug.Log(speed);
        // animator.SetFloat("MovementSpeed", speed);
        Debug.Log(currentState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with " + collision.name + " detected.");
        if (collision.tag == "Player")
        {
            currentState = EnemyState.chase;
        }
        //if (collision.gameObject.name == "Bullet(Clone)")
        //{
        //    Die();
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            currentState = EnemyState.patrol;
        }
    }

    void Patrol()
    {

        if (isMovingRight)
        {
            if (Physics2D.Raycast(boundsCheck.bounds.center, Vector2.right, boundsCheck.bounds.extents.x + .5f, groundLayerMask))
            {
                isMovingRight = false;
                transform.eulerAngles = new Vector2(0f, 180f);
                transform.position = Vector2.MoveTowards(transform.position, transform.position - Vector3.right, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, moveSpeed * Time.deltaTime);
            }
            
        }
        else
        {
            if (Physics2D.Raycast(boundsCheck.bounds.center, -Vector2.right, boundsCheck.bounds.extents.x + .5f, groundLayerMask))
            {
                isMovingRight = true;
                transform.eulerAngles = new Vector2(0f, 0f);
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position - Vector3.right, moveSpeed * Time.deltaTime);
            }
            
        }

    }

    void Chase()
    {

        if (Time.time >= nextAttackTime)
        {
            if (Physics2D.Raycast(attackPoint.position, Vector2.right, 1f, playerLayer))
            {
                    Debug.Log("Time now is " + Time.time);
                    Debug.Log("Next attack time " + nextAttackTime);
                    Attack();
                nextAttackTime = Time.time + 1f; /// attackRate;
            }
        }

        if (transform.position.x < target.position.x)
        {
            isMovingRight = true;
            transform.eulerAngles = new Vector2(0f, 0f);
        }
        else
        {
            isMovingRight = false;
            transform.eulerAngles = new Vector2(0f, 180f);
        }

        Vector2 newPosition = new Vector2(target.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        animator.SetTrigger("isAttacking");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, 0.5f, playerLayer);
        foreach (Collider2D hit in hits)
        {
            hit.GetComponent<PlayerController>().DealDamage(20);
            Debug.Log("Hit!");
        }
    }

    void Dead()
    {
        if (Physics2D.Raycast(boundsCheck.bounds.center, Vector2.down, 0.4f, groundLayerMask))
        {
            rb.simulated = false;
            this.enabled = false;
        }
    }

    public void Die()
    {
        animator.SetTrigger("IsDead");
        FindObjectOfType<AudioManager>().Play("SkeletonDie");
        currentState = EnemyState.dead;
        
        foreach (Collider2D coll in GetComponents<Collider2D>())
        {
            coll.enabled = false;
        }

        if (Physics2D.Raycast(boundsCheck.bounds.center, Vector2.down, 0.4f, groundLayerMask))
        {
            rb.simulated = false;
            this.enabled = false;
        }
    }
}

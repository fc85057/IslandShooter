using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int health = 100;
    public float speed = 8f;
    public float jumpSpeed = 20f;
    float barrelPadding;
    // bool isGrounded;
    public float attackRate = 2f;

    public Animator animator;
    Rigidbody2D rb;
    PolygonCollider2D groundCheck;
    [SerializeField] LayerMask groundLayerMask;

    public Transform gunBarrel;
    public GameObject bullet;
    public GameObject smoke;

    private float nextAttackTime = 0f;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<PolygonCollider2D>();
        GameManager.instance.playerHealth = 100;
        GameManager.instance.skeletonsKilled = 0;
        //isGrounded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (health <= 0)
        {
            Die();
        }


        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(Attack());
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles = new Vector2(0f, 0f);
            animator.SetBool("isMoving", true);
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles = new Vector2(0f, 180f);
            animator.SetBool("isMoving", true);
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * speed;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }


    }

    IEnumerator Attack()
    {
        animator.SetTrigger("isAttacking");
        Vector3 firePoint = gunBarrel.position;
        //firePoint.y += .3f;
        GameObject bulletTest = Instantiate(bullet, firePoint, gunBarrel.rotation);
        Debug.Log(bulletTest.name + " instantiated");
        //if (transform.eulerAngles.y == 0)
        //{
        //    firePoint.x += .2f;
        //}
        //else
        //{
        //    firePoint.x += -.2f;
        //}
        GameObject currentSmoke = Instantiate(smoke, firePoint, gunBarrel.rotation);
        yield return new WaitForSeconds(0.2f);
        Destroy(currentSmoke);
    }

    void Jump()
    {

        if (Physics2D.Raycast (groundCheck.bounds.center, Vector2.down, groundCheck.bounds.extents.y + 0.1f, groundLayerMask))
        {
            rb.velocity = Vector2.up * jumpSpeed;
        }

            
    }
    
    void Die()
    {
        Debug.Log("Dead");
        animator.SetTrigger("isDead");
        isDead = true;
        this.enabled = false;
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        GameManager.instance.ChangeHealth(health);
    }

}

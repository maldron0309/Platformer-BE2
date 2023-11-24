using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameManager gameManager;

    public float maxSpeed;
    public float jumpPower;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private CapsuleCollider2D collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }

        // Stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * 0.5f, rb.velocity.y);
        }

        // Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            sr.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // Animation
        if (Mathf.Abs(rb.velocity.x) < 0.5)
        {
            anim.SetBool("isRun", false);
        }
        else
        {
            anim.SetBool("isRun", true);
        }
    }

    private void FixedUpdate()
    {
        // Move speed
        float h = Input.GetAxisRaw("Horizontal");

        rb.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max speed
        if (rb.velocity.x > maxSpeed) // Right max speed
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < maxSpeed * (-1)) // Left max speed
        {
            rb.velocity = new Vector2(maxSpeed * (-1), rb.velocity.y);
        }

        if (rb.velocity.y < 0)
        {
            // 수정된 레이캐스트 파라미터
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position + Vector2.right * 1f, Vector3.down, 1,
                LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 2f)
                {
                    anim.SetBool("isJump", false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Attack
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            {
                OnDamaged(collision.transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            // Point
            // Contains : 대상 문자열에 비교문이 있으면 true
            bool isBronze = other.gameObject.name.Contains("Bronze");
            bool isSilver = other.gameObject.name.Contains("Silver");
            bool isGold = other.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;
            // Deactive Item
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
        }
    }

    private void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;
        // Reaction Force
        rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        // Enemy Die
        EnemyMovement enemyMove = enemy.GetComponent<EnemyMovement>();
        enemyMove.OnDamaged();
    }

    private void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown();
        
        // Change layer
        gameObject.layer = 8;

        sr.color = new Color(1, 1, 1, 0.4f);

        // Reaction force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        // Animation
        anim.SetTrigger("DoDamaged");
        Invoke("OffDamaged", 2);
    }

    private void OffDamaged()
    {
        gameObject.layer = 7;
        sr.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        // Sprite Alpha
        sr.color = new Color(1, 1, 1, 0.4f);
        // Sprite Flip Y
        sr.flipY = true;
        // Collider Disable
        collider.enabled = false;
        // Die Effect Jump
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rb.velocity = Vector2.zero;
    }
}
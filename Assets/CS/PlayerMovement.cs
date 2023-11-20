using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
            OnDamaged(collision.transform.position);
        }
    }

    private void OnDamaged(Vector2 targetPos)
    {
        // Change layer
        gameObject.layer = 8;

        sr.color = new Color(1, 1, 1,0.4f);
        
        // Reaction force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc,1) * 7,ForceMode2D.Impulse);
        
        // Animation
        anim.SetTrigger("DoDamaged");
        Invoke("OffDamaged",2);
    }

    private void OffDamaged()
    {
        gameObject.layer = 7;
        sr.color = new Color(1, 1, 1,1);

    }
    
}
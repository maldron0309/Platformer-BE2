using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    // 행동 지표 결정할 변수
    public int nextMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        
        Think();
        
        // 주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
        Invoke("Think",5);
    }

    private void FixedUpdate()
    {
        // Move
        rb.velocity = new Vector2(nextMove, rb.velocity.y);
        
        // Platform Check
        Vector2 front = new Vector2(rb.position.x + nextMove * 0.3f, rb.position.y);
        
        Debug.DrawRay(front,Vector3.down,new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(front, Vector3.down, 1,
            LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    // 재귀 함수 : 자신을 스스로 호출하는 함수
    private void Think()
    {
        // Set next Active
        nextMove = Random.Range(-1, 2);
        
        // sprite animation
        anim.SetInteger("RunSpeed",nextMove);
        
        // Flip sprite
        if (nextMove != 0)
        {
            sr.flipX = nextMove == 1;   
        }
        
        // Recursive0
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think",nextThinkTime);
    }

    private void Turn()
    {
        nextMove *= -1;
        sr.flipX = nextMove == 1;
        
        // CancelInvoke() : 현재 작동 중인 모든 Invoke함수를 멈추는 함수
        CancelInvoke();
        Invoke("Think",2);
    }
}
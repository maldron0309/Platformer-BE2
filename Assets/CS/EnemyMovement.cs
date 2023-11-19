using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    // 행동 지표 결정할 변수
    public int nextMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        Think();
        
        // 주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
        Invoke("Think",5);
    }

    private void FixedUpdate()
    {
        // Move
        rb.velocity = new Vector2(nextMove, rb.velocity.y);
        
        // Platform Check
        Vector2 front = new Vector2(rb.position.x + nextMove, rb.position.y);
        
        Debug.DrawRay(front,Vector3.down,new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(front, Vector2.down, 1,
            LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            Debug.Log("경고");
        }
    }

    // 재귀 함수 : 자신을 스스로 호출하는 함수
    private void Think()
    {
        nextMove = Random.Range(-1, 2);
        
        Invoke("Think",5);
    }
}
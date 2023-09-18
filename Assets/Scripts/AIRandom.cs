using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRandom : MonoBehaviour
{
    [SerializeField] private float wanderSpeed = 0.5f; // ความเร็วการเดิน
    [SerializeField] private float wanderRadius = 1f; // ระยะสุ่มเดิน
    [SerializeField] private int aiHealth; // กำหนดเลือดของ AI

    private Vector2 randomDestination; // เก็บค่าสุ่ม
    private float wanderTimer; // หน่วงเวลาการเปลี่ยนตำแหน่ง
    private bool isFacingRight = true; // ตรวจสอบว่า AI หันไปทางขวาหรือไม่
    private Animator animator;
    private bool aiFreeze;

    private void Start()
    {
        SetRandomDestination();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(aiFreeze != true)
        {
            Wander();
        } 
        // Flip ถ้า AI หันไปทางซ้ายและตำแหน่งปลายทางอยู่ทางขวา
        if (isFacingRight && randomDestination.x < transform.position.x)
        {
            Flip();
        }
        // Flip ถ้า AI หันไปทางขวาและตำแหน่งปลายทางอยู่ทางซ้าย
        else if (!isFacingRight && randomDestination.x > transform.position.x)
        {
            Flip();
        }
        // เช็คเลือดของ AI
        if(aiHealth <= 0)
        {
            aiFreeze = true;
            animator.SetTrigger("Death");
            Destroy(this.gameObject,1f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spell")
        {
            // ถ้าโจมตีด้านหลังเราสามารถลดเลือดตรงนี้
            if (aiHealth > 1)
            {
                animator.SetTrigger("Damaged");
            }
            aiHealth -= 1;
            Debug.Log("Skeleton Health -1");
        }
    }

    // เปลี่ยนตำแหน่งสุ่ม
    private void SetRandomDestination()
    {
        randomDestination = GetRandomPointInRadius(transform.position, wanderRadius);
    }

    // เคลื่อนที่ AI
    private void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= 3f)
        {
            SetRandomDestination();
            wanderTimer = 0f;
        }

        transform.position = Vector2.MoveTowards(transform.position, randomDestination, wanderSpeed * Time.deltaTime);
        //animator.SetBool("Walk",true);
    }

    // สร้างจุดสุ่มภายในรัศมี
    private Vector2 GetRandomPointInRadius(Vector2 center, float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized * radius;
        return center + randomDirection;
    }

    // สลับทิศทางของ AI
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
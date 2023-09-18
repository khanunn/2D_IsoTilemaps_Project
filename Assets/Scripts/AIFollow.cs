using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollow : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private float speed = 1f; // ควาามเร็ว
    [SerializeField] private float detectionRange = 4f; // ระยะที่จะตรวจสอบ
    [SerializeField] private float attackRange = 0.5f; // ระยะโจมตี
    [SerializeField] private int aiHelth; // กำหนดเลือดของ AI

    private Transform aiTransform;
    private float distance;
    private Transform playerTransform;
    private bool isFacingRight = true; // ตรวจสอบว่าตัวละครหันไปทางขวาหรือไม่
    private Animator animator;
    private bool aiFreeze;

    private void Start()
    {
        aiTransform = transform;
        playerTransform = player.transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        distance = Vector2.Distance(aiTransform.position, playerTransform.position);

        if(aiFreeze != true)
        {
            Vector2 aiDirection = playerTransform.position - aiTransform.position;
            aiDirection.Normalize();
            float angle = Mathf.Atan2(aiDirection.y, aiDirection.x) * Mathf.Rad2Deg;
            if (distance < detectionRange)
            {
                aiTransform.position = Vector2.MoveTowards(aiTransform.position, playerTransform.position, speed * Time.deltaTime);
                animator.SetTrigger("Run");
                // Flip ศัตรูหากตัวละครอยู่ด้านตรงข้าม
                if (aiDirection.x < 0 && isFacingRight)
                {
                    Flip();
                }
                else if (aiDirection.x > 0 && !isFacingRight)
                {
                    Flip();
                }
            }
        }
        if(distance < attackRange)
        {
            animator.SetBool("Attack",true);
            aiFreeze = true;
        }
        else
        {
            animator.SetBool("Attack",false);
            aiFreeze = false;
        }
        // เช็คเลือดของ AI
        if(aiHelth <= 0)
        {
            aiFreeze = true;
            animator.SetTrigger("Death");
            Destroy(this.gameObject,1f);
        }
    }

    // ฟังก์ชันสำหรับ Flip AI
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = aiTransform.localScale;
        scale.x *= -1;
        aiTransform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spell")
        {
            aiHelth -= 1;
            animator.SetTrigger("Damaged");
            Debug.Log("Goblin Helth -1");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEscape : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] private float evadeSpeed = 3f; // ความเร็ว
    [SerializeField] private float evadeDistance = 3f; // ระยะบินหนัี
    [SerializeField] private float attackDistance = 5f; // ระยะโจมตี
    [SerializeField] private int aiHealth; // กำหนดเลือดของ AI
    public GameObject projectilePrefab; // กระสุนของ AI
    public Transform attackPoint; // จุดปล่อยกระสุนของ AI

    private SpriteRenderer spriteRenderer; 
    private bool canAttack = true;
    private Animator animator;
    private bool aiFreeze;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!aiFreeze)
        {
            EvadePlayer();
            FlipIfNeeded();
            CheckAttack();
            CheckHealth();
        }
    }
    private void EvadePlayer()
    {
        if (playerTransform != null)
        {
            // ตรสวจสอบระยะของ player เพื่อหนี
            Vector3 direction = transform.position - playerTransform.position;
            if (direction.magnitude < evadeDistance)
            {
                direction.Normalize();
                transform.position += direction * evadeSpeed * Time.deltaTime;
            }
        }
    }
    private void FlipIfNeeded()
    {
        if (playerTransform != null)
        {
            Vector3 direction = transform.position - playerTransform.position;
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // หันไปทางขวา
            }
            else
            {
                spriteRenderer.flipX = true; // หันไปทางซ้าย
            }
        }
    }

    private void CheckAttack()
    {
        if (playerTransform != null)
        {
            // ตรวจสอบระยะระหว่าง player เพื่อโจมตี
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackDistance)
            {
                // หันหน้าไปทาง player
                Vector2 direction = playerTransform.position - transform.position;
                direction.Normalize();
                spriteRenderer.flipX = direction.x > 0 ? false : true;
                Attack();
            }
        }
    }
    private void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(DelayedAttack()); // หน่วงเวลา

            if (projectilePrefab != null && attackPoint != null)
            {
                // ให้ projectile พุ่งไปที่ตำแหน่งปัจจุบันของ player
                GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
                projectile.GetComponent<FlyingSpellController>().target = playerTransform;
            }
        }
    }
    private IEnumerator DelayedAttack()
    {
        canAttack = false; 
        yield return new WaitForSeconds(3f);
        canAttack = true; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spell")
        {
            if (aiHealth > 1)
            {
                animator.SetTrigger("Damaged");
                Debug.Log("Settrigger : Damaged");
            }
            aiHealth -= 1;
            Debug.Log("Flying Health -1");
        }
    }

    private void CheckHealth()
    {
        // เช็คเลือดของ AI
        if(aiHealth <= 0)
        {
            aiFreeze = true;
            animator.SetTrigger("Death");
            Destroy(this.gameObject,1f);
        }
    }
}

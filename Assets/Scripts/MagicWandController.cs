using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWandController : MonoBehaviour
{
    private Transform wandTransform; // อ้างอิงถึง Transform ของ MagicWand
    IsometricPlayerMovementController playerController; // รับค่าจากสคริป IPMC
    private Vector2 playerDirection; // เก็บ Direction
    public GameObject magicSpellPrefab; // เก็บ Prefab ของ MagicSpell
    private Vector2 lastDirection = Vector2.zero; // เก็บ Direction ล่าสุด
    public Transform spellPoint; // จุดปล่อย Spell

    // Start is called before the first frame update
    void Awake()
    {
        wandTransform = transform;
        playerController = transform.parent.GetComponent<IsometricPlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        // รับค่า Direction จากสคริปต์ "IsometricPlayerMovementController"
        playerDirection = playerController.GetDirection();

        // ถ้าเคลื่อนที่
        if (playerDirection.magnitude > 0)
        {
            // คำนวณมุมหมุนจาก Direction
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

            // หมุน MagicWand ให้ชี้ไปทาง Direction ที่คำนวณได้
            wandTransform.rotation = Quaternion.Euler(0f, 0f, angle);

            // บันทึก Direction ล่าสุด
            lastDirection = playerDirection;
        }
        else
        {
            // ถ้าไม่มีการเคลื่อนที่ ให้ MagicWand อยู่ใน Direction ล่าสุด
            float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
            wandTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        }
    }
    // เรียกใช้ใน Button Click
    public void FireMagicSpell()
    {
        Instantiate(magicSpellPrefab, spellPoint.position, spellPoint.rotation);
    }
}

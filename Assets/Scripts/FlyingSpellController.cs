using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpellController : MonoBehaviour
{
    [SerializeField] private float spellSpeed = 5f; // ความเร็วกระสุน
    public Transform target; // เป้าหมายโจมตี
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        // คำนวณทิศทางไปทาง target (Player)
        direction = (target.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // หมุน projectile เพื่อมองที่ target (Player)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // เคลื่อนที่ projectile ไปทาง target
            transform.rotation = rotation;
            transform.Translate(Vector2.right * spellSpeed * Time.deltaTime);
        }

        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}

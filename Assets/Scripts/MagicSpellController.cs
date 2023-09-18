using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpellController : MonoBehaviour
{
    public float spellSpeed = 80f; // ความเร็วของ MagicSpell
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * spellSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Shield")
        {
            Destroy(this.gameObject);
        }
    }
}

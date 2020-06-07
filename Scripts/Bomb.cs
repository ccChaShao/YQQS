using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public BoxCollider2D idleCollider;
    public BoxCollider2D toggleCollider;
    private Animator anim;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            idleCollider.enabled = false;
            toggleCollider.enabled = true;
            anim.SetTrigger("Toggle");
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}

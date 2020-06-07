using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repel : MonoBehaviour
{
    public float speed = 0;
    public Vector3 attacker;
    private Vector3 speedV3;
    // Start is called before the first frame update
    void Start()
    {
        speedV3 = attacker - transform.position;
        speedV3.Normalize();
        Destroy(this, 0.5f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += speedV3 * -1 * speed * Time.fixedDeltaTime;
    }
}

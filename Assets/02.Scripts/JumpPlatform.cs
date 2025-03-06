using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    GameObject obj;
    Rigidbody rb;
    BoxCollider box;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
    }

    float power = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            collision.transform.position.y > box.bounds.size.y + transform.position.y)
        {
            if (collision.gameObject.TryGetComponent<Rigidbody>(out rb) == false) return;
            obj = collision.gameObject;
            rb.AddForce(rb.transform.up * power, ForceMode.Impulse);
        }
    }
}

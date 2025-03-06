using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    GameObject obj;
    Rigidbody rb;

    float power = 50f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody>(out rb) == false) return;
        obj = collision.gameObject;
        rb.AddForce(rb.transform.up * power, ForceMode.Impulse);
        StartCoroutine(Down());
    }

    IEnumerator Down()
    {
        yield return new WaitForSeconds(5f);
        rb.AddForce(obj.transform.up * -(power -3f) , ForceMode.Impulse);
        Debug.Log("내려주는중~");
    }
}

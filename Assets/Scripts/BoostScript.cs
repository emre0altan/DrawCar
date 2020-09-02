using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour
{

    Vector3 force = new Vector3(800, 0, 0);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
}

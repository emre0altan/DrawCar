using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(300000, 0, 0));
        }
    }
}

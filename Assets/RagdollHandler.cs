using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GoRagdoll(false);
    }

    public void GoRagdoll(bool v)
    {
        if( v == true)
        {
            //Disable animator
            GetComponent<Animator>().enabled = false;
        }

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rigidbodies)
        {
            if(rb.gameObject != gameObject)
            {
                rb.useGravity = v;
                rb.isKinematic = !v;
            }
        }
    }
}

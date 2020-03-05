using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatExample : MonoBehaviour {
    public float Ax;
    public float t;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) {
            transform.rotation = Quaternion.Euler(Ax, 0, 0);
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(100f, 0f, 0f), t);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    private float furthestReached;
    
    // Start is called before the first frame update
    void Start()
    {
        furthestReached = target.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.transform.position.x > furthestReached)
        {
            furthestReached = target.transform.position.x;
        }
    }
    
    void LateUpdate()
    {
        transform.position = new Vector3(furthestReached + 1.0f, 0, transform.position.z);
    }
}

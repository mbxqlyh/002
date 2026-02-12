using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {

        if (target != null && target.position.x > 0 && target.position.x < 53)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
    }

    public void FollowTarget()
    {
        transform.position = new Vector3(0, 0, -10);
    }


}


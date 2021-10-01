using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Transform player;
    private Vector3 currentVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 finalPos = player.position;
        finalPos.x = transform.position.x;
        finalPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref currentVelocity, .02f, 10);
    }
}

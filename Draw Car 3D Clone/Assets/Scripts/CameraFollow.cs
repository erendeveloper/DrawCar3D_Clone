using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on Main Camera
public class CameraFollow : MonoBehaviour
{
    private Transform car;

    private float offsetY = 4f;

    private void Awake()
    {
        car = GameObject.FindGameObjectWithTag("Car").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = new Vector3(car.position.x, car.position.y + offsetY, transform.position.z);
    }
}

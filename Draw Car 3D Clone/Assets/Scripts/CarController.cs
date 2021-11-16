using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Added on Car GameObject which is a player

[System.Serializable]
public class AxleInfo
{
    public WheelCollider wheelCollider; //only collider
    public Transform wheelVisual; //only mesh
}

public class CarController : MonoBehaviour
{
    //Access Game Manager
    GameManager gameManagerScript;

    public List<AxleInfo> axleInfos;
    private float motorTorque = 2000f;

    Rigidbody carRigidbody;

    private void Awake()
    {
        gameManagerScript = Camera.main.GetComponent<GameManager>();
        carRigidbody = this.GetComponent<Rigidbody>();
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        if (gameManagerScript.GamePlay)
        {

            foreach (AxleInfo axleInfo in axleInfos)
            {

                axleInfo.wheelCollider.motorTorque = motorTorque;

                Vector3 position;    //ref
                Quaternion rotation; //ref
                axleInfo.wheelCollider.GetWorldPose(out position, out rotation);

                axleInfo.wheelVisual.transform.rotation = rotation;
            }
        }
        
    }

    public void EnableGravity()//after drawing, car falls down
    {
        carRigidbody.useGravity = true;
    }
}
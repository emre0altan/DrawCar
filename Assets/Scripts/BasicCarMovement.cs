using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCarMovement : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float motorValue = 200f;
    public float steeringValue;

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.wheel.steerAngle = steeringValue;
            }
            if (axleInfo.motor)
            {
                axleInfo.wheel.motorTorque = motorValue;
            }
        }
    }
}
[System.Serializable]
public class AxleInfo
{
    public WheelCollider wheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
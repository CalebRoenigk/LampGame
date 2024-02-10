using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationalTravel = 0;
    public float rotationalSpeedRamp = 10f;
    public float maxRotationalSpeed = 15f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rotationalTravel += rotationalSpeedRamp;
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            rotationalTravel -= rotationalSpeedRamp;
        }
        else
        {
            rotationalTravel = Mathf.Lerp(rotationalTravel, 0f, 0.075f);
            if (rotationalTravel <= 0.1f && rotationalTravel >= -0.1f)
            {
                rotationalTravel = 0f;
            }
        }
        

        rotationalTravel = Mathf.Clamp(rotationalTravel, -maxRotationalSpeed, maxRotationalSpeed);
        float rotationalDifference = rotationalTravel * Time.fixedDeltaTime;

        transform.eulerAngles = transform.eulerAngles + new Vector3(0f, rotationalDifference, 0f);

        if (rotationalTravel != 0f)
        {
            PlayerControlsUIManager.Instance.UpdatePlayerMovementUI();
        }
    }
}

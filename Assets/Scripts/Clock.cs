using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;

    public bool continuous;
    
    const float degreesPerHour = 30f;
    const float degreesPerMinute = 6f;
    const  float degreesPerSecond = 6f;
    
    // Start is called before the first frame update
    private void Update()
    {
        DateTime time = DateTime.Now;
        Debug.Log("current time: " + time.TimeOfDay);
        hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f); 
        minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
    }
}

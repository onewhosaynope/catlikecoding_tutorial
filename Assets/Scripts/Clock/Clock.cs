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
    
    
    /*
     private void Update()
    {
        DateTime time = DateTime.Now;
        Debug.Log("current time: " + time.TimeOfDay);
        hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f); 
        minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
    }
    */

    void UpdateContinuous()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        
        /*
         
        https://habr.com/ru/post/426863/
         
        Rotations are stored in Unity as quaternions.
        We can create one via the publicly-available Quaternion.Euler method. 
        It has regular angles for the X, Y, and Z axis as parameters and produces an appropriate quaternion.
        
        Quaternions are based on complex numbers and are used to represent 3D rotations. 
        While harder to understand than simple 3D vectors, they have some useful characteristics. 
        For example, they don't suffer from gimbal lock.
        
        UnityEngine.Quaternion is used as a simple value. It is a structure, not a class.
         
         */
        
        /*
        localRotation refers to the actual rotation of a transform component, independent of the rotation of its parents. 
        In other words, it is the rotation in the object's local space. 
        It's what gets displayed in its transform component in the inspector. 
        So if we were to rotate the clock's root object, its arms would rotate along with it, as we would expect.
        
        There is also a rotation property. 
        It refers to the final rotation of a transform component in world space, taking the transformations of its parents into account. 
        Had we used that, the arms would not adjust when we rotate the clock, as its rotation will be compensated for.

         */
        hoursTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSecond, 0f);
    }

    void UpdateDiscrete()
    {
        DateTime time = DateTime.Now;
        hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
    }

    private void Update()
    {
        if (continuous)
        {
            UpdateContinuous();
        }
        else
        {
            UpdateDiscrete();
        }
    }
}

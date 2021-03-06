﻿using UnityEngine;

public class MovingSpherePhysics : MonoBehaviour {

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f;

    Vector3 velocity;

    void Update () {
        Vector2 playerInput;
        
        // gets key input (wasd or arrows)
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        // ClampMagnitude - returns a copy of vector with its magnitude clamped to maxLength.
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;
        Vector3 newPosition = transform.localPosition + displacement;
        
        //if (newPosition.x < allowedArea.xMin) {
        //	newPosition.x = allowedArea.xMin;
        //	velocity.x = -velocity.x * bounciness;
        //}
        //…
        transform.localPosition = newPosition;
        
        /*if (newPosition.z < allowedArea.yMin) {
            newPosition.z = allowedArea.yMin;
            velocity.z = -velocity.z * bounciness;
        } else if (newPosition.z > allowedArea.yMax) {
            newPosition.z = allowedArea.yMax;
            velocity.z = -velocity.z * bounciness;
        }*/

        transform.localPosition = newPosition;
    }
}
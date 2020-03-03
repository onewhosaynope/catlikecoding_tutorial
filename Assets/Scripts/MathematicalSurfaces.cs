using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathematicalSurfaces : MonoBehaviour
{
    public Transform pointPrefab;
    Transform[] points;
    
    static GraphFunction[] functions = {
        SineFunction, Sine2DFunction, MultiSineFunction
    };
    
    [Range(10, 100)]
    public int resolution = 50;

    public GraphFunctionName function;
    
    void Awake () {
        
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        
        Vector3 position;
        position.y = 0f;
        position.z = 0f;
        
        points = new Transform[resolution * resolution];
    
        /*for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            
            if (x == resolution) {
                x = 0;
                z += 1;
            }
            
            Transform point = Instantiate(pointPrefab);
        
            position.x = (x + 0.5f) * step - 1f;
            position.z = (z + 0.5f) * step - 1f;
            
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
            
            points[i] = point;
        }*/
        
        
        // just to simplify iteration between i and z dimensions
        for (int i = 0, z = 0; z < resolution; z++) {
            position.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++) {
                Transform point = Instantiate(pointPrefab);
                position.x = (x + 0.5f) * step - 1f;
                point.localPosition = position;
                point.localScale = scale;
                point.SetParent(transform, false);
                points[i] = point;
            }
        }
    }
    
    void Update () {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            
            position.y = f(position.x, position.z, t);
            
            point.localPosition = position;
        }
    }


    private const float pi = Mathf.PI;
    
    /*
     static method can be called other objects in scripts just like 
        Graph.SineFunction(0f, 0f)
    */
    static float SineFunction (float x, float z, float t) {
        return Mathf.Sin(pi * (x + t));
    }

    static float MultiSineFunction (float x, float z, float t) {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
        return y;
    }
    
    static float Sine2DFunction (float x, float z, float t) {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f;
        return y;
    }
}
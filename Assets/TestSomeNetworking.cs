using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSomeNetworking : MonoBehaviour
{
    public VREventNetworker vrEventNetworker;
    public Transform cubeTransform;

    public NetworkVariable<Vector3> cubePosition;

    public NetworkAction<string> testAction; 

    // Should do this in Start, to init after VREventNetworker
    void Awake()
    {
        cubePosition = new NetworkVariable<Vector3>(default, nameof(cubePosition), vrEventNetworker, true);
        // cubePosition.OnValueChanged += UpdateCubePosition;

        // Nicer way to do this: attach this to the cube, the OnEnable() subscribe to the valuechanged event, then unsub when cube is destroyed OnDisable()
    
        testAction = new NetworkAction<string>(nameof(testAction), vrEventNetworker, true);
    }

    
    void Update()
    {
        cubePosition.Value = cubeTransform.position;
        testAction.Invoke("cube moved!");
    }
}

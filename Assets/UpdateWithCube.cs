using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWithCube : MonoBehaviour
{
    public TestSomeNetworking testSomeNetworking;
    public Transform cube;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - cube.position;
        testSomeNetworking.cubePosition.OnValueChanged += UpdateCubePosition;
    }

    void UpdateCubePosition(Vector3 cubePos)
    {
        transform.position = cubePos + offset;
    }

}

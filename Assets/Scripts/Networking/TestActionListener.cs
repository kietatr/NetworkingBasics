using UnityEngine;

public class TestActionListener : MonoBehaviour
{
    public TestSomeNetworking testSomeNetworking;

    void Start()
    {
        testSomeNetworking.testAction.Action += (x) => Debug.Log(x);
    }
}
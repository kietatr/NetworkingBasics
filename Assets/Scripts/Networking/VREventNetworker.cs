using System;
using System.Collections;
using System.Collections.Generic;
using IVLab.MinVR3;
using UnityEngine;

[RequireComponent(typeof(IVREventConnection))]
public class VREventNetworker : MonoBehaviour
{
    private IVREventConnection _connection;
    private const char ID_DELIMETER = '/';
    private static string _uid;

    public event Action<VREvent> OnVREventReceived;

    public static string MyUid => _uid;  // TODO: This is bad practice... but the demo is a week out :)

    public static string GetSenderUid(VREvent vrEvent)
    {
        string[] splitEventName = vrEvent.name.Split(ID_DELIMETER);
        return splitEventName[splitEventName.Length - 1];
    }

    public static bool SentFromThisMachine(string senderUid)
    {
        return senderUid == MyUid;
    }

    public static bool SentFromThisMachine(VREvent vrEvent)
    {
        return SentFromThisMachine(GetSenderUid(vrEvent));
    }

    private void Awake()
    {
        // Get connection
        _connection = GetComponent<IVREventConnection>();
        if (_connection == null)
            Debug.LogError("VREventNetworker requires a component of type IVREventConnection");
        _connection.OnVREventReceived += vrEvent => OnVREventReceived?.Invoke(vrEvent);
        // Set unique identifier
        _uid = Guid.NewGuid().ToString();
    }

    public void SendWithUid(VREvent vrEvent)
    {
        // Append user id to event name
        vrEvent.name += ID_DELIMETER + MyUid;
        // Send the event using the network connection
        _connection.Send(vrEvent);
    }

    // public void Send(VREvent vrEvent)
    // {
    //     // Send the event using the network connection
    //     _connection.Send(vrEvent);
    // }
}
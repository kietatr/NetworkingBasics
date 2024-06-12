using System;
using IVLab.MinVR3;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkAction<T> : INetworkNotifier
{
    protected string _actionName;
    protected VREventNetworker _vrEventNetworker;
    public event Action<T> Action;

    public void Invoke(T value)
    {
        Action?.Invoke(value);
        if (_vrEventNetworker != null && NotifyNetwork)
        {
            switch (value)
            {
                case Vector2 valueAsVector2:
                    _vrEventNetworker.SendWithUid(new VREventVector2(_actionName, valueAsVector2));
                    break;
                case Vector3 valueAsVector3:
                    _vrEventNetworker.SendWithUid(new VREventVector3(_actionName, valueAsVector3));
                    break;
                case Vector4 valueAsVector4:
                    _vrEventNetworker.SendWithUid(new VREventVector4(_actionName, valueAsVector4));
                    break;
                case Quaternion valueAsQuaternion:
                    _vrEventNetworker.SendWithUid(new VREventQuaternion(_actionName, valueAsQuaternion));
                    break;
                case GameObject valueAsGameObject:
                    _vrEventNetworker.SendWithUid(new VREventGameObject(_actionName, valueAsGameObject));
                    break;
                case Single valueAsSingle:
                    _vrEventNetworker.SendWithUid(new VREventFloat(_actionName, valueAsSingle));
                    break;
                case Int32 valueAsInt32:
                    _vrEventNetworker.SendWithUid(new VREventInt(_actionName, valueAsInt32));
                    break;
                case String valueAsString:
                    _vrEventNetworker.SendWithUid(new VREventString(_actionName, valueAsString));
                    break;
                default:
                    break;
            }
        }
    }

    public bool NotifyNetwork { get; set; }

    /// <summary>
    /// Constructs a network action with unique name.
    /// </summary>
    /// <param name="actionName">Action name. Must be unique from all other
    /// network variable and action names</param>
    /// <param name="vrEventNetworker">Networker responsible for sending vr events</param>
    /// <param name="notifyNetwork">Whether or not invoking the action will
    /// notify the network</param>
    public NetworkAction(string actionName, VREventNetworker vrEventNetworker, bool notifyNetwork = false)
    {
        _actionName = actionName;
        _vrEventNetworker = vrEventNetworker;
        NotifyNetwork = notifyNetwork;

        // Subscribe to receive networked vr events
        if (_vrEventNetworker != null)
            _vrEventNetworker.OnVREventReceived += HandleVREventReceived;
    }

    protected void HandleVREventReceived(VREvent vrEvent)
    {
        if (vrEvent.name.StartsWith(_actionName))
        {
            if (VREventNetworker.SentFromThisMachine(vrEvent)) return;
            // Temporarily disable notifying the network since we don't want
            // to trigger an infinite loop
            bool notifyNetworkValue = NotifyNetwork;
            NotifyNetwork = false;
            Invoke(vrEvent.GetData<T>());
            NotifyNetwork = notifyNetworkValue;
        }
    }
}

public class NetworkAction : INetworkNotifier
{
    protected string _actionName;
    protected VREventNetworker _vrEventNetworker;

    public event Action Action;

    public void Invoke()
    {
        Action?.Invoke();
        if (_vrEventNetworker != null && NotifyNetwork)
        {
            _vrEventNetworker.SendWithUid(new VREvent(_actionName));
        }
    }

    public bool NotifyNetwork { get; set; }

    public NetworkAction(string actionName, VREventNetworker vrEventNetworker, bool notifyNetwork = false)
    {
        _actionName = actionName;
        _vrEventNetworker = vrEventNetworker;
        NotifyNetwork = notifyNetwork;

        // Subscribe to receive networked vr events
        if (_vrEventNetworker != null)
            _vrEventNetworker.OnVREventReceived += HandleVREventReceived;
    }

    protected void HandleVREventReceived(VREvent vrEvent)
    {
        if (vrEvent.name.StartsWith(_actionName))
        {
            if (VREventNetworker.SentFromThisMachine(vrEvent)) return;
            // Temporarily disable notifying the network since we don't want
            // to trigger an infinite loop
            bool notifyNetworkValue = NotifyNetwork;
            NotifyNetwork = false;
            Invoke();
            NotifyNetwork = notifyNetworkValue;
        }
    }
}
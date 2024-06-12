using System;
using System.Collections;
using System.Collections.Generic;
using IVLab.MinVR3;
// using IVLab.Plotting;
using Newtonsoft.Json;
using UnityEngine;


public interface INetworkNotifier
{
    public bool NotifyNetwork { get; set; }
}

/// <summary>
/// A reference variable which notifies all listeners when it is changed,
/// including those listening over the network. Networking is done with
/// <see cref="VREvent"/> via <see cref="VREventNetworker"/>.
/// </summary>
/// <typeparam name="T">Limited to <see cref="Vector2"/>, <see cref="Vector3"/>,
/// <see cref="Vector4"/>, <see cref="Quaternion"/>, <see cref="GameObject"/>,
/// <see cref="Single"/>, <see cref="Int32"/> and <see cref="String"/> since
/// those are the only types currently supported by <see cref="VREvent"/>.
/// If not initialized with one of these types, it will instead attempt to
/// serialize it as JSON using <see cref="JsonConvert"/> and send it
/// over the network as a <see cref="VREventString"/>.</typeparam>
public class NetworkVariable<T> : INetworkNotifier
{
    protected T _value;
    protected string _variableName;
    protected VREventNetworker _vrEventNetworker;
    protected bool _unsupportedType;

    /// <summary>
    /// Current value of the variable.
    /// </summary>
    public T Value
    {
        get => _value;
        set
        {
            // Update the local value and notify
            _value = value;
            OnValueChanged?.Invoke(_value);
            // Update network if network writing is enabled
            if (_vrEventNetworker != null && NotifyNetwork)
            {
                switch (_value)
                {
                    case Vector2 valueAsVector2:
                        _vrEventNetworker.SendWithUid(new VREventVector2(_variableName, valueAsVector2));
                        break;
                    case Vector3 valueAsVector3:
                        _vrEventNetworker.SendWithUid(new VREventVector3(_variableName, valueAsVector3));
                        break;
                    case Vector4 valueAsVector4:
                        _vrEventNetworker.SendWithUid(new VREventVector4(_variableName, valueAsVector4));
                        break;
                    case Quaternion valueAsQuaternion:
                        _vrEventNetworker.SendWithUid(new VREventQuaternion(_variableName,
                            valueAsQuaternion));
                        break;
                    case GameObject valueAsGameObject:
                        _vrEventNetworker.SendWithUid(new VREventGameObject(_variableName,
                            valueAsGameObject));
                        break;
                    case Single valueAsSingle:
                        _vrEventNetworker.SendWithUid(new VREventFloat(_variableName, valueAsSingle));
                        break;
                    case Int32 valueAsInt32:
                        _vrEventNetworker.SendWithUid(new VREventInt(_variableName, valueAsInt32));
                        break;
                    case String valueAsString:
                        _vrEventNetworker.SendWithUid(new VREventString(_variableName, valueAsString));
                        break;
                    default:
                        string serializedValue = JsonConvert.SerializeObject(_value);
                        _vrEventNetworker.SendWithUid(new VREventString(_variableName, serializedValue));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Event invoked whenever the value of the variable is updated,
    /// whether locally or over the network.
    /// </summary>
    public event Action<T> OnValueChanged;

    /// <summary>
    /// Whether or not changing the variable's value will also
    /// write that change to the entire network. 
    /// </summary>
    public bool NotifyNetwork { get; set; }

    /// <summary>
    /// Constructs a network variable with a default value and unique name.
    /// </summary>
    /// <param name="defaultValue">Default value</param>
    /// <param name="variableName">Variable name. Must be unique from all other
    /// network variable and action names</param>
    /// <param name="vrEventNetworker">Networker responsible for sending vr events</param>
    /// <param name="notifyNetwork">Whether or not changing the variables value
    /// will update the entire network</param>
    /// <exception cref="ArgumentException">Thrown if not of supported type</exception>
    public NetworkVariable(T defaultValue, string variableName, VREventNetworker vrEventNetworker, bool notifyNetwork = false)
    {
        _value = defaultValue;
        _variableName = variableName;
        _vrEventNetworker = vrEventNetworker;
        NotifyNetwork = notifyNetwork;

        // Subscribe to receive networked vr events
        if (_vrEventNetworker != null)
            _vrEventNetworker.OnVREventReceived += HandleVREventReceived;

        // Throw an error on creation if this network variable
        // was created with an invalid generic type
        switch (_value)
        {
            case Vector2 valueAsVector2:
                break;
            case Vector3 valueAsVector3:
                break;
            case Vector4 valueAsVector4:
                break;
            case Quaternion valueAsQuaternion:
                break;
            case GameObject valueAsGameObject:
                break;
            case Single valueAsSingle:
                break;
            case Int32 valueAsInt32:
                break;
            case String valueAsString:
                break;
            default:
                _unsupportedType = true;
                break;
        }
    }

    protected void HandleVREventReceived(VREvent vrEvent)
    {
        if (vrEvent.name.StartsWith(_variableName))
        {
            if (VREventNetworker.SentFromThisMachine(vrEvent)) return;
            // Temporarily disable writing to network since we don't want
            // to trigger an infinite loop of writing everything we received
            // from the network back to the network
            bool notifyNetworkValue = NotifyNetwork;
            NotifyNetwork = false;
            Value = _unsupportedType ? JsonConvert.DeserializeObject<T>(vrEvent.GetJsonData()) : vrEvent.GetData<T>();
            NotifyNetwork = notifyNetworkValue;
        }
    }
}

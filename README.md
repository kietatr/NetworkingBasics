# NetworkingBasics
Minimal working Unity project that uses MinVR3 and Networking


## Setting up MinVR3 project

- Create a new Unity project (select the 3D Built-In Renderer Pipeline)
- From Unity's Package Manager, install these packages:
    - MinVR3 Unity package (Follow this readme: https://github.com/ivlab/MinVR3-UnityPackage)
    - Input System
    - OpenXR Plugin
    - Newtonsoft Json (Instructions: https://github.com/applejag/Newtonsoft.Json-for-Unity?tab=readme-ov-file)
- Go to Edit > Project Settings: 
    - Click on XR Plug-In Management > Check the box OpenXR
    - Click on OpenXR
    - Choose RenderMode: Multi-pass
    - Click on the Plus (+) button in Interaction Profiles, add your controller (e.g. Oculus Touch for Meta Quests)
- In your scene hierarchy:
    - Add a VRConfig game object with: Right-click > MinVR > VR Config > VRConfig_UnityXR
    - Click on the VREngine game object, from the VR Config Manager component, click on the dropdown and select VRConfig_UnityXR


## Set Up Networking

- Create an empty gameobject, name it Networking
- Create a child game object, name it VR Event Networker. Then for this child object, add these components:
    - `TcpJsonVREventConnection`
    - `VR Event Networker`


// // Basic structure of how a networked cube should work
// using UnityEngine;

// public class MovableNetworkedCube : MonoBehaviour
// {
//     public VREventNetworker vrEventNetworker;

//     public NetworkVariable<Vector3> cubePosition;

    

//     // Should do this in Start, to init after VREventNetworker
//     void Awake()
//     {
//         cubePosition = new NetworkVariable<Vector3>(default, nameof(cubePosition), vrEventNetworker, true);
//         cubePosition.OnValueChanged += HandleCubePositionUpdated;

        
//     }

//     void Update()
//     {
//         HandleCubeMovement();
//     }

//     void HandleCubeMovement()
//     {
//         // if (vrEventNetworker.MyUid != ownerUid) return;  // Only person with ownerUid can move this cube

//         Vector3 newCubePosition;
        
//        // Collision detection on cursors vs cube

//        // Checking delta movement since last frame

//        // Handle Rotation

//        // Updates the cube position based on movement/grabbing/rotation
//         cubePosition.Value = newCubePosition;
//     }

//     void HandleCubePositionUpdated(Vector3 newPosition)
//     {
//         transform.position = newPosition;
//     }
// }
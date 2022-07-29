using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Grab;
using Oculus.Interaction.HandPosing;
using Oculus.Interaction.HandPosing.Visuals;
using Oculus.Interaction.Input;
using UnityEngine;

public class GrabPoseVisualizer : MonoBehaviour
{
    [Header("Root of the object that stores the grab pose")]
    [SerializeField] private GrabInteractable _grabInteractable;
    
    [SerializeField, Optional]
    [Tooltip("Prototypes of the static hands (ghosts) that visualize holding poses")]
    private HandGhostProvider _ghostProvider;
    
    [Header("1a - During Play mode start visualizing the grab pose")]
    [SerializeField]
    private KeyCode _visualizeKey = KeyCode.V;
    
    [Header("1b - During Play mode stop the pose visualization")]
    [SerializeField]
    private KeyCode _visualizeStopKey = KeyCode.Space;
    
    private void Awake()
    {
        if (_ghostProvider == null)
        {
            HandGhostProvider.TryGetDefault(out _ghostProvider);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(_visualizeKey))
        {
            Logger.Instance.LogInfo($"Visualizing started");
            VisualizeGrabPose(true);
        }

        if (Input.GetKeyDown(_visualizeStopKey))
        {
            Logger.Instance.LogInfo($"Visualizing stopped");
            VisualizeGrabPose(false);
        }
    }

    private void VisualizeGrabPose(bool start)
    {
        if (start)
        {
            foreach (HandGrabPoint snap in _grabInteractable.GetComponentsInChildren<HandGrabPoint>(false))
            {
                AttachGhost(snap);
            }
        }
        else
        {
            foreach (HandGrabPoint snap in _grabInteractable.GetComponentsInChildren<HandGrabPoint>(false))
            {
                DetachGhost(snap);
            }
        }
    }
    
    private void AttachGhost(HandGrabPoint point)
    {
        if (_ghostProvider != null)
        {
            HandGhost ghostPrefab = _ghostProvider.GetHand(point.HandPose.Handedness);
            HandGhost ghost = GameObject.Instantiate(ghostPrefab, point.transform);
            ghost.SetPose(point);
        }
    }

    private void DetachGhost(HandGrabPoint point)
    {
        foreach (HandGhost pose in point.GetComponentsInChildren<HandGhost>())
        {
            DestroyImmediate(pose.gameObject);
        }
    }
}

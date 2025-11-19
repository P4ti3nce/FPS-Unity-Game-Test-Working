using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class Switchcam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerinput;
    [SerializeField]
    private int priorityBoostTotal = 10;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    private CinemachineVirtualCamera virtualCam;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerinput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }
    private void Ondisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
    private void StartAim()
    {
        virtualCam.Priority += priorityBoostTotal;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }
    private void CancelAim()
    {
        virtualCam.Priority -= priorityBoostTotal;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}

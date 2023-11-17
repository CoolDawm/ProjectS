using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private CinemachineFreeLook _cinemachineFreeLook;
    private float zoomSpeed = 0.25f; 

    void Start()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }
    
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            for (int i = 0; i < _cinemachineFreeLook.m_Orbits.Length; i++)
            {
                CinemachineFreeLook.Orbit orbit = _cinemachineFreeLook.m_Orbits[i];
                orbit.m_Radius -= zoomSpeed; 
                _cinemachineFreeLook.m_Orbits[i] = orbit;
            }
        }
        else if (scroll < 0f)
        {
            for (int i = 0; i < _cinemachineFreeLook.m_Orbits.Length; i++)
            {
                CinemachineFreeLook.Orbit orbit = _cinemachineFreeLook.m_Orbits[i];
                orbit.m_Radius += zoomSpeed; 
                _cinemachineFreeLook.m_Orbits[i] = orbit;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private CinemachineFreeLook _cinemachineFreeLook;
    private float zoomSpeed = 0.5f;
    private float currentZoomX;
    private CinemachineFreeLook.Orbit _orbit;
    void Start()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        currentZoomX=_cinemachineFreeLook.m_Orbits[1].m_Radius;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        CinemachineFreeLook.Orbit orbit= _cinemachineFreeLook.m_Orbits[1];
        _orbit = _cinemachineFreeLook.m_Orbits[1];
        if (scroll > 0f)
        {
            if (_orbit.m_Radius > 5.5f)
            {
                for (int i = 0; i < _cinemachineFreeLook.m_Orbits.Length; i++)
                {
                    orbit = _cinemachineFreeLook.m_Orbits[i];
                    if (orbit.m_Radius - zoomSpeed >= 2f)
                    {
                  
                        orbit.m_Radius -= zoomSpeed;
                        _cinemachineFreeLook.m_Orbits[i] = orbit;
                    }
                }
            }
        }
        else if (scroll < 0f)
        {
            if ( _orbit.m_Radius <10f)
            {
                for (int i = 0; i < _cinemachineFreeLook.m_Orbits.Length; i++)
                {
                    orbit = _cinemachineFreeLook.m_Orbits[i];
                    if (orbit.m_Radius + zoomSpeed <= 10f) 
                    {
                        orbit.m_Radius += zoomSpeed;
                        _cinemachineFreeLook.m_Orbits[i] = orbit;
                    }
                }
            }
           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private CinemachineFreeLook _cinemachineFreeLook;
    private float zoomSpeed = 0.25f;
    private float currentZoomX;
    void Start()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        currentZoomX=_cinemachineFreeLook.m_Orbits[1].m_Radius;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        CinemachineFreeLook.Orbit orbit= _cinemachineFreeLook.m_Orbits[1];
        if (scroll > 0f)
        {
            if (currentZoomX / orbit.m_Radius <= 3)
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
            if ( orbit.m_Radius/currentZoomX <= 2)
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

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVPSystems
{
    public class AddPlayerToCamera : MonoBehaviour
    {
        private GameObject _player;
        // Start is called before the first frame update
        void Start()
        {
            _player=GameObject.FindGameObjectWithTag("Player");
            GetComponent<CinemachineFreeLook>().Follow = _player.transform;
            GetComponent<CinemachineFreeLook>().LookAt = _player.transform;
        }

        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class DirectInteraction : MonoBehaviour
    {
        public event Action<bool> OnChangedRay;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Hands"))
            {
                Debug.Log("Activate Direct Ray");
                CharacterControllManager.Instance.distanceRayLeftController.gameObject.SetActive(false);
                CharacterControllManager.Instance.distanceRayRightController.gameObject.SetActive(false);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Hands"))
            {
                Debug.Log("Activate Distance Ray");
                CharacterControllManager.Instance.distanceRayLeftController.gameObject.SetActive(true);
                CharacterControllManager.Instance.distanceRayRightController.gameObject.SetActive(true);
            }
        }
    }
}

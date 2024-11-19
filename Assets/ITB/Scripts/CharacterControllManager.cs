using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class CharacterControllManager : MonoBehaviour
    {
        [Header("Reference to teleport movement")]
        public GameObject isTeleport;
        [Header("Reference to contonuos movement")]
        public GameObject isContinous;
        public static CharacterControllManager instance;
        public ActionBasedController leftController;
        public ActionBasedController rightController;
        public InputActionManager inputActionManager;

        public enum Hand { left, right};
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(instance);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //isContinous.SetActive(!isTeleport.activeSelf);
        }
        // Método para suscribir el conjunto de eventos (started, performed, canceled) a una acción de entrada.
        public void OnSuscribedEvents(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.performed += context => callback(context);
            action.canceled += context => callback(context);
        }



    }
}

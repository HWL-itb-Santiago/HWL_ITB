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
        [Header("Reference to Distance Ray Interactor Controller")]
        public XRRayInteractor distanceInteractorRay;

        [Header("Reference to Direct Ray Interactor Controller")]
        public XRDirectInteractor directInteractorRay;

        [Header("Reference to Teleport Controller")]
        public ActionBasedController isTeleport;
        [Header("Reference to Movement Provider")]
        public ContinuousMoveProviderBase isContinuous;

        [Header("Reference to SnapTurn Provider")]
        public SnapTurnProviderBase isSnapTurn;
        [Header("Reference to ContinuousTurn Provider")]
        public ContinuousTurnProviderBase isContinuousTurn;

        [Header("Reference to Player Controllers")]
        public ActionBasedController leftController;
        public ActionBasedController rightController;

        [Header("Reference to Input Action Manager")]
        public InputActionManager inputActionManager;

        public static CharacterControllManager instance;
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
            OnSuscribedEvents(leftController.activateAction.action, OnDistanceRay);
            OnSuscribedEvents(rightController.activateAction.action, OnTeleportMode);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnTeleportMode(InputAction.CallbackContext contex)
        {
            distanceInteractorRay.gameObject.SetActive(false);
            isContinuous.gameObject.SetActive(!isTeleport.gameObject.activeSelf);
            isSnapTurn.gameObject.SetActive(!isContinuous.gameObject.activeSelf);
            isContinuousTurn.gameObject.SetActive(!isSnapTurn.gameObject.activeSelf);
        }
        private void OnDistanceRay(InputAction.CallbackContext context)
        {
            distanceInteractorRay.gameObject.SetActive(!distanceInteractorRay.gameObject.activeSelf);
            directInteractorRay.gameObject.SetActive(!distanceInteractorRay.gameObject.activeSelf);
        }
        // Método para suscribir el conjunto de eventos (started, performed, canceled) a una acción de entrada.
        public void OnSuscribedEvents(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.performed += context => callback(context);
            action.canceled += context => callback(context);
        }
    }
}

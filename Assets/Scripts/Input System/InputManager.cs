using System;
using DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        #region Variables
        public static InputManager instance;

        public static Action<EnableInteractUI> IHavePressedInteractButton;

        [Header("Input Actions")]
        public InputAction moveAction;
        public InputAction aimAction;
        public InputAction interactAction;
        public InputAction pauseAction;
        public InputAction crouchAction;
        public InputAction sprintAction;
        public InputAction flashLightAction;
        public InputAction inventoryAction;
        public InputAction cancelAction;
        
        [Header("VISIBLE FOR DEBUG PURPOSES")]
        [SerializeField] private Vector2 moveAmount;
        [SerializeField] private Vector2 aimAmount;
        [SerializeField] private bool isPause;
        [SerializeField] private bool isSprint;
        [SerializeField] private bool isCrouch;
        [SerializeField] private bool isFlashLight;
        public bool isInMenu;
        
        private bool isInInteractableArea = false;

        private EnableInteractUI currentInteractable;
        
        

        #endregion
        
        // Start is called before the first frame update
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            // Assign callbacks for actions
            interactAction.performed += ctx => { OnInteract(ctx); };
            
            pauseAction.performed += ctx => { OnPause(ctx); };
            
            crouchAction.performed += ctx => { OnCrouch(ctx); };
            
            sprintAction.performed += ctx => { OnSprint(ctx); };
            sprintAction.canceled += ctx => { OnSprint(ctx); };

            flashLightAction.performed += ctx => { OnFlashLight(ctx); };

            inventoryAction.performed += ctx => { OnInventory(ctx); };

            cancelAction.performed += ctx => { OnCancelled(ctx); };
        }
        
        #region Getters
        public bool getIsPause()
        {
            return isPause;
        }

        public bool getIsCrouch()
        {
            return isCrouch;
        }

        public bool getSprintHeld()
        {
            return isSprint;
        }

        public bool getFlashlight()
        {
            return isFlashLight;
        }
        
        public Vector2 getMoveAmount()
        {
            return moveAmount;
        }

        public Vector2 getAimAmount()
        {
            return aimAmount;
        }
        
        #endregion
        
        // Update is called once per frame
        void Update()
        {
            // Read Value for action each frame
            moveAmount = moveAction.ReadValue<Vector2>();
            aimAmount = aimAction.ReadValue<Vector2>().normalized;
        }
        
        #region Input System Callbacks
        
        // Controls interaction input
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!isInInteractableArea) return;

            if (!isInMenu)
            {
                IHavePressedInteractButton.Invoke(currentInteractable);
            }
            else
            {
                if (DialogueManager.instance.GetIsActive())
                {
                    if (DialogueManager.instance.GetIsInDialogue())
                    {
                        DialogueManager.instance.advanceToNextDialogueLine();
                    }
                    else
                    {
                        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
                        
                        if (selectedObject == null)
                        {
                            EventSystem.current.SetSelectedGameObject(DialogueManager.instance.dialogueButtons[0].gameObject);
                            selectedObject = EventSystem.current.currentSelectedGameObject;
                            
                        }
                        
                        selectedObject.GetComponent<DialogueButton>().PressButton();
                    }
                }
            }
            
        }

        // Controls Pausing input
        public void OnPause(InputAction.CallbackContext context)
        {
            isPause = !isPause;
            
            if (isPause)
            {
                DisableCharacterInputs();
                cancelAction.Disable(); 
            }
            else
            {
                cancelAction.Enable(); 
            }

            if (!isInMenu && !isPause)
            {
                EnableCharacterInputs();
            }
        }

        // Controls crouching input
        public void OnCrouch(InputAction.CallbackContext context)
        {
            isCrouch = !isCrouch;

            if (isCrouch) { sprintAction.Disable(); }
            else { sprintAction.Enable();}
        }

        // Controls sprinting input
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isSprint = true;
            } else if (context.canceled)
            {
                isSprint = false;
            }
            
            if (isSprint) { crouchAction.Disable(); }
            else { crouchAction.Enable(); }
        }

        // Controls enabling flashlight input
        public void OnFlashLight(InputAction.CallbackContext context)
        {
            isFlashLight = !isFlashLight;
            
        }

        // Controls opening Inventory input
        public void OnInventory(InputAction.CallbackContext context)
        {
            isInMenu = true;
            DisableCharacterInputs();
            Debug.Log("Inventory Button Pressed");
        }

        // Controls canceling out of menus input
        public void OnCancelled(InputAction.CallbackContext context)
        {
            EnableCharacterInputs();
            DialogueManager.instance.disableDialogueUI();
        }

        // Disables all inputs the player can use in general gameplay
        public void DisableCharacterInputs()
        {
            moveAction.Disable();
            aimAction.Disable();
            crouchAction.Disable();
            sprintAction.Disable();
            flashLightAction.Disable();
            inventoryAction.Disable();
            cancelAction.Enable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Enables all inputs the player can use in general gameplay
        public void EnableCharacterInputs()
        {
            isInMenu = false;
            
            moveAction.Enable();
            aimAction.Enable();
            crouchAction.Enable();
            sprintAction.Enable();
            flashLightAction.Enable();
            inventoryAction.Enable();
            cancelAction.Disable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void SetInteractable(bool context, EnableInteractUI enableInteractUI)
        {
            isInInteractableArea = context;
            currentInteractable = enableInteractUI;
        }
        
        public void OnEnable()
        {
            interactAction.Enable();
            moveAction.Enable();
            aimAction.Enable();
            pauseAction.Enable();
            crouchAction.Enable();
            sprintAction.Enable();
            flashLightAction.Enable();
            inventoryAction.Enable();
            EnableInteractUI.ImInInteractionZone += SetInteractable;
        }

        public void OnDisable()
        {
            interactAction.Disable();
            moveAction.Disable();
            aimAction.Disable();
            pauseAction.Disable();
            crouchAction.Disable();
            sprintAction.Disable();
            flashLightAction.Disable();
            inventoryAction.Disable();
            EnableInteractUI.ImInInteractionZone -= SetInteractable;
        }
        
        #endregion
    }
    
}

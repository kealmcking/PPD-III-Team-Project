using System;
using DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        #region Variables
        public static InputManager instance;
       
        [Header("Input Actions")]
        public InputAction moveAction;
        public InputAction aimAction;
        public InputAction interactAction;
        public InputAction pauseAction;
        public InputAction crouchAction;
        public InputAction sprintAction;
        public InputAction inventoryAction;
        public InputAction flashlightAction;
        public InputAction cancelAction;
        public InputAction useAction;
        public InputAction dropAction;
        public InputAction throwAction;
        public InputAction skipCutSceneAction;
        [Header("VISIBLE FOR DEBUG PURPOSES")]
        [SerializeField] private Vector2 moveAmount;
        [SerializeField] private Vector2 aimAmount;
       // [SerializeField] private bool isPause;
        [SerializeField] private bool isSprint;
        [SerializeField] private bool isCrouch;     
        
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

            inventoryAction.performed += ctx => { OnInventory(ctx); };

            cancelAction.performed += ctx => { OnCancelled(ctx); };
            useAction.performed += ctx => { OnUse(ctx); };
            dropAction.performed += ctx => { OnDrop(ctx); };
            throwAction.performed += ctx => { OnThrow(ctx); };
            skipCutSceneAction.performed += ctx => { OnSkipCutScene(ctx); };
        }

      

        #region Getters

        public bool getIsCrouch()
        {
            return isCrouch;
        }

        public bool getSprintHeld()
        {
            return isSprint;
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

            if (GameManager.instance == null) return;
            if(!GameManager.instance.InventoryActive)
            aimAmount = aimAction.ReadValue<Vector2>().normalized;
        }
        
        #region Input System Callbacks
        
        //Controls skipping cutscene for main menu
        public void OnSkipCutScene(InputAction.CallbackContext context)
        {
            PlayerableDirectorManager.instance.SkipCutScene();
            skipCutSceneAction.Disable();
        }

        // Controls interaction input
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!isInInteractableArea) return;

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
            else 
            {
                EventSheet.IHavePressedInteractButton.Invoke();
            }
          
            
        }
        private void OnUse(InputAction.CallbackContext ctx)
        {
            EventSheet.UseHeldItem?.Invoke();
        }
        private void OnDrop(InputAction.CallbackContext ctx)
        {
            EventSheet.DropHeldItem?.Invoke();
        }
        private void OnThrow(InputAction.CallbackContext ctx)
        {
            EventSheet.ThrowHeldItem?.Invoke();
        }
        // Controls Pausing input
        public void OnPause(InputAction.CallbackContext context)
        {
            if(GameManager.instance.InventoryActive && GameManager.instance.MenuActive != GameManager.instance.MenuInventory)
            GameManager.instance.DeactivateInventoryUISecondary();
            else if(GameManager.instance.InventoryActive)GameManager.instance.DeactivateInventoryUI();
            
            if(GameManager.instance.CraftTableActive) GameManager.instance.DeactivateCraftTableUI();
            if (GameManager.instance.MenuActive == null && !DialogueManager.instance.IsActive)
            {
                DisableCharacterInputs();
                GameManager.instance.PauseGame();
                GameManager.instance.ActivatePauseMenu();
                //open main menu
            }
            else if (GameManager.instance.MenuActive != null && !DialogueManager.instance.IsActive)
            {
                EnableCharacterInputs();
                GameManager.instance.UnpauseGame();
                GameManager.instance.DeactivatePauseMenu();
                //close menu
            }
            else if(GameManager.instance.MenuActive != null && DialogueManager.instance.IsActive)
            {
                GameManager.instance.UnpauseGame();
                GameManager.instance.DeactivatePauseMenu();
                //close menu
            }
            else if(GameManager.instance.MenuActive== null && DialogueManager.instance.IsActive)
            {
                DialogueManager.instance.disableDialogueUI();
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

        // Controls opening Inventory input
        public void OnInventory(InputAction.CallbackContext context)
        {
            if (GameManager.instance.IsPauseActive || DialogueManager.instance.IsActive) return;
            if (GameManager.instance.InventoryActive)
            {
                GameManager.instance.DeactivateInventoryUI();
                TooltipManager.instance.hide();
            }
            else
            {
                GameManager.instance.ActivateInventoryUI();
            }
            Debug.Log("Inventory Button Pressed");
        }

        public void OnCancelled(InputAction.CallbackContext context)
        {
            if(GameManager.instance.InventoryActive && GameManager.instance.MenuActive != GameManager.instance.MenuInventory)
                GameManager.instance.DeactivateInventoryUISecondary();
            else if(GameManager.instance.InventoryActive)GameManager.instance.DeactivateInventoryUI();
            
            if(GameManager.instance.CraftTableActive) GameManager.instance.DeactivateCraftTableUI();
            if (GameManager.instance.MenuActive != null && !DialogueManager.instance.IsActive)
            {
                EnableCharacterInputs();
                GameManager.instance.UnpauseGame();
                GameManager.instance.DeactivatePauseMenu();
                //close menu
            }
            else if(GameManager.instance.MenuActive != null && DialogueManager.instance.IsActive)
            {
                GameManager.instance.UnpauseGame();
                GameManager.instance.DeactivatePauseMenu();
                //close menu
            }
            else if(GameManager.instance.MenuActive== null && DialogueManager.instance.IsActive)
            {
                DialogueManager.instance.disableDialogueUI();
                EnableCharacterInputs();
            }
            
            TutorialUIManager.Instance.CloseBlocked();
            TutorialUIManager.Instance.CloseCraft();
            TutorialUIManager.Instance.CloseDialogue();
            TutorialUIManager.Instance.CloseGameplay();
            TutorialUIManager.Instance.CloseSleeping();
            TutorialUIManager.Instance.CloseVoting();
        }

        // Disables all inputs the player can use in general gameplay
        public void DisableCharacterInputs()
        {
            //interactAction.Disable();
            moveAction.Disable();
            aimAction.Disable();
            crouchAction.Disable();
            sprintAction.Disable();
            if (!GameManager.instance.InventoryActive)
            {
                inventoryAction.Disable();
            }
            flashlightAction.Disable();

            aimAmount = Vector2.zero;
            
            cancelAction.Enable();
            useAction.Disable();
            dropAction.Disable();
            throwAction.Disable();
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }

        // Enables all inputs the player can use in general gameplay
        public void EnableCharacterInputs()
        {
            interactAction.Enable();
            cancelAction.Disable();
            skipCutSceneAction.Disable();
            flashlightAction.Enable();
            moveAction.Enable();
            aimAction.Enable();
            crouchAction.Enable();
            sprintAction.Enable();
            inventoryAction.Enable();
            useAction.Enable();
            dropAction.Enable();
            throwAction.Enable();
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }

        private void SetInteractable(bool context, EnableInteractUI enableInteractUI)
        {
            isInInteractableArea = context;
            currentInteractable = enableInteractUI;
        }
        
        public void OnEnable()
        {
            flashlightAction.Enable();
            interactAction.Enable();
            moveAction.Enable();
            aimAction.Enable();
            pauseAction.Enable();
            crouchAction.Enable();
            sprintAction.Enable();
            inventoryAction.Enable();
            useAction.Enable();
            dropAction.Enable();
            throwAction.Enable();
            skipCutSceneAction.Enable();
            EnableInteractUI.ImInInteractionZone += SetInteractable;
        }

        public void OnDisable()
        {
            flashlightAction.Disable();
            interactAction.Disable();
            moveAction.Disable();
            aimAction.Disable();
            pauseAction.Disable();
            crouchAction.Disable();
            sprintAction.Disable();
            inventoryAction.Disable();
            cancelAction.Disable();
            useAction.Disable();
            dropAction.Disable();
            throwAction.Disable();
            skipCutSceneAction.Disable();
            EnableInteractUI.ImInInteractionZone -= SetInteractable;
        }
        
        #endregion
    }
    
}

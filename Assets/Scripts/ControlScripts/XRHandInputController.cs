using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRHandInputController : MonoBehaviour
{
    [SerializeField] InputActionReference primaryButton;
    [SerializeField] InputActionReference secondaryButton;
    [SerializeField] InputActionReference trigger;

    [SerializeField] GameObject heldObject;
    IReloadable reloadable;
    IFireable fireable;
    private void OnEnable()
    {
        primaryButton.action.performed += OnPrimaryPressed;
        primaryButton.action.canceled += OnPrimaryReleased;
        secondaryButton.action.performed += OnSecondaryPressed;
        secondaryButton.action.canceled += OnSecondaryReleased;
        trigger.action.Enable();
    }

    private void OnDisable()
    {
        primaryButton.action.performed -= OnPrimaryPressed;
        primaryButton.action.canceled -= OnPrimaryReleased;
        secondaryButton.action.performed -= OnSecondaryPressed;
        secondaryButton.action.canceled -= OnSecondaryReleased;
        trigger.action.Disable();

    }




    private void Update()
    {
        if(fireable != null)
        {
            float triggerValue = trigger.action.ReadValue<float>();
            fireable.UpdateTriggerValue(triggerValue);
        }
    }


    void OnPrimaryPressed(InputAction.CallbackContext context)
    {
        if (reloadable != null)
        {
            reloadable.SlideRelease();
        }
    }

    void OnPrimaryReleased(InputAction.CallbackContext context)
    {
        
    }

    void OnSecondaryPressed(InputAction.CallbackContext context)
    {
        if (reloadable != null)
        {
            reloadable.MagRelease();
        }
    }

    void OnSecondaryReleased(InputAction.CallbackContext context)
    {
        
    }

    public void OnGrab(GameObject grabInteractable)
    {
        heldObject = grabInteractable;
        reloadable = heldObject.GetComponent<IReloadable>();
        fireable = heldObject.GetComponent<IFireable>();
    }

    public void OnRelease()
    {
        heldObject = null;
        reloadable = null;
        fireable = null;
    }
}

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NewXRSimpleInteractible : XRSimpleInteractable
{
    public XRHandInputController handInputController;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        
        Debug.Log("OnSelectEntered called.");
        handInputController = args.interactorObject.transform.GetComponentInParent<XRHandInputController>();
        if (handInputController != null)
        {
            handInputController.OnGrab(gameObject);
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (handInputController != null)
        {
            handInputController.OnRelease();
            handInputController = null;
        }
    }
}

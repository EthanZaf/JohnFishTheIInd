using UnityEngine;

public class Slide : MonoBehaviour
{
    public GameObject grabbingHand;
    public Vector3 handGrabStartingPosition;
    public float slideStartPosition;
    public GunManager gunManager;

    [SerializeField] private float minSlideLimit;
    [SerializeField] private float maxSlideLimit;
    [SerializeField] private float rackThreshold;


    public bool canRack = false;
    private void LateUpdate()
    {
        if (grabbingHand == null) return;
        //Slide back the gun based on the hand's position relative to the starting position up to a certain limit
        Vector3 handPositionTravelled = grabbingHand.transform.position - handGrabStartingPosition;

        Vector3 localHandPositionTravelled = transform.InverseTransformDirection(handPositionTravelled);

        float slidePosition = slideStartPosition + localHandPositionTravelled.z;

        slidePosition = Mathf.Clamp(slidePosition, minSlideLimit, maxSlideLimit);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, slidePosition);

        //If slide passes a certain limit, do gunManager.Rack();
        if (canRack && slidePosition >= rackThreshold)
        {
            gunManager.Rack();  
            canRack = false;
        }
        if (!canRack && slidePosition < rackThreshold)
        {
            canRack = true;
        }


    }
    public void OnGrab(NewXRGrabInteractible xRGrabInteractible)
    {
        GameObject hand = xRGrabInteractible.handInputController.gameObject;
        grabbingHand = hand;
        handGrabStartingPosition = hand.transform.position;
        slideStartPosition = transform.localPosition.z;
        canRack = true;
    }

    public void OnRelease()
    {
        if (gunManager.isSlideBack)
        {
            gunManager.ManualSlideRelease();
        }

        grabbingHand = null;
        handGrabStartingPosition = Vector3.zero;






    }
}

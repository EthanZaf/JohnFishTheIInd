using UnityEngine;

public class MagDetector : MonoBehaviour
{
    [SerializeField] AttachmentManager attachmentManager;
    [SerializeField] Transform magPoint;


    private void OnTriggerEnter(Collider other)
    {
        MagazineTip magazineTip = other.GetComponent<MagazineTip>();
        Magazine currentMagazine = attachmentManager.GetAttachment<Magazine>();

        if (magazineTip == null || currentMagazine != null) return;


        Magazine magazine = magazineTip.GetComponentInParent<Magazine>();
        attachmentManager.AttachToGun(magazine, AttachmentTypes.Magazine);


    }


}

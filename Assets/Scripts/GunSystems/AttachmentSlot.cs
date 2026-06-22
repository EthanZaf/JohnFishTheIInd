using UnityEngine;
[System.Serializable]
public struct AttachmentSlot
{

    public GunAttachment gunAttachment;
    public Transform attachPoint;
    public AttachmentTypes possibleAttachmentTypes;
}

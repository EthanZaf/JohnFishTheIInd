using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class AttachmentManager : MonoBehaviour
{

    public List<AttachmentSlot> attachmentSlots;

    public event Action<GunAttachment> OnAttachmentChanged;






    public T GetAttachment<T>() where T : GunAttachment
    {
        foreach (var slot in attachmentSlots)
        {
            if (slot.possibleAttachmentTypes is T attachment)
                return attachment;
        }

        return null;
    }


    public void AttachToGun(GunAttachment gunAttachment, AttachmentTypes attachmentType)
    {
        for (int i = 0; i < attachmentSlots.Count; i++)
        {
            if (attachmentSlots[i].possibleAttachmentTypes == attachmentType)
            {
                attachmentSlots[i].gunAttachment = gunAttachment;

                gunAttachment.transform.SetParent(attachmentSlots[i].attachPoint);
                gunAttachment.transform.localPosition = Vector3.zero;
                gunAttachment.transform.localRotation = Quaternion.identity;

                OnAttachmentChanged?.Invoke(gunAttachment);

                return;
            }
        }
    }

    public void RemoveAttachment(AttachmentTypes attachmentType)
    {
        for (int i = 0; i < attachmentSlots.Count; i++)
        {
            if (attachmentSlots[i].possibleAttachmentTypes == attachmentType)
            {
                GunAttachment removedAttachment = attachmentSlots[i].gunAttachment;

                if (removedAttachment != null)
                {
                    removedAttachment.transform.SetParent(null);
                }

                attachmentSlots[i].gunAttachment = null;

                // Notify listeners
                OnAttachmentChanged?.Invoke(null);

                return;
            }
        }
    }


}

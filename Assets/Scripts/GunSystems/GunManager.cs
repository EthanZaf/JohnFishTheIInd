using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class GunManager : MonoBehaviour, IFireable, IReloadable
{
    public enum GunState
    {
        
        Idle,
        RackBack,
        Firing,
        UnloadingMagazine
    }

    [SerializeField] bool isAutomatic;
    [SerializeField] bool hasBeenPulled;

    [SerializeField] Transform casingSpawnPoint;
    [SerializeField] Transform firePoint;
    [SerializeField] BulletStats bulletInChamber;
    [SerializeField] Magazine magazine;
    [SerializeField] AttachmentManager attachmentManager;
    Animator animator;

    [SerializeField] float triggerValue;
    [SerializeField] float triggerThreshold = 0.5f;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        attachmentManager = GetComponent<AttachmentManager>();
        attachmentManager.OnAttachmentChanged += AttachmentChanged;
    }

    private void OnDestroy()
    {
        attachmentManager.OnAttachmentChanged -= AttachmentChanged;
    }

    void AttachmentChanged(GunAttachment attachment)
    {
        UpdateMagazine();

    }

    void UpdateMagazine()
    {
        magazine = attachmentManager.GetAttachment<Magazine>();
    }

    public bool AttemptToChamberRound()
    {
        if(magazine == null)
        {
            bulletInChamber = null;
            return false;

        }

        BulletStats? bullet = magazine.UseBullet();

        if(bullet != null)
        {
            bulletInChamber = bullet;
            return true;
        }

        bulletInChamber = null;
        return false;
    }

    public void EjectRound()
    {
        
        Instantiate(bulletInChamber.bulletPrefab, casingSpawnPoint.position, casingSpawnPoint.rotation);


    }

    public void Fire()
    {
        if(bulletInChamber?.bulletPrefab != null)
        {
            //Do a hitscan, vfx, sfx, apply damage, fire cooldown.

            //Then eject the round
            EjectRound();

            //Then attempt to chamber the next round
            bool isThereANewRound = AttemptToChamberRound();

            if(isThereANewRound)
            {
                //return to slide/bolt forward position
                animator.SetTrigger("Fire");
            } else
            {

                //stay in slide/bolt back position, no round to chamber
                animator.SetTrigger("FireLastShot");

            }

        }
        else
        {
            //Play dry fire sound, no round to fire
        }
    }

    public void Rack()
    {
        //Eject the round in the chamber if there is one
        if(bulletInChamber?.bulletPrefab != null)
        {
            EjectRound();
        }
        //We'll use 416s since they can use the sliding handle to rack to a new mag.
        AttemptToChamberRound();
        //End in slide forward position;



    }

    public void SlideRelease()
    {
        animator.SetTrigger("SlideRelease");
        Rack();
    }

    public void ManualSlideRelease()
    {
        animator.SetTrigger("ManualSlideRelease");
        Rack();
    }

    public void MagRelease()
    {
        
    }

    public void UpdateTriggerValue(float value)
    {
        triggerValue = value;

        animator.SetFloat("TriggerPull", triggerValue);

        if (triggerValue >= triggerThreshold && !hasBeenPulled)
        {
            hasBeenPulled = true;
            Fire();
        }
        else if (triggerValue < triggerThreshold && hasBeenPulled)
        {
            hasBeenPulled = false;
        }
    }
}

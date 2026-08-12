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
    [SerializeField] GameObject propMagazine;
    [SerializeField] AttachmentManager attachmentManager;
    Animator animator;

    [SerializeField] float triggerValue;
    [SerializeField] float triggerThreshold = 0.5f;


    bool isSlideBack = false;



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
        
        if(magazine != null)
        {
            propMagazine.SetActive(true);
            magazine.GetComponent<MeshRenderer>().enabled = false;
        } else
        {
            propMagazine.SetActive(false);
        }

    }

    public bool AttemptToChamberRound()
    {
        if(magazine == null)
        {
            bulletInChamber = null;
            return false;

        }

        BulletStats bullet = magazine.UseBullet();

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
        
        GameObject bulletCase = Instantiate(bulletInChamber.bulletPrefab, casingSpawnPoint.position, casingSpawnPoint.rotation);

        Vector3 ejectDir = 
            casingSpawnPoint.right * Random.Range(1.5f, 5f) +
            casingSpawnPoint.up * Random.Range(-1.5f, -4.5f) +
            casingSpawnPoint.forward * Random.Range(-1.5f, -4.5f);

        Rigidbody rb = bulletCase.GetComponent<Rigidbody>();
        rb.AddForce(ejectDir * 0.4f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * Random.Range(0.2f, 0.6f), ForceMode.Impulse);



    }

    public void Fire()
    {
        if(bulletInChamber?.bulletPrefab != null)
        {
            //Do a hitscan, vfx, sfx, apply damage, fire cooldown.
            BulletCast(bulletInChamber.damage, bulletInChamber.force);

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
                isSlideBack = true;

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
        if (bulletInChamber?.bulletPrefab != null)
        {
            EjectRound();
        }
        //We'll use 416s since they can use the sliding handle to rack to a new mag.
        AttemptToChamberRound();
        //End in slide forward position;



    }

    public void BulletCast(float bulletDamage, float bulletForce)
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForceAtPosition(firePoint.forward * bulletForce, hit.point, ForceMode.Impulse);
            }

        }

        Debug.DrawRay(firePoint.position, firePoint.forward * 100f, Color.red, 1f);
    }

    public void ManualSlideRelease()
    {
        if(isSlideBack) 
        {
            animator.SetTrigger("ManualSlideRelease");
            AttemptToChamberRound();
            isSlideBack = false;
        }
    }

    

    public void UnloadMagazine()
    {
        //Check if there is a magazine attached
        if(magazine == null) return;

        //Do Animation
        animator.SetTrigger("MagRelease");


    }

    public void DetachMagazine()
    {
        if(magazine == null) return;

        //Set Magazine location to be the same as the prop mag
        magazine.transform.position = propMagazine.transform.position;
        //Turn on magazine renderer
        magazine.GetComponent<MeshRenderer>().enabled = true;
        //Temp Disable Magazine Tip Collider
        magazine.GetComponentInChildren<MagazineTip>().DisableTrigger();
        //Remove from attachment manager & unparent
        attachmentManager.RemoveAttachment(AttachmentTypes.Magazine);
        //Disable the prop magazine happens on updating the attachments

    }



    /////////////////// INTERFACES //////////////////////////
    public void MagRelease()
    {
        Debug.Log("MagRelease");
        UnloadMagazine();
    }

    public void SlideRelease()
    {
        if (isSlideBack)
        {
            animator.SetTrigger("SlideRelease");
            AttemptToChamberRound();
            isSlideBack = false;
        }
        else Rack();
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

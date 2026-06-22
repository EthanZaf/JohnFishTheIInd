using UnityEngine;

[System.Serializable]
public struct AttachmentStats
{
    [SerializeField] float accuracyMultiplier;
    [SerializeField] float fireRateMultiplier;
    [SerializeField] float recoilMultiplier;
    [SerializeField] float damageMultiplier;

    [SerializeField] float addedAccuracy;
    [SerializeField] float addedFireRate;
    [SerializeField] float addedRecoil;
    [SerializeField] float addedDamage;
}

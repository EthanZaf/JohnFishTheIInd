using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] BulletStats bulletStats;

    public BulletStats GetBulletStats()
    {
        return bulletStats;
    }

}

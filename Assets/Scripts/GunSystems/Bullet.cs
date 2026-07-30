using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] BulletStats bulletStats;
    [SerializeField] bool isSpent;

    public BulletStats GetBulletStats()
    {
        return bulletStats;
    }

    public void OnEnable()
    {
        //Remove bullet tip if spent

        if (isSpent)
        {
            
        }
    }

}

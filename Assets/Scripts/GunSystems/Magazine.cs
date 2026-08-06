using System.Collections.Generic;
using UnityEngine;

public class Magazine : GunAttachment
{
    
    [SerializeField] List<BulletStats> bulletStack;
    [SerializeField] Transform removePoint;

    [Space(10)]
    [Header("Debug Variables")]
    [SerializeField] BulletStats defaultTestBullet;


    public void LoadBullet(BulletStats bullet)
    {
        bulletStack.Push(bullet);
    }

    public BulletStats? UseBullet()
    {
        if (bulletStack.Count > 0)
        {
            return bulletStack.Pop();
        }
        else
        {
            Debug.Log("Mag's Empty");
            return null;
        }
    }

    public void RemoveBullet()
    {
        if (bulletStack.Count > 0)
        {
            Instantiate(bulletStack.Pop().bulletPrefab, removePoint.position, removePoint.rotation);
        }
    }

    public void AddBullet(BulletStats bullet)
    {
        bulletStack.Push(bullet);
    }

    public void AddTestBullet()
    {
        AddBullet(defaultTestBullet);
    }





}

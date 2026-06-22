using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] Stack<BulletStats> bulletStack;

    public void LoadBullet(BulletStats bullet)
    {
        bulletStack.Push(bullet);
    }

    public BulletStats? UnloadBullet()
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
}

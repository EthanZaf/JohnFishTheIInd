using System.Collections;
using UnityEngine;

public class MagazineTip : MonoBehaviour
{
    Collider collider;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
    }

    public void DisableTrigger()
    {
        StartCoroutine(TempDisable());
    }

    IEnumerator TempDisable()
    {
        collider.enabled = false;

        yield return new WaitForSeconds(1f);

        collider.enabled = true;
    }
}

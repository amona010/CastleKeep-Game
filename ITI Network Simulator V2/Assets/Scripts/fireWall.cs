using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireWall : MonoBehaviour {
    public Transform[] renders;
    private GameObject obj;

    private void Start()
    {
        renders = GetComponentsInChildren<Transform>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        identityHandler attacker = null;
        obj = other.gameObject;

        if(obj.CompareTag("Attacker_red") || obj.CompareTag("Attacker_blue") || obj.CompareTag("Attacker_orange") || obj.CompareTag("Attacker_gray"))
        {
            attacker = obj.GetComponent<identityHandler>();
            attacker.dieIfAtRisk();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (obj == other.gameObject)
        {
            obj = null;
        }
    }


}

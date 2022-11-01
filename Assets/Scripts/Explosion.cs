using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Health
{
    [SerializeField] [Range(100,1000)] float power;
    [SerializeField] GameObject fireEffect;
    [SerializeField] Transform expTransform;
    [SerializeField] GameObject destructibleObject;
    [SerializeField] GameObject dagilacakObject;
    [SerializeField] GameObject originalObject;
    protected override void Start()
    {
        base.Start();
        if(dagilacakObject != null)
        {
            for (int i = 0; i < dagilacakObject.transform.childCount; i++)
            {
                dagilacakObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                dagilacakObject.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
            }
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Death(deathTime);
        }
    }
    protected override void Death(float time)
    {
        base.Death(time);
        if(fireEffect != null)
        {
            var effect = Instantiate(fireEffect,gameObject.transform.position,Quaternion.identity);
            effect.transform.SetParent(this.transform);
        }
        if(destructibleObject != null)
        {
            originalObject.SetActive(false);
            destructibleObject.SetActive(true);
            for (int i = 0; i < dagilacakObject.transform.childCount; i++)
            {
                var rb = dagilacakObject.transform.GetChild(i).GetComponent<Rigidbody>();
                dagilacakObject.transform.GetChild(i).SetParent(null);
                Debug.Log(rb + " 1000");
                Vector3 exppos = new Vector3(rb.centerOfMass.x,rb.centerOfMass.y-1f,rb.centerOfMass.z);
                rb.AddExplosionForce(power*expMultiplier,expTransform.position,15);
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilencerBoulder : Boulder
{
    public float silenceDuration;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if(collision.gameObject.TryGetComponent(out GrappleController controller))
        {
            controller.SilenceGrapple(silenceDuration);
            //Destroy(gameObject);
        }
        Destroy(gameObject);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}

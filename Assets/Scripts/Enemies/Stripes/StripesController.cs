using System.Collections.Generic;
using UnityEngine;

public class StripesController : MonoBehaviour
{
    [SerializeField] public List<Stripe> stripes;

    private void Awake()
    {
        foreach (Stripe s in stripes)
            s.renderer.enabled = false;
    }

    public Stripe GetStripe(string stripeName)
    {
        foreach (Stripe s in stripes)
            if (s.gameObject.name == stripeName)
                return s;

        return null;
    }
}

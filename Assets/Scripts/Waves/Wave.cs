using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<Zone> _zones;

    public bool requiresCondition = true;

    public short enemiesRequired = 0;
}

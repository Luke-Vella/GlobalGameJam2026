using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tentacle
{
    public int tentacleId;
    public int healthPoints;
    public bool isVulnerable;

    public Tentacle(int _tentacleId, int _healthPoints, bool _isVulnerable)
    {
        tentacleId = _tentacleId;
        healthPoints = _isVulnerable?  _healthPoints: _healthPoints*2;
    }
}

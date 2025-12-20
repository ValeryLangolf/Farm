using System;
using UnityEngine;

public interface IInteractionDetector : IService
{
    event Action<RaycastHit2D[], int> HitsDetected;
    event Action InputStarted;
    event Action InputEnded;
}
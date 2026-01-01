using System;
using UnityEngine;
using System.Collections.Generic;

public class GardensDirector : MonoBehaviour, IService
{
    [SerializeField] private List<Garden> _gardens;

    private void Awake()
    {
        if (_gardens == null || _gardens.Count == 0)
            throw new ArgumentException(nameof(_gardens));
    }

    public IReadOnlyList<Garden> Gardens => _gardens;
}
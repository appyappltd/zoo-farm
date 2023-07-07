using System;
using UnityEngine;

namespace Logic.Plants.PlantSettings
{
    [Serializable]
    public class GrowStage
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private float _growTime;

        public float GrowTime => _growTime;
        public Mesh Mesh => _mesh;
    }
}
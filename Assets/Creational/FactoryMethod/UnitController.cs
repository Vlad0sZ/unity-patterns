using System;
using UnityEngine;

namespace Creational.FactoryMethod
{
    public class UnitController : MonoBehaviour
    {
        public Vector3[] positions;
        public UnitCreator creator;

        private void Start()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                creator.CreateUnit(positions[i]);
            }
            
        }
    }
}
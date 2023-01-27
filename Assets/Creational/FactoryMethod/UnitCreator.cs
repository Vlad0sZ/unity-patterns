using Demo_Project.Scripts;
using UnityEngine;

namespace Creational.FactoryMethod
{
    public abstract class UnitCreator : MonoBehaviour
    {
        public abstract Unit CreateUnit(Vector3 position);
    }
}
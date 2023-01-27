using Demo_Project.Scripts;
using UnityEngine;

namespace Creational.AbstractFactory
{
    public abstract class UnitFactory : MonoBehaviour
    {
        public abstract Unit CreatePawnUnit(Vector3 position);

        public abstract Unit CreateKnightUnit(Vector3 position);
    }
}
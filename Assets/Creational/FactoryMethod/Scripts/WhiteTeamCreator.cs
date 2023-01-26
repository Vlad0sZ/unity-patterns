using Creational.SingletonPattern.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Creational.FactoryMethod.Scripts
{
    // создатель белых фигур на доске
    public class WhiteTeamCreator : MonoBehaviour
    {
        public Unit whiteUnitPrefab;

        public Unit CreateUnit(Vector3 position)
        {
            return Instantiate(whiteUnitPrefab, position, Quaternion.identity);
        }
    }
}
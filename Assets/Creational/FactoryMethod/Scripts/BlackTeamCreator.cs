using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Creational.FactoryMethod.Scripts
{
    // Создатель темных фигур на доске
    public class BlackTeamCreator : MonoBehaviour
    {
        public Unit blackUnitPrefab;

        public Unit CreateUnit(Vector3 position)
        {
            return Instantiate(blackUnitPrefab, position, Quaternion.identity);
        }
    }
}
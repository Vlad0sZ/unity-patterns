using Creational.SingletonPattern.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Creational.FactoryMethod.Scripts
{
    // создатель белых фигур на доске
    public class WhiteTeamCreator : UnitCreator
    {
        public WhiteUnit whiteUnitPrefab;

        public override Unit CreateUnit(Vector3 position, Gameboard gameBoard)
        {
            WhiteUnit unit = Instantiate(whiteUnitPrefab, position, Quaternion.identity, transform);
            unit.Init(gameBoard);
            return unit;
        }
    }
}
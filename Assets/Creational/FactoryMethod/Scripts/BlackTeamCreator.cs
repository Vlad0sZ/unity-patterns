using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Creational.FactoryMethod.Scripts
{
    // Создатель темных фигур на доске
    public class BlackTeamCreator : UnitCreator
    {
        public BlackUnit blackUnitPrefab;


        public override Unit CreateUnit(Vector3 position, Gameboard gameBoard)
        {
            BlackUnit unit = Instantiate(blackUnitPrefab, position, Quaternion.identity, transform);
            unit.Init(gameBoard);
            return unit;
        }
    }
}
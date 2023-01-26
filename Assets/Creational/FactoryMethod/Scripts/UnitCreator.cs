using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Creational.FactoryMethod.Scripts
{
    // Абстрактный класс для создателей фигур
    public abstract class UnitCreator : MonoBehaviour
    {
        // Фабричный метод для создания юнитов на доске
        public abstract Unit CreateUnit(Vector3 position, Gameboard gameBoard);
    }
}
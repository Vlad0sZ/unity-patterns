using UnityEngine;

namespace Creational.SingletonPattern.Scripts
{
    public class Unit : MonoBehaviour
    {
        private Vector3Int _currentCell;

        public Vector3Int currentCell
        {
            get => _currentCell;
            set => _currentCell = value;
        }


        public virtual void Init(Gameboard gameBoard)
        {
            var cell = gameBoard.GetClosestCell(transform.position);
            gameBoard.SetUnit(cell, this);
            transform.position = gameBoard.GetCellCenterWorld(cell);
        }
    }
}
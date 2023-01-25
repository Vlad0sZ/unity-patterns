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


        public void Start()
        {
            // TODO add Gameboard instance 
            var board = FindObjectOfType<Gameboard>();
            if (board == null) return;

            var cell = board.GetClosestCell(transform.position);
            board.SetUnit(cell, this);
            transform.position = board.GetCellCenterWorld(cell);
        }
    }
}
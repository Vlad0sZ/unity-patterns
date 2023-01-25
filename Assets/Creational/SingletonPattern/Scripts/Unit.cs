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
            if (!Gameboard.IsExists()) return;

            var cell = Gameboard.Instance.GetClosestCell(transform.position);
            Gameboard.Instance.SetUnit(cell, this);
            transform.position = Gameboard.Instance.GetCellCenterWorld(cell);
        }
    }
}
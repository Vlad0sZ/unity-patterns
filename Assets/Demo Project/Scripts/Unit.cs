using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Demo_Project.Scripts
{
    public class Unit : MonoBehaviour
    {
        public Vector3Int CurrentCell { get; private set; }

        public Gameboard board;

        public void Start()
        {
            if (board == null) return;

            CurrentCell = board.GetClosestCell(transform.position);

            var center = board.GetCenterCell();
            var lookCell = new Vector3Int(center.x, 0, CurrentCell.z);
            var placementCell = board.GetCellCenterWorld(CurrentCell);
            var lookPosition = board.GetCellCenterWorld(lookCell);

            transform.position = placementCell;
            transform.rotation = Quaternion.LookRotation(lookPosition - placementCell, Vector3.up);

            board.SetUnit(CurrentCell, this);
        }
    }
}
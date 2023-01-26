using System;
using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Creational.FactoryMethod.Scripts
{
    public class UnitPlace : MonoBehaviour
    {
        public UnitCreator creator;
        public Vector3Int[] cells;


        private void Start()
        {
            if (!Gameboard.IsExists()) return;
            var board = Gameboard.Instance;
            
            foreach (Vector3Int cell in cells)
            {
                var position = board.GetCellCenterWorld(cell);
                creator.CreateUnit(position, board);
            }
        }
    }
}
using System;
using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Demo_Project.Scripts
{
    public class UserControl : MonoBehaviour
    {
        public Camera cam;
        public CellSelector selector;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                ClickUnitToSelect();
            }
        }

        private void ClickUnitToSelect()
        {
            var board = Gameboard.Instance;
            if (board== null) return;

            if (board.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var clickedCell))
            {
                selector.SelectUnit(board.GetUnit(clickedCell));
            }
            else
            {
                selector.SelectUnit(null);
            }
           
        }

        private void ClickCellToSelect()
        {
            // TODO add gameboard instance
            throw new NotImplementedException();
        }
    }
}
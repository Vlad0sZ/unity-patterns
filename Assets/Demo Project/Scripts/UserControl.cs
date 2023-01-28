using System;
using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Demo_Project.Scripts
{
    public class UserControl : MonoBehaviour
    {
        enum State
        {
            SelectingUnit,
            SelectingCell
        }

        public Camera cam;

        public Gameboard board;
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                ClickUnitToSelect();
            }
        }

        private void ClickUnitToSelect()
        {
            // TODO add gameboard instance
            if (board) return;
            
            if (board.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var clickedCell))
            {
                var unit = board.GetUnit(clickedCell);
                if (unit) Debug.Log("Unit clicked");
            }
        }

        private void ClickCellToSelect()
        {
            // TODO add gameboard instance
            throw new NotImplementedException();
        }
    }
}
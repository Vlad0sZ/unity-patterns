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
        
        private State state;
        private Unit selectedUnit;

        private void Start()
        {
            state = State.SelectingUnit;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseClicked();
            }
        }


        private void OnMouseClicked()
        {
            switch (state)
            {
                case State.SelectingUnit:
                    ClickUnitToSelect();
                    break;
                case State.SelectingCell:
                    ClickCellToSelect();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                selectedUnit = unit;
            }
            else
            {
                selectedUnit = null;
            }
        }

        private void ClickCellToSelect()
        {
            // TODO add gameboard instance
            throw new NotImplementedException();
        }
    }
}
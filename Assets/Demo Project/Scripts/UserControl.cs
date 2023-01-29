using System.Linq;
using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Demo_Project.Scripts
{
    public class UserControl : MonoBehaviour
    {
        public Camera cam;
        public CellSelector selector;
        
        public Unit.Side activeSide;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseClicked();
            }
        }

        private void OnMouseClicked()
        {
            ClickUnitToSelect();
        }

        private void ClickUnitToSelect()
        {
            var board = Gameboard.Instance;
            if (board== null) return;

            if (board.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var clickedCell))
            {
                var unit = board.GetUnit(clickedCell);
                if(unit != null && unit.team == activeSide) selector.SelectUnit(unit);
                else selector.SelectUnit(null);
            }
            else
            {
                selector.SelectUnit(null);
            }
           
        }

        private void ClickCellToSelect()
        {
            if (Gameboard.Instance.Raycast(cam.ScreenPointToRay(Input.mousePosition),
                    out Vector3Int clickedCell))
            {
                if (selector.displayedCells.Contains(clickedCell))
                {
                    var unit = Gameboard.Instance.GetUnit(clickedCell);
                    
                    if (unit == null)
                    {
                        
                    }
                }
            }
            
            selector.SelectUnit(null);
            // state
        }
    }
}
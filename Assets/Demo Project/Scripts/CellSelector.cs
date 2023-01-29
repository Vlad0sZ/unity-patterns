using System.Collections.Generic;
using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Demo_Project.Scripts
{
    public class CellSelector : MonoBehaviour
    {
        public GameObject selectorPrefab;
        public GameObject moveDisplayPrefab;

        private readonly List<GameObject> displayPool = new List<GameObject>();
        private int displayedCellCount;
        public Vector3Int[] displayedCells;

        private GameObject selector;
        private Unit selectedUnit;

        private void Start()
        {
            selector = Instantiate(selectorPrefab);
            selector.SetActive(false);
            int count = Gameboard.Instance.BoardSize;

            for (int i = 0; i < count; ++i)
            {
                var o = Instantiate(moveDisplayPrefab);
                o.SetActive(false);
                displayPool.Add(o);
            }

            displayedCellCount = 0;
            displayedCells = new Vector3Int[count];
        }

        public void SelectUnit(Unit unit)
        {
            if (unit == null)
            {
                DeselectUnit();
            }
            else
            {
                var board = Gameboard.Instance;
                selector.SetActive(true);
                selector.transform.position = unit.transform.position;
                selectedUnit = unit;
                
                int count = selectedUnit.GetSelectedCell(displayedCells, board);
                for (int i = 0; i < count; i++)
                {
                    displayPool[i].SetActive(true);
                    displayPool[i].transform.position = board.GetCellCenterWorld(displayedCells[i]);
                }
                
                CleanMoveIndicator(count, displayedCellCount);
                displayedCellCount = count;
            }
            
        }

        public void DeselectUnit()
        {
            selectedUnit = null;
            selector.gameObject.SetActive(false);
            CleanMoveIndicator(0, displayedCellCount);
            displayedCellCount = 0;
        }

        private void CleanMoveIndicator(int lowerBound, int upperBound)
        {
            for (int i = lowerBound; i < upperBound; ++i)
            {
                displayPool[i].SetActive(false);
            }
        }
    }
}
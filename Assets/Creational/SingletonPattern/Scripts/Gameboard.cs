using System.ComponentModel.Design;
using UnityEngine;

namespace Creational.SingletonPattern.Scripts
{
    public class Gameboard : MonoBehaviour
    {
        // TODO add gameboard instance

        private static Gameboard instance;
        public static Gameboard Instance => instance;
        public static bool IsExists() => instance != null;


        public int Width;

        public int Height;

        private Plane boardPlane;

        private Grid grid;

        private Unit[,] content;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
                return;
            }


            grid = GetComponent<Grid>();
            content = new Unit[Width, Height];

            boardPlane = new Plane(Vector3.up, Vector3.zero);
        }


        // Проверяем, находится ли точка на сетке (доске)
        public bool IsOnBoard(Vector3Int cell)
        {
            return cell.x >= 0 && cell.x < Width && cell.z >= 0 && cell.z < Height;
        }

        // Кинуть луч в сторону доски
        public bool Raycast(Ray ray, out Vector3Int cell)
        {
            cell = Vector3Int.zero;

            if (!boardPlane.Raycast(ray, out float d)) return false;

            Vector3Int clickedCell = grid.WorldToCell(ray.GetPoint(d));

            if (!IsOnBoard(clickedCell)) return false;

            cell = clickedCell;
            return true;
        }

        // установить юнит на клетку
        public void SetUnit(Vector3Int cell, Unit unit)
        {
            if (!IsOnBoard(cell))
                return;

            content[cell.x, cell.z] = unit;
        }

        // получить юнит с клетки
        public Unit GetUnit(Vector3Int cell)
        {
            if (!IsOnBoard(cell))
                return null;

            return content[cell.x, cell.z];
        }

        // получить ближаюшую игровую клетку
        public Vector3Int GetClosestCell(Vector3 worldPosition)
        {
            var idx = grid.WorldToCell(worldPosition);

            if (idx.x <= 0) idx.x = 0;
            else if (idx.x >= Width) idx.x = Width - 1;

            if (idx.z <= 0) idx.z = 0;
            else if (idx.z >= Height) idx.z = Height - 1;

            return idx;
        }

        public Vector3 GetCellCenterWorld(Vector3Int cell)
        {
            return grid.GetCellCenterWorld(cell);
        }


        // отрисовка на сцене игрового поля
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            Vector3 cubeSize = new Vector3(1, 0, 1);
            Vector3 cubePosition = new Vector3(.5f, 0, .5f);
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Height; z++)
                {
                    Gizmos.DrawWireCube(cubePosition + new Vector3(x, 0, z), cubeSize);
                }
            }
        }
    }
}
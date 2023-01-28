using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Demo_Project.Scripts
{
    public class PawnUnit : Unit
    {
        public override int GetSelectedCell(Vector3Int[] result, Gameboard board)
        {
            int count = 0;

            for (int z = -1; z <= 1; z++)
            {
                Vector3Int nextCell = CurrentCell + new Vector3Int(-1, 0, z);
                if (!board.IsOnBoard(nextCell)) continue;
                result[count] = nextCell;
                count++;
            }

            return count;
        }
    }
}
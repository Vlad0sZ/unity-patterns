using Creational.SingletonPattern.Scripts;
using UnityEngine;

namespace Demo_Project.Scripts
{
    public class KingUnit : Unit
    {
        public override int GetSelectedCell(Vector3Int[] result, Gameboard board)
        {
            int count = 0;
            var Distance = 1;
        
            for (int y = -Distance; y <= Distance; ++y)
            {
                for (int x = -Distance; x <= Distance; ++x)
                {
                    if(x == 0 && y == 0)
                        continue;
                    
                    var idx = CurrentCell + new Vector3Int(x,0, y);
                    if (board.IsOnBoard(idx))
                    {
                        result[count] = idx;
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
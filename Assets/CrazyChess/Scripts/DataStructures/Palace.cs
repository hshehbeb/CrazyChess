using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrazyChess.Scripts.DataStructures
{
    public class Palace
    {
        private RectInt _palaceRectArea;
        
        public Palace(RectInt palaceRectArea)
        {
            _palaceRectArea = palaceRectArea;
        }

        public IEnumerable<MoveInfo> KeepInsidersOnly(IEnumerable<MoveInfo> origin)
        {
            return origin.Where(move => InsidePalace(move.ArriveGrid));
        }

        public bool InsidePalace(Vector2Int pos)
        {
            return pos.x >= _palaceRectArea.xMin && 
                   pos.x <= _palaceRectArea.xMax && 
                   pos.y >= _palaceRectArea.yMin &&
                   pos.y <= _palaceRectArea.yMax;
        }
            
    }
}

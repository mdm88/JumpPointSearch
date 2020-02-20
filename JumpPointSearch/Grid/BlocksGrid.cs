using System.Collections.Generic;
using System.Linq;

namespace JumpPointSearch.Grid
{
    public class BlocksGrid : BaseGrid
    {
        public override int Width { get; protected set; }
        public override int Height { get; protected set; }
        
        private Dictionary<GridPos, Node> _nodes;
        
        public BlocksGrid(int iWidth, int iHeight, List<GridPos> iMatrix = null)
        {
            Width = iWidth;
            Height = iHeight;
            
            m_gridRect.minX = 0;
            m_gridRect.minY = 0;
            m_gridRect.maxX = iWidth-1;
            m_gridRect.maxY = iHeight - 1;
            BuildNodes(iMatrix);
        }

        protected void BuildNodes(List<GridPos> iWalkableGridList)
        {
            _nodes = new Dictionary<GridPos, Node>();
            if (iWalkableGridList == null) 
                return;

            foreach (GridPos gridPos in iWalkableGridList)
                SetWalkableAt(gridPos.x, gridPos.y, false);
        }
        
        public override Node GetNodeAt(int iX, int iY)
        {
            return GetNodeAt(new GridPos(iX, iY));
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            if (!_nodes.ContainsKey(iPos))
            {
                _nodes[iPos] = new Node(iPos.x, iPos.y, true);
            }
            
            return _nodes[iPos];
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            return IsWalkableAt(new GridPos(iX, iY));
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return IsInside(iPos) && (!_nodes.ContainsKey(iPos) || _nodes[iPos].walkable);
        }

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            return SetWalkableAt(new GridPos(iX, iY), iWalkable);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            if (!IsInside(iPos))
                return false;
            
            if (_nodes.ContainsKey(iPos))
            {
                _nodes[iPos].walkable = iWalkable;
            }
            else if (!iWalkable)
            {
                _nodes.Add(iPos, new Node(iPos.x, iPos.y, false));
            }

            return true;
        }

        private bool IsInside(GridPos iPos)
        {
            return iPos.x >= m_gridRect.minX && iPos.x <= m_gridRect.maxX && iPos.y >= m_gridRect.minY && iPos.y <= m_gridRect.maxY;
        }

        public override void Reset()
        {
            foreach (KeyValuePair<GridPos, Node> keyValue in _nodes)
            {
                keyValue.Value.Reset();
            }
        }

        public override BaseGrid Clone()
        {
            BaseGrid tNewGrid = new BlocksGrid(Width, Height);

            foreach (KeyValuePair<GridPos, Node> keyValue in _nodes)
            {
                tNewGrid.SetWalkableAt(keyValue.Key.x, keyValue.Key.y, keyValue.Value.walkable);
            }

            return tNewGrid;
        }
    }
}
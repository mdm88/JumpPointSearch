﻿/*! 
@file StaticGrid.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief StaticGrid Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

An Interface for the StaticGrid Class.

*/

namespace JumpPointSearch.Grid
{
    public class StaticGrid : BaseGrid
    {
        public override int Width { get; protected set; }

        public override int Height { get; protected set; }

        private Node[][] m_nodes;

        public StaticGrid(int iWidth, int iHeight, bool defaultWalkability = false, bool[][] iMatrix = null):base()
        {
            Width = iWidth;
            Height = iHeight;
            m_gridRect.minX = 0;
            m_gridRect.minY = 0;
            m_gridRect.maxX = iWidth-1;
            m_gridRect.maxY = iHeight - 1;
            m_nodes = buildNodes(iWidth, iHeight, defaultWalkability, iMatrix);
        }

        public StaticGrid(StaticGrid b)
            : base(b)
        {
            bool[][] tMatrix = new bool[b.Width][];
            for (int widthTrav = 0; widthTrav < b.Width; widthTrav++)
            {
                tMatrix[widthTrav] = new bool[b.Height];
                for (int heightTrav = 0; heightTrav < b.Height; heightTrav++)
                {
                    if(b.IsWalkableAt(widthTrav,heightTrav))
                        tMatrix[widthTrav][heightTrav] = true;
                    else
                        tMatrix[widthTrav][heightTrav] = false;
                }
            }
            m_nodes = buildNodes(b.Width, b.Height, false, tMatrix);
        }
       
        private Node[][] buildNodes(int iWidth, int iHeight, bool defaultWalkability, bool[][] iMatrix)
        {

            Node[][] tNodes = new Node[iWidth][];
            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                tNodes[widthTrav] = new Node[iHeight];
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    tNodes[widthTrav][heightTrav] = new Node(widthTrav, heightTrav, defaultWalkability);
                }
            }

            if (iMatrix == null)
            {
                return tNodes;
            }

            if (iMatrix.Length != iWidth || iMatrix[0].Length != iHeight)
            {
                throw new System.Exception("Matrix size does not fit");
            }


            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    if (iMatrix[widthTrav][heightTrav])
                    {
                        tNodes[widthTrav][heightTrav].walkable = true;
                    }
                    else
                    {
                        tNodes[widthTrav][heightTrav].walkable = false;
                    }
                }
            }
            return tNodes;
        }

        public override Node GetNodeAt(int iX, int iY)
        {
            return m_nodes[iX][iY];
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            return isInside(iX, iY) && m_nodes[iX][iY].walkable;
        }

        protected bool isInside(int iX, int iY)
        {
            return (iX >= 0 && iX < Width) && (iY >= 0 && iY < Height);
        }

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            m_nodes[iX][iY].walkable = iWalkable;
            return true;
        }

        protected bool isInside(GridPos iPos)
        {
            return isInside(iPos.x, iPos.y);
        }

        public override void Reset()
        {
            Reset(null);
        }

        public void Reset(bool[][] iMatrix)
        {
            for (int widthTrav = 0; widthTrav < Width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < Height; heightTrav++)
                {
                    m_nodes[widthTrav][heightTrav].Reset();
                }
            }

            if (iMatrix == null)
            {
                return;
            }
            if (iMatrix.Length != Width || iMatrix[0].Length != Height)
            {
                throw new System.Exception("Matrix size does not fit");
            }

            for (int widthTrav = 0; widthTrav < Width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < Height; heightTrav++)
                {
                    if (iMatrix[widthTrav][heightTrav])
                    {
                        m_nodes[widthTrav][heightTrav].walkable = true;
                    }
                    else
                    {
                        m_nodes[widthTrav][heightTrav].walkable = false;
                    }
                }
            }
        }

        public override BaseGrid Clone()
        {
            int tWidth = Width;
            int tHeight = Height;
            Node[][] tNodes = m_nodes;

            StaticGrid tNewGrid = new StaticGrid(tWidth, tHeight);

            Node[][] tNewNodes = new Node[tWidth][];
            for (int widthTrav = 0; widthTrav < tWidth; widthTrav++)
            {
                tNewNodes[widthTrav] = new Node[tHeight];
                for (int heightTrav = 0; heightTrav < tHeight; heightTrav++)
                {
                    tNewNodes[widthTrav][heightTrav] = new Node(widthTrav, heightTrav, tNodes[widthTrav][heightTrav].walkable);
                }
            }
            tNewGrid.m_nodes = tNewNodes;

            return tNewGrid;
        }
    }


}

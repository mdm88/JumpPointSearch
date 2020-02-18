using System.Collections.Generic;
using System.Linq;
using JumpPointSearch;
using JumpPointSearch.Grid;
using NUnit.Framework;

namespace Tests
{
    public class StaticGridTest
    {
        private BaseGrid _searchGrid;
        
        [SetUp]
        public void Setup()
        {
            _searchGrid = new StaticGrid(64, 32, true);
        }

        [Test]
        public void OkEmptyGridTest()
        {
            GridPos startPos=new GridPos(10,10); 
            GridPos endPos = new GridPos(20,10);
            JumpPointParam jpParam = new JumpPointParam(_searchGrid, startPos, endPos);
            
            List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam); 
            
            Assert.AreEqual(2, resultPathList.Count);
            Assert.AreEqual(startPos.x, resultPathList.First().x);
            Assert.AreEqual(startPos.y, resultPathList.First().y);
            Assert.AreEqual(endPos.x, resultPathList.Last().x);
            Assert.AreEqual(endPos.y, resultPathList.Last().y);
        }

        [Test]
        public void OkObstacleTest()
        {
            _searchGrid.SetWalkableAt(15, 10, false);
            
            GridPos startPos=new GridPos(10,10); 
            GridPos endPos = new GridPos(20,10);
            JumpPointParam jpParam = new JumpPointParam(_searchGrid, startPos, endPos);
            
            List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam); 
            
            Assert.Greater(resultPathList.Count, 2);
            Assert.AreEqual(startPos.x, resultPathList.First().x);
            Assert.AreEqual(startPos.y, resultPathList.First().y);
            Assert.AreEqual(endPos.x, resultPathList.Last().x);
            Assert.AreEqual(endPos.y, resultPathList.Last().y);
        }

        [Test]
        public void OkUnwalkableEndTest()
        {
            _searchGrid.SetWalkableAt(20, 10, false);
            
            GridPos startPos=new GridPos(10,10); 
            GridPos endPos = new GridPos(20,10);
            JumpPointParam jpParam = new JumpPointParam(_searchGrid, startPos, endPos);
            
            List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam); 
            
            Assert.AreEqual(2, resultPathList.Count);
            Assert.AreEqual(startPos.x, resultPathList.First().x);
            Assert.AreEqual(startPos.y, resultPathList.First().y);
            Assert.AreEqual(endPos.x, resultPathList.Last().x);
            Assert.AreEqual(endPos.y, resultPathList.Last().y);
        }

        [Test]
        public void ErrorUnrecheableTest()
        {
            _searchGrid.SetWalkableAt(19, 9, false);
            _searchGrid.SetWalkableAt(20, 9, false);
            _searchGrid.SetWalkableAt(21, 9, false);
            _searchGrid.SetWalkableAt(21, 10, false);
            _searchGrid.SetWalkableAt(21, 11, false);
            _searchGrid.SetWalkableAt(20, 11, false);
            _searchGrid.SetWalkableAt(19, 11, false);
            _searchGrid.SetWalkableAt(19, 10, false);
            
            GridPos startPos=new GridPos(10,10); 
            GridPos endPos = new GridPos(20,10);
            JumpPointParam jpParam = new JumpPointParam(_searchGrid, startPos, endPos);
            
            List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam); 
            
            Assert.IsEmpty(resultPathList);
        }
    }
}
using NUnit.Framework;
using Model.PlayerType;
using Model.Board;
using Moq;
using Layout.Layout;
using Position = Model.Position.Position;
using BoardClass = Model.Board.Board;

namespace Ataxx.Tests.Model.Board
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void Board_DefaultSize_CreatesCorrectBoard()
        {
            var board = new BoardClass();

            Assert.That(board.Size, Is.EqualTo(BoardConstants.DefaultSize));
        }

        [Test]
        public void Board_CustomSize_CreatesCorrectBoard()
        {
            var board = new BoardClass(10);

            Assert.That(board.Size, Is.EqualTo(10));
        }

        [Test]
        public void Board_SizeTooSmall_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new BoardClass(4));
        }

        [Test]
        public void Board_SizeTooLarge_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new BoardClass(21));
        }

        [Test]
        public void Initialize_WithBlockedCells_MarksCorrectCells()
        {
            var board = new BoardClass(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(3, 3, 7)).Returns(true);
            mockLayout.Setup(l => l.IsBlocked(3, 4, 7)).Returns(true);

            board.Initialize(mockLayout.Object);

            Assert.That(board.GetCell(new Position(3, 3)).IsBlocked, Is.True);
            Assert.That(board.GetCell(new Position(3, 4)).IsBlocked, Is.True);
        }

        [Test]
        public void Initialize_SetsCornerPieces_CorrectPlayers()
        {
            var board = new BoardClass(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            board.Initialize(mockLayout.Object);

            Assert.That(board.GetCell(new Position(0, 0)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(board.GetCell(new Position(0, 6)).OccupiedBy, Is.EqualTo(PlayerType.O));
            Assert.That(board.GetCell(new Position(6, 0)).OccupiedBy, Is.EqualTo(PlayerType.O));
            Assert.That(board.GetCell(new Position(6, 6)).OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void IsValidPosition_InsideBounds_ReturnsTrue()
        {
            var board = new BoardClass(7);

            Assert.That(board.IsValidPosition(new Position(0, 0)), Is.True);
            Assert.That(board.IsValidPosition(new Position(6, 6)), Is.True);
            Assert.That(board.IsValidPosition(new Position(3, 3)), Is.True);
        }

        [Test]
        public void IsValidPosition_OutsideBounds_ReturnsFalse()
        {
            var board = new BoardClass(7);

            Assert.That(board.IsValidPosition(new Position(-1, 0)), Is.False);
            Assert.That(board.IsValidPosition(new Position(0, -1)), Is.False);
            Assert.That(board.IsValidPosition(new Position(7, 0)), Is.False);
            Assert.That(board.IsValidPosition(new Position(0, 7)), Is.False);
        }

        [Test]
        public void GetCell_ValidPosition_ReturnsCell()
        {
            var board = new BoardClass(7);

            var cell = board.GetCell(new Position(3, 3));

            Assert.That(cell, Is.Not.Null);
        }

        [Test]
        public void GetCell_InvalidPosition_ThrowsException()
        {
            var board = new BoardClass(7);

            Assert.Throws<ArgumentException>(() => board.GetCell(new Position(-1, 0)));
            Assert.Throws<ArgumentException>(() => board.GetCell(new Position(7, 0)));
        }

        [Test]
        public void CountPieces_EmptyBoard_ReturnsZero()
        {
            var board = new BoardClass(7);

            var (xCount, oCount) = board.CountPieces();

            Assert.That(xCount, Is.EqualTo(0));
            Assert.That(oCount, Is.EqualTo(0));
        }

        [Test]
        public void CountPieces_WithPieces_ReturnsCorrectCounts()
        {
            var board = new BoardClass(7);
            board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);
            board.GetCell(new Position(0, 1)).OccupyBy(PlayerType.X);
            board.GetCell(new Position(0, 2)).OccupyBy(PlayerType.O);

            var (xCount, oCount) = board.CountPieces();

            Assert.That(xCount, Is.EqualTo(2));
            Assert.That(oCount, Is.EqualTo(1));
        }

        [Test]
        public void IsFull_EmptyBoard_ReturnsFalse()
        {
            var board = new BoardClass(7);

            var isFull = board.IsFull();

            Assert.That(isFull, Is.False);
        }

        [Test]
        public void IsFull_PartiallyFilled_ReturnsFalse()
        {
            var board = new BoardClass(7);
            board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);

            var isFull = board.IsFull();

            Assert.That(isFull, Is.False);
        }

        [Test]
        public void IsFull_CompletelyFilled_ReturnsTrue()
        {
            var board = new BoardClass(5);
            for (var row = 0; row < 5; row++)
            {
                for (var col = 0; col < 5; col++)
                {
                    board.GetCell(new Position(row, col)).OccupyBy(PlayerType.X);
                }
            }

            var isFull = board.IsFull();

            Assert.That(isFull, Is.True);
        }

        [Test]
        public void IsFull_WithBlockedCells_ReturnsTrueWhenAllNonBlockedFilled()
        {
            var board = new BoardClass(5);
            board.GetCell(new Position(2, 2)).MarkAsBlocked();
            
            for (var row = 0; row < 5; row++)
            {
                for (var col = 0; col < 5; col++)
                {
                    if (row == 2 && col == 2) continue;
                    board.GetCell(new Position(row, col)).OccupyBy(PlayerType.X);
                }
            }

            var isFull = board.IsFull();

            Assert.That(isFull, Is.True);
        }

        [Test]
        public void Clone_CreatesIndependentCopy()
        {
            var board = new BoardClass(7);
            board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);

            var clonedBoard = board.Clone();
            clonedBoard.GetCell(new Position(0, 1)).OccupyBy(PlayerType.O);

            Assert.That(board.GetCell(new Position(0, 1)).IsEmpty, Is.True);
            Assert.That(clonedBoard.GetCell(new Position(0, 1)).IsOccupied, Is.True);
        }

        [Test]
        public void Clone_CopiesBlockedCells()
        {
            var board = new BoardClass(7);
            board.GetCell(new Position(3, 3)).MarkAsBlocked();

            var clonedBoard = board.Clone();

            Assert.That(clonedBoard.GetCell(new Position(3, 3)).IsBlocked, Is.True);
        }

        [Test]
        public void GetCells_ReturnsCopyOfCells()
        {
            var board = new BoardClass(7);
            board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);

            var cells = board.GetCells();
            cells[0, 1].OccupyBy(PlayerType.O);

            Assert.That(board.GetCell(new Position(0, 1)).IsEmpty, Is.True);
        }

        [Test]
        public void GetCells_ReturnsCorrectDimensions()
        {
            var board = new BoardClass(7);

            var cells = board.GetCells();

            Assert.That(cells.GetLength(0), Is.EqualTo(7));
            Assert.That(cells.GetLength(1), Is.EqualTo(7));
        }
    }
}
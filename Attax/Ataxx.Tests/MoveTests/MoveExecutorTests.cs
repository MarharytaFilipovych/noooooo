using NUnit.Framework;
using Model.Board;
using Model.PlayerType;
using Move.Executor;
using MoveClass = Move.Move;
using Position = Model.Position.Position;

namespace Ataxx.Tests.Move.Executor
{
    [TestFixture]
    public class MoveExecutorTests
    {
        private MoveExecutor _executor;
        private Board _board;

        [SetUp]
        public void SetUp()
        {
            _executor = new MoveExecutor();
            _board = new Board(7);
        }

        [Test]
        public void ExecuteMove_CloneMove_OriginalPieceRemains()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(from).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(to).OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void ExecuteMove_JumpMove_OriginalPieceRemoved()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 5);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(from).IsEmpty, Is.True);
            Assert.That(_board.GetCell(to).OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void ExecuteMove_DiagonalClone_OriginalPieceRemains()
        {
            var from = new Position(3, 3);
            var to = new Position(4, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(from).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(to).OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void ExecuteMove_DiagonalJump_OriginalPieceRemoved()
        {
            var from = new Position(3, 3);
            var to = new Position(5, 5);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(from).IsEmpty, Is.True);
            Assert.That(_board.GetCell(to).OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void ExecuteMove_AdjacentOpponentPieces_ConvertsToCurrentPlayer()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(3, 5)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.O);
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(new Position(3, 5)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(4, 4)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(converted.Count, Is.EqualTo(2));
            Assert.That(converted, Contains.Item(new Position(3, 5)));
            Assert.That(converted, Contains.Item(new Position(4, 4)));
        }

        [Test]
        public void ExecuteMove_NoAdjacentOpponents_NoConversions()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(converted, Is.Empty);
        }

        [Test]
        public void ExecuteMove_AllEightDirections_ConvertsAllAdjacentOpponents()
        {
            var from = new Position(1, 3);
            var to = new Position(3, 3);
            _board.GetCell(from).OccupyBy(PlayerType.X);
    
            _board.GetCell(new Position(2, 2)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(2, 3)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(2, 4)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(3, 2)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(3, 4)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(4, 2)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(4, 3)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.O);
    
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(converted.Count, Is.EqualTo(8));  // Changed from 7 to 8
            Assert.That(_board.GetCell(new Position(2, 2)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(2, 3)).OccupiedBy, Is.EqualTo(PlayerType.X));  // Now gets converted
            Assert.That(_board.GetCell(new Position(2, 4)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(3, 2)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(3, 4)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(4, 2)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(4, 3)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(4, 4)).OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void ExecuteMove_BlockedCellsAdjacent_DoesNotConvertBlockedCells()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(3, 5)).MarkAsBlocked();
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.O);
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(new Position(3, 5)).IsBlocked, Is.True);
            Assert.That(converted.Count, Is.EqualTo(1));
            Assert.That(converted, Contains.Item(new Position(4, 4)));
        }

        [Test]
        public void ExecuteMove_OwnPiecesAdjacent_DoesNotConvertOwnPieces()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(3, 5)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.O);
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(new Position(3, 5)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(converted.Count, Is.EqualTo(1));
            Assert.That(converted, Contains.Item(new Position(4, 4)));
        }

        [Test]
        public void ExecuteMove_EmptyCellsAdjacent_DoesNotConvertEmptyCells()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.O);
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(_board.GetCell(new Position(2, 3)).IsEmpty, Is.True);
            Assert.That(_board.GetCell(new Position(3, 5)).IsEmpty, Is.True);
            Assert.That(converted.Count, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteMove_PlayerO_ConvertsPlayerXPieces()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(3, 5)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.O);

            Assert.That(_board.GetCell(new Position(3, 5)).OccupiedBy, Is.EqualTo(PlayerType.O));
            Assert.That(_board.GetCell(new Position(4, 4)).OccupiedBy, Is.EqualTo(PlayerType.O));
            Assert.That(converted.Count, Is.EqualTo(2));
        }

        [Test]
        public void ExecuteMove_EdgeOfBoard_ConvertsOnlyValidAdjacentPieces()
        {
            var from = new Position(0, 0);
            var to = new Position(0, 1);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(0, 2)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(1, 1)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(1, 2)).OccupyBy(PlayerType.O);
            var move = new MoveClass(from, to);

            var converted = _executor.ExecuteMove(_board, move, PlayerType.X);

            Assert.That(converted.Count, Is.EqualTo(3));
            Assert.That(_board.GetCell(new Position(0, 2)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(1, 1)).OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(_board.GetCell(new Position(1, 2)).OccupiedBy, Is.EqualTo(PlayerType.X));
        }
    }
}
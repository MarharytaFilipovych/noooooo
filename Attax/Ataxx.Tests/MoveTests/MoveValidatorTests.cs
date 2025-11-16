using NUnit.Framework;
using Model.Board;
using Model.PlayerType;
using Move.Validator;
using MoveClass = Move.Move;
using Position = Model.Position.Position;

namespace Ataxx.Tests.Move.Validator
{
    [TestFixture]
    public class MoveValidatorTests
    {
        private MoveValidator _validator;
        private Board _board;

        [SetUp]
        public void SetUp()
        {
            _validator = new MoveValidator();
            _board = new Board(7);
        }

        [Test]
        public void IsValidMove_CloneMoveDistanceOne_ReturnsTrue()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.True);
            Assert.That(error, Is.Null);
        }

        [Test]
        public void IsValidMove_JumpMoveDistanceTwo_ReturnsTrue()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 5);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.True);
            Assert.That(error, Is.Null);
        }

        [Test]
        public void IsValidMove_DiagonalCloneMove_ReturnsTrue()
        {
            var from = new Position(3, 3);
            var to = new Position(4, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.True);
            Assert.That(error, Is.Null);
        }

        [Test]
        public void IsValidMove_DiagonalJumpMove_ReturnsTrue()
        {
            var from = new Position(3, 3);
            var to = new Position(5, 5);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.True);
            Assert.That(error, Is.Null);
        }

        [Test]
        public void IsValidMove_AllDiagonalDirections_ReturnsTrue()
        {
            var from = new Position(3, 3);
            _board.GetCell(from).OccupyBy(PlayerType.X);

            var diagonalTargets = new[]
            {
                new Position(2, 2),
                new Position(2, 4),
                new Position(4, 2),
                new Position(4, 4)
            };

            foreach (var to in diagonalTargets)
            {
                var move = new MoveClass(from, to);
                var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out _);
                Assert.That(isValid, Is.True, $"Move to {to} should be valid");
            }
        }

        [Test]
        public void IsValidMove_InvalidDistanceThree_ReturnsFalse()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 6);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("Incorrect move distance"));
        }

        [Test]
        public void IsValidMove_InvalidDistanceZero_ReturnsFalse()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 3);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("Incorrect move distance"));
        }

        [Test]
        public void IsValidMove_FromPositionNotOccupiedByPlayer_ReturnsFalse()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.O);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("not occupied by your piece"));
        }

        [Test]
        public void IsValidMove_FromPositionEmpty_ReturnsFalse()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("not occupied by your piece"));
        }

        [Test]
        public void IsValidMove_TargetPositionBlocked_ReturnsFalse()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(to).MarkAsBlocked();
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("not empty"));
        }

        [Test]
        public void IsValidMove_TargetPositionOccupied_ReturnsFalse()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(to).OccupyBy(PlayerType.O);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("not empty"));
        }

        [Test]
        public void IsValidMove_TargetOccupiedBySamePlayer_ReturnsFalse()
        {
            var from = new Position(3, 3);
            var to = new Position(3, 4);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(to).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("not empty"));
        }

        [Test]
        public void IsValidMove_FromPositionOutOfBounds_ReturnsFalse()
        {
            var from = new Position(-1, 0);
            var to = new Position(0, 0);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("out of board bounds"));
        }

        [Test]
        public void IsValidMove_ToPositionOutOfBounds_ReturnsFalse()
        {
            var from = new Position(6, 6);
            var to = new Position(8, 8);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            var move = new MoveClass(from, to);

            var isValid = _validator.IsValidMove(_board, move, PlayerType.X, out var error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("out of board bounds"));
        }

        [Test]
        public void GetValidMoves_PlayerWithOnePiece_ReturnsAllValidMoves()
        {
            var centerPos = new Position(3, 3);
            _board.GetCell(centerPos).OccupyBy(PlayerType.X);

            var validMoves = _validator.GetValidMoves(_board, PlayerType.X);

            Assert.That(validMoves.Count, Is.EqualTo(24));
        }

        [Test]
        public void GetValidMoves_NoPlayerPieces_ReturnsEmptyList()
        {
            var validMoves = _validator.GetValidMoves(_board, PlayerType.X);

            Assert.That(validMoves, Is.Empty);
        }

        [Test]
        public void GetValidMoves_CornerPiece_ReturnsLimitedMoves()
        {
            var cornerPos = new Position(0, 0);
            _board.GetCell(cornerPos).OccupyBy(PlayerType.X);

            var validMoves = _validator.GetValidMoves(_board, PlayerType.X);

            Assert.That(validMoves.Count, Is.LessThan(24));
            Assert.That(validMoves.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetValidMoves_WithBlockedCells_ExcludesBlockedTargets()
        {
            var from = new Position(3, 3);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(3, 4)).MarkAsBlocked();

            var validMoves = _validator.GetValidMoves(_board, PlayerType.X);

            Assert.That(validMoves, Has.None.Matches<MoveClass>(m => m.To.Equals(new Position(3, 4))));
        }

        [Test]
        public void GetValidMoves_WithOccupiedCells_ExcludesOccupiedTargets()
        {
            var from = new Position(3, 3);
            _board.GetCell(from).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(3, 4)).OccupyBy(PlayerType.O);

            var validMoves = _validator.GetValidMoves(_board, PlayerType.X);

            Assert.That(validMoves, Has.None.Matches<MoveClass>(m => m.To.Equals(new Position(3, 4))));
        }

        [Test]
        public void GetValidMoves_MultiplePieces_ReturnsAllValidMovesForAllPieces()
        {
            _board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(6, 6)).OccupyBy(PlayerType.X);

            var validMoves = _validator.GetValidMoves(_board, PlayerType.X);

            Assert.That(validMoves.Count, Is.GreaterThan(0));
            Assert.That(validMoves, Has.Some.Matches<MoveClass>(m => m.From.Equals(new Position(0, 0))));
            Assert.That(validMoves, Has.Some.Matches<MoveClass>(m => m.From.Equals(new Position(6, 6))));
        }

        [Test]
        public void GetValidMoves_PlayerO_ReturnsMovesForPlayerO()
        {
            var pos = new Position(3, 3);
            _board.GetCell(pos).OccupyBy(PlayerType.O);

            var validMoves = _validator.GetValidMoves(_board, PlayerType.O);

            Assert.That(validMoves.Count, Is.EqualTo(24));
            Assert.That(validMoves, Has.All.Matches<MoveClass>(m => m.From.Equals(pos)));
        }
    }
}
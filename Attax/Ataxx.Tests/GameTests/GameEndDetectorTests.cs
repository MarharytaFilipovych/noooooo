using NUnit.Framework;
using Moq;
using Model.Board;
using Model.PlayerType;
using Model.Game.EndDetector;
using Move.Validator;
using Layout.Layout;
using MoveClass = Move.Move;
using Position = Model.Position.Position;

namespace Ataxx.Tests.Game.EndDetector
{
    [TestFixture]
    public class GameEndDetectorTests
    {
        private GameEndDetector _detector;
        private Mock<IMoveValidator> _mockValidator;
        private Board _board;

        [SetUp]
        public void SetUp()
        {
            _detector = new GameEndDetector();
            _mockValidator = new Mock<IMoveValidator>();
            _board = new Board(7);
            
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
        }

        [Test]
        public void CheckGameEnd_PlayerXEliminated_PlayerOWins()
        {
            _board = new Board(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
            
            _board.GetCell(new Position(0, 0)).Clear();
            _board.GetCell(new Position(6, 6)).Clear();
            _board.GetCell(new Position(3, 3)).OccupyBy(PlayerType.O);

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.True);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.O));
        }

        [Test]
        public void CheckGameEnd_PlayerOEliminated_PlayerXWins()
        {
            _board = new Board(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
            
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            _board.GetCell(new Position(3, 3)).OccupyBy(PlayerType.X);

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.O);

            Assert.That(result.IsEnded, Is.True);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void CheckGameEnd_BothPlayersHavePieces_GameNotEnded()
        {
            var validMoves = new System.Collections.Generic.List<MoveClass>
            {
                new MoveClass(new Position(0, 0), new Position(0, 1))
            };

            _mockValidator.Setup(v => v.GetValidMoves(_board, It.IsAny<PlayerType>()))
                .Returns(validMoves);

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.False);
        }

        [Test]
        public void CheckGameEnd_BoardFull_DeterminesWinnerByCount()
        {
            _board = new Board(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
            
            _board.GetCell(new Position(0, 0)).Clear();
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            _board.GetCell(new Position(6, 6)).Clear();
            
            for (var row = 0; row < 7; row++)
            {
                for (var col = 0; col < 7; col++)
                {
                    var player = (row + col) % 2 == 0 ? PlayerType.X : PlayerType.O;
                    _board.GetCell(new Position(row, col)).OccupyBy(player);
                }
            }

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.True);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void CheckGameEnd_BoardFullWithTie_ReturnsNoneAsWinner()
        {
            _board = new Board(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
            
            _board.GetCell(new Position(0, 0)).Clear();
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            _board.GetCell(new Position(6, 6)).Clear();
            
            _board.GetCell(new Position(3, 3)).MarkAsBlocked();
    
            int xCount = 0, oCount = 0;
            for (var row = 0; row < 7; row++)
            {
                for (var col = 0; col < 7; col++)
                {
                    if (row == 3 && col == 3) continue;
            
                    if (xCount < 24)
                    {
                        _board.GetCell(new Position(row, col)).OccupyBy(PlayerType.X);
                        xCount++;
                    }
                    else
                    {
                        _board.GetCell(new Position(row, col)).OccupyBy(PlayerType.O);
                        oCount++;
                    }
                }
            }

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.True);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.None));
        }

        [Test]
        public void CheckGameEnd_NoMovesAvailableForBothPlayers_DeterminesWinnerByCount()
        {
            _board = new Board(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
            
            _board.GetCell(new Position(0, 0)).Clear();
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            _board.GetCell(new Position(6, 6)).Clear();
            
            _board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(0, 1)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(6, 6)).OccupyBy(PlayerType.O);

            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.X))
                .Returns(new System.Collections.Generic.List<MoveClass>());
            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.O))
                .Returns(new System.Collections.Generic.List<MoveClass>());

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.True);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void CheckGameEnd_NoMovesButEqualPieces_ReturnsDraw()
        {
            _board = new Board(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
            
            _board.GetCell(new Position(0, 0)).Clear();
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            _board.GetCell(new Position(6, 6)).Clear();
            
            _board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(6, 6)).OccupyBy(PlayerType.O);

            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.X))
                .Returns(new System.Collections.Generic.List<MoveClass>());
            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.O))
                .Returns(new System.Collections.Generic.List<MoveClass>());

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.True);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.None));
        }

        [Test]
        public void CheckGameEnd_CurrentPlayerHasMoves_GameContinues()
        {
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            
            _board.GetCell(new Position(3, 3)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.O);

            var validMoves = new System.Collections.Generic.List<MoveClass>
            {
                new MoveClass(new Position(3, 3), new Position(3, 4))
            };

            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.X))
                .Returns(validMoves);
            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.O))
                .Returns(new System.Collections.Generic.List<MoveClass>());

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.False);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.None));
        }

        [Test]
        public void CheckGameEnd_OpponentHasMoves_GameContinues()
        {
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            
            _board.GetCell(new Position(3, 3)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(4, 4)).OccupyBy(PlayerType.O);

            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.X))
                .Returns(new System.Collections.Generic.List<MoveClass>());
            
            var opponentMoves = new System.Collections.Generic.List<MoveClass>
            {
                new MoveClass(new Position(4, 4), new Position(4, 5))
            };
            _mockValidator.Setup(v => v.GetValidMoves(_board, PlayerType.O))
                .Returns(opponentMoves);

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.False);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.None));
        }

        [Test]
        public void CheckGameEnd_CurrentPlayerNoMoves_OpponentNoMoves_GameEnds()
        {
            _board = new Board(7);
            var mockLayout = new Mock<IBoardLayout>();
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _board.Initialize(mockLayout.Object);
            
            _board.GetCell(new Position(0, 0)).Clear();
            _board.GetCell(new Position(0, 6)).Clear();
            _board.GetCell(new Position(6, 0)).Clear();
            _board.GetCell(new Position(6, 6)).Clear();
            
            _board.GetCell(new Position(0, 0)).OccupyBy(PlayerType.X);
            _board.GetCell(new Position(6, 6)).OccupyBy(PlayerType.O);
            _board.GetCell(new Position(6, 5)).OccupyBy(PlayerType.O);

            _mockValidator.Setup(v => v.GetValidMoves(_board, It.IsAny<PlayerType>()))
                .Returns(new System.Collections.Generic.List<MoveClass>());

            var result = _detector.CheckGameEnd(_board, _mockValidator.Object, PlayerType.X);

            Assert.That(result.IsEnded, Is.True);
            Assert.That(result.Winner, Is.EqualTo(PlayerType.O));
        }
    }
}
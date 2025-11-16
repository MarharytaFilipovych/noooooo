using NUnit.Framework;
using Moq;
using Model.Game.Game;
using Model.Game.Settings;
using Model.Game.TurnTimer;
using Model.Game.CareTaker;
using Model.Game.EndDetector;
using Model.PlayerType;
using Move.Executor;
using Move.Generator;
using Move.Validator;
using Stats.Tracker;
using Layout.Factory;
using GameMode.Factory;
using GameMode;
using GameMode.ModeConfigurations;
using MoveClass = Move.Move;
using BoardClass = Model.Board.Board;
using Position = Model.Position.Position;

namespace Ataxx.Tests.Game
{
    [TestFixture]
    public class AtaxxGameTests
    {
        private AtaxxGame _game;
        private Mock<IStatsTracker> _mockStatsTracker;
        private Mock<ITurnTimer> _mockTurnTimer;
        private Mock<IMoveValidator> _mockValidator;
        private Mock<IMoveExecutor> _mockExecutor;
        private Mock<IMoveGenerator> _mockGenerator;
        private Mock<IGameEndDetector> _mockEndDetector;
        private Mock<IGameSettings> _mockSettings;
        private Mock<ICareTakerFactory> _mockCareTakerFactory;
        private Mock<ICareTaker> _mockCareTaker;
        private Mock<IBoardLayoutFactory> _mockLayoutFactory;
        private Mock<IGameModeFactory> _mockGameModeFactory;

        [SetUp]
        public void SetUp()
        {
            _mockStatsTracker = new Mock<IStatsTracker>();
            _mockTurnTimer = new Mock<ITurnTimer>();
            _mockValidator = new Mock<IMoveValidator>();
            _mockExecutor = new Mock<IMoveExecutor>();
            _mockGenerator = new Mock<IMoveGenerator>();
            _mockEndDetector = new Mock<IGameEndDetector>();
            _mockSettings = new Mock<IGameSettings>();
            _mockCareTakerFactory = new Mock<ICareTakerFactory>();
            _mockCareTaker = new Mock<ICareTaker>();
            _mockLayoutFactory = new Mock<IBoardLayoutFactory>();
            _mockGameModeFactory = new Mock<IGameModeFactory>();

            _mockCareTakerFactory.Setup(f => f.Create(It.IsAny<AtaxxGame>()))
                .Returns(_mockCareTaker.Object);

            var mockLayout = new Mock<Layout.Layout.IBoardLayout>();
            mockLayout.Setup(l => l.Name).Returns("Test layout");
            mockLayout.Setup(l => l.Type).Returns(Layout.LayoutType.Classic);
            mockLayout.Setup(l => l.IsBlocked(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            _mockLayoutFactory.Setup(f => f.GetRandomLayout(It.IsAny<Random>()))
                .Returns(mockLayout.Object);

            _mockGameModeFactory.Setup(f => f.GetDefaultConfiguration())
                .Returns(new PvPConfiguration());

            _mockSettings.Setup(s => s.BoardSize).Returns(7);

            _game = new AtaxxGame(
                _mockStatsTracker.Object,
                _mockTurnTimer.Object,
                _mockValidator.Object,
                _mockExecutor.Object,
                _mockGenerator.Object,
                _mockEndDetector.Object,
                _mockSettings.Object,
                _mockCareTakerFactory.Object,
                _mockLayoutFactory.Object,
                _mockGameModeFactory.Object
            );
        }

        [Test]
        public void StartGame_InitializesBoard()
        {
            _game.StartGame();

            Assert.That(_game.Board, Is.Not.Null);
            Assert.That(_game.Board.Size, Is.EqualTo(7));
            Assert.That(_game.CurrentPlayer, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void StartGame_WithCustomSize_UsesCustomSize()
        {
            _game.StartGame(10);

            Assert.That(_game.Board.Size, Is.EqualTo(10));
        }

        [Test]
        public void StartGame_MarksSettingsAsStarted()
        {
            _game.StartGame();

            _mockSettings.Verify(s => s.MarkGameAsStarted(), Times.Once);
        }

        [Test]
        public void MakeMove_BeforeGameStart_ThrowsException()
        {
            var from = new Position(0, 0);
            var to = new Position(0, 1);

            Assert.Throws<InvalidOperationException>(() => _game.MakeMove(from, to));
        }

        [Test]
        public void MakeMove_AfterGameEnd_ThrowsException()
        {
            _game.StartGame();
            
            _mockEndDetector.Setup(d => d.CheckGameEnd(It.IsAny<BoardClass>(), 
                It.IsAny<IMoveValidator>(), It.IsAny<PlayerType>()))
                .Returns(new GameEndResult(true, PlayerType.X));

            _mockValidator.Setup(v => v.IsValidMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>(), out It.Ref<string?>.IsAny))
                .Returns(true);

            _mockExecutor.Setup(e => e.ExecuteMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>()))
                .Returns(new List<Position>());

            _mockValidator.Setup(v => v.GetValidMoves(It.IsAny<BoardClass>(), 
                It.IsAny<PlayerType>()))
                .Returns(new List<MoveClass>());

            _game.MakeMove(new Position(0, 0), new Position(0, 1));

            Assert.Throws<InvalidOperationException>(() => 
                _game.MakeMove(new Position(0, 0), new Position(0, 1)));
        }

        [Test]
        public void MakeMove_InvalidMove_ReturnsFalse()
        {
            _game.StartGame();

            string? error = "Invalid move";
            _mockValidator.Setup(v => v.IsValidMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>(), out error))
                .Returns(false);

            var result = _game.MakeMove(new Position(0, 0), new Position(5, 5));

            Assert.That(result, Is.False);
        }

        [Test]
        public void MakeMove_ValidMove_ExecutesMoveAndSwitchesPlayer()
        {
            _game.StartGame();

            _mockValidator.Setup(v => v.IsValidMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>(), out It.Ref<string?>.IsAny))
                .Returns(true);

            _mockExecutor.Setup(e => e.ExecuteMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>()))
                .Returns(new List<Position>());

            _mockEndDetector.Setup(d => d.CheckGameEnd(It.IsAny<BoardClass>(), 
                It.IsAny<IMoveValidator>(), It.IsAny<PlayerType>()))
                .Returns(new GameEndResult(false, PlayerType.None));

            _mockValidator.Setup(v => v.GetValidMoves(It.IsAny<BoardClass>(), 
                It.IsAny<PlayerType>()))
                .Returns(new List<MoveClass> { new MoveClass(new Position(0, 0), new Position(0, 1)) });

            var initialPlayer = _game.CurrentPlayer;
            var result = _game.MakeMove(new Position(0, 0), new Position(0, 1));

            Assert.That(result, Is.True);
            Assert.That(_game.CurrentPlayer, Is.Not.EqualTo(initialPlayer));
        }

        [Test]
        public void MakeMove_PlayerHasNoValidMovesButOpponentDoes_SkipsToOpponent()
        {
            _game.StartGame();

            _mockValidator.Setup(v => v.IsValidMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>(), out It.Ref<string?>.IsAny))
                .Returns(true);

            _mockExecutor.Setup(e => e.ExecuteMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>()))
                .Returns(new List<Position>());

            _mockEndDetector.Setup(d => d.CheckGameEnd(It.IsAny<BoardClass>(), 
                It.IsAny<IMoveValidator>(), It.IsAny<PlayerType>()))
                .Returns(new GameEndResult(false, PlayerType.None));

            _mockValidator.Setup(v => v.GetValidMoves(It.IsAny<BoardClass>(), PlayerType.O))
                .Returns(new List<MoveClass>());

            _mockValidator.Setup(v => v.GetValidMoves(It.IsAny<BoardClass>(), PlayerType.X))
                .Returns(new List<MoveClass> { new MoveClass(new Position(0, 0), new Position(0, 1)) });

            _game.MakeMove(new Position(0, 0), new Position(0, 1));

            Assert.That(_game.CurrentPlayer, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void MakeMove_GameEnds_RecordsStatistics()
        {
            _game.StartGame();

            _mockValidator.Setup(v => v.IsValidMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>(), out It.Ref<string?>.IsAny))
                .Returns(true);

            _mockExecutor.Setup(e => e.ExecuteMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>()))
                .Returns(new List<Position>());

            _mockEndDetector.Setup(d => d.CheckGameEnd(It.IsAny<BoardClass>(), 
                It.IsAny<IMoveValidator>(), It.IsAny<PlayerType>()))
                .Returns(new GameEndResult(true, PlayerType.X));

            _mockValidator.Setup(v => v.GetValidMoves(It.IsAny<BoardClass>(), 
                It.IsAny<PlayerType>()))
                .Returns(new List<MoveClass>());

            _game.MakeMove(new Position(0, 0), new Position(0, 1));

            Assert.That(_game.IsEnded, Is.True);
            Assert.That(_game.Winner, Is.EqualTo(PlayerType.X));
            _mockStatsTracker.Verify(s => s.RecordGame(PlayerType.X, It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetValidMoves_ReturnsMovesForCurrentPlayer()
        {
            _game.StartGame();

            var expectedMoves = new List<MoveClass>
            {
                new MoveClass(new Position(0, 0), new Position(0, 1)),
                new MoveClass(new Position(0, 0), new Position(1, 0))
            };

            _mockValidator.Setup(v => v.GetValidMoves(It.IsAny<BoardClass>(), PlayerType.X))
                .Returns(expectedMoves);

            var moves = _game.GetValidMoves();

            Assert.That(moves, Is.EqualTo(expectedMoves));
        }

        [Test]
        public void UndoLastMove_InPvPMode_ReturnsFalse()
        {
            _mockSettings.Setup(s => s.GameModeType).Returns((ModeType?)ModeType.PvP);
            _mockGameModeFactory.Setup(f => f.GetConfiguration(ModeType.PvP))
                .Returns(new PvPConfiguration());

            _game.StartGame();

            var result = _game.UndoLastMove();

            Assert.That(result, Is.False);
            _mockCareTaker.Verify(c => c.Undo(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UndoLastMove_InPvEMode_CallsCareTaker()
        {
            _mockSettings.Setup(s => s.GameModeType).Returns(ModeType.PvE);
            _mockSettings.Setup(s => s.BotDifficulty).Returns(BotDifficulty.Easy);
            
            _game.StartGame();

            _mockCareTaker.Setup(c => c.Undo(It.IsAny<int>())).Returns(true);

            var result = _game.UndoLastMove();

            Assert.That(result, Is.True);
            _mockCareTaker.Verify(c => c.Undo(3), Times.Once);
        }

        [Test]
        public void GetPieceCounts_ReturnsCorrectCounts()
        {
            _game.StartGame();

            var (xCount, oCount) = _game.GetPieceCounts();

            Assert.That(xCount, Is.EqualTo(2));
            Assert.That(oCount, Is.EqualTo(2));
        }

        [Test]
        public void SaveAndRestore_PreservesGameState()
        {
            _game.StartGame();

            _mockValidator.Setup(v => v.IsValidMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>(), out It.Ref<string?>.IsAny))
                .Returns(true);

            _mockExecutor.Setup(e => e.ExecuteMove(It.IsAny<BoardClass>(), 
                It.IsAny<MoveClass>(), It.IsAny<PlayerType>()))
                .Returns(new List<Position>());

            _mockEndDetector.Setup(d => d.CheckGameEnd(It.IsAny<BoardClass>(), 
                It.IsAny<IMoveValidator>(), It.IsAny<PlayerType>()))
                .Returns(new GameEndResult(false, PlayerType.None));

            _mockValidator.Setup(v => v.GetValidMoves(It.IsAny<BoardClass>(), 
                It.IsAny<PlayerType>()))
                .Returns(new List<MoveClass> { new MoveClass(new Position(0, 0), new Position(0, 1)) });

            _game.MakeMove(new Position(0, 0), new Position(0, 1));
            var playerAfterMove = _game.CurrentPlayer;

            var memento = _game.Save();

            _game.MakeMove(new Position(0, 6), new Position(0, 5));

            _game.Restore(memento);

            Assert.That(_game.CurrentPlayer, Is.EqualTo(playerAfterMove));
        }

        [Test]
        public void GetGameState_ReturnsCompleteState()
        {
            _game.StartGame();

            var state = _game.GetGameState();

            Assert.That(state.BoardSize, Is.EqualTo(7));
            Assert.That(state.CurrentPlayer, Is.EqualTo(PlayerType.X));
            Assert.That(state.XCount, Is.EqualTo(2));
            Assert.That(state.OCount, Is.EqualTo(2));
            Assert.That(state.isEnded, Is.False);
            Assert.That(state.Winner, Is.EqualTo(PlayerType.None));
        }
    }
}
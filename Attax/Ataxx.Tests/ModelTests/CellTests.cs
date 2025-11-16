using NUnit.Framework;
using Model;
using Model.PlayerType;

namespace Ataxx.Tests.Model
{
    [TestFixture]
    public class CellTests
    {
        private Cell _cell;

        [SetUp]
        public void SetUp()
        {
            _cell = new Cell();
        }

        [Test]
        public void NewCell_IsEmptyAndNotBlocked()
        {
            Assert.That(_cell.IsEmpty, Is.True);
            Assert.That(_cell.IsBlocked, Is.False);
            Assert.That(_cell.IsOccupied, Is.False);
            Assert.That(_cell.OccupiedBy, Is.EqualTo(PlayerType.None));
        }

        [Test]
        public void OccupyBy_ValidPlayer_OccupiesCell()
        {
            _cell.OccupyBy(PlayerType.X);

            Assert.That(_cell.IsOccupied, Is.True);
            Assert.That(_cell.IsEmpty, Is.False);
            Assert.That(_cell.OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void OccupyBy_PlayerO_OccupiesCell()
        {
            _cell.OccupyBy(PlayerType.O);

            Assert.That(_cell.IsOccupied, Is.True);
            Assert.That(_cell.OccupiedBy, Is.EqualTo(PlayerType.O));
        }

        [Test]
        public void OccupyBy_NonePlayer_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _cell.OccupyBy(PlayerType.None));
        }

        [Test]
        public void OccupyBy_AlreadyOccupied_ThrowsException()
        {
            _cell.OccupyBy(PlayerType.X);

            Assert.Throws<InvalidOperationException>(() => _cell.OccupyBy(PlayerType.O));
        }

        [Test]
        public void OccupyBy_BlockedCell_ThrowsException()
        {
            _cell.MarkAsBlocked();

            Assert.Throws<InvalidOperationException>(() => _cell.OccupyBy(PlayerType.X));
        }

        [Test]
        public void MarkAsBlocked_EmptyCell_BlocksCell()
        {
            _cell.MarkAsBlocked();

            Assert.That(_cell.IsBlocked, Is.True);
            Assert.That(_cell.IsEmpty, Is.False);
        }

        [Test]
        public void MarkAsBlocked_OccupiedCell_ThrowsException()
        {
            _cell.OccupyBy(PlayerType.X);

            Assert.Throws<InvalidOperationException>(() => _cell.MarkAsBlocked());
        }

        [Test]
        public void ConvertTo_OccupiedCell_ChangesPlayer()
        {
            _cell.OccupyBy(PlayerType.X);

            _cell.ConvertTo(PlayerType.O);

            Assert.That(_cell.OccupiedBy, Is.EqualTo(PlayerType.O));
            Assert.That(_cell.IsOccupied, Is.True);
        }

        [Test]
        public void ConvertTo_OccupiedCellReverse_ChangesPlayer()
        {
            _cell.OccupyBy(PlayerType.O);

            _cell.ConvertTo(PlayerType.X);

            Assert.That(_cell.OccupiedBy, Is.EqualTo(PlayerType.X));
        }

        [Test]
        public void ConvertTo_NonePlayer_ThrowsException()
        {
            _cell.OccupyBy(PlayerType.X);

            Assert.Throws<ArgumentException>(() => _cell.ConvertTo(PlayerType.None));
        }

        [Test]
        public void ConvertTo_EmptyCell_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _cell.ConvertTo(PlayerType.X));
        }

        [Test]
        public void ConvertTo_BlockedCell_ThrowsException()
        {
            _cell.MarkAsBlocked();

            Assert.Throws<InvalidOperationException>(() => _cell.ConvertTo(PlayerType.X));
        }

        [Test]
        public void Clear_OccupiedCell_ClearsCell()
        {
            _cell.OccupyBy(PlayerType.X);

            _cell.Clear();

            Assert.That(_cell.IsEmpty, Is.True);
            Assert.That(_cell.OccupiedBy, Is.EqualTo(PlayerType.None));
            Assert.That(_cell.IsOccupied, Is.False);
        }

        [Test]
        public void Clear_EmptyCell_RemainsEmpty()
        {
            _cell.Clear();

            Assert.That(_cell.IsEmpty, Is.True);
        }

        [Test]
        public void Clear_BlockedCell_ThrowsException()
        {
            _cell.MarkAsBlocked();

            Assert.Throws<InvalidOperationException>(() => _cell.Clear());
        }

        [Test]
        public void Clone_EmptyCell_ReturnsIdenticalCell()
        {
            var cloned = _cell.Clone();

            Assert.That(cloned.OccupiedBy, Is.EqualTo(_cell.OccupiedBy));
            Assert.That(cloned.IsBlocked, Is.EqualTo(_cell.IsBlocked));
            Assert.That(cloned.IsEmpty, Is.EqualTo(_cell.IsEmpty));
            Assert.That(cloned, Is.Not.SameAs(_cell));
        }

        [Test]
        public void Clone_OccupiedCell_ReturnsIdenticalCell()
        {
            _cell.OccupyBy(PlayerType.X);

            var cloned = _cell.Clone();

            Assert.That(cloned.OccupiedBy, Is.EqualTo(_cell.OccupiedBy));
            Assert.That(cloned.IsBlocked, Is.EqualTo(_cell.IsBlocked));
            Assert.That(cloned, Is.Not.SameAs(_cell));
        }

        [Test]
        public void Clone_BlockedCell_ReturnsIdenticalCell()
        {
            _cell.MarkAsBlocked();

            var cloned = _cell.Clone();

            Assert.That(cloned.IsBlocked, Is.True);
            Assert.That(cloned, Is.Not.SameAs(_cell));
        }

        [Test]
        public void Clone_IndependentCopy_ModificationsDoNotAffectOriginal()
        {
            _cell.OccupyBy(PlayerType.X);
            var cloned = _cell.Clone();

            cloned.ConvertTo(PlayerType.O);

            Assert.That(_cell.OccupiedBy, Is.EqualTo(PlayerType.X));
            Assert.That(cloned.OccupiedBy, Is.EqualTo(PlayerType.O));
        }
    }
}
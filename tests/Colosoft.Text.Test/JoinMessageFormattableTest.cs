using System;
using System.Globalization;

namespace Colosoft.Text.Test
{
    public class JoinMessageFormattableTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLeftMessageIsNull()
        {
            var right = new TextMessageFormattable("right");

            Assert.Throws<ArgumentNullException>(() => new JoinMessageFormattable(null, ", ", right));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRightMessageIsNull()
        {
            var left = new TextMessageFormattable("left");

            Assert.Throws<ArgumentNullException>(() => new JoinMessageFormattable(left, ", ", null));
        }

        [Fact]
        public void Format_ShouldConcatenateLeftSeparatorAndRight()
        {
            var left = new TextMessageFormattable("Hello");
            var right = new TextMessageFormattable("World");
            var sut = new JoinMessageFormattable(left, " - ", right);

            var result = sut.Format(CultureInfo.InvariantCulture);

            Assert.Equal("Hello - World", result);
        }

        [Fact]
        public void FormatWithParameters_ShouldForwardParametersToBothMessages()
        {
            var left = new TextMessageFormattable("{0:N1}");
            var right = new TextMessageFormattable("{1}");
            var sut = new JoinMessageFormattable(left, " | ", right);

            var result = sut.Format(new CultureInfo("en-US"), 12.3m, "done");

            Assert.Equal("12.3 | done", result);
        }

        [Fact]
        public void Join_ShouldReturnAnotherJoinMessageFormattable()
        {
            var left = new TextMessageFormattable("A");
            var right = new TextMessageFormattable("B");
            var sut = new JoinMessageFormattable(left, ",", right);

            var result = sut.Join("-", new TextMessageFormattable("C"));

            Assert.IsType<JoinMessageFormattable>(result);
            Assert.Equal("A,B-C", result.Format(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Matches_ShouldReturnTrue_WhenBothMessagesMatch()
        {
            var sut = new JoinMessageFormattable(new FakeMessageFormattable(true), ",", new FakeMessageFormattable(true));

            var result = sut.Matches(CultureInfo.InvariantCulture);

            Assert.True(result);
        }

        [Fact]
        public void Matches_ShouldReturnFalse_WhenAnyMessageDoesNotMatch()
        {
            var sut = new JoinMessageFormattable(new FakeMessageFormattable(true), ",", new FakeMessageFormattable(false));

            var result = sut.Matches(CultureInfo.InvariantCulture);

            Assert.False(result);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenMessagesAreEquivalent()
        {
            var left = new TextMessageFormattable("left");
            var right = new TextMessageFormattable("right");
            var sut = new JoinMessageFormattable(left, ",", right);
            var other = new JoinMessageFormattable(left, ",", right);

            var result = sut.Equals(other);

            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenMessagesAreDifferent()
        {
            var sut = new JoinMessageFormattable(new TextMessageFormattable("left"), ",", new TextMessageFormattable("right"));
            var other = new JoinMessageFormattable(new TextMessageFormattable("left"), ",", new TextMessageFormattable("other"));

            var result = sut.Equals(other);

            Assert.False(result);
        }

        [Fact]
        public void Clone_ShouldCreateEquivalentButDifferentInstance()
        {
            var sut = new JoinMessageFormattable(new TextMessageFormattable("L"), "-", new TextMessageFormattable("R"));

            var clone = (JoinMessageFormattable)sut.Clone();

            Assert.NotSame(sut, clone);
            Assert.True(sut.Equals(clone));
            Assert.Equal(sut.Format(CultureInfo.InvariantCulture), clone.Format(CultureInfo.InvariantCulture));
        }

        private class FakeMessageFormattable : IMessageFormattable
        {
            private readonly bool matches;

            public FakeMessageFormattable(bool matches)
            {
                this.matches = matches;
            }

            public string Format()
            {
                return string.Empty;
            }

            public string Format(CultureInfo culture)
            {
                return string.Empty;
            }

            public string Format(CultureInfo culture, params object[] parameters)
            {
                return string.Empty;
            }

            public IMessageFormattable Join(string separator, IMessageFormattable message)
            {
                return new JoinMessageFormattable(this, separator, message);
            }

            public bool Matches(CultureInfo culture)
            {
                return this.matches;
            }

            public bool Equals(IMessageFormattable? other)
            {
                return ReferenceEquals(this, other);
            }
        }
    }
}

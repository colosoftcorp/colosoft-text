using System.Globalization;

namespace Colosoft.Text.Test
{
    public class TextMessageFormattableTest
    {
        [Fact]
        public void Constructor_ShouldSetTextAndParameters()
        {
            var sut = new TextMessageFormattable("Hello {0}", "World");

            Assert.Equal("Hello {0}", sut.Text);
            Assert.Single(sut.Parameters);
            Assert.Equal("World", sut.Parameters[0]);
        }

        [Fact]
        public void Format_ShouldReturnNull_WhenTextIsNull()
        {
            var sut = new TextMessageFormattable(null);

            var result = sut.Format(CultureInfo.InvariantCulture, "value");

            Assert.Null(result);
        }

        [Fact]
        public void Format_ShouldReturnNull_WhenTextIsEmpty()
        {
            var sut = new TextMessageFormattable(string.Empty);

            var result = sut.Format(CultureInfo.InvariantCulture, "value");

            Assert.Null(result);
        }

        [Fact]
        public void Format_ShouldApplyCulture_WhenParametersAreProvided()
        {
            var sut = new TextMessageFormattable("Value: {0:N2}");
            var culture = new CultureInfo("pt-BR");

            var result = sut.Format(culture, 1234.5m);

            Assert.Equal("Value: 1.234,50", result);
        }

        [Fact]
        public void InterfaceFormatWithCulture_ShouldUseInstanceParameters()
        {
            var inner = new TextMessageFormattable("{0:N1}", 12.3m);
            var sut = new TextMessageFormattable("Value: {0}", inner);
            var formattable = (IMessageFormattable)sut;

            var result = formattable.Format(new CultureInfo("en-US"));

            Assert.Equal("Value: 12.3", result);
        }

        [Fact]
        public void Join_ShouldReturnJoinMessageFormattable()
        {
            var left = new TextMessageFormattable("Hello");
            var right = new TextMessageFormattable("World");

            var joined = left.Join(", ", right);

            Assert.IsType<JoinMessageFormattable>(joined);
            Assert.Equal("Hello, World", joined.Format(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenTextAndParameterReferencesMatch()
        {
            var parameter = new object();
            var left = new TextMessageFormattable("{0}", parameter);
            var right = new TextMessageFormattable("{0}", parameter);

            var result = left.Equals(right);

            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenParametersDiffer()
        {
            var left = new TextMessageFormattable("{0}", "A");
            var right = new TextMessageFormattable("{0}", "B");

            var result = left.Equals(right);

            Assert.False(result);
        }

        [Fact]
        public void Clone_ShouldCreateNewInstance_WithSameContent()
        {
            var sut = new TextMessageFormattable("{0}", "A");

            var clone = (TextMessageFormattable)sut.Clone();

            Assert.NotSame(sut, clone);
            Assert.Equal(sut.Text, clone.Text);
            Assert.True(sut.Equals(clone));
        }

        [Fact]
        public void ToString_ShouldReturnRawText_WhenNoExplicitParametersAreProvided()
        {
            var sut = new TextMessageFormattable("Value: {0}", "X");

            var result = sut.ToString();

            Assert.Equal("Value: {0}", result);
        }
    }
}

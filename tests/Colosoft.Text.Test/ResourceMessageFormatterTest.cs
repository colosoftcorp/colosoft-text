using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Colosoft.Text.Test
{
    public class ResourceMessageFormatterTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenBaseNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ResourceMessageFormatter(null, "Greeting", typeof(FakeResources)));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ResourceMessageFormatter("FakeBase", null, typeof(FakeResources)));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenResourceTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ResourceMessageFormatter("FakeBase", "Greeting", null));
        }

        [Fact]
        public void Create_ShouldBuildFormatterFromResourceProperty()
        {
            var sut = ResourceMessageFormatter.Create(() => FakeResources.Greeting, "World");

            Assert.Equal("Fake.Base", sut.BaseName);
            Assert.Equal("Greeting", sut.Name);
            Assert.Equal(typeof(FakeResources), sut.ResourceType);
            Assert.Single(sut.Parameters);
            Assert.Equal("World", sut.Parameters[0]);
        }

        [Fact]
        public void Format_ShouldReturnFormattedResource_WhenParametersAreProvided()
        {
            var sut = new ResourceMessageFormatter("Fake.Base", "Greeting", typeof(FakeResources), "World");

            var result = sut.Format(CultureInfo.InvariantCulture, sut.Parameters);

            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void Format_ShouldReturnNull_WhenResourceKeyDoesNotExist()
        {
            var sut = new ResourceMessageFormatter("Fake.Base", "Missing", typeof(FakeResources));

            var result = sut.Format(CultureInfo.InvariantCulture);

            Assert.Null(result);
        }

        [Fact]
        public void Format_ShouldResolveNestedMessageFormattableParameter()
        {
            var nested = new TextMessageFormattable("{0}", "done");
            var sut = new ResourceMessageFormatter("Fake.Base", "Nested", typeof(FakeResources), nested);

            var result = sut.Format(CultureInfo.InvariantCulture, sut.Parameters);

            Assert.Equal("Nested done", result);
        }

        [Fact]
        public void FormatWithoutCulture_ShouldUseCurrentThreadCulture()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                var sut = new ResourceMessageFormatter("Fake.Base", "Number", typeof(FakeResources), 1234.5m);

                var result = sut.Format();

                Assert.Equal("Valor: 1.234,50", result);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public void Join_ShouldReturnJoinMessageFormattable()
        {
            var left = new ResourceMessageFormatter("Fake.Base", "Greeting", typeof(FakeResources), "A");
            var right = new TextMessageFormattable("B");

            var result = left.Join(" - ", right);

            Assert.IsType<JoinMessageFormattable>(result);
            Assert.Equal("Hello A - B", result.Format(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenAllValuesAndParameterReferencesMatch()
        {
            var parameter = new object();
            var left = new ResourceMessageFormatter("Fake.Base", "Greeting", typeof(FakeResources), parameter);
            var right = new ResourceMessageFormatter("Fake.Base", "Greeting", typeof(FakeResources), parameter);

            var result = left.Equals(right);

            Assert.True(result);
        }

        [Fact]
        public void Clone_ShouldCreateEquivalentInstance()
        {
            var sut = new ResourceMessageFormatter("Fake.Base", "Greeting", typeof(FakeResources), "X");

            var clone = (ResourceMessageFormatter)sut.Clone();

            Assert.NotSame(sut, clone);
            Assert.True(sut.Equals(clone));
        }

        [Fact]
        public void ToString_ShouldReturnFormattedMessage()
        {
            var sut = new ResourceMessageFormatter("Fake.Base", "Greeting", typeof(FakeResources), "Copilot");

            var result = sut.ToString();

            Assert.Equal("Hello Copilot", result);
        }

        [Fact]
        public void Matches_ShouldAlwaysReturnTrue()
        {
            var sut = new ResourceMessageFormatter("Fake.Base", "Greeting", typeof(FakeResources));

            var result = sut.Matches(CultureInfo.InvariantCulture);

            Assert.True(result);
        }

        private sealed class FakeResources
        {
            private static readonly ResourceManager ResourceManagerValue = new FakeResourceManager();

            public static ResourceManager ResourceManager => ResourceManagerValue;

            public static string Greeting => string.Empty;

            public static string Number => string.Empty;

            public static string Nested => string.Empty;

            public static string Missing => string.Empty;
        }

        private sealed class FakeResourceManager : ResourceManager
        {
            public FakeResourceManager()
                : base("Fake.Base", typeof(FakeResources).GetTypeInfo().Assembly)
            {
            }

            public override string GetString(string name, CultureInfo? culture)
            {
                if (name == "Greeting")
                {
                    return "Hello {0}";
                }

                if (name == "Number")
                {
                    return "Valor: {0:N2}";
                }

                if (name == "Nested")
                {
                    return "Nested {0}";
                }

                return string.Empty;
            }
        }
    }
}

namespace WeatherStats_Tests.KeyVault
{
    public class Tests
    {
        private WeatherStats.KeyVault.IKeyVault _keyVault = null!;
            
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            _keyVault = new WeatherStats.KeyVault.KeyVault();
            Assert.That(_keyVault, Is.Not.Null);
        }
    }
}
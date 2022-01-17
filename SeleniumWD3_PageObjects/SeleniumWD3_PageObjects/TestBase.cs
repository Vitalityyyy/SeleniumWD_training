using NUnit.Framework;


namespace SeleniumWD3_PageObjects
{
    public class TestBase
    {
        public Application app;

        [SetUp]
        public void Start()
        {
            app = new Application();
        }
        [TearDown]
        public void TeardownTest()
        {
            app.Stop();
        }
    }
}

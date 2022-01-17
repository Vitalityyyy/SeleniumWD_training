using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWD2_litecart
{
    [TestFixture]
    public class T4_7_Stickerscheck
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private StringBuilder verificationErrors;
        private string baseURL;

        [SetUp]
        public void Start()
        {
            driver = new FirefoxDriver(@"C:\distr");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            baseURL = "http://localhost/litecart";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void StickerscheckTest()
        {
            GoToHomepage();
            CheckStickers();
        }

        [TearDown]
        public void Stop()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        public void GoToHomepage()
        {
            driver.Navigate().GoToUrl("http://localhost/litecart");
        }
        public void CheckStickers()
        {
            var products = driver.FindElements(By.CssSelector(".product"));
            foreach (IWebElement product in products)
            {
                var stickers = product.FindElements(By.CssSelector(".sticker"));
                Assert.That(stickers.Count == 1);
            }
        }
    }
}


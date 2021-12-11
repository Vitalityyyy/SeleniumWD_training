using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWD1
{
    [TestFixture]
    public class UnitTest1
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new FirefoxDriver(@"C:\distr");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void OpenBrowserPage()
        {
            //driver.Url = "https://github.com/";
            driver.Navigate().GoToUrl("https://github.com/");
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

    }
}

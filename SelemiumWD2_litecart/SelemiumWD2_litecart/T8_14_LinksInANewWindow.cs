using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Collections.Generic;

namespace SelemiumWD2_litecart
{
    [TestFixture]
    public class T8_14_LinksInANewWindow
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void start()
        {
            driver = new FirefoxDriver(@"C:\distr");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            baseURL = "http://localhost/litecart/admin/";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void LinksInANewWindowTest()
        {
            GoToHomepage();
            Login();
            Click(CountriesApp);
            Click(AddNewCountryButton);
            OpenCloseNewWindowAndReturn();
            OpenCloseNewWindowAndReturn2();
        }

        [TearDown]
        public void stop()
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
            driver.Navigate().GoToUrl("http://localhost/litecart/admin/");
        }

        public static Func<IWebDriver, IWebElement> Condition(By locator)
        {
            return (driver) =>
            {
                var element = driver.FindElements(locator).FirstOrDefault();
                return element != null && element.Displayed && element.Enabled ? element : null;
            };
        }

        protected void Click(By locator)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(Condition(locator)).Click();
        }

        public void FillTheField(By locator, string value)
        {
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(value);
        }

        public void Login()
        {
            FillTheField(UsernameField, "admin");
            FillTheField(PasswordField, "admin"); 
            Click(LoginButton);
        }

        public void OpenCloseNewWindowAndReturn()
        {
            string mainWindow = driver.CurrentWindowHandle; // запомнить ID текущего окна
            int previousWinCount = driver.WindowHandles.Count; // запомнить Count всех открытых окон
            ICollection<IWebElement> extLinkList = driver.FindElements(ExternalLinks);
            foreach (IWebElement extLink in extLinkList)
            {
                extLink.Click(); // открыть новое окно
                wait.Until(driver => driver.WindowHandles.Count == (previousWinCount + 1));
                driver.SwitchTo().Window(driver.WindowHandles.Last()); // переключиться в новое окно
                driver.Close();
                driver.SwitchTo().Window(mainWindow);
            }
        }

        public string ThereIsWindowOtherThan(ICollection<string> oldWindows)
        {
            ICollection<string> newWindows = driver.WindowHandles; // запомнить ID всех открытых окон
            ICollection<string> newWindow = newWindows.Except(oldWindows).ToList();
            return newWindow.First();
        }
        public void OpenCloseNewWindowAndReturn2()
        {
            string mainWindow = driver.CurrentWindowHandle; // запомнить ID текущего окна
            ICollection<string> oldWindows = driver.WindowHandles; // запомнить ID всех открытых окон
            ICollection<IWebElement> extLinkList = driver.FindElements(ExternalLinks);
            foreach (IWebElement extLink in extLinkList)
            {
                extLink.Click(); // открыть новое окно
                string newWindow = wait.Until(d => ThereIsWindowOtherThan(oldWindows));
                driver.SwitchTo().Window(newWindow); // переключиться в новое окно
                driver.Close();
                driver.SwitchTo().Window(mainWindow);
            }
        }

        #region Locators
        public By UsernameField { get { return By.Name("username"); } }
        public By PasswordField { get { return By.Name("password"); } }
        public By LoginButton { get { return By.Name("login"); } }
        public By CountriesApp { get { return By.XPath("//li[3]/a/span[2]"); } }
        public By AddNewCountryButton { get { return By.LinkText("Add New Country"); } }
        public By ExternalLinks { get { return By.CssSelector("i.fa.fa-external-link"); } }
        #endregion
    }
}
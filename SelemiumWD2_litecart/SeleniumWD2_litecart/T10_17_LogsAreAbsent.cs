using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Collections.Generic;

namespace SeleniumWD2_litecart
{
    [TestFixture]
    public class T10_17_LogsAreAbsent
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private StringBuilder verificationErrors;
        private string baseURL;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver(@"C:\distr");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            baseURL = "http://localhost/litecart/admin/";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void LogsAreAbsentTest()
        {
            GoToHomepage();
            Login();
            GoToCategory();
            OpenProductAndCheckLogs();
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

        public void GoToCategory()
        {
            driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1");
        }

        public void OpenProductAndCheckLogs()
        {
            ICollection<IWebElement> productsC = driver.FindElements(ProductLinks);
            for (int i = 0; i < productsC.Count; i++)
            {
                var products = driver.FindElements(ProductLinks);
                products[i].Click();
                foreach (LogEntry l in driver.Manage().Logs.GetLog("browser"))
                {
                    Console.WriteLine(l);
                    Assert.IsEmpty(l.Message);
                }
                GoToCategory();
            }
        }

        #region Locators
        public By UsernameField { get { return By.Name("username"); } }
        public By PasswordField { get { return By.Name("password"); } }
        public By LoginButton { get { return By.Name("login"); } }
        public By ProductLinks { get { return By.CssSelector("table.dataTable td:nth-child(3) a[href*=product]"); } }

        #endregion
    }
}
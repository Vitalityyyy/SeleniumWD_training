using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWD2_litecart
{
    [TestFixture]
    public class T4_6_ClickAllMenuSections
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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            baseURL = "http://localhost/litecart/admin/";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void ClickAllMenuSectionsTest()
        {
            GoToHomepage();
            Login();
            ClickAllMenuSections();
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
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
        public void Login()
        {
            driver.FindElement(By.Name("username")).Click();
            driver.FindElement(By.Name("username")).Clear();
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }
        public void ClickAllMenuSections()
        {
            var topMenuList0 = driver.FindElements(By.Id("app-"));
            for (int i = 0; i < topMenuList0.Count; i++)
            {
                var topMenuList = driver.FindElements(By.Id("app-"));
                topMenuList[i].Click();
                Assert.IsTrue(IsElementPresent(By.CssSelector("h1")));
                var subMenuList0 = driver.FindElements(By.CssSelector("[id^=doc]"));
                for (int n = 0; n < subMenuList0.Count; n++)
                {
                    var subMenuList = driver.FindElements(By.CssSelector("[id^=doc]"));
                    subMenuList[n].Click();
                    Assert.IsTrue(IsElementPresent(By.CssSelector("h1")));
                }
            }
        }
    }
}
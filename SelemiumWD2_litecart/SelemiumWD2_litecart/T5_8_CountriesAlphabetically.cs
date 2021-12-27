using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace SelemiumWD2_litecart
{
    [TestFixture]
    public class T5_8_CountriesAlphabetically
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
            baseURL = "http://localhost/litecart/admin/?app=countries&doc=countries";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void CountriesAlphabeticallyTest()
        {
            GoToHomepage();
            Login();
            GoToCountriesPage();
            CheckCountriesOrder();
            CheckZonesOrder();
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
        public void GoToCountriesPage()
        {
            driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=countries&doc=countries");
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
        public void CheckCountriesOrder()
        {
            IList<IWebElement> countryRows = driver.FindElements(By.CssSelector(".row"));
            List<string> countryTextList = new List<string>();
            foreach (var countryRow in countryRows)
            {
                string countryText = countryRow.FindElement(By.XPath("./td[5]")).Text;
                countryTextList.Add(countryText);
            }
            List<string> countryTextListSorted = countryTextList;
            countryTextListSorted.Sort();
            Assert.AreEqual(countryTextList, countryTextListSorted);
        }
        public void CheckZonesOrder()
        {
            IList<IWebElement> countryRows0 = driver.FindElements(By.CssSelector(".row"));
            for (int n = 0; n < countryRows0.Count; n++)
            {
                IList<IWebElement> countryRows = driver.FindElements(By.CssSelector(".row"));
                int countryZoneCount = Convert.ToInt32(countryRows[n].FindElement(By.XPath("./td[6]")).Text);
                if (countryZoneCount > 0)
                {
                    countryRows[n].FindElement(By.XPath("./td[5]/a")).Click();
                    IList<IWebElement> zoneNames = driver.FindElements(By.CssSelector("table#table-zones td:nth-child(3)"));
                    List<string> zoneTextList = new List<string>();
                    foreach (var zoneName in zoneNames)
                    {
                        string zoneText = zoneName.Text;
                        zoneTextList.Add(zoneText);
                    }
                    zoneTextList.RemoveAt(zoneTextList.Count - 1);
                    List<string> zoneTextListSorted = zoneTextList;
                    zoneTextListSorted.Sort();
                    Assert.AreEqual(zoneTextList, zoneTextListSorted);
                    GoToCountriesPage();
                }
            }
        }
    }
}


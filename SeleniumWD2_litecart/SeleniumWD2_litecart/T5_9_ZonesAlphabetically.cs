using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace SeleniumWD2_litecart
{
    [TestFixture]
    public class T5_9_ZonesAlphabetically
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
            baseURL = "http://localhost/litecart/admin/";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void ZonesAlphabeticallyTest()
        {
            GoToHomepage();
            Login();
            GoToGeoZonesPage();
            CheckZonesOrder();
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
        public void GoToGeoZonesPage()
        {
            driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones");
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

        public void CheckZonesOrder()
        {
            IList<IWebElement> countryRows0 = driver.FindElements(By.CssSelector(".row"));
            for (int n = 0; n < countryRows0.Count; n++)
            {
                IList<IWebElement> countryRows = driver.FindElements(By.CssSelector(".row"));
                countryRows[n].FindElement(By.XPath("./td[3]/a")).Click();
                IList<IWebElement> zoneNames = driver.FindElements(By.XPath("//*[@id='table-zones']//td[3]"));
                List<string> zoneTextList = new List<string>();
                foreach (var zoneName in zoneNames)
                {
                    string zoneText = zoneName.FindElement(By.XPath(".//option[@selected='selected']")).Text;
                    zoneTextList.Add(zoneText);
                }
                List<string> zoneTextListSorted = new List<string>(zoneTextList.Count);
                zoneTextList.ForEach((item) =>
                {
                    zoneTextListSorted.Add((string)item.Clone());
                });
                zoneTextListSorted.Sort();
                Assert.AreEqual(zoneTextList, zoneTextListSorted);
                GoToGeoZonesPage();
            }
        }
    }
}


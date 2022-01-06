using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.IO;
using OpenQA.Selenium.Interactions;

namespace SelemiumWD2_litecart
{
    [TestFixture]
    public class T6_12_AddProduct
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
        public void AddProductTest()
        {
            GoToHomepage();
            Login();
            GoToCatalogPage();
            AddNewProduct();
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
        public void GoToCatalogPage()
        {
            driver.FindElement(By.LinkText("Catalog")).Click();
        }
        public void Login()
        {
            driver.FindElement(By.Name("username")).Clear();
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }
        public static Random rnd = new Random();
        public static string GenerateRandomString()
        {
            int a = Convert.ToInt32(rnd.NextDouble() * 10);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < a; i++)
            {
                builder.Append(Convert.ToChar(97 + Convert.ToInt32(rnd.NextDouble() * 25)));
            }
            return builder.ToString();
        }
        public static string GenerateProductName()
        {
            string productName = "Product " + GenerateRandomString();
            return productName;
        }
        public void AddNewProduct ()
        {
            //General Tab
            driver.FindElement(By.LinkText("Add New Product")).Click();
            driver.FindElement(By.Name("status")).Click();
            string productName = GenerateProductName();
            driver.FindElement(By.Name("name[en]")).Clear();
            driver.FindElement(By.Name("name[en]")).SendKeys(productName);
            driver.FindElement(By.Name("code")).Clear();
            driver.FindElement(By.Name("code")).SendKeys("00001");
            driver.FindElement(By.XPath("//div[@id='tab-general']/table/tbody/tr[7]/td/div/table/tbody/tr[4]/td/input")).Click();
            driver.FindElement(By.Name("quantity")).Clear();
            driver.FindElement(By.Name("quantity")).SendKeys("1");
            string relativePath = "./product_pic.jpg";
            string fullPath = Path.GetFullPath(relativePath);
            driver.FindElement(By.Name("new_images[]")).SendKeys(fullPath);
            driver.FindElement(By.Name("date_valid_from")).Clear();
            driver.FindElement(By.Name("date_valid_from")).SendKeys("2022-02-01");
            driver.FindElement(By.Name("date_valid_to")).Clear();
            driver.FindElement(By.Name("date_valid_to")).SendKeys("2023-02-01");
            //Information Tab
            driver.FindElement(By.LinkText("Information")).Click();
            new SelectElement(driver.FindElement(By.Name("manufacturer_id"))).SelectByText("ACME Corp.");
            driver.FindElement(By.Name("keywords")).Clear();
            driver.FindElement(By.Name("keywords")).SendKeys("Some keywords");
            driver.FindElement(By.Name("short_description[en]")).Clear();
            driver.FindElement(By.Name("short_description[en]")).SendKeys("Some short description");
            driver.FindElement(By.CssSelector(".trumbowyg-editor")).Click();
            Actions action = new Actions(driver);
            action.SendKeys("Some description").Perform();
            driver.FindElement(By.Name("head_title[en]")).Clear();
            driver.FindElement(By.Name("head_title[en]")).SendKeys("Some head title");
            driver.FindElement(By.Name("meta_description[en]")).Clear();
            driver.FindElement(By.Name("meta_description[en]")).SendKeys("Some meta description");
            //Prices Tab
            driver.FindElement(By.LinkText("Prices")).Click();
            driver.FindElement(By.Name("purchase_price")).Clear();
            driver.FindElement(By.Name("purchase_price")).SendKeys("1");
            new SelectElement(driver.FindElement(By.Name("purchase_price_currency_code"))).SelectByText("Euros");
            driver.FindElement(By.Name("prices[USD]")).Clear();
            driver.FindElement(By.Name("prices[USD]")).SendKeys("10");
            driver.FindElement(By.Name("prices[EUR]")).Clear();
            driver.FindElement(By.Name("prices[EUR]")).SendKeys("10");
            driver.FindElement(By.Name("save")).Click();
            Assert.IsTrue(driver.FindElement(By.LinkText(productName)).Displayed);
        }
    }
}


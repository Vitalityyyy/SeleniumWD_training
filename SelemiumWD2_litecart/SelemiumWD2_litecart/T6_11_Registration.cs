using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;


namespace SelemiumWD2_litecart
{
    [TestFixture]
    public class T6_11_Registration
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
            baseURL = "http://localhost/litecart";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void Registration()
        {
            GoToHomepage();
            GoToRegistrationPage();
            string email = GenerateRandomEmail();
            string password = GenerateRandomString();
            FillTheForm(email, password);
            Logout();
            Login(email, password);
            Logout();
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
            driver.Navigate().GoToUrl("http://localhost/litecart");
        }
        public void GoToRegistrationPage()
        {
            driver.FindElement(By.LinkText("New customers click here")).Click();
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
        public static string GenerateRandomEmail()
        {
            string email = GenerateRandomString() + "@mail.ru";
            return email;
        }
        public static string GenerateRandomNumber()
        {
            int number = rnd.Next(10000, 99999);
            return number.ToString();
        }
        public void FillTheForm (string email, string password)
        {
            driver.FindElement(By.Name("firstname")).Clear();
            driver.FindElement(By.Name("firstname")).SendKeys(GenerateRandomString());
            driver.FindElement(By.Name("lastname")).Clear();
            driver.FindElement(By.Name("lastname")).SendKeys(GenerateRandomString());
            driver.FindElement(By.Name("address1")).Clear();
            driver.FindElement(By.Name("address1")).SendKeys(GenerateRandomString());
            driver.FindElement(By.Name("postcode")).Clear();
            driver.FindElement(By.Name("postcode")).SendKeys(GenerateRandomNumber());
            driver.FindElement(By.Name("city")).Clear();
            driver.FindElement(By.Name("city")).SendKeys(GenerateRandomString());
            driver.FindElement(By.CssSelector("[role=combobox]")).Click();
            driver.FindElement(By.CssSelector("input[type=search]")).Clear();
            driver.FindElement(By.CssSelector("input[type=search]")).SendKeys("United States");
            driver.FindElement(By.CssSelector("input[type=search]")).SendKeys(Keys.Enter);
            driver.FindElement(By.Name("email")).Clear();
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("phone")).Clear();
            driver.FindElement(By.Name("phone")).SendKeys(GenerateRandomNumber());
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("confirmed_password")).Clear();
            driver.FindElement(By.Name("confirmed_password")).SendKeys(password);
            driver.FindElement(By.Name("create_account")).Click();
        }
        public void Logout ()
        {
            driver.FindElement(By.LinkText("Logout")).Click();
        }
        public void Login (string email, string password)
        {
            driver.FindElement(By.Name("email")).Clear();
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("login")).Click();
        }
    }
}
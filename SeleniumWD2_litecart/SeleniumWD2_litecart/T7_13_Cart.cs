using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace SeleniumWD2_litecart
{
    [TestFixture]
    public class T7_13_Cart
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private StringBuilder verificationErrors;
        private string baseURL;

        [SetUp]
        public void Start()
        {
            driver = new FirefoxDriver(@"C:\distr");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            baseURL = "http://localhost/litecart";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void CartTest()
        {
            Add3ProductsToCart();
            RemoveProductsFromCart();
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

        public void WaitUntilTextInput(By locator, string text)
        {
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            wait.Message = "Element with text '" + text + "' was not visible in 5 seconds";
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElementLocated(locator, text));
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

        public void Add3ProductsToCart()
        {
            int i = 0;
            while (i < 3)
            {
                GoToHomepage();
                Click(TheFirstProduct);
                int itemsCountBefore = Convert.ToInt32(driver.FindElement(ItemsCount).Text);
                string itemsCountAfter = Convert.ToString(itemsCountBefore + 1);
                if (IsElementPresent(Size))
                {
                    new SelectElement(driver.FindElement(Size)).SelectByText("Small");
                }
                Click(AddToCartButton);
                WaitUntilTextInput(ItemsCount, itemsCountAfter);
                i++;
            }
        }

        public void RemoveProductsFromCart()
        {
            Click(GoToCart);
            int n = 0;
            int c = driver.FindElements(By.CssSelector("td.item")).Count;
            while (n < c)
            {
                int itemCountBefore = driver.FindElements(ItemsInTheTable).Count;
                if (IsElementPresent(TheFirstShortcut))
                {
                    Click(TheFirstShortcut);
                }                
                Click(RemoveButton);
                wait.Until((driver) => driver.FindElements(By.CssSelector("td.item")).Count() == itemCountBefore - 1);
                n++;
            }
        }

        #region Locators
        public By TheFirstProduct { get { return By.CssSelector("div#box-most-popular li:first-child"); } }
        public By ItemsCount { get { return By.ClassName("quantity"); } }
        public By Size { get { return By.Name("options[Size]"); } }
        public By AddToCartButton { get { return By.Name("add_cart_product"); } }
        public By GoToCart { get { return By.LinkText("Checkout »"); } }
        public By ItemsInTheTable { get { return By.CssSelector("td.item"); } }
        public By TheFirstShortcut { get { return By.XPath("//*[@id='box-checkout-cart']/ul/li[1]/a"); } }
        public By RemoveButton { get { return By.Name("remove_cart_item"); } }
        #endregion
    }
}
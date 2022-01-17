using System;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Linq;



namespace SeleniumWD3_PageObjects
{
    public class Application
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private StringBuilder verificationErrors;
        private string baseURL;

        public Application()
        {
            driver = new FirefoxDriver(@"C:\distr");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            baseURL = "http://localhost/litecart";
            verificationErrors = new StringBuilder();
        }
        private MainPage mainPage;
        private ProductPage productPage;
        private CartPage cartPage;
        public MainPage MainPage
        {
            get
            {
                if (mainPage == null)
                {
                    mainPage = new MainPage();
                }
                return mainPage;
            }
        }
        public ProductPage ProductPage
        {
            get
            {
                if (productPage == null)
                {
                    productPage = new ProductPage();
                }
                return productPage;
            }
        }
        public CartPage CartPage
        {
            get
            {
                if (cartPage == null)
                {
                    cartPage = new CartPage();
                }
                return cartPage;
            }
        }

        public void Stop()
        {
            driver.Quit();
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
                MainPage.GoToMainPage(driver);
                Click(MainPage.TheFirstProduct);
                int itemsCountBefore = Convert.ToInt32(driver.FindElement(MainPage.ItemsCount).Text);
                string itemsCountAfter = Convert.ToString(itemsCountBefore + 1);
                if (IsElementPresent(ProductPage.Size))
                {
                    new SelectElement(driver.FindElement(ProductPage.Size)).SelectByText("Small");
                }
                Click(ProductPage.AddToCartButton);
                WaitUntilTextInput(MainPage.ItemsCount, itemsCountAfter);
                i++;
            }
        }

        public void RemoveProductsFromCart()
        {
            Click(ProductPage.GoToCart);
            int n = 0;
            int c = driver.FindElements(CartPage.ItemsInTheTable).Count;
            while (n < c)
            {
                int itemCountBefore = driver.FindElements(CartPage.ItemsInTheTable).Count;
                if (IsElementPresent(CartPage.TheFirstShortcut))
                {
                    Click(CartPage.TheFirstShortcut);
                }
                Click(CartPage.RemoveButton);
                wait.Until((driver) => driver.FindElements(CartPage.ItemsInTheTable).Count() == itemCountBefore - 1);
                n++;
            }
        }

    }
}

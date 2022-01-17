using OpenQA.Selenium;


namespace SeleniumWD3_PageObjects
{
    public class MainPage
    {

        public void GoToMainPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://localhost/litecart");
        }
        public By TheFirstProduct { get { return By.CssSelector("div#box-most-popular li:first-child"); } }
        public By ItemsCount { get { return By.ClassName("quantity"); } }
    }
}

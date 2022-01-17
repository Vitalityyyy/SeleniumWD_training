using OpenQA.Selenium;


namespace SeleniumWD3_PageObjects
{
    public class CartPage
    {
        public By ItemsInTheTable { get { return By.CssSelector("td.item"); } }
        public By TheFirstShortcut { get { return By.XPath("//*[@id='box-checkout-cart']/ul/li[1]/a"); } }
        public By RemoveButton { get { return By.Name("remove_cart_item"); } }
    }
}

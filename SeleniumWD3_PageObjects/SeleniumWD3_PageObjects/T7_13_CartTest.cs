using NUnit.Framework;


namespace SeleniumWD3_PageObjects
{
    [TestFixture]
    public class T7_13_CartTest : TestBase
    {
        [Test]
        public void CartTest()
        {
            app.Add3ProductsToCart();
            app.RemoveProductsFromCart();
        }
    }
}
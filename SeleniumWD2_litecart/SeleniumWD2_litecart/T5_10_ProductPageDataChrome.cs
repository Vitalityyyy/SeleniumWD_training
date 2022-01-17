using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWD2_litecart
{
    [TestFixture]
    public class T5_10_ProductPageDataChrome
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private StringBuilder verificationErrors;
        private string baseURL;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver(@"C:\distr");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            baseURL = "http://localhost/litecart";
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void ProductPageDataChromeTest()
        {
            GoToHomepage();
            // Get Product Name And Prices On Main Page
            IWebElement priceDiscount1 = driver.FindElement(By.CssSelector("div#box-campaigns strong.campaign-price"));
            IWebElement priceNormal1 = driver.FindElement(By.CssSelector("div#box-campaigns s.regular-price"));
            string name1 = driver.FindElement(By.CssSelector("div#box-campaigns div.name")).Text;
            string priceNormal1_text = priceNormal1.Text;
            string priceDiscount1_text = priceDiscount1.Text;
            // Check Normal Price Styles On Main Page
            string priceNormal1_strikeout = priceNormal1.GetCssValue("text-decoration").Substring(0, 12);
            Assert.AreEqual(priceNormal1_strikeout, "line-through");
            string priceNormal1_R = priceNormal1.GetCssValue("color").Substring(5, 3);
            string priceNormal1_G = priceNormal1.GetCssValue("color").Substring(10, 3);
            string priceNormal1_B = priceNormal1.GetCssValue("color").Substring(15, 3);
            Assert.IsTrue(priceNormal1_R == priceNormal1_G && priceNormal1_R == priceNormal1_B);
            // Check Discount Price Styles On Main Page
            string priceDiscount1_G = priceDiscount1.GetCssValue("color").Substring(10, 1);
            string priceDiscount1_B = priceDiscount1.GetCssValue("color").Substring(13, 1);
            Assert.IsTrue(priceDiscount1_G == "0" && priceDiscount1_B == "0");
            int priceDiscount1_bold = Convert.ToInt32(priceDiscount1.GetCssValue("font-weight"));
            Assert.IsTrue(priceDiscount1_bold >= 600 && priceDiscount1_bold <= 900);
            // Compare Prices Size On Main Page
            double priceNormal1_size = Convert.ToDouble(priceNormal1.GetCssValue("font-size").Substring(0, priceNormal1.GetCssValue("font-size").Length-2));
            double priceDiscount1_size = Convert.ToDouble(priceDiscount1.GetCssValue("font-size").Substring(0, priceDiscount1.GetCssValue("font-size").Length - 2));
            Assert.IsTrue(priceDiscount1_size > priceNormal1_size);
            // Go To Product Page
            driver.FindElement(By.CssSelector("div#box-campaigns a.link")).Click();
            // Get Product Name And Prices On Product Page
            IWebElement priceDiscount2 = driver.FindElement(By.CssSelector("div.price-wrapper strong.campaign-price"));
            IWebElement priceNormal2 = driver.FindElement(By.CssSelector("div.price-wrapper s.regular-price"));
            string name2 = driver.FindElement(By.CssSelector("[itemprop=name]")).Text;
            string priceNormal2_text = priceNormal2.Text;
            string priceDiscount2_text = priceDiscount2.Text;
            // Compare Product Name And Prices
            Assert.AreEqual(name1,name2);
            Assert.AreEqual(priceNormal1_text, priceNormal2_text);
            Assert.AreEqual(priceDiscount1_text, priceDiscount2_text);
            // Check Normal Price Styles On Product Page
            string priceNormal2_strikeout = priceNormal2.GetCssValue("text-decoration").Substring(0, 12);
            Assert.AreEqual(priceNormal2_strikeout, "line-through");
            string priceNormal2_R = priceNormal2.GetCssValue("color").Substring(5, 3);
            string priceNormal2_G = priceNormal2.GetCssValue("color").Substring(10, 3);
            string priceNormal2_B = priceNormal2.GetCssValue("color").Substring(15, 3);
            Assert.IsTrue(priceNormal2_R == priceNormal2_G && priceNormal2_R == priceNormal2_B);
            // Check Discount Price Styles On Product Page
            string priceDiscount2_G = priceDiscount2.GetCssValue("color").Substring(10, 1);
            string priceDiscount2_B = priceDiscount2.GetCssValue("color").Substring(13, 1);
            Assert.IsTrue(priceDiscount2_G == "0" && priceDiscount2_B == "0");
            int priceDiscount2_bold = Convert.ToInt32(priceDiscount2.GetCssValue("font-weight"));
            Assert.IsTrue(priceDiscount2_bold >= 600 && priceDiscount2_bold <= 900);
            // Compare Prices Size On Product Page
            double priceNormal2_size = Convert.ToDouble(priceNormal2.GetCssValue("font-size").Substring(0, priceNormal2.GetCssValue("font-size").Length - 2));
            double priceDiscount2_size = Convert.ToDouble(priceDiscount2.GetCssValue("font-size").Substring(0, priceDiscount2.GetCssValue("font-size").Length - 2));
            Assert.IsTrue(priceDiscount2_size > priceNormal2_size);
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

    }
}


using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Android.Enums;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.BiDi.Script;
using OpenQA.Selenium.Interactions;
using System;

namespace MSAB_Example
{
    public class Tests
    {
        private AndroidDriver _driver;
        [SetUp]
        public void Setup()
        {
            var serverURI = new Uri(Environment.GetEnvironmentVariable("AppiumHost") ?? "http://127.0.0.1:4723/");
            var driverOptions = new AppiumOptions()
            {
                AutomationName = AutomationName.AndroidUIAutomator2,
                PlatformName = "Android",
                DeviceName = "CHANGETOYOURPHONE"
            };
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.whatsapp");
            driverOptions.AddAdditionalAppiumOption("appActivity", "com.whatsapp.Main");
            driverOptions.AddAdditionalAppiumOption("noReset", true);
            driverOptions.AddAdditionalAppiumOption("fullReset", false);
            _driver = new AndroidDriver(serverURI, driverOptions, TimeSpan.FromSeconds(100));
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }
        [Test]
        public void TestSendingTextMessage()
        {
            var ExpectedTextMessage = "This is an test message!" + Guid.NewGuid();
            Console.WriteLine("Pre-requisite: Start the app!");
            _driver.ActivateApp("com.whatsapp");
            Console.WriteLine("Step 1: Click on the 'Search' pannel...");
            _driver.FindElement(By.Id("com.whatsapp:id/search_bar_inner_layout")).Click();
            Console.WriteLine("Step 2: Type out the name of the text recipiant...");
            _driver.FindElement(By.Id("com.whatsapp:id/search_input")).SendKeys("Petra");
            Console.WriteLine("Step 3: Click on the name of the text recipiant...");
            _driver.FindElement(By.XPath("//android.widget.TextView[@content-desc=\"Petra Ivarsson (Du)\"]")).Click();
            Console.WriteLine("Step 4: Type out the text message...");
            _driver.FindElement(By.Id("com.whatsapp:id/entry")).SendKeys(ExpectedTextMessage);
            Console.WriteLine("Step 5: Click on the 'Send' button...");
            _driver.FindElement(By.Id("com.whatsapp:id/send")).Click();
            Console.WriteLine("Lastly: verify that the message we just sent, was actually sent...");
            var ActualTextMessage = _driver.PageSource.Contains(ExpectedTextMessage);
            Assert.That(ActualTextMessage,Is.True);
        }
        [TearDown]
        public void CleanUp()
        {
            _driver.Dispose();
        }
    }
}
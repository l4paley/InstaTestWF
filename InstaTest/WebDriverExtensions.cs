using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace InstaTest
{
    public static class WebDriverExtensions
    {
        public static IWebElement
            FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                try
                {
                    return wait.Until(drv =>
                    {
                        try { return drv.FindElement(by); }
                        catch { return null; }
                    }
                    );
                }
                catch
                {
                    return null;
                }
            }

            return driver.FindElement(by);
        }

        public static IReadOnlyCollection<IWebElement>
            FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                try
                {
                    return wait.Until(drv => drv.FindElements(by));
                }
                catch
                {
                    return null;
                }
            }

            return driver.FindElements(by);
        }
    }
}

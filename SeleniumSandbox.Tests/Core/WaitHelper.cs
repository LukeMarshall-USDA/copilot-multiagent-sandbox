using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumSandbox.Tests.Core;

public static class WaitHelper
{
    public static IWebElement WaitForElement(IWebDriver driver, By locator, int timeoutSeconds = 10)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        return wait.Until(d => d.FindElement(locator));
    }

    public static IWebElement WaitForClickable(IWebDriver driver, By locator, int timeoutSeconds = 10)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    public static bool WaitForWindowCount(IWebDriver driver, int expectedCount, out string? newWindowHandle, int timeoutSeconds = 10)
    {
        var existingHandles = driver.WindowHandles.ToHashSet();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));

        try
        {
            var reached = wait.Until(d => d.WindowHandles.Count >= expectedCount);
            if (!reached)
            {
                newWindowHandle = null;
                return false;
            }

            var handles = driver.WindowHandles;
            newWindowHandle = handles.FirstOrDefault(h => !existingHandles.Contains(h));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            newWindowHandle = null;
            return false;
        }
    }
}

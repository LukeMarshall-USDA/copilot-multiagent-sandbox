using OpenQA.Selenium;

namespace SeleniumSandbox.Tests.Core;

public abstract class BasePage
{
    protected readonly IWebDriver _driver;

    protected BasePage(IWebDriver driver) => _driver = driver;

    protected IWebElement WaitFor(By locator, int timeoutSeconds = 10) 
        => WaitHelper.WaitForElement(_driver, locator, timeoutSeconds);
}

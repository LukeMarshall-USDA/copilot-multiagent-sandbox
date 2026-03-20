using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumSandbox.Tests.Core;

public static class DriverFactory
{
    public static IWebDriver CreateChromeDriver(bool headless = true)
    {
        var options = new ChromeOptions();
        
        if (headless)
        {
            options.AddArgument("--headless=new");
        }
        
        options.AddArgument("--disable-gpu");
        options.AddArgument("--window-size=1920,1080");
        
        return new ChromeDriver(options);
    }
}

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using System.Threading;
using NUnit.Allure.Core;
using NUnit.Allure.Attributes;
using CodeJam;

    namespace TutoricaE2ETests;


[AllureNUnit]
[AllureSuite("Smoke Tests")]
public class Tests
{
    IWebDriver? driver;
    Actions actions;
        
    [SetUp]
    public void Setup()
    {
        this.driver = new EdgeDriver();
        this.driver.Url = "http://192.168.56.101:5044/";
        this.driver.Manage().Window.Maximize();

        this.actions = new Actions(driver);
    }

    [Test]
    public void PostFeedback() 
    {
        
        this.driver?.Navigate().GoToUrl("http://192.168.56.101:5044/about"); 
        Thread.Sleep(2000);

       
        // fill email
        IWebElement? emailField = this.driver?.FindElement(By.Id("email"));  
        emailField?.Clear();
        emailField.SendKeys("simon_petersonr@localnet.ua");
        

        // fill message
        IWebElement? textField = this.driver?.FindElement(By.Id("textArea"));  
        textField.Clear();
        textField.SendKeys("Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.");


        // submit button
        IWebElement? submitButton = this.driver?.FindElement(By.XPath("//button[@type='submit']"));
        submitButton.Click();
        Thread.Sleep(500);


        //verifying submit message
        IWebElement? textElement = this.driver.FindElement(By.XPath("//h2"));
        string submitMessageText = textElement.Text.ToUpper();
        
        Code.AssertState(string.Equals(submitMessageText, "Спасибо! Сообщение отправлено! Мы обязательно свяжемся с вами!".ToUpper()), "submitMessage is not valid!");
                
    }
    
    [Test]
    public void PostUniversity() 
     {
        
        this.driver?.Navigate().GoToUrl("http://192.168.56.101:5044/adduni"); 
        Thread.Sleep(2000);

        // fill first name and last name
        IWebElement? fnlnField = this.driver?.FindElement(By.Id("fnln"));  
        fnlnField.Clear();
        fnlnField.SendKeys("Joshua Montesque");


         // fill university name
        IWebElement? uniField = this.driver?.FindElement(By.Id("uniName"));  
        uniField.Clear();
        uniField.SendKeys("University of Hannover");


        // fill phone number
        IWebElement? phoneField = this.driver?.FindElement(By.Id("phoneNumber"));  
        phoneField.Clear();
        phoneField.SendKeys("762-847-0139");


        // fill email
        IWebElement? emailField = this.driver?.FindElement(By.Id("email"));  
        emailField.Clear();
        emailField.SendKeys("humboldt@localnet.ua");


        // description
        IWebElement? descriptionField = this.driver?.FindElement(By.Id("description"));  
        descriptionField.Clear();
        descriptionField.SendKeys("Dignissim lacus convallis massa mauris enim ad mattis magnis senectus montes, mollis taciti phasellus accumsan bibendum semper blandit suspendisse faucibus nibh est.");
        

        // fill years
        IWebElement? yearsField = this.driver?.FindElement(By.Id("years"));  
        yearsField.Clear();
        yearsField.SendKeys(Keys.Backspace);
        yearsField.SendKeys("9");
        

        // fill investmentUSD
        IWebElement? invUSDField = this.driver?.FindElement(By.Id("investmentUSD"));  
        invUSDField.SendKeys(Keys.Backspace);
        invUSDField.SendKeys("2000");
        invUSDField.SendKeys(Keys.Tab);


        IWebElement? submitButton = this.driver?.FindElement(By.XPath("//form/button"));

        // scroll down to footer
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", 
                    this.driver?.FindElement(By.XPath("//footer")));
        Thread.Sleep(100);

        // save course and priceUAH values for verification

        IWebElement? courseField = this.driver?.FindElement(By.Id("course"));
        IWebElement? invUAHField = this.driver?.FindElement(By.Id("investmentUAH"));  
        float course = float.Parse(courseField.GetAttribute("value"));
        float invUAH = float.Parse(invUAHField.GetAttribute("value"));

        // -------------------- ASSERTS --------------------
        Code.AssertState(course == 25.12F, "Course is invalid");
        Code.AssertState(invUAH == 50240, "investmentUAH was calculated wrong"); // 

        // if everything is OK go to list page
        submitButton.Click();

        
        // -------------------- NEW PAGE ------------------
        // verifying list page
        Thread.Sleep(2000);
        IWebElement? listPageHeader = this.driver?.FindElement(By.XPath("//table"));
        // string header = listPageHeader.Text;

        // scroll down to footer
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", 
                    this.driver?.FindElement(By.XPath("//footer")));
        Thread.Sleep(1000);

        IWebElement? tableElement = this.driver?.FindElement(By.XPath("//table/tbody/tr[80]/td[1]"));
        string insertedText = tableElement.Text;


        // -------------------- ASSERTS --------------------
        Code.AssertState(!(listPageHeader is null), "We have not moved to a new page");
        Code.AssertState(string.Equals(insertedText, "Joshua Montesque"), "Item hasn't been added to the database"); 

    }

     [Test]
    public void PostUniversityWrongYears() 
     {
        
         this.driver?.Navigate().GoToUrl("http://192.168.56.101:5044/adduni"); 
        Thread.Sleep(2000);

        // fill first name and last name
        IWebElement? fnlnField = this.driver?.FindElement(By.Id("fnln"));  
        fnlnField.Clear();
        fnlnField.SendKeys("Joshua Montesque");


         // fill university name
        IWebElement? uniField = this.driver?.FindElement(By.Id("uniName"));  
        uniField.Clear();
        uniField.SendKeys("University of Hannover");


        // fill phone number
        IWebElement? phoneField = this.driver?.FindElement(By.Id("phoneNumber"));  
        phoneField.Clear();
        phoneField.SendKeys("762-847-0139");


        // fill email
        IWebElement? emailField = this.driver?.FindElement(By.Id("email"));  
        emailField.Clear();
        emailField.SendKeys("humboldt@localnet.ua");


        // description
        IWebElement? descriptionField = this.driver?.FindElement(By.Id("description"));  
        descriptionField.Clear();
        descriptionField.SendKeys("Dignissim lacus convallis massa mauris enim ad mattis magnis senectus montes, mollis taciti phasellus accumsan bibendum semper blandit suspendisse faucibus nibh est.");
        

        // fill years
        IWebElement? yearsField = this.driver?.FindElement(By.Id("years"));  
        yearsField.Clear();
        yearsField.SendKeys(Keys.Backspace);
        yearsField.SendKeys("16");
        

        // fill investmentUSD
        IWebElement? invUSDField = this.driver?.FindElement(By.Id("investmentUSD"));  
        invUSDField.SendKeys(Keys.Backspace);
        invUSDField.SendKeys("2000");
        invUSDField.SendKeys(Keys.Tab);


        IWebElement? submitButton = this.driver?.FindElement(By.XPath("//button[normalize-space()='Submit']"));

        // scroll down to footer
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", 
                    this.driver?.FindElement(By.XPath("//footer")));
        Thread.Sleep(100);

        submitButton.Click();
        Thread.Sleep(100);

        IWebElement? alertBlock = this.driver?.FindElement(By.XPath("//div[@class='alert alert-danger']"));

        // -------------------- ASSERTS --------------------
        Code.AssertState(!(alertBlock is null), "Years field is not valid"); 
    }
    
    
    [TearDown]
    public void endTests() {
        Thread.Sleep(2000);
        this.driver?.Close();
    }
}

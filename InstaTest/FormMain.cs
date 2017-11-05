using System;
using System.Drawing;
using System.Windows.Forms;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;

namespace InstaTest
{

    public partial class FormMain : Form
    {
        const string InstagramLoginButtonXpath
            = "//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button";
        
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button12.BackColor = Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint.TryParse(textBox10.Text, out uint ThreadsCount);
            if (ThreadsCount < 1) ThreadsCount = 1;

            for (int index = 0; index < ThreadsCount; ++index)
            {
                new Thread(InstagramLikerThreatWorker)
                {
                    IsBackground = true
                }.Start();
            }
        }

        private void InstagramLikerThreatWorker()
        {
            int? accountId = RequestNextAccountId();
            while (accountId.HasValue)
            {
                StartStandartLiker(accountId.Value);
                accountId = RequestNextAccountId();
            }
        }

        private int nextAccountId = 0;
        private int? RequestNextAccountId()
        {
            if (nextAccountId >= Convert.ToInt32(textBox4.Text))//listBox1.Items.Count)
                return null;

            return nextAccountId++;
        }

        private void StartStandartLiker(int accountId)
        {
            string listenBox1 = Convert.ToString(listBox1.Items[accountId]);
            string[] AccPwdMailPwd = listenBox1.Split(':');
            string listenBox2 = Convert.ToString(listBox2.Items[accountId]);
            string[] ProxyPortLoginpPwdP = listenBox2.Split(':');
            var profileManager = new FirefoxProfileManager();
            var profileManager2 = new FirefoxProfileManager();
            FirefoxProfile profileInstagram = profileManager.GetProfile("Selenium");
            FirefoxProfile profileMail = profileManager2.GetProfile("Mail.ru");
            //profileManager.GetProfile("Selenium");
            String PROXY = ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1];
            //MessageBox.Show(PROXY);
            Proxy proxy = new Proxy()
            {
                HttpProxy = PROXY,
                // FtpProxy = PROXY,
                // SslProxy = PROXY,
            };

            profileInstagram.SetProxyPreferences(proxy);

             FirefoxDriver browserInstagram = new FirefoxDriver(profileInstagram);
            // FirefoxDriver browserMail = new FirefoxDriver(profileMail);
            //FirefoxDriver browserInstagram = new FirefoxDriver();
            
            browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
            //Browser2.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@mail.ru"); //Good
           

            Thread.Sleep(3000);

            browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]);
            Thread.Sleep(3000);
            browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
            Thread.Sleep(3000);
            browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 5).Click(); // InstaLoginButton
            Thread.Sleep(3001);

            if (browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//div[2]//form//span//button"), 30) != null)
            {
                Thread.Sleep(3000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//div[2]//form//span//button")).Click();//Нажать кнопку "выслать код"    
                
                FirefoxDriver browserMail = new FirefoxDriver();                                                                                                                        // Thread.Sleep(60000);
                browserMail.Navigate().GoToUrl("http://mail.ru"); //Good
                browserMail.FindElement(By.CssSelector("#mailbox__login"), 5).SendKeys(AccPwdMailPwd[2]); //ListBox1Item3(mail) 
                browserMail.FindElement(By.CssSelector("#mailbox__password"), 5).SendKeys(AccPwdMailPwd[3]); //ListBox1Item4(pwdmail)
                Thread.Sleep(30000);
                browserMail.FindElement(By.XPath("//*[@id=\"mailbox__auth__button\"]")).Click(); // MailLoginButton
                Thread.Sleep(4000);
                browserMail.FindElement(By.XPath("//*[@id=\"b-letters\"]//div//div[2]//div//div[2]//div[1]//div//a//div[4]//div[3]//div[1]//div"), 5).Click(); //Click On InstaMessage in Mail
                Thread.Sleep(4000);
                string element = browserMail.FindElement(By.XPath("//table/tbody/tr/td/table/tbody/tr[4]/td/table/tbody/tr/td/table/tbody/tr[2]/td/table/tbody/tr/td/table/tbody/tr/td[2]/table/tbody/tr/td/p[4]/font"), 30).Text;
                Thread.Sleep(8000);
                //MessageBox.Show(element);
                string instaCode1 = Convert.ToString(element);

                browserInstagram.FindElement(By.XPath("//*[@id=\"securityCode\"]")).SendKeys(instaCode1); // Paste 'var InstaCode' 
                Thread.Sleep(3000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//form//span//button")).Click();

                Thread.Sleep(6000);
                browserMail.Quit();
                browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]); //ListBox1Item1(instalogin) или userNameField
                Thread.Sleep(5000);
                browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
                Thread.Sleep(2000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 20).Click();
                if (browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//div[2]//form//span//button"), 30) != null)
                {
                    Thread.Sleep(3000);
                    browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//div[2]//form//span//button")).Click();//Нажать кнопку "выслать код"   
                    Thread.Sleep(4000);
                    browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//p//a")).Click(); //Нажать кнопку "выслать код повторно" 


                    FirefoxDriver browserMail1 = new FirefoxDriver();                                                                                                                        // Thread.Sleep(60000);
                    browserMail1.Navigate().GoToUrl("http://mail.ru"); //Good
                    browserMail1.FindElement(By.CssSelector("#mailbox__login"), 5).SendKeys(AccPwdMailPwd[2]); //ListBox1Item3(mail) 
                    browserMail1.FindElement(By.CssSelector("#mailbox__password"), 5).SendKeys(AccPwdMailPwd[3]); //ListBox1Item4(pwdmail)
                    Thread.Sleep(30000);
                    browserMail1.FindElement(By.XPath("//*[@id=\"mailbox__auth__button\"]")).Click(); // MailLoginButton
                    Thread.Sleep(4000);
                    browserMail1.FindElement(By.XPath("//*[@id=\"b-letters\"]//div//div[2]//div//div[2]//div[1]//div//a//div[4]//div[3]//div[1]//div"), 5).Click(); //Click On InstaMessage in Mail
                    Thread.Sleep(4000);
                    string element1 = browserMail1.FindElement(By.XPath("//table/tbody/tr/td/table/tbody/tr[4]/td/table/tbody/tr/td/table/tbody/tr[2]/td/table/tbody/tr/td/table/tbody/tr/td[2]/table/tbody/tr/td/p[4]/font"), 30).Text;
                    Thread.Sleep(8000);
                    //MessageBox.Show(element);
                    string instaCode = Convert.ToString(element1);

                    browserInstagram.FindElement(By.XPath("//*[@id=\"securityCode\"]")).SendKeys(instaCode); // Paste 'var InstaCode' 
                    Thread.Sleep(3000);
                    browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//form//span//button")).Click();

                    Thread.Sleep(6000);
                    browserMail1.Quit();
                    browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                    browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]); //ListBox1Item1(instalogin) или userNameField
                    Thread.Sleep(5000);
                    browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
                    Thread.Sleep(2000);
                    browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 20).Click();
                    Thread.Sleep(10000);
                    IJavaScriptExecutor js1 = (IJavaScriptExecutor)browserInstagram;
                    string title1 = (string)js1.ExecuteScript("return document.cookie");
                   // MessageBox.Show(title1);
                    String sessionId = ((RemoteWebDriver)browserInstagram).SessionId.ToString();
                    MessageBox.Show(sessionId);
                    string ConfirmValue = (AccPwdMailPwd[0] + ":" + AccPwdMailPwd[1] + ":" + AccPwdMailPwd[2] + ":" + AccPwdMailPwd[3] + ":" + ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1] + ":" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + ":" + title1);
                 //   MessageBox.Show(ConfirmValue);
                    listBox5.Items.Add(ConfirmValue);
                    /* browserInstagram.Navigate().GoToUrl(textBox5.Text);
                     Thread.Sleep(3000);
                     browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();*/
                    // Thread.Sleep(20000);
                    browserInstagram.Quit();

                }

                Thread.Sleep(10000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)browserInstagram;
                string title = (string)js.ExecuteScript("return document.cookie");
                MessageBox.Show(title);
                string ConfirmValue1 = (AccPwdMailPwd[0] + ":" + AccPwdMailPwd[1] + ":" + AccPwdMailPwd[2] + ":" + AccPwdMailPwd[3] + ":" + ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1] + ":" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + ":" + title);
                MessageBox.Show(ConfirmValue1);
                    listBox5.Items.Add(ConfirmValue1);
                /* browserInstagram.Navigate().GoToUrl(textBox5.Text);
                 Thread.Sleep(3000);
                 browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();*/
                // Thread.Sleep(20000);
                browserInstagram.Quit();
            }
            else if (browserInstagram.FindElement(By.XPath("//*[@id=\"securityCode\"]"), 15) != null)
            {
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//p//a")).Click(); //Нажать кнопку "выслать код повторно" 
                FirefoxDriver browserMail = new FirefoxDriver();                                                                                                  
                browserMail.Navigate().GoToUrl("http://mail.ru"); //Good
                browserMail.FindElement(By.CssSelector("#mailbox__login"), 5).SendKeys(AccPwdMailPwd[2]); //ListBox1Item3(mail) 
                browserMail.FindElement(By.CssSelector("#mailbox__password"), 5).SendKeys(AccPwdMailPwd[3]); //ListBox1Item4(pwdmail)
                browserMail.FindElement(By.XPath("//*[@id=\"mailbox__auth__button\"]"), 5).Click(); // MailLoginButton
                browserMail.FindElement(By.XPath("//*[@id=\"b-letters\"]//div//div[2]//div//div[2]//div[1]//div//a//div[4]//div[3]//div[1]//div"), 5).Click(); //Click On InstaMessage in Mail
                Thread.Sleep(5000);
                var element = browserMail.FindElement(By.XPath("//table/tbody/tr/td/table/tbody/tr[4]/td/table/tbody/tr/td/table/tbody/tr[2]/td/table/tbody/tr/td/table/tbody/tr/td[2]/table/tbody/tr/td/p[4]/font")).Text;

                string instaCode = Convert.ToString(element);
                browserInstagram.FindElement(By.XPath("//*[@id=\"securityCode\"]")).SendKeys(instaCode);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//form//span//button")).Click();

                Thread.Sleep(3000);
                browserMail.Quit();
                browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]); //ListBox1Item1(instalogin) или userNameField
                browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 20).Click();
                Thread.Sleep(10000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)browserInstagram;
                string title = (string)js.ExecuteScript("return document.cookie");
                MessageBox.Show(title);
                string ConfirmValue = (AccPwdMailPwd[0] + ":" + AccPwdMailPwd[1] + ":" + AccPwdMailPwd[2] + ":" + AccPwdMailPwd[3] + ":" + ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1] + ":" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + ":" + title);
                MessageBox.Show(ConfirmValue);
                listBox5.Items.Add(ConfirmValue);
                /*  Thread.Sleep(3000);
                  browserInstagram.Navigate().GoToUrl(textBox5.Text);
                  Thread.Sleep(3000);
                  browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();*/
                browserInstagram.Quit();
            }
            else if (browserInstagram.FindElement(By.XPath("//section/nav/div[2]/div/div/div[3]/div/div[3]/a"), 5) != null)
            
            {
                Thread.Sleep(10000);
                IJavaScriptExecutor js1 = (IJavaScriptExecutor)browserInstagram;
                string title1 = (string)js1.ExecuteScript("return document.cookie");
                // MessageBox.Show(title1);
                String sessionId = ((RemoteWebDriver)browserInstagram).SessionId.ToString();
               // MessageBox.Show(sessionId);
                string ConfirmValue = (AccPwdMailPwd[0] + ":" + AccPwdMailPwd[1] + ":" + AccPwdMailPwd[2] + ":" + AccPwdMailPwd[3] + ":" + ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1] + ":" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + ":" + title1);
                //   MessageBox.Show(ConfirmValue);
                this.Invoke((Action)(()=> listBox5.Items.Add(ConfirmValue + ":" + sessionId)));
                
                //browserMail.Quit();
                // browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                /*   Thread.Sleep(5000);
                   browserInstagram.Navigate().GoToUrl(textBox5.Text);
                   Thread.Sleep(5000);
                   browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();*/
                browserInstagram.Quit();
            }
            /*   FirefoxProfile profile = profileManager.GetProfile("Selenium");

              String PROXY = ProxyPortLoginpPwd[0] + ":" + ProxyPortLoginpPwd[1];

              Proxy proxy = new Proxy()
              {
                  HttpProxy = PROXY,
                  // FtpProxy = PROXY,
                  // SslProxy = PROXY,
              };

               profile.SetProxyPreferences(proxy);
              FirefoxDriver browser = new FirefoxDriver(profile);

              browser.Navigate().GoToUrl(
                  "https://" + ProxyPortLoginpPwd[2] +
                  ":" + ProxyPortLoginpPwd[3] +
                  "@instagram.com/accounts/login/"
              );

              browser.FindElement(By.XPath("//input[@name='username']"), 15).SendKeys(AccPwdMailPwd[0]); //ListBox1Item1(instalogin) или userNameField
              browser.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
              Thread.Sleep(4 * 1000);
              browser.FindElement(By.XPath(InstagramLoginButtonXpath)).Click();
              Thread.Sleep(5 * 1000);*/

            //Thread.Sleep(5000);
            //------------------------------------//Follow NatusVincere account\\---------------------------------------------
            /*Browser1.Navigate().GoToUrl("https://www.instagram.com/natus_vincere_official/");
            Thread.Sleep(5000);
            Browser1.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//article//header//div[2]//div[1]//span//span[1]//button")).Click(); */
            //------------------------------------//Like L4paley post\\---------------------------------------------
            //Browser1.Navigate().GoToUrl("https://www.instagram.com/p/BWSvegxhjNQ/?taken-by=l4paley");

            /* browser.Navigate().GoToUrl(textBox5.Text);
             Thread.Sleep(3000);
             browser.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();
             Thread.Sleep(5000);

             browser.Quit();*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //IWebDriver Browser1 = new OpenQA.Selenium.Opera.OperaDriver(); //Chrome.ChromeDriver();
            IWebDriver browser = new ChromeDriver();

            string firstElement = Convert.ToString(listBox1.Items[0]);
            string[] AccPwdMailPwd = firstElement.Split(':');
            browser.Navigate().GoToUrl("http://mail.ru");
            
            browser.FindElement(By.CssSelector("#mailbox__login"), 30).SendKeys(AccPwdMailPwd[2]);
            browser.FindElement(By.CssSelector("#mailbox__password"), 30).SendKeys(AccPwdMailPwd[3]);
            browser.FindElement(By.XPath("//*[@id=\"mailbox__auth__button\"]"), 30).Click();
            browser.FindElement(
                By.XPath("//*[@id=\"b-letters\"]//div//div[2]//div//div[2]//div[1]//div//a//div[4]//div[3]//div[1]//div"),
                30
            ).Click();
            
            string element = browser.FindElement(By.XPath("//table/tbody/tr/td/table/tbody/tr[4]/td/table/tbody/tr/td/table/tbody/tr[2]/td/table/tbody/tr/td/table/tbody/tr/td[2]/table/tbody/tr/td/p[4]/font"), 30).Text;
            MessageBox.Show(element);

        }





        private void button3_Click(object sender, EventArgs e)
        {
            int z = Convert.ToInt32(textBox1.Text);
            for (int ii = 0; ii < z; ii++)
            {
                FirefoxProfile profile = new FirefoxProfile();
                String PROXY = textBox2.Text + ":" + textBox3.Text;
                Proxy proxy = new Proxy()
                {
                    HttpProxy = PROXY,
                    // FtpProxy = PROXY,
                    // SslProxy = PROXY,
                };

                profile.SetProxyPreferences(proxy);
                FirefoxDriver browserInstagram = new FirefoxDriver(profile);
                FirefoxDriver browserMail = new FirefoxDriver();
                
                browserInstagram.Navigate().GoToUrl("http://www.instagram.com");
                Thread.Sleep(5000);
                browserMail.Navigate().GoToUrl("https://temp-mail.ru/");
                Thread.Sleep(5000);
                
                var element = browserMail.FindElement(By.XPath("//*[@id=\"mail\"]"));
                var innerHtml = element.GetAttribute("value");

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var name = "";
                var password = "";

                var random = new Random();
                for (int i = 0; i < 8; i++)
                {
                    name += chars[random.Next(chars.Length)];
                    password += chars[random.Next(chars.Length)];
                }

                browserInstagram.FindElement(By.XPath("//input[@name='emailOrPhone']"), 10).SendKeys(Convert.ToString(innerHtml));
                browserInstagram.FindElement(By.XPath("//input[@name='fullName']"), 5).SendKeys(Convert.ToString(name));
                browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(Convert.ToString(password));
                
                Thread.Sleep(5000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//article//div[2]//div[1]//div//form//div[6]//span//button")).Click();
                Thread.Sleep(15000);
                browserInstagram.Navigate().GoToUrl("https://www.instagram.com/natus_vincere_official");
                browserInstagram.Navigate().GoToUrl("https://www.instagram.com/p/BWSvegxhjNQ/?taken-by=l4paley");
                browserInstagram.Navigate().GoToUrl("https://www.instagram.com/f1/");
                browserInstagram.Navigate().GoToUrl("https://www.instagram.com/p/BWSvegxhjNQ/?taken-by=l4paley");
                
                Thread.Sleep(5000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();
                
                browserInstagram.Close();
                browserMail.Close();
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(textBox4.Text);
            string listenBox1 = Convert.ToString(listBox1.Items[n]);
            string[] AccPwdMailPwd = listenBox1.Split(':');

            string listenBox2 = Convert.ToString(listBox2.Items[n]);
            string[] ProxyPortLoginpPwdP = listenBox2.Split(':');

            var profileManager = new FirefoxProfileManager();
            var profileManager2 = new FirefoxProfileManager();
            FirefoxProfile profileInstagram = profileManager.GetProfile("Selenium");
            FirefoxProfile profileMail = profileManager2.GetProfile("Mail.ru");
            //profileManager.GetProfile("Selenium");
            String PROXY = ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1];
            MessageBox.Show(PROXY);
            Proxy proxy = new Proxy()
            {
                HttpProxy = PROXY,
                // FtpProxy = PROXY,
                // SslProxy = PROXY,
            };

            profileInstagram.SetProxyPreferences(proxy);

            FirefoxDriver browserInstagram = new FirefoxDriver(profileInstagram);
            FirefoxDriver browserMail = new FirefoxDriver(profileMail);//
            browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
            //Browser2.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@mail.ru"); //Good
            browserMail.Navigate().GoToUrl("http://mail.ru"); //Good

            Thread.Sleep(5000);
            
            browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]);
            browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
            browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 5).Click(); // InstaLoginButton
            
            if (browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//div[2]//form//span//button"), 30) != null)
            {
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//div[2]//form//span//button")).Click();//Нажать кнопку "выслать код"       
                Thread.Sleep(60000);
                browserMail.FindElement(By.CssSelector("#mailbox__login"), 5).SendKeys(AccPwdMailPwd[2]); //ListBox1Item3(mail) 
                browserMail.FindElement(By.CssSelector("#mailbox__password"), 5).SendKeys(AccPwdMailPwd[3]); //ListBox1Item4(pwdmail)
                browserMail.FindElement(By.XPath("//*[@id=\"mailbox__auth__button\"]")).Click(); // MailLoginButton
                browserMail.FindElement(By.XPath("//*[@id=\"b-letters\"]//div//div[2]//div//div[2]//div[1]//div//a//div[4]//div[3]//div[1]//div"), 5).Click(); //Click On InstaMessage in Mail
                var element = browserInstagram.FindElement(By.XPath("//*[@id=\"b-letter\"]//div[2]//div[9]//div"), 5).Text;//*[@id="b-letter"]/div[2]/div[9]/div/div[2]/div/div

                string instaCode = Convert.ToString(element);

                browserInstagram.FindElement(By.XPath("//*[@id=\"securityCode\"]")).SendKeys(instaCode); // Paste 'var InstaCode' 
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//form//span//button")).Click();
                
                Thread.Sleep(5000);
                browserMail.Close();
                browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]); //ListBox1Item1(instalogin) или userNameField
                browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 20).Click();
                
                Thread.Sleep(5000);
                browserInstagram.Navigate().GoToUrl("https://www.instagram.com/p/BWSvegxhjNQ/?taken-by=l4paley");
                Thread.Sleep(5000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();
                browserInstagram.Close();
            }
            else if (browserInstagram.FindElement(By.XPath("//*[@id=\"securityCode\"]"), 15) != null)
            {
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//p//a")).Click(); //Нажать кнопку "выслать код повторно" 
                browserMail.FindElement(By.CssSelector("#mailbox__login"), 5).SendKeys(AccPwdMailPwd[2]); //ListBox1Item3(mail) 
                browserMail.FindElement(By.CssSelector("#mailbox__password"), 5).SendKeys(AccPwdMailPwd[3]); //ListBox1Item4(pwdmail)
                browserMail.FindElement(By.XPath("//*[@id=\"mailbox__auth__button\"]"), 5).Click(); // MailLoginButton
                browserMail.FindElement(By.XPath("//*[@id=\"b-letters\"]//div//div[2]//div//div[2]//div[1]//div//a//div[4]//div[3]//div[1]//div"), 5).Click(); //Click On InstaMessage in Mail
                Thread.Sleep(15000);
                var element = browserMail.FindElement(By.XPath("//*[@id=\"b-thread\"]//div//div//div")).Text;
                
                string instaCode = Convert.ToString(element);
                browserInstagram.FindElement(By.XPath("//*[@id=\"securityCode\"]")).SendKeys(instaCode);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//div//div//form//span//button")).Click();
                
                Thread.Sleep(5000);
                browserMail.Close();
                browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]); //ListBox1Item1(instalogin) или userNameField
                browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 20).Click();
                
                Thread.Sleep(5000);
                browserInstagram.Navigate().GoToUrl("https://www.instagram.com/p/BWSvegxhjNQ/?taken-by=l4paley");
                Thread.Sleep(5000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();
                browserInstagram.Close();
            }
            else if (browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//nav//div[2]//div//div//div[2]//div//div//span[2]"), 5) != null)
            {
                browserMail.Close();
                browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                
                browserInstagram.Navigate().GoToUrl("https://www.instagram.com/p/BWSvegxhjNQ/?taken-by=l4paley");
                Thread.Sleep(5000);
                browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();
                browserInstagram.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox4.Items.Add(dateTimePicker1.Value.ToString("yyyy-MM-dd") + " " + textBox7.Text + "-" + textBox8.Text + " " + textBox9.Text + " " + textBox11.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox4.Text = listBox1.Items.Count.ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listBox4.Items.Count == 0)
            {
                timer1.Enabled = false;
            }
            else if (listBox4.Items.Count > 0)
            {
                button11.BackColor = Color.Green;
                button12.BackColor = SystemColors.Control;
                timer1.Interval = 300000;
                timer1.Tick += timer_Tick;
                timer1.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (listBox4.Items.Count > 0)
            {

                IWebDriver browser = new ChromeDriver();


                int n = 0;
                string listenBox4 = Convert.ToString(listBox4.Items[n]);
                string[] DateTimeLinkTrig = listenBox4.Split(' ');
                string Link = DateTimeLinkTrig[2];
                browser.Navigate().GoToUrl(Link);
                Thread.Sleep(5000);

                string PublicationCount = browser.FindElement(By.XPath("//section/main/article/header/div[2]/ul/li[1]/span/span"), 30).Text;
                //MessageBox.Show(PublicationCount);
                int PubCount = Convert.ToInt32(PublicationCount);
                browser.Quit();

                if (PubCount>Convert.ToInt32(label14.Text))
                {
                    button7_Click(this, EventArgs.Empty);
                    textBox10.Text = "2";
                    textBox5.Text = Link;
                    button1_Click(this, EventArgs.Empty);
                    listBox4.Items.Remove(listBox4.Items[n]);
                }
               
                
                    timer1.Start();
               




                /*  int n = 0;
                  string listenBox4 = Convert.ToString(listBox4.Items[n]);
                  string[] DateTimeLink = listenBox4.Split(' ');
                  string[] Date = DateTimeLink[0].Split('-');
                  string Year = Date[0];
                  string Month = Date[1];
                  string Day = Date[2];
                  string[] Time = DateTimeLink[1].Split('-');
                  string Hour = Time[0];
                  string Minutes = Time[1];
                  string Link = DateTimeLink[2];

                  timer1.Stop();
                  if ((DateTime.Now.Year == Convert.ToInt32(Year)) && (DateTime.Now.Month == Convert.ToInt32(Month)) && (DateTime.Now.Day == Convert.ToInt32(Day)) && (DateTime.Now.Hour == Convert.ToInt32(Hour)) && (DateTime.Now.Minute == Convert.ToInt32(Minutes) && (DateTime.Now.Second == 0)))
                  {
                      // тут реализовать ваше нажатие
                      // нажимать будет только если текущее время 15:00
                      // button_Click(this, EventArgs.Empty);   
                      // MessageBox.Show("YA NA STARTE");
                      listBox4.Items.Remove(listBox4.Items[n]);
                      //------------------------------Код для генериции картинки-------------------------------------------------------



                      //------------------------------Код для генериции картинки-------------------------------------------------------

                      textBox5.Text = Link;
                      button1_Click(this, EventArgs.Empty);
                  }*/
            }

            timer1.Start();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            button12.BackColor = Color.Red;
            button11.BackColor = SystemColors.Control;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox4.Items.Remove(listBox4.SelectedItem);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (listBox4.Items.Count != 0) {
            IWebDriver browser = new ChromeDriver();


            int n = 0;
            string listenBox4 = Convert.ToString(listBox4.Items[n]);
            string[] DateTimeLinkTrig = listenBox4.Split(' ');
            string Link = DateTimeLinkTrig[2];
            browser.Navigate().GoToUrl(Link);
            Thread.Sleep(5000);

            string PublicationCount = browser.FindElement(By.XPath("//section/main/article/header/div[2]/ul/li[1]/span/span"), 30).Text;
            //MessageBox.Show(PublicationCount);
            label14.Text = PublicationCount;
            browser.Quit();

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            listBox1.Items.AddRange(File.ReadAllLines(openFileDialog.FileName));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            listBox2.Items.AddRange(File.ReadAllLines(openFileDialog.FileName));
        }

        private void button15_Click(object sender, EventArgs e)
        {
           /* OpenFileDialog open_dialog = new OpenFileDialog(); //создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла

            if (open_dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Bitmap image = new Bitmap(open_dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Invalidate();
            }
            catch
            {
                MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;

            new FormImageViewer
            {
                WindowState = FormWindowState.Maximized,
                BackgroundImage = pictureBox1.Image,
                BackgroundImageLayout = ImageLayout.Stretch
            }.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            const string sPath = "C:\\Confirmed.txt";

            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(sPath);
            foreach (var item in listBox5.Items)
            {
                SaveFile.WriteLine(item);
            }

            SaveFile.Close();

            MessageBox.Show("Programs saved!");
        }

        private void InstaWorker(int accountId)
        {
            string listenBox1 = Convert.ToString(listBox1.Items[accountId]);
            string[] AccPwdMailPwd = listenBox1.Split(':');
            string listenBox2 = Convert.ToString(listBox2.Items[accountId]);
            string[] ProxyPortLoginpPwdP = listenBox2.Split(':');
            var profileManager = new FirefoxProfileManager();

            FirefoxProfile profileInstagram = profileManager.GetProfile("Selenium");
               
            String PROXY = ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1];
     
            Proxy proxy = new Proxy()
            {
                HttpProxy = PROXY,
                // FtpProxy = PROXY,
                // SslProxy = PROXY,
            };

            profileInstagram.SetProxyPreferences(proxy);
            

            FirefoxDriver browserInstagram = new FirefoxDriver(profileInstagram);
            Thread.Sleep(5000);

            browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]);
            browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
            browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 5).Click(); // InstaLoginButton
                                                                                                                                                             // var cookies = listBox5.Items[accountId].ToString().Split(':');
                                                                                                                                                             // string[] CookMember = cookies[8].Split(';');
                                                                                                                                                             // string sessionId = cookies[9];
            browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
            Thread.Sleep(5000);
            browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@/p/BXioaw6B9uX/?taken-by=l4paley"); //Good  http://username:password@website.com
            Thread.Sleep(3000);
            browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//div//article//div[2]//section[1]//a[1]")).Click();
            Thread.Sleep(500);
            browserInstagram.Close();
            //browserInstagram.WebStorage.SessionStorage.SetItem

            //browserInstagram.Manage().Cookies.DeleteAllCookies();

            //browserInstagram.Manage().Cookies.AddCookie(new Cookie("sessionid", //sessionId));

            /*foreach (var Cookie in CookMember)
            {
                var data = Cookie.Split(new[] { '=' }, 2);
                browserInstagram.Manage().Cookies.AddCookie(new Cookie(data[0].Trim(), data[1].Trim()));
                Console.WriteLine(data[0].Trim());
                Console.WriteLine(data[1].Trim());
            }    */                                                                                                                                       //Browser2.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@mail.ru"); //Good
                                                                                                                                                          //  browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com");

            Thread.Sleep(3000);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            InstaWorker(0);


        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            listBox5.Items.AddRange(File.ReadAllLines(openFileDialog.FileName));
        }
    }
}

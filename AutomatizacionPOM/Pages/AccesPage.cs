using AutomatizacionPOM.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class AccessPage
    {
        private IWebDriver driver;
        Utilities utilities;

        public AccessPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // ====== LOGIN ======
        private By usernameField = By.XPath("//input[@id='Email']");
        private By passwordField = By.XPath("//input[@id='Password']");
        private By loginButton = By.XPath("//button[normalize-space()='Iniciar']");
        private By acceptButton = By.XPath("//button[contains(text(),'Aceptar')]");
        private By logo = By.XPath("//img[@id='ImagenLogo']");

        // ====== TESORERÍA Y FINANZAS ======
        private By TesoreriaField = By.XPath("//span[normalize-space()='Tesorería y Finanzas']");
        private By IngresosegresosField = By.XPath("//a[normalize-space()='Ingresos/Egresos']");

        // ====== VENTAS ======
        private By VentaField = By.XPath("//a[@class='menu-lista-cabecera']/span[text()='Venta']");
        private By NuevaVentaField = By.XPath("//a[normalize-space()='Nueva Venta']");

        // ====== RESTAURANTE ======
        private By RestauranteField = By.XPath("//a[@class='menu-lista-cabecera']/span[text()='Restaurante']");
        private By AtencionField = By.XPath("//a[normalize-space()='Atención']");

        // ====== GASTO ======
        private By GastoField = By.XPath("//body/div[@id='wrapper']/aside[@class='main-sidebar control-sidebar-light']/div[@class='slimScrollDiv']/section[@class='sidebar width-230 pb-50']/ul[@class='sidebar-menu tree']/li[9]/a[1]");
        private By GastoVerField = By.XPath("//a[@href='/Gasto/Index']");
        private By GastoConceptoField = By.XPath("//a[normalize-space()='Concepto']");
        private By GastoReporteField = By.XPath("//a[normalize-space()='Reporte']");

        // ====== MÉTODOS ======

        public void OpenToAplicattion(string url)
        {
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(4000);
        }

        public void LoginToApplication(string username, string password)
        {
            utilities.EnterText(usernameField, username);
            Thread.Sleep(1000);

            utilities.EnterText(passwordField, password);
            Thread.Sleep(1000);

            utilities.ClickButton(loginButton);
            Thread.Sleep(3000);

            utilities.ClickButton(acceptButton);
            Thread.Sleep(3000);

            // Verificar login exitoso
            var successElement = driver.FindElement(logo);
            Assert.IsNotNull(successElement, "No se encontró el elemento de éxito después del login.");
        }

        /// <summary>
        /// Permite ingresar a un módulo del sistema.
        /// </summary>
        public void enterModulo(string modulo)
        {
            switch (modulo)
            {
                case "Venta":
                    utilities.ClickButton(VentaField);
                    break;

                case "Restaurante":
                    utilities.ClickButton(RestauranteField);
                    break;

                case "Tesorería y Finanzas":
                    utilities.ClickButton(TesoreriaField);
                    break;

                case "Gasto":
                    utilities.ClickButton(GastoField);
                    break;

                default:
                    throw new ArgumentException($"El módulo '{modulo}' no es válido.");
            }

            Thread.Sleep(3000);
        }

        /// <summary>
        /// Permite ingresar a un submódulo dentro de un módulo activo.
        /// </summary>
        public void enterSubModulo(string submodulo)
        {
            switch (submodulo)
            {
                // ==== SUBMÓDULOS VENTA ====
                case "Nueva Venta":
                    driver.FindElement(NuevaVentaField).Click();
                    break;

                // ==== SUBMÓDULOS RESTAURANTE ====
                case "Atención":
                    driver.FindElement(AtencionField).Click();
                    break;

                // ==== SUBMÓDULOS TESORERÍA ====
                case "Ingresos/Egresos":
                    driver.FindElement(IngresosegresosField).Click();
                    break;

                // ==== SUBMÓDULOS GASTO ====
                case "Ver":
                    driver.FindElement(GastoVerField).Click();
                    break;

                case "Concepto":
                    driver.FindElement(GastoConceptoField).Click();
                    break;

                case "Reporte":
                    driver.FindElement(GastoReporteField).Click();
                    break;

                default:
                    throw new ArgumentException($"El submódulo '{submodulo}' no es válido.");
            }

            Thread.Sleep(5000);
        }
    }
}

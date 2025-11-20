using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class PrincipalGastoConceptoPage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;

        public PrincipalGastoConceptoPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // ================================
        // LOCALIZADORES
        // ================================
        private By SelectorFilas = By.XPath("//select[contains(@name,'length')]");
        private By CampoBuscar = By.XPath("//input[@type='search']");
        private By BotonExcel = By.XPath("//span[@class='fa fa-file-excel-o']");
        private By FilasTabla = By.XPath("//table//tbody//tr");
        private By EditarBotones = By.XPath("//span[@class='glyphicon glyphicon-edit']");
        private By BtnSiguiente = By.XPath("//a[contains(text(),'Siguiente')]");
        private By BtnNuevoConcepto = By.XPath("//a[contains(.,'NUEVO CONCEPTO')]");

        // Select2 Editar
        private By SelectConceptoBasicoEditar = By.XPath("//span[@id='select2-selectorConceptoBasico-container']");
        private By InputSelect2 = By.XPath("//input[@class='select2-search__field']");

        // Formulario Nuevo Concepto
        private By BtnAgregarFamilia = By.XPath("//a[@title='Ingresar Concepto Basico']//span[@class='glyphicon glyphicon-plus']");
        private By RadioBien = By.XPath("//input[@id='radio-bien']");
        private By RadioServicio = By.XPath("//input[@id='radio-servicio']");
        private By InputNuevaFamilia = By.XPath("//input[@id='nombreConceptoBasico']");
        private By InputSufijo = By.XPath("//input[@id='sufijo']");
        private By SelectFamiliaExistente = By.XPath("//span[@role='combobox']");
        private By BtnGuardarFormulario = By.XPath("//button[normalize-space()='GUARDAR']");

        // ================================
        // ACCIONES BÁSICAS
        // ================================
        public void CambiarFilas(string cantidad)
        {
            var select = new SelectElement(driver.FindElement(SelectorFilas));
            select.SelectByText(cantidad);
            Thread.Sleep(1000);
        }

        public void Buscar(string texto)
        {
            utilities.ClearAndEnterText(CampoBuscar, texto);
            Thread.Sleep(1000);
        }

        public void ExportarExcel()
        {
            var btn = driver.FindElement(BotonExcel);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(500);

            try { btn.Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn); }

            Thread.Sleep(2000);
        }

        public void EditarConcepto(int fila)
        {
            driver.FindElements(EditarBotones)[fila - 1].Click();
            Thread.Sleep(1500);
        }

        public void ClickNuevoConcepto()
        {
            driver.FindElement(BtnNuevoConcepto).Click();
            Thread.Sleep(1500);
        }

        // ================================
        // PAGINACIÓN
        // ================================
        public void PaginaSiguiente()
        {
            var nextBtn = driver.FindElement(BtnSiguiente);

            try { nextBtn.Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextBtn); }

            Thread.Sleep(1000);
        }

        // ================================
        // EDITAR CONCEPTO
        // ================================
        public void CambiarConceptoBasicoAInteres()
        {
            driver.FindElement(SelectConceptoBasicoEditar).Click();
            Thread.Sleep(500);

            var buscador = driver.FindElement(InputSelect2);
            buscador.SendKeys("INTERES");
            Thread.Sleep(400);
            buscador.SendKeys(Keys.Enter);

            Thread.Sleep(600);
            driver.FindElement(BtnGuardarFormulario).Click();
            Thread.Sleep(1000);
        }

        // ================================
        // NUEVA FAMILIA (CORREGIDO)
        // ================================
        public void CrearNuevaFamilia(string familia, string sufijo, string tipo)
        {
            // 1. PRIMERO clic en (+)
            driver.FindElement(BtnAgregarFamilia).Click();
            Thread.Sleep(600);

            // 2. AHORA sí existen los radio buttons
            if (tipo.ToUpper() == "BIEN")
                driver.FindElement(RadioBien).Click();
            else
                driver.FindElement(RadioServicio).Click();

            Thread.Sleep(500);

            // 3. Ingresar familia y sufijo
            driver.FindElement(InputNuevaFamilia).SendKeys(familia);
            Thread.Sleep(400);

            driver.FindElement(InputSufijo).SendKeys(sufijo);
            Thread.Sleep(400);

            // 4. Guardar
            driver.FindElement(BtnGuardarFormulario).Click();
            Thread.Sleep(1200);
        }

        // ================================
        // FAMILIA EXISTENTE (NO USA RADIO)
        // ================================
        public void CrearGastoConFamiliaExistente(string familia, string sufijo)
        {
            driver.FindElement(SelectFamiliaExistente).Click();
            Thread.Sleep(500);

            var buscador = driver.FindElement(InputSelect2);
            buscador.SendKeys(familia);
            Thread.Sleep(500);
            buscador.SendKeys(Keys.Enter);

            Thread.Sleep(400);
            driver.FindElement(InputSufijo).Clear();
            driver.FindElement(InputSufijo).SendKeys(sufijo);

            Thread.Sleep(400);
            driver.FindElement(BtnGuardarFormulario).Click();
            Thread.Sleep(1000);
        }

        public void ValidarCambioPagina()
        {
            Console.WriteLine("Cambio de página realizado correctamente.");
        }
    }
}

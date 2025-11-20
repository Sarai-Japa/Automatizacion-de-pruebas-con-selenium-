using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using System.Threading;
using System;
using System.IO;

namespace AutomatizacionPOM.Pages
{
    public class ReporteGastoPage
    {
        private readonly IWebDriver driver;
        private readonly Utilities utilities;

        public ReporteGastoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // ============================
        // LOCALIZADORES
        // ============================

        // Fechas
        private By FechaInicial = By.XPath("//input[@id='dateStart']");
        private By FechaFinal = By.XPath("//input[@id='dateEnd']");

        // Radios
        private By RadioGlobal = By.XPath("//input[@id='radio1']");
        private By RadioEstablecimiento = By.XPath("//input[@id='radio2']");
        private By RadioCentro = By.XPath("//input[@id='radio3']");

        // 🔥 CORREGIDOS: Select2 dinámicos por LABEL
        private By SelectEstablecimiento =
            By.XPath("//label[contains(text(),'Establecimiento')]/following::span[contains(@class,'select2-selection')][1]");

        private By SelectCaja =
            By.XPath("//label[contains(text(),'Caja')]/following::span[contains(@class,'select2-selection')][1]");

        private By InputSelect = By.XPath("//input[@class='select2-search__field']");

        // Botón Ver
        private By BtnVer = By.XPath("//a[normalize-space()='VER']");

        // Iframe del Reporte
        private By IframeReporte = By.XPath("//iframe[contains(@src,'Report')]");

        // Botón Exportar
        private By BtnExportar = By.XPath("//span[@id='ReportViewer1_ctl09_ctl04_ctl00_ButtonImg']");

        // Opciones exportación
        private By ExportExcel = By.XPath("//a[normalize-space()='Excel']");
        private By ExportPDF = By.XPath("//a[normalize-space()='PDF']");
        private By ExportWord = By.XPath("//a[normalize-space()='Word']");

        // ============================
        // MÉTODOS
        // ============================

        public void SeleccionarFechaInicial(string fecha)
        {
            driver.FindElement(FechaInicial).Clear();
            driver.FindElement(FechaInicial).SendKeys(fecha);
            Thread.Sleep(500);
        }

        public void SeleccionarFechaFinal(string fecha)
        {
            driver.FindElement(FechaFinal).Clear();
            driver.FindElement(FechaFinal).SendKeys(fecha);
            Thread.Sleep(500);
        }

        public void SeleccionarModo(string modo)
        {
            modo = modo.ToUpper();

            if (modo == "GLOBAL")
                driver.FindElement(RadioGlobal).Click();

            if (modo == "ESTABLECIMIENTO")
                driver.FindElement(RadioEstablecimiento).Click();

            if (modo == "CENTRO" || modo == "CENTRO DE ATENCION")
                driver.FindElement(RadioCentro).Click();

            Thread.Sleep(800);
        }

        public void SeleccionarEstablecimiento(string texto)
        {
            driver.FindElement(SelectEstablecimiento).Click();
            Thread.Sleep(600);

            driver.FindElement(InputSelect).SendKeys(texto);
            Thread.Sleep(600);

            driver.FindElement(InputSelect).SendKeys(Keys.Enter);
            Thread.Sleep(600);
        }

        public void SeleccionarCaja(string texto)
        {
            driver.FindElement(SelectCaja).Click();
            Thread.Sleep(600);

            driver.FindElement(InputSelect).SendKeys(texto);
            Thread.Sleep(600);

            driver.FindElement(InputSelect).SendKeys(Keys.Enter);
            Thread.Sleep(600);
        }

        public void ClickVerReporte()
        {
            driver.FindElement(BtnVer).Click();
            Thread.Sleep(2500);
        }

        public void ValidarReporteVisible()
        {
            driver.SwitchTo().Frame(driver.FindElement(IframeReporte));
            Thread.Sleep(1500);
            driver.SwitchTo().DefaultContent();
        }

        // ============================
        // DESCARGA
        // ============================
        public void EsperarDescarga(string extension, int tiempoMaximoSegundos = 25)
        {
            string carpetaDescargas = @"C:\Users\sarai\Downloads";
            bool archivoDescargado = false;
            int tiempo = 0;

            while (!archivoDescargado && tiempo < tiempoMaximoSegundos * 2)
            {
                var archivos = Directory.GetFiles(carpetaDescargas, $"*.{extension}");

                if (archivos.Length > 0 &&
                    !archivos[0].EndsWith(".crdownload"))
                {
                    archivoDescargado = true;
                    break;
                }

                Thread.Sleep(500);
                tiempo++;
            }

            if (!archivoDescargado)
                throw new Exception("El archivo no terminó de descargarse.");
        }

        // ============================
        // EXPORTAR REPORTE
        // ============================
        public void ExportarReporte(string formato)
        {
            driver.SwitchTo().Frame(driver.FindElement(IframeReporte));
            Thread.Sleep(1500);

            driver.FindElement(BtnExportar).Click();
            Thread.Sleep(1000);

            switch (formato.ToUpper())
            {
                case "EXCEL":
                    driver.FindElement(ExportExcel).Click();
                    EsperarDescarga("xlsx");
                    break;

                case "PDF":
                    driver.FindElement(ExportPDF).Click();
                    EsperarDescarga("pdf");
                    break;

                case "WORD":
                    driver.FindElement(ExportWord).Click();
                    EsperarDescarga("docx");
                    break;
            }

            Thread.Sleep(2000);
            driver.SwitchTo().DefaultContent();
        }
    }
}

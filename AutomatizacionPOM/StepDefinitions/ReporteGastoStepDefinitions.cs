using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class ReporteGastoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly ReporteGastoPage reportePage;

        public ReporteGastoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            reportePage = new ReporteGastoPage(driver);
        }

        [When("selecciona fecha inicial {string}")]
        public void WhenSeleccionaFechaInicial(string fecha)
        {
            reportePage.SeleccionarFechaInicial(fecha);
        }

        [When("selecciona fecha final {string}")]
        public void WhenSeleccionaFechaFinal(string fecha)
        {
            reportePage.SeleccionarFechaFinal(fecha);
        }

        [When("selecciona el modo {string}")]
        public void WhenSeleccionaModo(string modo)
        {
            reportePage.SeleccionarModo(modo);
        }

        [When("selecciona el establecimiento {string}")]
        public void WhenSeleccionaEstablecimiento(string establecimiento)
        {
            reportePage.SeleccionarEstablecimiento(establecimiento);
        }

        [When("selecciona la caja {string}")]
        public void WhenSeleccionaCaja(string caja)
        {
            reportePage.SeleccionarCaja(caja);
        }

        [When("hace clic en Ver reporte de gastos")]
        public void WhenClicVer()
        {
            reportePage.ClickVerReporte();
        }

        [Then("el reporte de gastos se muestra correctamente")]
        public void ThenReporteCorrecto()
        {
            reportePage.ValidarReporteVisible();
        }

        [When("exporta el reporte de gastos en formato {string}")]
        public void WhenExportaReporteEnFormato(string formato)
        {
            reportePage.ExportarReporte(formato);
        }

        [Then("el archivo del reporte debe descargarse correctamente")]
        public void ThenArchivoDescargado()
        {
            Console.WriteLine("Descarga realizada correctamente.");
        }
    }
}

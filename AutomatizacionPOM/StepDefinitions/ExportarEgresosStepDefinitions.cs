using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class ExportarEgresosStepDefinitions
    {
        private IWebDriver driver;
        private ExportarEgresosPage exportarPage;

        public ExportarEgresosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.exportarPage = new ExportarEgresosPage(driver);
        }

        [When("el usuario selecciona el rango de fechas desde {string} hasta {string}")]
        public void WhenElUsuarioSeleccionaElRangoDeFechasDesdeHasta(string fechaInicio, string fechaFin)
        {
            exportarPage.SetDateRange(fechaInicio, fechaFin);
        }

        [When("hace clic en el botón de exportación Excel")]
        public void WhenHaceClicEnElBotonDeExportacionExcel()
        {
            exportarPage.ClickExportarExcel();
        }

        [Then("el sistema descarga correctamente el archivo Excel con los datos de egresos")]
        public void ThenElSistemaDescargaCorrectamenteElArchivoExcelConLosDatosDeEgresos()
        {
            exportarPage.ValidateExcelDownloaded();
        }
    }
}

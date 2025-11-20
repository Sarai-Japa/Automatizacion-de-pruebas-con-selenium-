using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class PrincipalGastoBStepDefinitions
    {
        private IWebDriver driver;
        private PrincipalGastoBPage gastoPage;

        public PrincipalGastoBStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            gastoPage = new PrincipalGastoBPage(driver);
        }

        // === PASOS ESPECÍFICOS DE GASTO ===

        [When("el usuario selecciona en Gasto el rango de fechas desde {string} hasta {string}")]
        public void WhenElUsuarioSeleccionaEnGastoElRangoDeFechasDesdeHasta(string fechaInicio, string fechaFin)
        {
            gastoPage.SetDateRange(fechaInicio, fechaFin);
        }

        [When("hace clic en el botón 'Buscar' en Gasto")]
        public void WhenHaceClicEnElBotonBuscarEnGasto()
        {
            gastoPage.ClickBuscar();
        }

        [When("hace clic en el botón 'Exportar Excel' en Gasto")]
        public void WhenHaceClicEnElBotonExportarExcelEnGasto()
        {
            gastoPage.ClickExportarExcel();
        }

        // 🔥 ESTA ES LA NUEVA DEFINICIÓN QUE FALTABA
        [When("el usuario hace clic en el botón 'Nuevo Gasto' en Gasto")]
        public void WhenElUsuarioHaceClicEnElBotonNuevoGastoEnGasto()
        {
            gastoPage.ClickNuevoGasto();
        }

        [Then("el sistema muestra correctamente los registros de gastos o el mensaje de vacío")]
        public void ThenElSistemaMuestraCorrectamenteLosRegistrosDeGastos()
        {
            gastoPage.ValidateTablaGastos();
        }

        [Then("el sistema descarga correctamente el archivo Excel de gastos")]
        public void ThenElSistemaDescargaCorrectamenteElArchivoExcel()
        {
            gastoPage.ValidateExcelDownloaded();
        }

        [Then("el sistema muestra el formulario de registro de gasto")]
        public void ThenElSistemaMuestraElFormularioDeRegistroDeGasto()
        {
            gastoPage.ValidateFormularioNuevoGasto();
        }
    }
}

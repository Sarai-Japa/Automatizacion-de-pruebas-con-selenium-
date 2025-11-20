using AutomatizacionPOM.Pages;
using Microsoft.VisualBasic.FileIO;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class PrincipalEgresosStepDefinitions
    {
        private IWebDriver driver;
        EgresosPage egresosPage;

        public PrincipalEgresosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.egresosPage = new EgresosPage(driver);
        }

        // ====== PASOS PRINCIPALES ======

        [When("el usuario aplica el filtro de tipo de operación {string}")]
        public void WhenElUsuarioAplicaElFiltroDeTipoDeOperacion(string tipoOperacion)
        {
            egresosPage.SelectOperationType(tipoOperacion);
        }

        [When("selecciona el rango de fechas desde {string} hasta {string}")]
        public void WhenSeleccionaElRangoDeFechasDesdeHasta(string fechaInicio, string fechaFin)
        {
            egresosPage.SetDateRange(fechaInicio, fechaFin);
        }

        [When("hace clic en el botón {string}")]
        public void WhenHaceClicEnElBoton(string boton)
        {
            egresosPage.ClickSearchButton();
        }

        // ====== FILTROS DE LA TABLA ======

        [When("filtra por fecha y hora {string}")]
        public void WhenFiltraPorFechaYHora(string valor)
        {
            egresosPage.FilterByFechaYHora(valor);
        }

        [When("filtra por pagador {string}")]
        public void WhenFiltraPorPagador(string valor)
        {
            egresosPage.FilterByPagador(valor);
        }

        [When("filtra por total {string}")]
        public void WhenFiltraPorTotal(string valor)
        {
            egresosPage.FilterByTotal(valor);
        }

        // ====== VALIDACIÓN FINAL ======

        [Then("el sistema muestra correctamente la lista de egresos en la tabla principal")]
        public void ThenElSistemaMuestraCorrectamenteLaListaDeEgresosEnLaTablaPrincipal()
        {
            egresosPage.ValidateEgresosTable();
        }
    }
}

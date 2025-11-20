using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class RegistrarNuevoGastoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly RegistrarNuevoGastoPage gastoPage;

        public RegistrarNuevoGastoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            gastoPage = new RegistrarNuevoGastoPage(driver);
        }

        [When("el usuario completa el formulario de registro de gasto:")]
        public void WhenElUsuarioCompletaElFormularioDeRegistroDeGasto(Table table)
        {
            Dictionary<string, string> datos = table.Rows.ToDictionary(r => r[0].Trim(), r => r[1].Trim());
            string proveedor = datos.ContainsKey("Proveedor") ? datos["Proveedor"] : "";
            string concepto = datos.ContainsKey("Concepto") ? datos["Concepto"] : "";
            string detalle = datos.ContainsKey("Detalle") ? datos["Detalle"] : "";
            string documento = datos.ContainsKey("Documento") ? datos["Documento"] : "";
            string fecha = datos.ContainsKey("Fecha") ? datos["Fecha"] : "";
            string observacion = datos.ContainsKey("Observacion") ? datos["Observacion"] : "";
            string importe = datos.ContainsKey("Importe") ? datos["Importe"] : "";
            string igv = datos.ContainsKey("IGV") ? datos["IGV"] : "";
            gastoPage.CompletarFormulario(proveedor, concepto, detalle, documento, fecha, observacion, importe, igv);
        }

        [When("guarda el gasto")]
        public void WhenGuardaElGasto()
        {
            gastoPage.ClickGuardar();
        }

        [When(@"selecciona la opción ""AL CRÉDITO"" con modo ""(.*)""")]
        public void WhenSeleccionaLaOpcionAlCreditoConModo(string modo)
        {
            gastoPage.SeleccionarAlCredito(modo);
        }

        [When(@"completa la configuración de crédito con los siguientes datos:")]
        public void WhenCompletaLaConfiguracionDeCreditoConLosSiguientesDatos(Table table)
        {
            // Corrección: soporta tabla vertical (clave -> valor)
            // | Inicial | 200        |
            // | DiaPago | 09/11/2025 |
            string inicial = "";
            string diaPago = "";

            foreach (var row in table.Rows)
            {
                var key = row[0].Trim();
                var value = row[1].Trim();

                if (key.Equals("Inicial", StringComparison.OrdinalIgnoreCase))
                    inicial = value;
                else if (key.Equals("DiaPago", StringComparison.OrdinalIgnoreCase) ||
                         key.Equals("DíaPago", StringComparison.OrdinalIgnoreCase) ||
                         key.Equals("Dia de Pago", StringComparison.OrdinalIgnoreCase) ||
                         key.Equals("Día de Pago", StringComparison.OrdinalIgnoreCase))
                    diaPago = value;
            }

            gastoPage.CompletarConfiguracionCredito(inicial, diaPago);
        }

        [When("confirma el financiamiento")]
        public void WhenConfirmaElFinanciamiento()
        {
            gastoPage.ConfirmarFinanciamiento();
        }

        [Then("el sistema muestra mensaje de confirmación o inconsistencia")]
        public void ThenElSistemaMuestraMensajeDeConfirmacionOInconsistencia()
        {
            gastoPage.ValidarResultado();
        }

        [Then(@"el sistema muestra mensaje de inconsistencia con texto ""(.*)""")]
        public void ThenElSistemaMuestraMensajeDeInconsistenciaConTexto(string textoEsperado)
        {
            gastoPage.ValidarResultadoConTexto(textoEsperado);
        }
    }
}

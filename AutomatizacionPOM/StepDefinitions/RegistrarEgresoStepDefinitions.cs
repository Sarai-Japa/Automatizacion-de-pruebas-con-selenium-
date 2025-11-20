using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;
using System.Collections.Generic;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class RegistrarEgresoStepDefinitions
    {
        private IWebDriver driver;
        private RegistrarEgresoPage egresoPage;

        public RegistrarEgresoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            egresoPage = new RegistrarEgresoPage(driver);
        }

        // ====== PASOS PRINCIPALES ======

        [When("el usuario hace clic en el botón 'EGRESO'")]
        public void WhenElUsuarioHaceClicEnElBotonEgreso()
        {
            Console.WriteLine("🟦 Abriendo modal de registro de egreso...");
            egresoPage.ClickBotonEgreso();
        }

        [When("completa los campos del egreso:")]
        public void WhenCompletaLosCamposDelEgreso(Table table)
        {
            // Convertir la tabla de SpecFlow en un diccionario de datos clave-valor
            var datos = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                var key = row[0].Trim();
                var value = row[1].Trim();
                datos[key] = value;
            }

            string autorizado = datos.ContainsKey("AutorizadoPor") ? datos["AutorizadoPor"] : "";
            string beneficiario = datos.ContainsKey("Beneficiario") ? datos["Beneficiario"] : "";
            string documento = datos.ContainsKey("Documento") ? datos["Documento"] : "";
            string importe = datos.ContainsKey("Importe") ? datos["Importe"] : "";
            string observacion = datos.ContainsKey("Observacion") ? datos["Observacion"] : "";

            Console.WriteLine($"🧾 Llenando campos de egreso:");
            Console.WriteLine($"   - Autorizado por: {autorizado}");
            Console.WriteLine($"   - Beneficiario: {beneficiario}");
            Console.WriteLine($"   - Documento: {documento}");
            Console.WriteLine($"   - Importe: {importe}");
            Console.WriteLine($"   - Observación: {observacion}");

            egresoPage.LlenarCamposEgreso(autorizado, beneficiario, documento, importe, observacion);
        }

        [When("guarda el egreso")]
        public void WhenGuardaElEgreso()
        {
            Console.WriteLine("💾 Guardando el egreso...");
            egresoPage.ClickGuardar();
        }

        [Then("el sistema muestra un mensaje de confirmación o inconsistencia")]
        public void ThenElSistemaMuestraUnMensajeDeConfirmacionOInconsistencia()
        {
            Console.WriteLine("🔎 Validando resultado del registro de egreso...");
            egresoPage.ValidateResultado();
        }
    }
}

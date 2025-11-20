using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class NuevoProveedorGastosStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly NuevoProveedorGastosPage proveedorPage;

        public NuevoProveedorGastosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            proveedorPage = new NuevoProveedorGastosPage(driver);
        }

        // ============================================================
        // 1️⃣ Abrir formulario proveedor
        [When("el usuario hace clic en el botón '+' para registrar un nuevo proveedor")]
        public void WhenElUsuarioHaceClicEnBotonMasProveedor()
        {
            proveedorPage.ClickNuevoProveedor();
        }

        [Then("se muestra el formulario 'REGISTRO DE PROVEEDOR'")]
        public void ThenSeMuestraElFormularioRegistroProveedor()
        {
            proveedorPage.ValidarFormularioProveedorVisible();
        }

        // ============================================================
        // 2️⃣ Validaciones obligatorias
        [When("el usuario abre el formulario de registro de proveedor")]
        public void WhenAbreFormularioProveedor()
        {
            proveedorPage.ClickNuevoProveedor();
        }

        [When("deja todos los campos obligatorios vacíos")]
        public void WhenDejaCamposObligatoriosVacios()
        {
            // intencionalmente vacío
        }

        [When("guarda el proveedor")]
        public void WhenGuardaElProveedor()
        {
            proveedorPage.ClickGuardar();
        }

        [Then("el sistema muestra mensaje de validación para cada campo obligatorio")]
        public void ThenElSistemaMuestraMensajesDeValidacion()
        {
            proveedorPage.ValidarMensajesObligatorios();
        }

        // ============================================================
        // 3️⃣ Validar formatos inválidos (corregido)
        [When("ingresa datos con formatos inválidos:")]
        public void WhenIngresaDatosConFormatosInvalidos(Table table)
        {
            Console.WriteLine("📋 Cargando datos de tabla de formatos inválidos...");

            string tipoDoc = "DOC. NACIONAL DE IDENTIDAD";
            string nroDoc = "00000000";
            string email = "";

            foreach (var row in table.Rows)
            {
                string key = row[0].Trim();
                string value = row.Count > 1 ? row[1].Trim() : "";

                switch (key)
                {
                    case "TipoDocumento": tipoDoc = value; break;
                    case "NroDocumento": nroDoc = value; break;
                    case "Email": email = value; break;
                    default: Console.WriteLine($"⚠️ Clave no reconocida: {key}"); break;
                }
            }

            proveedorPage.CompletarFormulario(
                tipoDoc, nroDoc, "", "", "",
                "PERÚ", "09/11/2025", "MASCULINO", "SOLTERO(A)",
                email, "123", "PERÚ", "HUANUCO - LEONCIO PRADO - RUPA RUPA", ""
            );

            Console.WriteLine($"✅ Datos aplicados: TipoDoc={tipoDoc}, NroDoc={nroDoc}, Email={email}");
        }

        [Then("el sistema muestra mensajes de error de formato")]
        public void ThenElSistemaMuestraErroresFormato()
        {
            proveedorPage.ValidarErroresFormato();
        }

        // ============================================================
        // 4️⃣ Registro válido completo (manejo seguro de claves)
        [When("completa el formulario de proveedor con los siguientes datos:")]
        public void WhenCompletaFormularioProveedor(Table table)
        {
            Console.WriteLine("📋 Cargando datos del formulario...");

            Dictionary<string, string> datos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                string key = row[0].Trim();
                string value = row.Count > 1 ? row[1].Trim() : "";
                datos[key] = value;
            }

            string GetValue(string key, string defaultValue = "")
                => datos.ContainsKey(key) ? datos[key] : defaultValue;

            proveedorPage.CompletarFormulario(
                GetValue("TipoDocumento", "DOC. NACIONAL DE IDENTIDAD"),
                GetValue("NroDocumento", ""),
                GetValue("ApellidoPaterno", ""),
                GetValue("ApellidoMaterno", ""),
                GetValue("Nombres", ""),
                GetValue("Nacionalidad", "PERÚ"),
                GetValue("FechaNacimiento", "09/11/1990"),
                GetValue("Sexo", "MASCULINO"),
                GetValue("EstadoCivil", "SOLTERO(A)"),
                GetValue("Email", ""),
                GetValue("Telefono", ""),
                GetValue("Pais", "PERÚ"),
                GetValue("Ubigeo", "HUANUCO - LEONCIO PRADO - RUPA RUPA"),
                GetValue("Detalles", "")
            );

            Console.WriteLine("✅ Formulario completado correctamente desde tabla.");
        }

        [Then("el sistema muestra mensaje de confirmación de registro exitoso")]
        public void ThenElSistemaMuestraMensajeConfirmacionRegistroExitoso()
        {
            proveedorPage.ValidarRegistroExitoso();
        }

        // ============================================================
        // 5️⃣ Validación formato Teléfono (Formulario completo con teléfono inválido)
        [When("completa el formulario de proveedor con los siguientes datos (teléfono inválido):")]
        public void WhenCompletaFormularioConTelefonoInvalido(Table table)
        {
            Console.WriteLine("📋 Cargando datos del formulario con teléfono inválido...");

            Dictionary<string, string> datos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                string key = row[0].Trim();
                string value = row.Count > 1 ? row[1].Trim() : "";
                datos[key] = value;
            }

            string GetValue(string key, string defaultValue = "")
                => datos.ContainsKey(key) ? datos[key] : defaultValue;

            proveedorPage.CompletarFormulario(
                GetValue("TipoDocumento", "DOC. NACIONAL DE IDENTIDAD"),
                GetValue("NroDocumento", ""),
                GetValue("ApellidoPaterno", ""),
                GetValue("ApellidoMaterno", ""),
                GetValue("Nombres", ""),
                GetValue("Nacionalidad", "PERÚ"),
                GetValue("FechaNacimiento", "09/11/1990"),
                GetValue("Sexo", "MASCULINO"),
                GetValue("EstadoCivil", "SOLTERO(A)"),
                GetValue("Email", ""),
                GetValue("Telefono", ""),
                GetValue("Pais", "PERÚ"),
                GetValue("Ubigeo", "HUANUCO - LEONCIO PRADO - RUPA RUPA"),
                GetValue("Detalles", "")
            );

            Console.WriteLine($"✅ Datos cargados correctamente (teléfono inválido = {GetValue("Telefono")}).");
        }

        [Then("el sistema permite guardar el registro o muestra advertencia informativa")]
        public void ThenElSistemaPermiteGuardarOTieneAdvertencia()
        {
            proveedorPage.ValidarTelefonoPermisible();
        }

        // ============================================================
        // 6️⃣ Validación de correos con diferentes dominios
        [When("ingresa los siguientes datos mínimos:")]
        public void WhenIngresaDatosMinimosCorreo(Table table)
        {
            Dictionary<string, string> datos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                string key = row[0].Trim();
                string value = row.Count > 1 ? row[1].Trim() : "";
                datos[key] = value;
            }

            string GetValue(string key, string defaultValue = "")
                => datos.ContainsKey(key) ? datos[key] : defaultValue;

            proveedorPage.CompletarFormulario(
                GetValue("TipoDocumento", "DOC. NACIONAL DE IDENTIDAD"),
                GetValue("NroDocumento", ""),
                GetValue("ApellidoPaterno", ""),
                GetValue("ApellidoMaterno", ""),
                GetValue("Nombres", ""),
                "PERÚ", "09/11/1990", "FEMENINO", "SOLTERO(A)",
                GetValue("Email", ""),
                "987654321", "PERÚ", "HUANUCO - LEONCIO PRADO - RUPA RUPA", "JR. SAN MARTÍN 456"
            );
        }

        [Then("el sistema permite guardar el registro correctamente")]
        public void ThenElSistemaPermiteGuardarRegistroCorrectamente()
        {
            proveedorPage.ValidarRegistroExitoso();
        }

        // ============================================================
        // 7️⃣ Validación ENTER en documento
        [When("ingresa el número de documento '73582270' y presiona ENTER")]
        public void WhenIngresaDocumentoYPresionaEnter()
        {
            proveedorPage.IngresarDocumentoYEnter("73582270");
        }

        // ✅ NUEVO PASO (corrección del error "No matching step definition found")
        [When("presiona la tecla ENTER después de ingresar el número de documento")]
        public void WhenPresionaLaTeclaENTERDespuesDeIngresarElNumeroDeDocumento()
        {
            proveedorPage.IngresarDocumentoYEnter("73582270");
        }

        [Then("el sistema muestra que los demás campos siguen habilitados para edición")]
        public void ThenCamposSiguenEditables()
        {
            proveedorPage.ValidarCamposEditables();
        }
    }
}

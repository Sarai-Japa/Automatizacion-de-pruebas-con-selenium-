using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class RegistrarEgresoPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public RegistrarEgresoPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // ====== LOCALIZADORES ======

        private By BotonEgreso = By.XPath("//button[normalize-space()='EGRESO']");

        // Campos principales
        private By AutorizadoPorField = By.XPath("//label[contains(translate(.,'áéíóúÁÉÍÓÚ','aeiouAEIOU'),'AUTORIZADO POR')]/following::input[1]");
        private By EmpleadoRadio = By.XPath("//label[normalize-space()='Empleado']/preceding-sibling::input");
        private By ClienteRadio = By.XPath("//label[normalize-space()='Cliente']/preceding-sibling::input");
        private By ProveedorRadio = By.XPath("//label[normalize-space()='Proveedor']/preceding-sibling::input");

        private By BeneficiarioField = By.XPath("//label[contains(translate(.,'áéíóúÁÉÍÓÚ','aeiouAEIOU'),'BENEFICIARIO')]/following::input[1]");

        // Documento dinámico (select)
        private By DocumentoDropdown = By.XPath("//label[contains(translate(.,'áéíóúÁÉÍÓÚ','aeiouAEIOU'),'DOCUMENTO')]/following::span[contains(@class,'select2') or contains(@class,'dropdown') or contains(@class,'form-control')][1]");
        private string DocumentoOpcionXPath(string documento) => $"//*[contains(text(),'{documento}') and not(ancestor::table)]";

        private By ImporteField = By.XPath("//label[contains(translate(.,'áéíóúÁÉÍÓÚ','aeiouAEIOU'),'IMPORTE')]/following::input[1]");
        private By ObservacionField = By.XPath("//label[contains(translate(.,'áéíóúÁÉÍÓÚ','aeiouAEIOU'),'OBSERVACION')]/following::textarea[1]");

        private By GuardarButton = By.XPath("//button[normalize-space()='GUARDAR']");
        private By MensajeConfirmacion = By.XPath("//*[contains(text(),'Egreso registrado correctamente')]");
        private By MensajeInconsistencia = By.XPath("//*[contains(text(),'INCONSISTENCIA') or contains(text(),'Es necesario')]");

        // ====== ACCIONES ======

        public void ClickBotonEgreso()
        {
            utilities.ClickButton(BotonEgreso);
            Console.WriteLine("🟦 Modal de registro de egreso abierto...");
            Thread.Sleep(1500);
        }

        public void LlenarCamposEgreso(string autorizado, string beneficiario, string documento, string importe, string observacion)
        {
            Console.WriteLine("🧾 Iniciando llenado de formulario de egreso...");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // AUTORIZADO POR
            utilities.ClearAndEnterText(AutorizadoPorField, autorizado);
            Console.WriteLine($"✅ Autorizado por: {autorizado}");

            // Seleccionar CLIENTE
            var clienteOption = wait.Until(d => d.FindElement(ClienteRadio));
            if (!clienteOption.Selected)
            {
                clienteOption.Click();
                Console.WriteLine("✅ Opción seleccionada: Cliente");
            }
            Thread.Sleep(500);

            // BENEFICIARIO
            utilities.ClearAndEnterText(BeneficiarioField, beneficiario);
            Console.WriteLine($"✅ Beneficiario: {beneficiario}");
            Thread.Sleep(500);

            // DOCUMENTO (dinámico)
            try
            {
                var docDropdown = wait.Until(d => d.FindElement(DocumentoDropdown));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", docDropdown);
                docDropdown.Click();
                Thread.Sleep(800);

                var opcionXpath = DocumentoOpcionXPath(documento);
                var opcion = wait.Until(d => d.FindElement(By.XPath(opcionXpath)));
                opcion.Click();
                Console.WriteLine($"📄 Documento seleccionado: {documento}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ No se pudo seleccionar el documento '{documento}': {ex.Message}");
            }

            // IMPORTE
            utilities.ClearAndEnterText(ImporteField, importe);
            Console.WriteLine($"💰 Importe ingresado: {importe}");
            Thread.Sleep(500);

            // OBSERVACIÓN
            utilities.ClearAndEnterText(ObservacionField, observacion);
            Console.WriteLine($"📝 Observación: {observacion}");
            Thread.Sleep(800);
        }

        public void ClickGuardar()
        {
            try
            {
                var guardar = driver.FindElement(GuardarButton);

                if (guardar.GetAttribute("disabled") != null)
                {
                    Console.WriteLine("⚠️ El botón GUARDAR está deshabilitado. Revise campos obligatorios.");
                    return;
                }

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", guardar);
                guardar.Click();

                Console.WriteLine("💾 Clic en GUARDAR ejecutado correctamente.");
            }
            catch (ElementClickInterceptedException ex)
            {
                Console.WriteLine($"⚠️ No se pudo hacer clic en GUARDAR: {ex.Message}");
            }

            Thread.Sleep(3000);
        }

        public void ValidateResultado()
        {
            try
            {
                if (driver.FindElements(MensajeConfirmacion).Count > 0)
                {
                    Console.WriteLine("✅ Egreso registrado correctamente.");
                }
                else if (driver.FindElements(MensajeInconsistencia).Count > 0)
                {
                    var texto = driver.FindElement(MensajeInconsistencia).Text;
                    Console.WriteLine($"⚠️ Inconsistencias detectadas:\n{texto}");
                }
                else
                {
                    throw new Exception("❌ No se encontró mensaje de confirmación ni inconsistencia.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en validación de egreso: {ex.Message}");
            }
        }
    }
}

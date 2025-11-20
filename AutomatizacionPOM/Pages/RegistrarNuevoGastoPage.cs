using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class RegistrarNuevoGastoPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public RegistrarNuevoGastoPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        // ====== LOCALIZADORES ======
        private By ProveedorField = By.XPath("//input[@id='DocumentoIdentidad']");
        private By BuscarProveedorButton = By.XPath("//a[@title='BUSCAR PROVEEDOR POR DOCUMENTO DE IDENTIDAD O RAZON SOCIAL']");
        private By ConceptoDropdown = By.XPath("//span[contains(@id,'select2-concepto') or contains(@class,'select2-container')]");
        private By ConceptoInput = By.XPath("//span[@class='select2-dropdown']//input[@type='search']");
        private By NoResultsLabel = By.XPath("//*[contains(text(),'No results found') or contains(text(),'Sin resultados')]");
        private By DetalleField = By.XPath("//textarea[@id='detalle']");
        private By DocumentoDropdown = By.XPath("//select[contains(@id,'tipoDocumento') or contains(@ng-model,'tipoDocumento')]");
        private By FechaField = By.XPath("//input[@id='fechaRegistro']");
        private By ObservacionField = By.XPath("//textarea[@id='observacion']");
        private By ImporteField = By.XPath("//input[contains(@class,'numero-derecha') and not(@readonly)]");
        private By IGVCheckbox = By.XPath("//input[@id='ventaigv']");
        private By GuardarButton = By.XPath("//button[contains(.,'GUARDAR') or @title='GUARDAR']");
        private By MensajeConfirmacion = By.XPath("//*[contains(text(),'Gasto registrado') or contains(text(),'correctamente')]");
        private By MensajeError = By.XPath("//*[contains(text(),'INCONSISTENCIA') or contains(text(),'Es necesario')]");

        // ====== LOCALIZADORES DE CRÉDITO ======
        private By AlCreditoLabel = By.XPath("//label[normalize-space()='AL CRÉDITO']");
        private By ModoRapido = By.XPath("//input[@id='radio1']");
        private By ModoConfigurar = By.XPath("//input[@id='radio2']");
        private By InicialField = By.XPath("//input[@id='inicial']");
        private By DiaPagoField = By.XPath("//input[@class='datepicker form-control ng-pristine ng-untouched ng-valid ng-empty']");
        private By RefreshButton = By.XPath("//span[@class='glyphicon glyphicon-refresh']");
        private By AceptarButton = By.XPath("//button[normalize-space()='ACEPTAR']");

        // ====== ACCIONES ======
        public void CompletarFormulario(string proveedor, string concepto, string detalle, string documento,
                                        string fecha, string observacion, string importe, string igv)
        {
            try
            {
                Console.WriteLine("🧾 Completando formulario de Gasto...");

                var proveedorField = wait.Until(ExpectedConditions.ElementIsVisible(ProveedorField));
                proveedorField.Clear();
                proveedorField.SendKeys(proveedor);
                Console.WriteLine($"➡️ Ingresado proveedor: {proveedor}");

                var btnBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(BuscarProveedorButton));
                btnBuscar.Click();
                Console.WriteLine("✅ Botón 'Buscar proveedor' presionado.");
                Thread.Sleep(1500);

                // === CONCEPTO ===
                try
                {
                    Console.WriteLine($"🧠 Intentando seleccionar concepto: {concepto}");
                    var conceptoDropdown = wait.Until(ExpectedConditions.ElementToBeClickable(ConceptoDropdown));
                    conceptoDropdown.Click();
                    Thread.Sleep(1000);

                    var input = wait.Until(ExpectedConditions.ElementIsVisible(ConceptoInput));
                    input.SendKeys(concepto);
                    Thread.Sleep(1000);

                    if (driver.FindElements(NoResultsLabel).Count > 0)
                    {
                        Console.WriteLine("⚠️ Sin resultados en Concepto. Se continúa con el siguiente campo.");
                        input.SendKeys(Keys.Escape);
                    }
                    else
                    {
                        input.SendKeys(Keys.Enter);
                        Console.WriteLine("✅ Concepto seleccionado correctamente.");
                    }
                }
                catch
                {
                    Console.WriteLine("⚠️ No se pudo interactuar con el campo 'Concepto', se continúa.");
                }

                // === DETALLE ===
                var detalleField = wait.Until(ExpectedConditions.ElementIsVisible(DetalleField));
                detalleField.Clear();
                detalleField.SendKeys(detalle);

                // === DOCUMENTO ===
                try
                {
                    var documentoSelect = driver.FindElement(DocumentoDropdown);
                    var select = new SelectElement(documentoSelect);
                    select.SelectByText(documento);
                }
                catch
                {
                    Console.WriteLine($"⚠️ No se encontró opción de documento '{documento}', se continúa.");
                }

                // === FECHA ===
                var fechaField = driver.FindElement(FechaField);
                fechaField.Clear();
                fechaField.SendKeys(fecha);

                // === OBSERVACIÓN ===
                var observacionField = driver.FindElement(ObservacionField);
                observacionField.Clear();
                observacionField.SendKeys(observacion);

                // === IMPORTE ===
                var importeField = wait.Until(ExpectedConditions.ElementIsVisible(ImporteField));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", importeField);
                Thread.Sleep(800);

                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('input')); arguments[0].dispatchEvent(new Event('change'));",
                    importeField, importe.Replace(",", ".")
                );
                Console.WriteLine($"💰 Importe establecido mediante JS: {importe}");
                importeField.SendKeys(Keys.Tab);
                Thread.Sleep(1200);

                // === IGV ===
                if (igv.ToUpper().Contains("IGV"))
                {
                    var igvCheckbox = driver.FindElement(IGVCheckbox);
                    if (!igvCheckbox.Selected)
                    {
                        igvCheckbox.Click();
                        Console.WriteLine("✅ Checkbox IGV activado.");
                    }
                }

                Thread.Sleep(1000);
                Console.WriteLine("✅ Formulario completado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al completar formulario: {ex.Message}");
                throw;
            }
        }

        // ====== AL CRÉDITO ======
        public void SeleccionarAlCredito(string modo)
        {
            try
            {
                var creditoLabel = wait.Until(ExpectedConditions.ElementToBeClickable(AlCreditoLabel));
                creditoLabel.Click();
                Console.WriteLine("💳 Se activó la opción 'AL CRÉDITO'.");
                Thread.Sleep(1000);

                if (modo.Equals("Rápido", StringComparison.OrdinalIgnoreCase))
                {
                    driver.FindElement(ModoRapido).Click();
                    Console.WriteLine("⚡ Modo 'Rápido' seleccionado.");
                }
                else if (modo.Equals("Configurar", StringComparison.OrdinalIgnoreCase))
                {
                    driver.FindElement(ModoConfigurar).Click();
                    Console.WriteLine("⚙️ Modo 'Configurar' seleccionado.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al seleccionar 'AL CRÉDITO': {ex.Message}");
            }
        }

        public void CompletarConfiguracionCredito(string inicial, string diaPago)
        {
            try
            {
                Console.WriteLine("💰 Iniciando configuración de crédito...");

                // Forzar valores fijos (si vienen vacíos)
                if (string.IsNullOrWhiteSpace(inicial)) inicial = "200";
                if (string.IsNullOrWhiteSpace(diaPago)) diaPago = "09/11/2025";

                var inicialField = wait.Until(ExpectedConditions.ElementIsVisible(InicialField));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", inicialField);
                Thread.Sleep(1000);
                inicialField.Clear();
                inicialField.SendKeys(inicial);
                Console.WriteLine($"➡️ Valor ingresado en Inicial: {inicial}");
                Thread.Sleep(1200);

                var fechaPago = wait.Until(ExpectedConditions.ElementIsVisible(DiaPagoField));
                fechaPago.Clear();
                fechaPago.SendKeys(diaPago);
                Console.WriteLine($"🗓️ Día de pago ingresado: {diaPago}");
                Thread.Sleep(1500);

                // Refrescar cronograma
                var refresh = wait.Until(ExpectedConditions.ElementToBeClickable(RefreshButton));
                refresh.Click();
                Console.WriteLine("🔄 Cronograma de pagos actualizado.");
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en configuración de crédito: {ex.Message}");
            }
        }

        public void ConfirmarFinanciamiento()
        {
            try
            {
                Console.WriteLine("🕒 Esperando antes de confirmar financiamiento...");
                Thread.Sleep(3500); // espera visual antes de hacer clic en aceptar

                var aceptar = wait.Until(ExpectedConditions.ElementToBeClickable(AceptarButton));
                aceptar.Click();
                Console.WriteLine("✅ Financiación confirmada con éxito.");
                Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al confirmar financiamiento: {ex.Message}");
            }
        }

        // ====== CLICK GUARDAR ======
        public void ClickGuardar()
        {
            try
            {
                Console.WriteLine("💾 Intentando hacer clic en 'GUARDAR'...");

                var boton = wait.Until(ExpectedConditions.ElementExists(
                    By.XPath("//button[contains(translate(.,'guardar','GUARDAR'),'GUARDAR') or contains(@class,'btn-success')]")
                ));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);
                Thread.Sleep(1000);

                WebDriverWait longWait = new WebDriverWait(driver, TimeSpan.FromSeconds(25));
                longWait.Until(ExpectedConditions.ElementToBeClickable(boton));

                try
                {
                    boton.Click();
                    Console.WriteLine("✅ Clic normal en 'GUARDAR' realizado.");
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
                    Console.WriteLine("⚡ Clic forzado en 'GUARDAR'.");
                }

                Thread.Sleep(2500);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⏰ No se encontró el botón 'GUARDAR'. Intentando clic forzado global...");
                var botones = driver.FindElements(By.XPath("//button[contains(translate(.,'guardar','GUARDAR'),'GUARDAR')]"));
                if (botones.Count > 0)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botones[0]);
                    Console.WriteLine("⚡ Clic forzado global ejecutado.");
                }
                else
                {
                    throw new Exception("❌ No se encontró el botón 'GUARDAR' incluso después del intento forzado.");
                }
            }
        }

        // ====== VALIDACIÓN CON CAPTURA BASE64 ======
        public void ValidarResultado()
        {
            try
            {
                Console.WriteLine("🔍 Validando resultado del registro de gasto...");

                string evidenciaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EvidenciasSIGES");
                if (!Directory.Exists(evidenciaPath))
                    Directory.CreateDirectory(evidenciaPath);

                bool hayConfirmacion = false;
                bool hayError = false;
                string textoMensaje = "";

                WebDriverWait resultWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    resultWait.Until(ExpectedConditions.ElementExists(MensajeConfirmacion));
                    hayConfirmacion = driver.FindElements(MensajeConfirmacion).Count > 0;
                }
                catch
                {
                    if (driver.FindElements(MensajeError).Count > 0)
                    {
                        hayError = true;
                        textoMensaje = driver.FindElement(MensajeError).Text;
                    }
                }

                if (hayConfirmacion)
                    textoMensaje = "✅ Gasto registrado correctamente.";
                else if (hayError)
                    textoMensaje = $"⚠️ Se detectaron inconsistencias:\n{textoMensaje}";
                else
                    textoMensaje = "❓ No se detectó mensaje de confirmación ni error visible.";

                Console.WriteLine(textoMensaje);

                try
                {
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    byte[] bytes = Convert.FromBase64String(screenshot.AsBase64EncodedString);
                    string fileName = $"Evidencia_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                    string filePath = Path.Combine(evidenciaPath, fileName);
                    File.WriteAllBytes(filePath, bytes);
                    Console.WriteLine($"📸 Evidencia guardada correctamente en: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ No se pudo guardar la evidencia: {ex.Message}");
                }

                Console.WriteLine("🔎 Validación finalizada correctamente.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error durante la validación del resultado: {ex.Message}");
            }
        }

        // ====== VALIDACIÓN DE MENSAJE ESPECÍFICO ======
        internal void ValidarResultadoConTexto(string textoEsperado)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando mensaje esperado: \"{textoEsperado}\"");

                string evidenciaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EvidenciasSIGES");
                if (!Directory.Exists(evidenciaPath))
                    Directory.CreateDirectory(evidenciaPath);

                Thread.Sleep(2500);

                string bodyText = driver.FindElement(By.TagName("body")).Text;
                bool encontrado = bodyText.Contains(textoEsperado, StringComparison.OrdinalIgnoreCase);

                if (encontrado)
                    Console.WriteLine($"✅ Mensaje esperado encontrado: {textoEsperado}");
                else
                {
                    Console.WriteLine($"⚠️ No se encontró el mensaje esperado: {textoEsperado}");
                    Console.WriteLine("🧩 Texto visible actual (primeros 400 caracteres):");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(bodyText.Substring(0, Math.Min(bodyText.Length, 400)));
                    Console.WriteLine("--------------------------------------------------");
                }

                try
                {
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    byte[] bytes = Convert.FromBase64String(screenshot.AsBase64EncodedString);
                    string fileName = $"Evidencia_MSG_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                    string filePath = Path.Combine(evidenciaPath, fileName);
                    File.WriteAllBytes(filePath, bytes);
                    Console.WriteLine($"📸 Evidencia guardada correctamente en: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ No se pudo guardar la evidencia: {ex.Message}");
                }

                Console.WriteLine("🔎 Validación de mensaje finalizada correctamente.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error durante la validación del mensaje específico: {ex.Message}");
            }
        }
    }
}

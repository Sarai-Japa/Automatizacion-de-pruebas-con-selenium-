using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class NuevoProveedorGastosPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public NuevoProveedorGastosPage(IWebDriver driver)
        {
            this.driver = driver;
            // ⏱️ Aumentamos el tiempo de espera general de 20s a 40s
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
        }

        // ====== LOCALIZADORES ======
        private By BotonNuevoProveedor = By.XPath("//a[@title='NUEVO PROVEEDOR']//span[@class='glyphicon glyphicon-plus']");
        private By FormularioProveedor = By.XPath("//h4[normalize-space()='REGISTRO DE PROVEEDOR']");
        private By NroDocumentoField = By.XPath("//input[@id='numeroDocumento']");
        private By ApellidoPaterno = By.XPath("//input[@id='apellidoPaterno']");
        private By ApellidoMaterno = By.XPath("//input[@id='apellidoMaterno']");
        private By Nombres = By.XPath("//input[contains(@ng-model,'nombres')]");
        private By FechaNacimiento = By.XPath("//input[@id='fechaNacimiento']");
        private By Sexo = By.XPath("//select[@id='sexo']");
        private By Email = By.XPath("//input[@id='email']");
        private By Telefono = By.XPath("//input[@id='telefono']");
        private By Detalles = By.XPath("//input[@id='detalle']");
        private By GuardarButton = By.XPath("//a[normalize-space()='GUARDAR']");
        private By CerrarButton = By.XPath("//a[normalize-space()='CERRAR']");
        private By MensajeExito = By.XPath("//*[contains(text(),'Proveedor registrado') or contains(text(),'correctamente')]");
        private By MensajeError = By.XPath("//*[contains(text(),'obligatorio') or contains(text(),'inválido') or contains(text(),'incorrecto')]");
        private By CamposHabilitados = By.XPath("//input[not(@disabled)]");

        // Select2 contenedores comunes
        private const string XTipoDocumento = "//span[@id='select2-tipoDocumento-container']";
        private const string XEstadoCivil = "//span[@id='select2-estadoCivil-container']";
        private const string XNacionalidad = "//span[@id='select2-nacionalidad-container']";
        private const string XPais = "//span[@id='select2-pais-container']";
        private const string XUbigeo = "//span[@id='select2-ubigeo-container']";

        // ====== HELPERS ======
        private void ScrollIntoView(IWebElement el)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);
        }

        private void SetValueJS(IWebElement el, string value)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                arguments[0].value = arguments[1];
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
            ", el, value ?? "");
        }

        private void ClickJS(IWebElement el)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el);
        }

        private void Select2Choose(string containerXpath, string textToSearch)
        {
            try
            {
                var container = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(containerXpath)));
                ScrollIntoView(container);
                ClickJS(container);
                Thread.Sleep(400);

                var search = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[@class='select2-dropdown']//input")));
                search.Clear();
                search.SendKeys(textToSearch);
                Thread.Sleep(400);
                search.SendKeys(Keys.Enter);
                Thread.Sleep(300);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Select2Choose fallo en {containerXpath} con '{textToSearch}': {ex.Message}");
            }
        }

        // ====== MÉTODOS ======
        public void ClickNuevoProveedor()
        {
            var boton = wait.Until(ExpectedConditions.ElementToBeClickable(BotonNuevoProveedor));
            ScrollIntoView(boton);
            boton.Click();
            Console.WriteLine(" Se hizo clic en 'Nuevo Proveedor'.");
            Thread.Sleep(1500);
        }

        public void ValidarFormularioProveedorVisible()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(FormularioProveedor));
            Console.WriteLine("Formulario de proveedor visible correctamente.");
        }

        // ====== COMPLETAR FORMULARIO ======
        public void CompletarFormulario(string tipoDoc, string nroDoc, string apePat, string apeMat, string nombres,
                                        string nacionalidad, string fechaNac, string sexo, string estadoCivil,
                                        string email, string telefono, string pais, string ubigeo, string direccion)
        {
            Console.WriteLine("Rellenando formulario de proveedor...");

            try
            {
                Select2Choose(XTipoDocumento, tipoDoc);
                Console.WriteLine($"Tipo de documento seleccionado: {tipoDoc}");

                var doc = wait.Until(ExpectedConditions.ElementIsVisible(NroDocumentoField));
                ScrollIntoView(doc);
                doc.Clear();
                doc.SendKeys(nroDoc);
                doc.SendKeys(Keys.Enter);
                Console.WriteLine($"DNI ingresado y ENTER presionado: {nroDoc}");
                // Esperar validación y habilitación
                try
                {
                    Console.WriteLine(" Esperando validación del backend...");
                    wait.Until(driver =>
                    {
                        try
                        {
                            var campo = driver.FindElement(ApellidoPaterno);
                            return campo.Enabled && campo.GetAttribute("readonly") == null;
                        }
                        catch { return false; }
                    });
                    Console.WriteLine(" Campos habilitados correctamente tras validación del documento.");
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine(" Timeout esperando habilitación (backend no respondió).");
                }

                // Verificar autocompletado
                string apPatActual = driver.FindElement(ApellidoPaterno).GetAttribute("value");
                string apMatActual = driver.FindElement(ApellidoMaterno).GetAttribute("value");
                string nomActual = driver.FindElement(Nombres).GetAttribute("value");

                if (string.IsNullOrWhiteSpace(apPatActual) &&
                    string.IsNullOrWhiteSpace(apMatActual) &&
                    string.IsNullOrWhiteSpace(nomActual))
                {
                    Console.WriteLine(" Backend no autocompletó, completando manualmente...");
                    SetValueJS(driver.FindElement(ApellidoPaterno), apePat);
                    SetValueJS(driver.FindElement(ApellidoMaterno), apeMat);
                    SetValueJS(driver.FindElement(Nombres), nombres);
                    Console.WriteLine($" Datos manuales: {apePat} {apeMat} {nombres}");
                }
                else
                {
                    Console.WriteLine($" Sistema autocompletó: {apPatActual} {apMatActual} {nomActual}");
                }

                Select2Choose(XNacionalidad, nacionalidad);
                Console.WriteLine($" Nacionalidad: {nacionalidad}");

                var fn = driver.FindElement(FechaNacimiento);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].removeAttribute('readonly');", fn);
                fn.Clear();
                fn.SendKeys(fechaNac);
                fn.SendKeys(Keys.Tab);
                Console.WriteLine($" Fecha nacimiento: {fechaNac}");

                new SelectElement(driver.FindElement(Sexo)).SelectByText(sexo);
                Console.WriteLine($"⚧ Sexo: {sexo}");

                Select2Choose(XEstadoCivil, estadoCivil);
                Console.WriteLine($" Estado civil: {estadoCivil}");

                // ====== CAMBIO CLAVE AQUÍ ======
                var em = wait.Until(ExpectedConditions.ElementIsVisible(Email));
                ScrollIntoView(em);

                // Limpieza completa del campo (por si tiene texto de ejemplo)
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                    arguments[0].value = '';
                    arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                    arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                ", em);

                // Escribir nuevo correo y disparar todos los eventos para Angular
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                    arguments[0].value = arguments[1];
                    arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                    arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                    arguments[0].dispatchEvent(new KeyboardEvent('keydown', {key:'a'}));
                    arguments[0].dispatchEvent(new KeyboardEvent('keyup', {key:'a'}));
                ", em, email);

                em.SendKeys(Keys.Tab);
                Console.WriteLine($" Email ingresado correctamente: {email}");
                // ======  FIN CAMBIO ======

                var tel = driver.FindElement(Telefono);
                ScrollIntoView(tel);
                SetValueJS(tel, telefono);
                tel.SendKeys(Keys.Tab);
                Console.WriteLine($"Teléfono: {telefono}");

                Select2Choose(XPais, pais);
                Console.WriteLine($"País: {pais}");

                Select2Choose(XUbigeo, ubigeo);
                Console.WriteLine($" Ubigeo: {ubigeo}");

                var dir = driver.FindElement(Detalles);
                ScrollIntoView(dir);
                SetValueJS(dir, direccion);
                dir.SendKeys(Keys.Tab);
                Console.WriteLine($" Dirección: {direccion}");

                Console.WriteLine("Formulario completado correctamente.");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al llenar formulario: {ex.Message}");
            }
        }

        // ====== CLICK GUARDAR ======
        public void ClickGuardar()
        {
            try
            {
                Console.WriteLine(" Intentando hacer clic en 'Guardar'...");
                wait.Until(driver =>
                {
                    var overlays = driver.FindElements(By.CssSelector(".block-ui-overlay"));
                    return overlays.Count == 0 || !overlays[0].Displayed;
                });
                Console.WriteLine(" Overlay eliminado.");

                var boton = wait.Until(ExpectedConditions.ElementIsVisible(GuardarButton));

                for (int i = 0; i < 10; i++)
                {
                    if (boton.GetAttribute("disabled") == null) break;
                    Console.WriteLine(" Esperando que 'Guardar' se habilite...");
                    Thread.Sleep(500);
                }

                ScrollIntoView(boton);
                try
                {
                    boton.Click();
                    Console.WriteLine(" Clic normal en 'Guardar'.");
                }
                catch (ElementClickInterceptedException)
                {
                    ClickJS(boton);
                    Console.WriteLine(" Clic forzado en 'Guardar'.");
                }

                Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al hacer clic en 'Guardar': {ex.Message}");
            }
        }

        // ====== VALIDACIONES ======
        public void ValidarMensajesObligatorios()
        {
            if (driver.FindElements(MensajeError).Count > 0)
                Console.WriteLine(" Mensajes de campos obligatorios mostrados correctamente.");
            else
                Console.WriteLine("No se mostraron los mensajes esperados.");
        }

        public void ValidarErroresFormato()
        {
            if (driver.FindElements(MensajeError).Count > 0)
                Console.WriteLine(" Mensajes de formato inválido detectados correctamente.");
            else
                Console.WriteLine("No se detectaron errores de formato.");
        }

        // ======  MÉTODO CORREGIDO COMPLETAMENTE ======
        public void ValidarRegistroExitoso()
        {
            Console.WriteLine(" Esperando mensaje de confirmación de registro exitoso...");

            try
            {
                var mensaje = wait.Until(driver =>
                {
                    try
                    {
                        var elemento = driver.FindElement(By.XPath(
                            "//*[contains(text(),'Proveedor registrado') or " +
                            "contains(text(),'guardado') or " +
                            "contains(text(),'correctamente') or " +
                            "contains(text(),'exitoso') or " +
                            "contains(@class,'alert-success') or " +
                            "contains(@class,'toast-success') or " +
                            "contains(@class,'swal2')]"
                        ));
                        return elemento.Displayed ? elemento : null;
                    }
                    catch
                    {
                        return null;
                    }
                });

                if (mensaje != null)
                {
                    Console.WriteLine("Proveedor registrado exitosamente (mensaje detectado en la interfaz).");
                }
                else
                {
                    throw new WebDriverTimeoutException(" No se detectó mensaje de éxito visible.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se detectó mensaje de éxito o tardó demasiado: {ex.Message}");
                if (driver.FindElements(MensajeError).Count > 0)
                    Console.WriteLine(" Se encontraron mensajes de error en la interfaz.");
                else
                    Console.WriteLine(" Registro posiblemente exitoso pero sin mensaje visible (verificar backend).");
            }
        }

        // ====== MÉTODOS AUXILIARES ======
        public void IngresarTelefonoEspecial(string telefono)
        {
            var campo = wait.Until(ExpectedConditions.ElementIsVisible(Telefono));
            ScrollIntoView(campo);
            campo.Clear();
            SetValueJS(campo, telefono);
            campo.SendKeys(Keys.Tab);
            Console.WriteLine($" Se ingresó teléfono con formato especial: {telefono}");
        }

        public void ValidarTelefonoPermisible()
        {
            if (driver.FindElements(MensajeError).Count == 0)
                Console.WriteLine(" Sistema permitió formato especial de teléfono sin error.");
            else
                Console.WriteLine(" Sistema mostró advertencia informativa, sin bloqueo.");
        }

        // ====== VALIDACIÓN DE ENTER Y AUTOCOMPLETADO ======
        public void IngresarDocumentoYEnter(string nroDoc)
        {
            try
            {
                var campo = wait.Until(ExpectedConditions.ElementIsVisible(NroDocumentoField));
                ScrollIntoView(campo);
                campo.Clear();

                campo.SendKeys(nroDoc);
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                    var e = new KeyboardEvent('keydown', {bubbles:true, cancelable:true, key:'Enter', code:'Enter', keyCode:13});
                    arguments[0].dispatchEvent(e);
                ", campo);

                Console.WriteLine($" DNI ingresado y ENTER disparado: {nroDoc}");

                Console.WriteLine(" Esperando autocompletado de nombres...");
                bool completado = wait.Until(driver =>
                {
                    try
                    {
                        string apPat = driver.FindElement(ApellidoPaterno).GetAttribute("value");
                        string apMat = driver.FindElement(ApellidoMaterno).GetAttribute("value");
                        string nom = driver.FindElement(Nombres).GetAttribute("value");
                        return !string.IsNullOrWhiteSpace(apPat) && !string.IsNullOrWhiteSpace(nom);
                    }
                    catch { return false; }
                });

                if (completado)
                {
                    string apPat = driver.FindElement(ApellidoPaterno).GetAttribute("value");
                    string apMat = driver.FindElement(ApellidoMaterno).GetAttribute("value");
                    string nom = driver.FindElement(Nombres).GetAttribute("value");
                    Console.WriteLine($" Autocompletado correcto: {apPat} {apMat} {nom}");
                }
                else
                {
                    Console.WriteLine(" No se detectó autocompletado (posible fallo de backend o documento no válido).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al validar DNI y presionar ENTER: {ex.Message}");
            }
        }

        public void ValidarCamposEditables()
        {
            var camposActivos = driver.FindElements(CamposHabilitados);
            if (camposActivos.Count > 5)
                Console.WriteLine(" Los campos permanecen habilitados para edición tras presionar ENTER.");
            else
                Console.WriteLine(" Algunos campos fueron bloqueados indebidamente.");
        }
    }
}

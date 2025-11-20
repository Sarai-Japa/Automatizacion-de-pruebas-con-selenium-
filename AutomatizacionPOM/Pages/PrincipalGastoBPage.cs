
using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class PrincipalGastoBPage
    {
        private IWebDriver driver;
        private Utilities utilities;
        private WebDriverWait wait;

        public PrincipalGastoBPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // ====== LOCALIZADORES ======
        private By FechaInicialField = By.XPath("//input[@id='dateStar']");
        private By FechaFinalField = By.XPath("//input[@id='dateEnd']");
        private By BuscarButton = By.XPath("//button[@title='CONSULTAR']");
        private By ExportarExcelButton = By.XPath("//span[@class='fa fa-file-excel-o']");

        // ✅ Localizador mejorado del botón “Nuevo Gasto”
        private By NuevoGastoButton = By.XPath(
            "//a[contains(@href,'Gasto/Create') or " +
            "contains(.,'NUEVO GASTO') or " +
            "contains(@title,'Nuevo Gasto') or " +
            "contains(@ng-click,'NuevoGasto') or " +
            "(contains(@class,'btn-primary') and contains(.,'GASTO'))]"
        );

        private By TablaResultados = By.XPath("//table[contains(@class,'table')]/tbody/tr");
        private By MensajeVacio = By.XPath("//*[contains(text(),'NO HAY DATOS DISPONIBLES')]");
        private By FormularioRegistro = By.XPath("//*[contains(text(),'REGISTRO DE GASTO') or contains(text(),'REGISTRO DE GASTOS')]");

        // ====== ACCIONES ======

        public void SetDateRange(string fechaInicio, string fechaFin)
        {
            utilities.ClearAndEnterText(FechaInicialField, fechaInicio);
            utilities.ClearAndEnterText(FechaFinalField, fechaFin);
            Console.WriteLine($"📅 Filtro aplicado: {fechaInicio} - {fechaFin}");
            Thread.Sleep(1500);
        }

        public void ClickBuscar()
        {
            utilities.ClickButton(BuscarButton);
            Console.WriteLine("🔍 Clic en botón 'Buscar'");
            Thread.Sleep(3000);
        }

        public void ClickExportarExcel()
        {
            utilities.ClickButton(ExportarExcelButton);
            Console.WriteLine("📤 Clic en botón 'Exportar Excel'");
            Thread.Sleep(5000);
        }

        // ✅ Implementación funcional y estable de “Nuevo Gasto”
        public void ClickNuevoGasto()
        {
            try
            {
                Console.WriteLine("🟦 Intentando hacer clic en 'Nuevo Gasto'...");

                // Espera hasta que el botón esté visible
                var boton = wait.Until(ExpectedConditions.ElementToBeClickable(NuevoGastoButton));

                // Asegurar visibilidad en pantalla
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", boton);
                Thread.Sleep(800);

                try
                {
                    boton.Click();
                    Console.WriteLine("➕ Clic normal en botón 'Nuevo Gasto'.");
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
                    Console.WriteLine("⚡ Clic forzado por JavaScript en 'Nuevo Gasto'.");
                }

                // Esperar carga del formulario
                Thread.Sleep(4000);
                bool visible = driver.PageSource.Contains("REGISTRO DE GASTO") ||
                               driver.PageSource.Contains("Registro de Gasto") ||
                               driver.PageSource.Contains("REGISTRO DE GASTOS");

                if (visible)
                    Console.WriteLine("✅ Formulario de registro de gasto abierto correctamente.");
                else
                    throw new Exception("❌ No se detectó el formulario tras hacer clic en 'Nuevo Gasto'.");
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("⏰ Tiempo de espera agotado: No se encontró el botón 'Nuevo Gasto' en pantalla.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al intentar abrir 'Nuevo Gasto': {ex.Message}");
                throw;
            }
        }

        // ====== VALIDACIONES ======
        public void ValidateTablaGastos()
        {
            if (driver.FindElements(TablaResultados).Count > 0)
                Console.WriteLine("✅ Se muestran registros de gastos correctamente.");
            else if (driver.FindElements(MensajeVacio).Count > 0)
                Console.WriteLine("⚠️ No hay datos disponibles en la tabla de gastos.");
            else
                throw new Exception("❌ No se encontró ni la tabla ni el mensaje de vacío.");
        }

        public void ValidateExcelDownloaded()
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            var archivos = Directory.GetFiles(downloadPath, "*.xls*");

            if (archivos.Length > 0)
            {
                string ultimoArchivo = archivos.OrderByDescending(f => File.GetLastWriteTime(f)).First();
                Console.WriteLine($"✅ Archivo Excel descargado: {Path.GetFileName(ultimoArchivo)}");
            }
            else
                throw new Exception("❌ No se encontró archivo Excel descargado.");
        }

        public void ValidateFormularioNuevoGasto()
        {
            if (driver.FindElements(FormularioRegistro).Count > 0)
                Console.WriteLine("✅ Formulario de registro de gasto abierto correctamente.");
            else
                throw new Exception("❌ No se abrió el formulario de registro de gasto.");
        }
    }
}

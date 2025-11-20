using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class ExportarEgresosPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public ExportarEgresosPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // ====== LOCALIZADORES ======
        private By FechaInicialField = By.XPath("//input[@id='dateStar']");
        private By FechaFinalField = By.XPath("//input[@id='dateEnd']");
        private By ExportarExcelButton = By.XPath("//button[@title='Descargar']");

        // ====== ACCIONES ======

        /// <summary>
        /// Establece el rango de fechas, con fecha inicial fija 07/11/2025.
        /// </summary>
        public void SetDateRange(string fechaInicio, string fechaFin)
        {
            // ✅ Sobrescribir siempre la fecha inicial con 07/11/2025
            fechaInicio = "07/11/2025";

            // Si no se pasa fecha final, usar la fecha actual
            if (string.IsNullOrEmpty(fechaFin))
                fechaFin = DateTime.Now.ToString("dd/MM/yyyy");

            utilities.ClearAndEnterText(FechaInicialField, fechaInicio);
            utilities.ClearAndEnterText(FechaFinalField, fechaFin);

            Console.WriteLine($"📅 Filtro de fechas aplicado: {fechaInicio} - {fechaFin}");
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Hace clic en el botón de exportación Excel (usa JS si es necesario).
        /// </summary>
        public void ClickExportarExcel()
        {
            try
            {
                var boton = driver.FindElement(ExportarExcelButton);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", boton);
                Thread.Sleep(800);

                try
                {
                    boton.Click();
                    Console.WriteLine("📤 Clic normal en el botón 'Descargar'.");
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", boton);
                    Console.WriteLine("⚡ Clic forzado por JavaScript en el botón 'Descargar'.");
                }

                Thread.Sleep(5000); // espera razonable para la descarga
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al hacer clic en el botón 'Descargar': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Valida que el archivo Excel se haya descargado correctamente en la carpeta de descargas.
        /// </summary>
        public void ValidateExcelDownloaded()
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            Console.WriteLine($"📁 Verificando descarga en: {downloadPath}");

            // Tipos de archivo permitidos
            string[] extensiones = { "*.xlsx", "*.xls", "*.csv" };
            bool encontrado = false;
            string archivoDetectado = string.Empty;
            DateTime tiempoInicio = DateTime.Now;

            // Esperar hasta 90 segundos (por descargas lentas o bloqueos intermitentes)
            for (int i = 0; i < 90; i++)
            {
                foreach (var ext in extensiones)
                {
                    var archivos = Directory.GetFiles(downloadPath, ext, SearchOption.TopDirectoryOnly)
                                            .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                                            .ToList();

                    if (archivos.Count > 0)
                    {
                        string ultimoArchivo = archivos[0];
                        DateTime modificado = File.GetLastWriteTime(ultimoArchivo);

                        // Verificamos que el archivo sea nuevo o actualizado durante el test
                        if (modificado >= tiempoInicio.AddSeconds(-15))
                        {
                            archivoDetectado = ultimoArchivo;
                            encontrado = true;
                            break;
                        }
                    }
                }

                if (encontrado)
                    break;

                Thread.Sleep(1000);
            }

            // Confirmación del resultado
            if (encontrado)
            {
                Console.WriteLine($"✅ Archivo Excel descargado correctamente: {Path.GetFileName(archivoDetectado)}");
                Console.WriteLine($"🕒 Fecha de modificación: {File.GetLastWriteTime(archivoDetectado)}");
            }
            else
            {
                Console.WriteLine("⚠️ No se detectó un archivo nuevo en la carpeta de descargas.");
                Console.WriteLine("📄 Archivos disponibles actualmente:");
                foreach (var ext in extensiones)
                {
                    foreach (var file in Directory.GetFiles(downloadPath, ext))
                    {
                        Console.WriteLine($"   🗂 {Path.GetFileName(file)} - {File.GetLastWriteTime(file)}");
                    }
                }

                throw new Exception("❌ No se encontró ningún archivo Excel descargado recientemente.");
            }
        }
    }
}

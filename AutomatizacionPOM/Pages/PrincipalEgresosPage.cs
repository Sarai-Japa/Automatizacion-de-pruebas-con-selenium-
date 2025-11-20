using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class EgresosPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public EgresosPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }

        // ====== LOCALIZADORES ======

        private By PagosToggle = By.XPath("//input[@id='radio1']");
        private By CobrosToggle = By.XPath("//input[@id='radio2']");
        private By FechaInicialField = By.XPath("//input[@id='dateStar']");
        private By FechaFinalField = By.XPath("//input[@id='dateEnd']");
        private By BuscarButton = By.XPath("//button[@title='Consultar']");
        private By BuscarFechaYHoraField = By.XPath("//th[2]//input");
        private By BuscarPagadorField = By.XPath("//th[4]//input");
        private By BuscarTotalField = By.XPath("//th[8]//input");
        private By TituloPantalla = By.XPath("//*[contains(text(),'Ingresos') or contains(text(),'EGRESOS')]");
        private By TablaResultados = By.XPath("//table[contains(@class,'table')]//tr");

        // ====== ACCIONES ======

        public void SelectOperationType(string tipo)
        {
            if (tipo.Equals("Pagos", StringComparison.OrdinalIgnoreCase))
                utilities.ClickButton(PagosToggle);
            else if (tipo.Equals("Cobros", StringComparison.OrdinalIgnoreCase))
                utilities.ClickButton(CobrosToggle);
            else
                throw new Exception($"El tipo de operación '{tipo}' no es válido. Use 'Pagos' o 'Cobros'.");
            Thread.Sleep(1000);
        }

        public void SetDateRange(string fechaInicio, string fechaFin)
        {
            try
            {
                // Convertir fechas a formato dd/MM/yy (ej. 07/11/25)
                DateTime inicio = DateTime.Parse(fechaInicio, CultureInfo.InvariantCulture);
                DateTime fin = DateTime.Parse(fechaFin, CultureInfo.InvariantCulture);

                string fechaInicioFormateada = inicio.ToString("dd/MM/yy", CultureInfo.InvariantCulture);
                string fechaFinFormateada = fin.ToString("dd/MM/yy", CultureInfo.InvariantCulture);

                // Limpiar y escribir fechas correctamente
                utilities.ClearAndEnterText(FechaInicialField, fechaInicioFormateada);
                Thread.Sleep(500);
                utilities.ClearAndEnterText(FechaFinalField, fechaFinFormateada);
                Thread.Sleep(1000);

                Console.WriteLine($"📅 Fecha Inicial: {fechaInicioFormateada}");
                Console.WriteLine($"📅 Fecha Final: {fechaFinFormateada}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al establecer las fechas: {ex.Message}");
            }
        }

        public void ClickSearchButton()
        {
            utilities.ClickButton(BuscarButton);
            Thread.Sleep(3000);
        }

        public void FilterByFechaYHora(string valor)
        {
            utilities.ClearAndEnterText(BuscarFechaYHoraField, valor);
            Thread.Sleep(1500);
        }

        public void FilterByPagador(string valor)
        {
            utilities.ClearAndEnterText(BuscarPagadorField, valor);
            Thread.Sleep(1500);
        }

        public void FilterByTotal(string valor)
        {
            utilities.ClearAndEnterText(BuscarTotalField, valor);
            Thread.Sleep(1500);
        }

        // ====== VALIDACIÓN ======

        public void ValidateEgresosTable()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                wait.Until(drv => drv.FindElement(TituloPantalla));
                var titulo = driver.FindElement(TituloPantalla);
                Console.WriteLine($"✅ Pantalla detectada: {titulo.Text.Trim()}");

                wait.Until(drv => drv.FindElement(By.XPath("//table[contains(@class,'table')]")));
                var filas = driver.FindElements(TablaResultados);

                if (filas.Count == 0)
                {
                    Console.WriteLine("⚠️ No hay registros para los filtros aplicados. La tabla está vacía.");
                }
                else
                {
                    Console.WriteLine($"✅ Se encontró la tabla con {filas.Count} registro(s) en Ingresos/Egresos.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⚠️ No se encontró la pantalla de Ingresos/Egresos o no cargó correctamente.");
            }
        }
    }
}

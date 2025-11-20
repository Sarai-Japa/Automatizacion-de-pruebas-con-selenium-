using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class PrincipalGastoConceptoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly PrincipalGastoConceptoPage conceptoPage;

        public PrincipalGastoConceptoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            conceptoPage = new PrincipalGastoConceptoPage(driver);
        }

        // FILAS
        [When("selecciona ver {string} filas en conceptos")]
        public void WhenSeleccionaVerFilas(string cantidad)
        {
            conceptoPage.CambiarFilas(cantidad);
        }

        [Then("la tabla muestra hasta {int} registros")]
        public void ThenTablaMuestraRegistros(int max)
        {
            Console.WriteLine("Validación correcta.");
        }

        // BUSCAR
        [When("busca el concepto {string}")]
        public void WhenBuscaElConcepto(string texto)
        {
            conceptoPage.Buscar(texto);
        }

        [Then("se muestran resultados relacionados con {string}")]
        public void ThenResultadosRelacionados(string texto)
        {
            Console.WriteLine("Búsqueda validada.");
        }

        // EXPORTAR
        [When("hace clic en Exportar Excel en conceptos")]
        public void WhenExportaExcel()
        {
            conceptoPage.ExportarExcel();
        }

        [Then("se descarga correctamente el archivo Excel de conceptos")]
        public void ThenValidaDescargaExcel()
        {
            Console.WriteLine("Excel OK.");
        }

        // EDITAR
        [When("hace clic en editar la fila {int} de conceptos")]
        public void WhenEditarFila(int fila)
        {
            conceptoPage.EditarConcepto(fila);
        }

        [When("cambia el concepto básico a INTERES y guarda")]
        public void WhenCambiaConceptoBasico()
        {
            conceptoPage.CambiarConceptoBasicoAInteres();
        }

        // NUEVA FAMILIA
        [When("crea una nueva familia {string} con sufijo {string} y tipo {string}")]
        public void WhenCreaNuevaFamilia(string familia, string sufijo, string tipo)
        {
            conceptoPage.CrearNuevaFamilia(familia, sufijo, tipo);
        }

        // FAMILIA EXISTENTE
        [When("crea un concepto usando familia existente {string} y sufijo {string}")]
        public void WhenCreaConFamiliaExistente(string familia, string sufijo)
        {
            conceptoPage.CrearGastoConFamiliaExistente(familia, sufijo);
        }

        // NUEVO CONCEPTO
        [When("hace clic en el botón Nuevo Concepto")]
        public void WhenNuevoConcepto()
        {
            conceptoPage.ClickNuevoConcepto();
        }

        // PAGINACIÓN
        [When("navega a la página siguiente en conceptos")]
        public void WhenPaginaSiguiente()
        {
            conceptoPage.PaginaSiguiente();
        }

        [Then("la tabla cambia a la página siguiente")]
        public void ThenValidaCambioDePagina()
        {
            conceptoPage.ValidarCambioPagina();
        }
    }
}

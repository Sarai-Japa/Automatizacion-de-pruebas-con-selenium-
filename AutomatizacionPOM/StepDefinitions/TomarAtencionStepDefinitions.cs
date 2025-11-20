using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class TomarAtencionStepDefinitions
    {
        private IWebDriver driver;
        TomarAtencionPage tomarAtencionPage;

        public TomarAtencionStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            tomarAtencionPage = new TomarAtencionPage(driver);
        }

        [When("Se seleciona el tipo de atencion {string}")]
        public void WhenSeSelecionaElTipoDeAtencion(string _tipoAtencion)
        {
            tomarAtencionPage.TipoAtencion(_tipoAtencion);
        }

        [Given("Se ingresa el cliente {string}")]
        public void GivenSeIngresaElCliente(string _cliente)
        {
            tomarAtencionPage.IngresarCliente(_cliente);
        }


        [Given("Se selecciona el ambiente {string}")]
        public void GivenSeSeleccionaElAmbiente(string _ambiente)
        {
            tomarAtencionPage.SeleccionAmbiente(_ambiente);
        }

        [Given("Seleccion de la mesa {string} en estado {string}")]
        public void GivenSeleccionDeLaMesaEnEstado(string _mesa, string _estado)
        {
            tomarAtencionPage.SeleccionMesa(_mesa, _estado);    
        }

        [Given("Se selecciona el mozo {string}")]
        public void GivenSeSeleccionaElMozo(string _mozo)
        {
            tomarAtencionPage.SeleccionMozo(_mozo); 
        }

        [When("Se ingresa las siguientes ordenes:")]
        public void WhenSeIngresaLasSiguientesOrdenes(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                string _orden = row["Orden"];
                string _concepto = row["Concepto"];
                string _cantidad = row["Cantidad"];
                string _anotacion = row["Anotacion"];

                switch (_orden)
                {
                    case "CODIGO":

                        break;

                    case "ITEM":
                        tomarAtencionPage.SeleccionItem(_concepto, _cantidad);
                        break;

                    default:
                        throw new ArgumentException($"ORDEN NO INGRESADA: {_orden}");
                }

                if (!string.IsNullOrWhiteSpace(_anotacion))
                {
                    tomarAtencionPage.DetalleItem("Agregar anotacion", tomarAtencionPage.Formato("nombre", _concepto), _cantidad, _anotacion);
                }
            }
        }

        [Then("Se procede a {string} la orden")]
        public void ThenSeProcedeALaOrden(string _opcionOrden)
        {
            tomarAtencionPage.OpcionOrden(_opcionOrden);
        }
    }
}

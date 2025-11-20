using AutomatizacionPOM.Pages;
using Microsoft.VisualBasic.FileIO;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class NuevaVentaStepDefinitions
    {
        private IWebDriver driver;
        RegistroVentaPage newSale;

        public NuevaVentaStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.newSale = new RegistroVentaPage(driver);
        }

        [When("el usuario agrega el concepto {string}")]
        public void WhenElUsuarioAgregaElConcepto(string _concepto)
        {
            newSale.SelectConcept(_concepto);
        }

        [When("ingresa la cantidad {string}")]
        public void WhenIngresaLaCantidad(string _cantidad)
        {
            newSale.EnterAmount(_cantidad);
        }

        [When("selecciona igv")]
        public void WhenSeleccionaIgv()
        {
            newSale.ClicIGV();
        }

        [When("selecciona al cliente con documento {string}")]
        public void WhenSeleccionaAlClienteConDocumento(string dni)
        {
            newSale.EnterCustomer(dni);
        }

        [When("selecciona el tipo de comprobante {string}")]
        public void WhenSeleccionaElTipoDeComprobante(string option)
        {
            newSale.SelectTypeDocument(option);
        }

        [When("selecciona el tipo de pago {string}")]
        public void WhenSeleccionaElTipoDePago(string pago)
        {
            newSale.SelectPaymentType(pago);
        }

        [When("selecciona el medio de pago {string}")]
        public void WhenSeleccionaElMedioDePago(string option)
        {
            newSale.PaymentMethod(option);
        }

        [When("ingrese la informacion del pago {string}")]
        public void WhenIngreseLaInformacionDelPago(string value)
        {
            newSale.InformationPayment(value);
        }

        [Then("la venta se guarda correctamente")]
        public void ThenLaVentaSeGuardaCorrectamente()
        {
            newSale.SaveSale();
        }
    }
}

using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatizacionPOM.Pages
{
    public class RegistroVentaPage
    {
        private IWebDriver driver;
        Utilities utilities;
        public RegistroVentaPage(IWebDriver driver)
        {
            this.driver = driver;
            utilities = new Utilities(driver);
        }
        public static readonly By selConceptSelection = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[1]/div[1]/div[1]/registrador-detalles[1]/div[1]/div[1]/selector-concepto-comercial[1]/ng-form[1]/div[1]/div[3]/div[1]/div[1]/span[1]/span[1]/span[1]");
        public static readonly By ConceptAmount = By.Id("cantidad-0");
        public static readonly By IgvActive = By.Id("ventaigv0");
        public static readonly By IdCustomer = By.Id("DocumentoIdentidad");
        public static readonly By TypeDocumentField = By.XPath("//body/div[@id='wrapper']/div[1]/section[1]/div[1]/div[1]/div[1]/form[1]/div[2]/facturacion-venta[1]/form[1]/div[1]/div[2]/div[1]/div[6]/selector-comprobante[1]/div[1]/ng-form[1]/div[1]/div[1]/span[1]/span[1]/span[1]");
        public static readonly By DebitCardButton = By.Id("labelMedioPago-0-18");
        public static readonly By CashPaymentOption = By.Id("radio1");
        public static readonly By PaymentInformation = By.XPath("//div[@class='box box-primary box-solid']//textarea[@id='informacion']");
        public static readonly By SaveSaleButton = By.XPath("//button[normalize-space()='GUARDAR VENTA']");

        public void SelectConcept(string codeconcept)
        {
            utilities.SelectOption(selConceptSelection, codeconcept);
        }
        public void EnterAmount(string amount)
        {
            utilities.ClearAndEnterText(ConceptAmount, amount);
        }

        public void ClicIGV()
        {
            utilities.ClickButton(IgvActive);
        }

        public void EnterCustomer(string dni)
        {
            utilities.ClearAndEnterText(IdCustomer, dni);
            utilities.Enter(IdCustomer);
        }

        public void SelectTypeDocument(string option)
        {
            utilities.SelectOption(TypeDocumentField, option);
        }

        public void SelectPaymentType(string option)
        {
            utilities.ClickButton(CashPaymentOption);
        }

        public void PaymentMethod(string option)
        {
            utilities.ClickButton(DebitCardButton);
        }
        public void InformationPayment(string information)
        {
            utilities.EnterText(PaymentInformation, information);
        }

        public void SaveSale()
        {
            utilities.ClickButton(SaveSaleButton);
        }
    }
}

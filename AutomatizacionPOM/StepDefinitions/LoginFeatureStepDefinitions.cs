using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class LoginFeatureStepDefinitions
    {
        private IWebDriver driver;
        AccessPage accessPage;

        public LoginFeatureStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            accessPage = new AccessPage(driver);
        }

        [Given("el usuario ingresa al ambiente {string}")]
        public void GivenElUsuarioIngresaAlAmbiente(string _ambiente)
        {
            accessPage.OpenToAplicattion(_ambiente);
        }

        [When("el usuario inicia sesión con usuario {string} y contraseña {string}")]
        public void WhenElUsuarioIniciaSesionConUsuarioYContrasena(string _user, string _password)
        {
           accessPage.LoginToApplication(_user, _password);
        }

        [When("accede al módulo {string}")]
        public void WhenAccedeAlModulo(string modulo)
        {
            accessPage.enterModulo(modulo);
        }

        [When("accede al submódulo {string}")]
        public void WhenAccedeAlSubmodulo(string submodulo)
        {
            accessPage.enterSubModulo(submodulo);
        }


    }
}

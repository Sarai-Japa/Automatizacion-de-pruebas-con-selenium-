Feature: NuevaVenta

Background: 
Given el usuario ingresa al ambiente 'https://taller2025-qa.sigesonline.com/'
When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
And accede al módulo 'Venta'
And accede al submódulo 'Nueva Venta'


@RegistrarVenta
Scenario: Registro de una nueva venta con pago al contado
	When el usuario agrega el concepto '400000437'
	And ingresa la cantidad '0'
    And selecciona igv
    And selecciona al cliente con documento '71310154'
    And selecciona el tipo de comprobante 'BOLETA'
    And selecciona el tipo de pago 'Crédito'
    And selecciona el medio de pago 'TDEB'
    And ingrese la informacion del pago 'cancelado'
    Then la venta se guarda correctamente



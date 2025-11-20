Feature: AtencionSinMesa

Background: 
Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
When el usuario inicia sesión con usuario 'admin@tintoymadero.com' y contraseña 'calidad'
And accede al módulo 'Restaurante'
And accede al submódulo 'Atención'
And Se seleciona el tipo de atencion 'Sin mesa'

@ItemDeUnaOrdenAtencionSinMesa
Scenario: Registro de orden con ítem y cantidad positiva
	Given Se ingresa el cliente 'PEDRO'
	And Se selecciona el mozo 'DIEGO EDUARDO CRUZ ORELLANA'
	When Se ingresa las siguientes ordenes:
	| Orden		| Concepto									| Cantidad	| Anotacion			|
	| ITEM		| CARTA 1/4 POLLO A LA BRASA C/PT			| 2			| Sin ensalada		|
	| ITEM		| BEBIDAS 1/2 JARRA DE CHICHA MORADA		| 1			|					|
	
	Then Se procede a 'guardar' la orden

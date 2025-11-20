Feature: PrincipalEgresos
  Validar la visualización y filtrado del listado de egresos en Tesorería y Finanzas

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Tesorería y Finanzas'
    And accede al submódulo 'Ingresos/Egresos'

  @FiltrarEgresos
  Scenario: Filtrar egresos por fecha, pagador y total
    When el usuario aplica el filtro de tipo de operación 'Pagos'
    And selecciona el rango de fechas desde '08/01/22' hasta '07/11/25'
    And hace clic en el botón 'Buscar'
    And filtra por fecha y hora '07/11/2025 07:39:07 a. m.'
    And filtra por pagador 'RUC : 10229679541 : GALVEZ ALVAREZ ROBERTO EDMUNDO'
    And filtra por total '300.00'
    Then el sistema muestra correctamente la lista de egresos en la tabla principal

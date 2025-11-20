Feature: PrincipalGastoB
  Validar la visualización y exportación de gastos en el módulo Gasto - Principal B

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Gasto'
    And accede al submódulo 'Ver'

  @ConsultarGasto
  Scenario: Consultar lista de gastos por rango de fechas
    When el usuario selecciona en Gasto el rango de fechas desde '07/11/2025' hasta '09/11/2025'
    And hace clic en el botón 'Buscar' en Gasto
    Then el sistema muestra correctamente los registros de gastos o el mensaje de vacío

  @ExportarGasto
  Scenario: Exportar la lista de gastos a Excel
    When el usuario selecciona en Gasto el rango de fechas desde '07/11/2025' hasta '09/11/2025'
    And hace clic en el botón 'Exportar Excel' en Gasto
    Then el sistema descarga correctamente el archivo Excel de gastos

  @NuevoGasto
  Scenario: Verificar acceso al registro de un nuevo gasto
    When el usuario hace clic en el botón 'Nuevo Gasto' en Gasto
    Then el sistema muestra el formulario de registro de gasto

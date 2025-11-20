Feature: ExportarEgresos
  Validar la exportación del listado de egresos a Excel desde Tesorería y Finanzas

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Tesorería y Finanzas'
    And accede al submódulo 'Ingresos/Egresos'

  @ExportarEgresosExcel
  Scenario: Exportar listado de egresos a formato Excel
    When el usuario selecciona el rango de fechas desde '2025-11-07' hasta '2025-11-09'
    And hace clic en el botón de exportación Excel
    Then el sistema descarga correctamente el archivo Excel con los datos de egresos

@ModuloReporteGasto
Feature: Reporte de Gastos

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Gasto'
    And accede al submódulo 'Reporte'

  @ReporteGlobal
  Scenario: Generar reporte de gastos global
    When selecciona fecha inicial "18/11/2025"
    And selecciona fecha final "18/11/2025"
    And selecciona el modo "GLOBAL"
    And hace clic en Ver reporte de gastos
    Then el reporte de gastos se muestra correctamente

  @ReporteEstablecimiento
  Scenario: Generar reporte por establecimiento
    When selecciona fecha inicial "18/11/2025"
    And selecciona fecha final "18/11/2025"
    And selecciona el modo "ESTABLECIMIENTO"
    And selecciona el establecimiento "CORPORACION FERRETERA RUDHAYFRE S.A.C"
    And hace clic en Ver reporte de gastos
    Then el reporte de gastos se muestra correctamente

  @ReporteCentroAtencion
  Scenario: Generar reporte por centro de atención
    When selecciona fecha inicial "18/11/2025"
    And selecciona fecha final "18/11/2025"
    And selecciona el modo "CENTRO DE ATENCION"
    And selecciona el establecimiento "CORPORACION FERRETERA RUDHAYFRE S.A.C"
    And selecciona la caja "CA ALMACEN CASTILLO"
    And hace clic en Ver reporte de gastos
    Then el reporte de gastos se muestra correctamente

  @ExportarExcel
  Scenario: Exportar reporte de gastos en Excel
    When selecciona fecha inicial "18/11/2025"
    And selecciona fecha final "18/11/2025"
    And selecciona el modo "GLOBAL"
    And hace clic en Ver reporte de gastos
    And exporta el reporte de gastos en formato "Excel"
    Then el archivo del reporte debe descargarse correctamente

  @ExportarPDF
  Scenario: Exportar reporte de gastos en PDF
    When selecciona fecha inicial "18/11/2025"
    And selecciona fecha final "18/11/2025"
    And selecciona el modo "GLOBAL"
    And hace clic en Ver reporte de gastos
    And exporta el reporte de gastos en formato "PDF"
    Then el archivo del reporte debe descargarse correctamente

  @ExportarWord
  Scenario: Exportar reporte de gastos en Word
    When selecciona fecha inicial "18/11/2025"
    And selecciona fecha final "18/11/2025"
    And selecciona el modo "GLOBAL"
    And hace clic en Ver reporte de gastos
    And exporta el reporte de gastos en formato "Word"
    Then el archivo del reporte debe descargarse correctamente

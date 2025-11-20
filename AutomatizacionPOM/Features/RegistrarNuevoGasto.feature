Feature: RegistrarNuevoGasto
  Validar el registro de un nuevo gasto en el módulo Gasto

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Gasto'
    And accede al submódulo 'Ver'
    And el usuario hace clic en el botón 'Nuevo Gasto' en Gasto

  # =============================================================
  # 🧾 Escenario 1 - Registro válido
  @RegistrarNuevoGasto
  Scenario: Registrar un nuevo gasto con datos válidos
    When el usuario completa el formulario de registro de gasto:
      | Proveedor     | 73582270 |
      | Concepto      | MANTENIMIENTO |
      | Detalle       | Compra de repuestos de oficina |
      | Documento     | NOTA DE GASTO (INTERNA) |
      | Fecha         | 09/11/2025 |
      | Observacion   | Ninguna |
      | Importe       | 250.00 |
      | IGV           | GASTO CON IGV |
    And guarda el gasto
    Then el sistema muestra mensaje de confirmación o inconsistencia


      # =============================================================
  # ⚠️ BI-GAS-12 - Validación de campos obligatorios (importe = 0)
  @ValidacionImporteCero
  Scenario: Validar mensaje cuando el importe es 0
    When el usuario completa el formulario de registro de gasto:
      | Proveedor     | 73582270 |
      | Concepto      | MANTENIMIENTO |
      | Detalle       | Compra de útiles de oficina |
      | Documento     | NOTA DE GASTO (INTERNA) |
      | Fecha         | 09/11/2025 |
      | Observacion   | Ninguna |
      | Importe       | 0.00 |
      | IGV           | GASTO CON IGV |
    And guarda el gasto
    Then el sistema muestra mensaje de inconsistencia con texto "Es necesario que el importe sea mayor a 0."


  # =============================================================
  # ⚠️ BI-GAS-13 - Validación de campos obligatorios (total = 0)
  @ValidacionTotalCero
  Scenario: Validar mensaje cuando el total es 0
    When el usuario completa el formulario de registro de gasto:
      | Proveedor     | 73582270 |
      | Concepto      | MANTENIMIENTO |
      | Detalle       | Compra de útiles |
      | Documento     | NOTA DE GASTO (INTERNA) |
      | Fecha         | 09/11/2025 |
      | Observacion   | Ninguna |
      | Importe       | 0.00 |
      | IGV           | GASTO CON IGV |
    And guarda el gasto
    Then el sistema muestra mensaje de inconsistencia con texto "Es necesario que el total sea mayor a 0."


  # =============================================================
  # ⚠️ BI-GAS-16 - Gasto sin IGV (validación múltiple con campos vacíos)
  @ValidacionCamposObligatorios
  Scenario: Validar mensajes múltiples cuando se dejan varios campos vacíos o en 0
    When el usuario completa el formulario de registro de gasto:
      | Proveedor     |          |
      | Concepto      |          |
      | Detalle       |          |
      | Documento     | NOTA DE GASTO (INTERNA) |
      | Fecha         | 09/11/2025 |
      | Observacion   | Ninguna |
      | Importe       | 800.00 |
      | IGV           | SIN IGV |
    And guarda el gasto
    Then el sistema muestra mensaje de inconsistencia con texto "Es necesario seleccionar un concepto para el gasto."
    And el sistema muestra mensaje de inconsistencia con texto "Es necesario que el importe sea mayor a 0."
    And el sistema muestra mensaje de inconsistencia con texto "Es necesario que el total sea mayor a 0."


  # =============================================================
  # ⚠️ BI-GAS-17 - Valor Límite: Importe extremadamente alto
  @ValidacionImporteExcesivo
  Scenario: Validar comportamiento cuando el importe es excesivamente alto
    When el usuario completa el formulario de registro de gasto:
      | Proveedor     | 73582270 |
      | Concepto      | MANTENIMIENTO |
      | Detalle       | Validar cálculo por valor extremo |
      | Documento     | NOTA DE GASTO (INTERNA) |
      | Fecha         | 09/11/2025 |
      | Observacion   | Ninguna |
      | Importe       | 9999999999999999999999999999999999999999 |
      | IGV           | GASTO CON IGV |
    And guarda el gasto
    Then el sistema muestra mensaje de inconsistencia con texto "El importe excede el valor permitido o genera desbordamiento en el cálculo."


  # =============================================================
  # 💳 BI-GAS-18 - Registro con gasto al crédito (Rápido)
  @GastoCreditoRapido
  Scenario: Registrar un gasto con la opción 'Al Crédito' en modo Rápido
    When el usuario completa el formulario de registro de gasto:
      | Proveedor     | 73582270 |
      | Concepto      | MANTENIMIENTO |
      | Detalle       | Compra a crédito en modo rápido |
      | Documento     | NOTA DE GASTO (INTERNA) |
      | Fecha         | 09/11/2025 |
      | Observacion   | Ninguna |
      | Importe       | 1000.00 |
      | IGV           | GASTO CON IGV |
    And selecciona la opción "AL CRÉDITO" con modo "Rápido"
    And guarda el gasto
    Then el sistema muestra mensaje de confirmación o inconsistencia


  # =============================================================
  # ⚙️ BI-GAS-19 - Registro con gasto al crédito (Configurar)
  @GastoCreditoConfigurar
  Scenario: Registrar un gasto con la opción 'Al Crédito' en modo Configurar
    When el usuario completa el formulario de registro de gasto:
      | Proveedor     | 73582270 |
      | Concepto      | MANTENIMIENTO |
      | Detalle       | Compra a crédito configurada |
      | Documento     | NOTA DE GASTO (INTERNA) |
      | Fecha         | 09/11/2025 |
      | Observacion   | Ninguna |
      | Importe       | 220000 |
      | IGV           | GASTO CON IGV |
    And selecciona la opción "AL CRÉDITO" con modo "Configurar"
    And completa la configuración de crédito con los siguientes datos:
      | Inicial      | 200 |
      | DiaPago      | 09/11/2025 |
    And confirma el financiamiento
    Then el sistema muestra mensaje de inconsistencia con texto "Es necesario seleccionar un concepto para el gasto."

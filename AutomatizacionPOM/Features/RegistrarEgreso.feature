Feature: RegistrarEgreso
  Validar el registro de un egreso desde el panel de Tesorería y Finanzas.

  # -------------------------------------------------------------
  # Este escenario valida que un usuario con acceso al sistema
  # pueda registrar un nuevo egreso correctamente, completando
  # todos los campos obligatorios del formulario.
  # -------------------------------------------------------------

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Tesorería y Finanzas'
    And accede al submódulo 'Ingresos/Egresos'

  @RegistrarEgreso
  Scenario: Registrar un nuevo egreso con datos válidos
    When el usuario hace clic en el botón 'EGRESO'
    And completa los campos del egreso:
      | AutorizadoPor | 73582270                   |
      | Beneficiario  | 45440133                   |
      | Documento     | NOTA DE EGRESO             |
      | Importe       | 300.00                     |
      | Observacion   | Pago por materiales varios |
    And guarda el egreso
    Then el sistema muestra un mensaje de confirmación o inconsistencia

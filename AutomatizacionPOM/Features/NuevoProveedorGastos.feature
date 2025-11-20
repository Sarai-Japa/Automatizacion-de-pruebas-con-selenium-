Feature: NuevoProveedorGastos
  Validar el registro de un nuevo proveedor desde el módulo de Gasto

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Gasto'
    And accede al submódulo 'Ver'
    And el usuario hace clic en el botón 'Nuevo Gasto' en Gasto

  # ============================================================
  # Escenario 1: Apertura del registro de proveedor
  @AbrirFormularioProveedor
  Scenario: Abrir el formulario de registro de proveedor desde Nuevo Gasto
    When el usuario hace clic en el botón '+' para registrar un nuevo proveedor
    Then se muestra el formulario 'REGISTRO DE PROVEEDOR'

  # ============================================================
  # Escenario 2: Validaciones de campos obligatorios
  @ValidacionesProveedor
  Scenario: Verificar mensajes de error por campos vacíos
    When el usuario abre el formulario de registro de proveedor
    And deja todos los campos obligatorios vacíos
    And guarda el proveedor
    Then el sistema muestra mensaje de validación para cada campo obligatorio

  # ============================================================
  # Escenario 3: Validar formato de documento y correo electrónico
  @ValidacionFormatos
  Scenario: Validar formatos incorrectos en los campos de documento y correo
    When el usuario abre el formulario de registro de proveedor
    And ingresa datos con formatos inválidos:
      | TipoDocumento | DOC. NACIONAL DE IDENTIDAD |
      | NroDocumento  | ABC123 |
      | Email         | correo_invalido |
    And guarda el proveedor
    Then el sistema muestra mensajes de error de formato

  # ============================================================
  # Escenario 4: Registro válido de proveedor completo
  @RegistroProveedorValido
  Scenario: Registrar un nuevo proveedor con todos los datos válidos
    When el usuario abre el formulario de registro de proveedor
    And completa el formulario de proveedor con los siguientes datos:
      | TipoDocumento   | DOC. NACIONAL DE IDENTIDAD |
      | NroDocumento    | 25658498                   |
      | ApellidoPaterno | JAPA                       |
      | ApellidoMaterno | ARQUENO                    |
      | Nombres         | SARAI YOLANDA             |
      | Nacionalidad    | PERÚ                       |
      | FechaNacimiento | 09/11/1990                |
      | Sexo            | FEMENINO                   |
      | EstadoCivil     | SOLTERO(A)                 |
      | Email           | saraijp8@gmail.com         |
      | Telefono        | 999888777                  |
      | Pais            | PERÚ                       |
      | Ubigeo          | HUANUCO - LEONCIO PRADO - RUPA RUPA |
      | Detalles        | JR. SAN MARTÍN 123 |
    And guarda el proveedor
    Then el sistema muestra mensaje de confirmación de registro exitoso
# Escenario 5: Validar formato especial en campo Teléfono
@ValidacionTelefono
Scenario: Validar formato de teléfono con caracteres especiales
  When el usuario abre el formulario de registro de proveedor
  And completa el formulario de proveedor con los siguientes datos:
    | TipoDocumento   | DOC. NACIONAL DE IDENTIDAD |
    | NroDocumento    | 45440133 |
    | ApellidoPaterno | JAPA |
    | ApellidoMaterno | MOLINA |
    | Nombres         |  JHON |
    | Nacionalidad    | PERÚ |
    | FechaNacimiento | 09/11/1990 |
    | Sexo            | MASCULINO |
    | EstadoCivil     | CASADO |
    | Email           | molinahjhon@gmail.com |
    | Telefono        | #####SSFF |
    | Pais            | PERÚ |
    | Ubigeo          | HUANUCO - LEONCIO PRADO - RUPA RUPA |
    | Detalles        | JR. SAN MARTÍN 123 |
  And guarda el proveedor
  Then el sistema permite guardar el registro o muestra advertencia informativa


  # ============================================================
  # Escenario 6: Validar aceptación de correos con diferentes dominios
  @ValidacionCorreos
  Scenario Outline: Validar que el sistema acepte diferentes dominios de correo electrónico
    When el usuario abre el formulario de registro de proveedor
    And ingresa los siguientes datos mínimos:
      | TipoDocumento | DOC. NACIONAL DE IDENTIDAD |
      | NroDocumento  | 73582270 |
      | Email         | sarajapa8@outlook.com  |
      | ApellidoPaterno | JAPA |
      | ApellidoMaterno | ARQUEÑO |
      | Nombres | SARAI YOLANDA |
    And guarda el proveedor
    Then el sistema permite guardar el registro correctamente

    Examples:
      | Correo                     |
      | sarajapa8@hotmail.com      |
      | sarajapa8@outlook.com      |
      | sarajapa8@l.hotmail.com    |
      | sarajapa8@gmail.com        |
      | sarajapa8@yahoo.es         |

 # ============================================================
# Escenario 7: Verificar que se pueda seguir editando después de presionar ENTER
@ValidacionEnterDocumento
Scenario: Validar que tras ingresar el número de documento y presionar ENTER se puedan editar otros campos
  When el usuario abre el formulario de registro de proveedor
  And completa el formulario de proveedor con los siguientes datos:
    | TipoDocumento   | DOC. NACIONAL DE IDENTIDAD |
    | NroDocumento    | 73582270                   |
  And presiona la tecla ENTER después de ingresar el número de documento
  Then el sistema muestra que los demás campos siguen habilitados para edición

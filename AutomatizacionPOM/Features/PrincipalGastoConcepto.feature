@ModuloConceptoGasto
Feature: Gestión de Conceptos de Gasto

  Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
    And accede al módulo 'Gasto'
    And accede al submódulo 'Concepto'

  @Escenario1CrearNuevaFamilia
  Scenario: Crear nueva familia desde botón +
    When hace clic en el botón Nuevo Concepto
    And crea una nueva familia "ABARROTES" con sufijo "ABRRTS" y tipo "SERVICIO"
    Then la tabla muestra hasta 25 registros

  @Escenario2CrearConFamiliaExistente
  Scenario: Crear un nuevo gasto usando familia existente
    When hace clic en el botón Nuevo Concepto
    And crea un concepto usando familia existente "INTERES" y sufijo "INTT"
    Then la tabla muestra hasta 25 registros

  @EditarConcepto
  Scenario: Editar concepto existente
    When hace clic en editar la fila 1 de conceptos
    And cambia el concepto básico a INTERES y guarda
    Then la tabla cambia a la página siguiente

  @VerFilas
  Scenario: Cambiar la cantidad de filas visibles
    When selecciona ver "25" filas en conceptos
    Then la tabla muestra hasta 25 registros

  @BuscarConcepto
  Scenario: Buscar un concepto por nombre
    When busca el concepto "DESCUENTO"
    Then se muestran resultados relacionados con "DESCUENTO"

  @ExportarExcel
  Scenario: Exportar listado de conceptos
    When hace clic en Exportar Excel en conceptos
    Then se descarga correctamente el archivo Excel de conceptos

  @Paginacion
  Scenario: Navegar entre páginas de conceptos
    When navega a la página siguiente en conceptos
    Then la tabla cambia a la página siguiente

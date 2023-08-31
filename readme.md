# Guía de Configuración y Uso

Esta guía proporciona ayuda para configurar y utilizar el proyecto "SegurosApp". Se recomienda seguir estos pasos en orden para lograr una experiencia fluida.

## Pasos de Configuración

### 1. Configuración de la Base de Datos y Datos de Seguros

- Ejecutar el script `schema-seguros.sql`.
  Este script creará la base de datos y cargará los datos de seguros iniciales.

### 2. Ejecutar el backend .Net Core

- Abrir la solución del proyecto backend en Visual Studio u otro IDE compatible.
- Compilar y ejecutar el proyecto backend.

### 3. Ejecutar el frontend Angular

- Abrir la carpeta del proyecto frontend en la terminal.
- Ejecutar el comando `npm install` para instalar las dependencias.
- Ejecutar el comando `ng serve` para iniciar el servidor de desarrollo.

## Uso del Sistema

### 1. Agregar datos de clientes y subir el documento

- Acceder a la pestaña "Agregar Cliente" en la barra lateral.
- Completar el formulario con la información del cliente y dar click en "Crear".
- En el mismo formulario, se puede utilizar el botón de "Subir Documento" para seleccionar y subir el archivo `clientes.xlsx`.

### 2. Editar o eliminar un seguro

- En la ventana principal, se encuentran los botones de "Editar" o "Eliminar".
- Realizar las modificaciones necesarias y confirmar la edición o eliminación.

### 3. Asignar seguros a clientes

- Acceder a la pestaña "Asignar Seguro".
- Utilizar el panel de expansión para seleccionar un cliente.
- Marcar los checkbox junto a los seguros que se desean asignar a ese cliente.

### 4. Finalizar la asignación

- Asegurarse de revisar todas las asignaciones de seguros.
- Confirmar las asignaciones y cerrar el panel de expansión.

### 5. Editar o eliminar un cliente

- En el mismo panel de expansión de un cliente, se encuentran los botones de "Editar" o "Eliminar" como iconos de Material Design.
- Realizar las modificaciones necesarias y confirma la edición o eliminación.









-- Creación de la BD
CREATE DATABASE Seguro;


USE Seguro
CREATE TABLE Seguro (
    id INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(255) NOT NULL,
	codigo VARCHAR(50) NOT NULL,
    suma money NOT NULL,
    prima money NOT NULL
);

CREATE TABLE Cliente (
    id INT PRIMARY KEY IDENTITY,
    cedula VARCHAR(10) NOT NULL,
    nombre VARCHAR(255) NOT NULL,
    telefono VARCHAR(10) NOT NULL,
    edad INT NOT NULL
);

CREATE TABLE ClienteSeguro (
    clienteid INT NOT NULL,
    seguroid INT NOT NULL,
	PRIMARY KEY (clienteid, seguroid),
    CONSTRAINT FK_Cliente_ID FOREIGN KEY (clienteid) REFERENCES Cliente(id),
    CONSTRAINT FK_Seguro_ID FOREIGN KEY (seguroid) REFERENCES Seguro(id)
);

--Inserción de datos de seguro
use Seguro
insert into Seguro(nombre, codigo, suma, prima) values
('Chubb Seguros Ecuador', 'CSE01', 250000, 3500),
('Prueba Seguros Ecuador', 'PSE01', 250000, 3500),
('Criss Seguros Ecuador', 'CSE01', 250000, 3500);
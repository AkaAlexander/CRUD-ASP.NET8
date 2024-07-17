CREATE TABLE Usuario (
    Id INT PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Username NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50),
    Website NVARCHAR(255)
);

CREATE TABLE Direccion (
    UserId INT PRIMARY KEY,
    Street NVARCHAR(255),
    Suite NVARCHAR(255),
    City NVARCHAR(255),
    Zipcode NVARCHAR(20),
    FOREIGN KEY (UserId) REFERENCES Usuario(Id)
);

CREATE TABLE Geo (
    DireccionId INT PRIMARY KEY,
    Lat DECIMAL(10, 7),
    Lng DECIMAL(10, 7),
    FOREIGN KEY (DireccionId) REFERENCES Direccion(UserId)
);

CREATE TABLE Compania (
    UserId INT PRIMARY KEY,
    Name NVARCHAR(255),
    CatchPhrase NVARCHAR(255),
    Bs NVARCHAR(255),
    FOREIGN KEY (UserId) REFERENCES Usuario(Id)
);
CREATE DATABASE DatabaseDropYourComplain
USE DatabaseDropYourComplain
SELECT * FROM AddComplainInDatabaseTable
CREATE TABLE AddComplainInDatabaseTable
(
Id INT IDENTITY (1,1) PRIMARY KEY,
ComplainCategory VARCHAR (25),
ComplainaArea VARCHAR (25),
RoadNumber VARCHAR (25),
ComplainDeatils VARCHAR (MAX),
Photo VARCHAR (MAX),
ComplainerName VARCHAR (25),
Email VARCHAR (25),
ContractNumber VARCHAR (11),
GiveStatus VARCHAR(25)

 )
 
 CREATE TABLE Login
 (
 Id INT IDENTITY (1,1),
 UserName VARCHAR(25),
 Password VARCHAR(25)
 
 )
USE Tutorica;

create table items (
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	firstname NVARCHAR(60) NOT NULL,
	lastname NVARCHAR(60) NOT NULL,
	name NVARCHAR(60) NOT NULL,
	phonenumber VARCHAR(40) NOT NULL,
	email VARCHAR(50) NOT NULL,
	description TEXT NOT NULL,
	years INT NOT NULL,
	investment_usd DECIMAL(12,2) NOT NULL,
	course DECIMAL(5,2) NOT NULL,
	investment_uah DECIMAL(25,4) NOT NULL
);
USE Tutorica;

create table items (
	Id INT,
	firstname NVARCHAR(60),
	lastname NVARCHAR(60),
	Enterprisename NVARCHAR(250),
	phonenumber VARCHAR(40),
	email VARCHAR(50),
	entdescription TEXT,
	age INT,
	annual_turnover_usd NUMERIC(12,2),
	course NUMERIC(5,2),
	annual_turnover_uah NUMERIC(20,4)
);


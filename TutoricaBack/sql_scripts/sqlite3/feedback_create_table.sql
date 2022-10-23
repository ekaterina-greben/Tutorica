USE Tutorica;

create table feedback (
	Id INT IDENTITY(1,1), PRIMARY KEY,
	email TEXT,
	msgtext TEXT
);
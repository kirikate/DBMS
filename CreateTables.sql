USE PizzaPlace

CREATE TABLE Users(
	id BIGINT PRIMARY KEY IDENTITY,
	first_name NCHAR(20) NOT NULL,
	last_name NCHAR(20) NOT NULL,
	password BIGINT NOT NULL,
	email NCHAR(100) NOT NULL UNIQUE,
	role CHAR(3) DEFAULT('usr') NOT NULL,
	phone CHAR(9) UNIQUE
)

CREATE TABLE Addresses(
	id BIGINT PRIMARY KEY IDENTITY,
	userId BIGINT REFERENCES Users(id),
	adress NVARCHAR(200) NOT NULL,
	entrance NVARCHAR(10),
	number NCHAR(10)
)

CREATE TABLE Review(
	id BIGINT PRIMARY KEY IDENTITY,
	userId BIGINT REFERENCES Users(id) NOT NULL,
	date DATETIME NOT NULL,
	dateOfUpdate DATETIME,
	text NVARCHAR(1000),
	grade INT DEFAULT(5) NOT NULL
)

CREATE TABLE Products(
	id BIGINT PRIMARY KEY IDENTITY,
	name NCHAR(30) NOT NULL,
	type NCHAR(30) NOT NULL,
	price MONEY NOT NULL DEFAULT(1),
	size CHAR(3),
	ingridients NCHAR(100)
)

CREATE INDEX ProductsByType ON Products(type)

CREATE TABLE Coupons(
	id BIGINT PRIMARY KEY IDENTITY,
	number INT NOT NULL,
	price MONEY NOT NULL,
	dateOfStart DATE NOT NULL,
	dateOfExpiration DATE
)

CREATE INDEX CouponsByNumber ON Coupons(number) INCLUDE (dateOfStart)

CREATE TABLE Orders(
	id BIGINT PRIMARY KEY IDENTITY,
	userId BIGINT REFERENCES Users(id) NOT NULL,
	dateOfOrder DATETIME NOT NULL,
	dateOfDelivery DATETIME,
	price INT CHECK(price >=0),
	addressId BIGINT REFERENCES Addresses(id)
)

CREATE INDEX OrdersByUsers ON Orders(userId)

CREATE INDEX AddressesByUsers ON Addresses(userId)

CREATE TABLE ProductsToCoupons(
	couponId BIGINT REFERENCES Coupons(id),
	productId BIGINT REFERENCES Products(id),
	[count] INT DEFAULT(1) NOT NULL
)

CREATE INDEX ProductsInCoupons ON ProductsToCoupons(couponId)

CREATE TABLE ProductsToOrders(
	orderId BIGINT REFERENCES Orders(id),
	productId BIGINT REFERENCES Products(id),
	[count] INT DEFAULT(1) NOT NULL
)
CREATE INDEX ProductsInOrders ON ProductsToOrders(orderId)

CREATE TABLE CouponsToOrders(
	orderId BIGINT REFERENCES Orders(id),
	couponId BIGINT REFERENCES Coupons(id),
	[count] INT DEFAULT(1)
)
CREATE INDEX CouponsInOrders ON CouponsToOrders(orderId)

CREATE TABLE ProductsToUserCart(
	userId BIGINT REFERENCES Users(id),
	productId BIGINT REFERENCES Products(id),
	[count] INT DEFAULT(1) NOT NULL
)

CREATE TABLE CouponsToUserCart(
	userId BIGINT REFERENCES Users(id),
	couponId BIGINT REFERENCES Coupons(id),
	[count] INT DEFAULT(1) NOT NULL
)

CREATE INDEX CouponsInUserCart ON CouponsToUserCart(userId)
CREATE INDEX ProductsInUserCart ON ProductsToUserCart(userId)
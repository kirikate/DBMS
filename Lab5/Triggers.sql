--BEGIN TRANSACTION insertIntoGoods
--	INSERT INTO Goods(price, size, productId) VALUES (1, N'RRR', 10);

--Rollback TRANSACTION insertIntoGoods;
--THROW 50001,'Attempt to insert null value in [Phone Number] is not allowed',1

--CREATE TRIGGER checkSizeOfGood
--ON Goods
--INSTEAD OF INSERT
--AS
--if (SELECT size FROM inserted) NOT IN ('S', 'M', 'L')
--	THROW 50001,'Attempt to insert not (S, M, L) value in [Goods.size] is not allowed',1
--ELSE
--	INSERT INTO Goods (price, size, productId) SELECT price, size, productId FROM inserted


--INSERT INTO Goods(price, size, productId) VALUES (1, N'RR', 10);
--SELECT * FROM Goods;

--CREATE TRIGGER checkSizeOfGood
--ON Goods
--INSTEAD OF INSERT
--AS
--if (SELECT size FROM inserted) NOT IN ('S', 'M', 'L')
--	THROW 50001,'Attempt to insert not (S, M, L) value in [Goods.size] is not allowed',1
--ELSE
--	INSERT INTO Goods (price, size, productId) SELECT price, size, productId FROM inserted

--CREATE TRIGGER checkCoupon ON CouponsToOrders
--INSTEAD OF INSERT
--AS
--if (SELECT dateOfExpiration FROM Coupons WHERE id = (SELECT couponId FROM inserted)) >= GETDATE()
-- --	DELETE FROM Orders WHERE id = (SELECT orderId FROM inserted);
--	THROW 50002,'Attempt to add an expired coupon to order', 1
--ELSE
--INSERT INTO CouponsToOrders(orderId, couponId, [count]) SELECT orderId, couponId, [count] FROM inserted



--INSERT INTO CouponsToOrders(orderId, couponId, [count]) VALUES (8, 5, 1)

--CREATE TRIGGER countPrices ON GoodsToOrders
--AFTER INSERT
--AS
--DECLARE @priceOfIns MONEY
--SET @priceOfIns = (SELECT price FROM Goods WHERE id = (SELECT productId FROM inserted)) * (SELECT [count] FROM inserted)
--UPDATE Orders 
--SET price = price + @priceOfIns

ENABLE TRIGGER countPrices ON GoodsToOrders
SELECT * FROM Orders
INSERT INTO GoodsToOrders(orderId, productId, [count]) VALUES (8, 5, 1)
SELECT * FROM Orders

--CREATE TRIGGER addToCount ON GoodsToOrders
--INSTEAD OF INSERT
--AS
--if EXISTS (SELECT orderId, productId FROM GoodsToOrders WHERE orderId = (SELECT orderId FROM inserted) AND productId = (SELECT productId FROM inserted))
--	UPDATE GoodsToOrders 
--	SET [count] = [count] + (SELECT [count] FROM INSERTED)
--	WHERE orderId = (SELECT orderId FROM inserted) AND productId = (SELECT productId FROM inserted)
--ELSE
--	INSERT INTO GoodsToOrders(orderId, productId, [count]) (SELECT orderId, productId, [count] FROM inserted)

--SELECT * FROM GoodsToOrders
--INSERT INTO GoodsToOrders(orderId, productId, [count]) VALUES (8,5,2)
--SELECT * FROM GoodsToOrders
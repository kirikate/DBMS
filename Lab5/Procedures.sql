--USE PizzaPlace
--CREATE PROCEDURE GetUserOrders
--@userId INT
--AS
--BEGIN
--SELECT * FROM Orders 
--WHERE Orders.userId = @userId
--END;

--EXEC GetUserOrders @usr = 65

--CREATE PROCEDURE GetOrderProducts
--@ordr BIGINT
--AS
--BEGIN
--SELECT Products.[name], Goods.price FROM GoodsToOrders
--JOIN Goods ON Goods.id = GoodsToOrders.productId
--JOIN Products ON Goods.productId = Products.id
--WHERE GoodsToOrders.orderId = @ordr
--END;

EXEC GetOrderProducts @ordr = 14

--CREATE PROCEDURE GetOrderCoupons 
--@ordr BIGINT
--AS
--BEGIN
--SELECT Coupons.number, CouponsToOrders.[count], Coupons.price, Coupons.price * CouponsToOrders.[count]
--FROM CouponsToOrders
--JOIN Coupons ON Coupons.id = CouponsToOrders.orderId
--WHERE CouponsToOrders.orderId = @ordr
--END;

--EXEC GetOrderCoupons @ordr = 14

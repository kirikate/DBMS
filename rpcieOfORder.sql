CREATE PROCEDURE countPrice
@orderId BIGINT AS
BEGIN
	DECLARE @res MONEY
	SET @res = SELECT SUM(Coupons.price) FROM CouponsToOrders
	JOIN Coupons ON CouponsToOrders.couponId = Coupons.Id 
	WHERE CouponsToOrders.orderId = orderid
	RETURN @res
END;


/*UPDATE Orders
SET price = SUM(Coupons.price) FROM CouponsToOrders
	JOIN Coupons ON CouponsToOrders.couponId = Coupons.Id 
	WHERE CouponsToOrders.orderId = Orders.id

UPDATE Orders
SET price = Orders.price + SUM(Goods.Price) FROM GoodsToOrders
	JOIN Goods ON GoodsToOrders.productId = Goods.Id 
	WHERE GoodsToOrders.orderId = Orders.id

SET price = SUM(Goods.Price) 
	FROM GoodsToOrders 
	JOIN Goods ON GoodsToOrders.productId = Goods.id
	WHERE GoodsToOrders.orderId = Orders.Id*/


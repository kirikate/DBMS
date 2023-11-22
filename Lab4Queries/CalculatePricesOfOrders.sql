UPDATE Orders
SET price = 
(SELECT  SUM(ISNULL(Coupons.price,0) * ISNULL(CouponsToOrders.[count], 0)
+ ISNULL(Goods.price,0) * ISNULL(GoodsToOrders.[count],0))
FROM Orders AS O
LEFT JOIN CouponsToOrders ON Orders.Id = CouponsToOrders.orderId
LEFT JOIN Coupons ON CouponsToOrders.couponId = Coupons.id
LEFT JOIN GoodsToOrders ON GoodsToOrders.orderId = Orders.Id
LEFT JOIN Goods ON GoodsToOrders.productId = Goods.id
WHERE Orders.id = O.id
)

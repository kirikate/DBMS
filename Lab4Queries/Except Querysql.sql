SELECT Coupons.id, Coupons.number,Coupons.price, SUM(Goods.price * GoodsToCoupons.[count]) as 'Product sum $'
FROM Coupons 
JOIN GoodsToCoupons ON Coupons.id = GoodsToCoupons.couponId
JOIN Goods ON Goods.id = GoodsToCoupons.productId
GROUP BY Coupons.id, Coupons.number, Coupons.price

EXCEPT
SELECT Coupons.id, Coupons.number, Coupons.price, SUM(Goods.price * GoodsToCoupons.[count]) as 'Product sum $'
FROM Coupons 
JOIN GoodsToCoupons ON Coupons.id = GoodsToCoupons.couponId
JOIN Goods ON Goods.id = GoodsToCoupons.productId
WHERE EXISTS(SELECT * FROM CouponsToOrders WHERE couponId = Coupons.id)
GROUP BY Coupons.id, Coupons.number, Coupons.price
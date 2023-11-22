SELECT Coupons.Id, Coupons.number, (Products.Name + Goods.size) AS 'Product', GoodsToCoupons.[count]
FROM GoodsToCoupons
JOIN Coupons ON GoodsToCoupons.couponId = Coupons.id
JOIN Goods ON GoodsToCoupons.productId = Goods.id
JOIN Products ON Goods.productId = Products.id
GROUP BY Coupons.id, Coupons.number, GoodsToCoupons.count, Products.name, Goods.size

--count prices

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

--coupon stats
SELECT Coupons.id, number, Coupons.price, SUM(CouponsToOrders.[count]) as 'number of selled coupons', 
Coupons.price * SUM(CouponsToOrders.[count]) as 'money by coupon', COUNT(Orders.id) as 'Orders with coupon', 
SUM(Orders.price) as 'Sum of orders $', AVG(Orders.price) as 'Average order $'
FROM Coupons
JOIN CouponsToOrders ON CouponsToOrders.couponId = Coupons.id
JOIN Orders ON CouponsToOrders.orderId = Orders.id
WHERE EXISTS( 
	SELECT * 
	FROM CouponsToOrders 
	WHERE CouponsToOrders.couponId = Coupons.id
)
GROUP BY Coupons.id, number, Coupons.price
ORDER BY Coupons.price * SUM(CouponsToOrders.[count]) DESC

--except
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

-- orders and users
SELECT Orders.Id, Orders.dateOfOrder, Orders.dateOfDelivery, Orders.price, Users.Id As 'UsersId', Users.first_name, Users.last_name
FROM Orders
JOIN Users ON Users.id = Orders.userId

--	Заказы с общей потраченной суммой пользователя
SELECT Orders.id, Orders.dateOfOrder, Users.first_name + Users.last_name as 'name of user',
Orders.price, SUM (Orders.price) OVER(PARTITION BY Orders.userId) as [Total orders $]
FROM Orders
JOIN Users ON Users.id = Orders.userId
ORDER BY [Total orders $] DESC

--query for goods
SELECT Goods.id, Products.name, Goods.size, Goods.price
FROM Goods
JOIN Products ON Goods.productId = Products.id
WHERE Goods.price BETWEEN 8 AND 15

--readable coupons
SELECT Coupons.Id, Coupons.number, (Products.Name + Goods.size) AS 'Product', GoodsToCoupons.[count]
FROM GoodsToCoupons
JOIN Coupons ON GoodsToCoupons.couponId = Coupons.id
JOIN Goods ON GoodsToCoupons.productId = Goods.id
JOIN Products ON Goods.productId = Products.id
GROUP BY Coupons.id, Coupons.number, GoodsToCoupons.count, Products.name, Goods.size


--readable goods
SELECT Products.name, Products.type, Goods.size, Goods.price
FROM Goods
JOIN Products ON Goods.productId = Products.id

--сумма заказов по датам
SELECT DATEFROMPARTS(YEAR(Orders.dateOfOrder), MONTH(Orders.dateOfOrder), DAY(Orders.dateOfOrder)) AS 'day', 
SUM(Orders.price) as 'money', COUNT(*) as 'numb of orders'
FROM Orders
GROUP BY DATEFROMPARTS(YEAR(Orders.dateOfOrder), MONTH(Orders.dateOfOrder), DAY(Orders.dateOfOrder))
ORDER BY SUM(Orders.price) DESC

--сумма заказов по дням недели
SELECT DATENAME(WEEKDAY, Orders.price) AS 'day of week', 
SUM(Orders.price) as 'money', COUNT(*) as 'numb of orders'
FROM Orders
GROUP BY DATENAME(WEEKDAY, Orders.price)
ORDER BY SUM(Orders.price) DESC

----данные купонов
--SELECT Coupons.Id, Coupons.number, (Products.Name + Goods.size) AS 'Product', GoodsToCoupons.[count]
--FROM GoodsToCoupons
--JOIN Coupons ON GoodsToCoupons.couponId = Coupons.id
--JOIN Goods ON GoodsToCoupons.productId = Goods.id
--JOIN Products ON Goods.productId = Products.id
--GROUP BY Coupons.id, Coupons.number, GoodsToCoupons.count, Products.name, Goods.size


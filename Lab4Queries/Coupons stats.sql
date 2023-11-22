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
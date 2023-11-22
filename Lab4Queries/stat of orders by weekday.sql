--сумма заказов по дням недели
SELECT DATENAME(WEEKDAY, Orders.price) AS 'day of week', 
SUM(Orders.price) as 'money', COUNT(*) as 'numb of orders'
FROM Orders
GROUP BY DATENAME(WEEKDAY, Orders.price)
ORDER BY SUM(Orders.price) DESC
--сумма заказов по датам
SELECT DATEFROMPARTS(YEAR(Orders.dateOfOrder), MONTH(Orders.dateOfOrder), DAY(Orders.dateOfOrder)) AS 'day', 
SUM(Orders.price) as 'money', COUNT(*) as 'numb of orders'
FROM Orders
GROUP BY DATEFROMPARTS(YEAR(Orders.dateOfOrder), MONTH(Orders.dateOfOrder), DAY(Orders.dateOfOrder))
ORDER BY SUM(Orders.price) DESC


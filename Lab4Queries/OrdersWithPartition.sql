--	Заказы с общей потраченной суммой пользователя
SELECT Orders.id, Orders.dateOfOrder, Users.first_name + Users.last_name as 'name of user',
Orders.price, SUM (Orders.price) OVER(PARTITION BY Orders.userId) as [Total orders $]
FROM Orders
JOIN Users ON Users.id = Orders.userId
ORDER BY [Total orders $] DESC
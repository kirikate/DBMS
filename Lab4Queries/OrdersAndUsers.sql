SELECT Orders.Id, Orders.dateOfOrder, Orders.dateOfDelivery, Orders.price, Users.Id As 'UsersId', Users.first_name, Users.last_name
FROM Orders
JOIN Users ON Users.id = Orders.userId
SELECT Addresses.id, Addresses.adress, Users.Id AS 'User id', Users.first_name, Users.last_name
FROM Addresses
JOIN Users ON Addresses.userId = Users.Id
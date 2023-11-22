SELECT Goods.id, Products.name, Goods.size, Goods.price
FROM Goods
JOIN Products ON Goods.productId = Products.id
WHERE Goods.price BETWEEN 8 AND 15
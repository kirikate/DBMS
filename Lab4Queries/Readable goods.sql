SELECT Products.name, Products.type, Goods.size, Goods.price
FROM Goods
JOIN Products ON Goods.productId = Products.id
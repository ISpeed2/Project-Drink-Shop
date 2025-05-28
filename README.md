# Project
Проектирование системы “Онлайн-магазин напитков”

Сопроводительное описание

**Проблема:** Современные потребители ожидают удобного и быстрого доступа к широкому ассортименту товаров. Онлайн-магазин напитков решает проблему предоставления удобного способа покупки напитков из дома или с мобильного устройства, а также обеспечивает возможность доставки.

## Ключевые функции:

**Каталог напитков:** Отображение и фильтрация товаров по категориям, брендам, ценам и другим параметрам.

**Корзина:** Добавление, изменение и удаление товаров из корзины.

**Оформление заказа:** Ввод данных для доставки, выбор способа оплаты и подтверждение заказа.

**Управление пользователями:** Регистрация, авторизация и управление профилем пользователя.

**Управление заказами:** Просмотр истории заказов, отслеживание статуса заказа.

**Администрирование (для администратора):** Добавление/удаление/изменение товаров, управление категориями и брендами, обработка заказов, управление пользователями, анализ продаж.


## Выбор диаграмм и обоснование:

**Диаграмма классов:** Эта диаграмма позволяет визуализировать структуру приложения, показывая основные классы, их атрибуты (данные) и методы (действия), а также отношения между ними (например, наследование, ассоциация). Это необходимо для понимания структуры данных и базовых компонентов системы.

**Диаграмма последовательностей:** Эта диаграмма иллюстрирует взаимодействие объектов во времени для выполнения конкретных сценариев, таких как “Оформление заказа” или “Добавление товара в корзину”. Она помогает понять порядок выполнения операций и взаимодействие между различными компонентами системы.

**Диаграмма вариантов использования:** Эта диаграмма описывает взаимодействие пользователей (клиентов и администраторов) с системой, показывая основные сценарии использования, такие как “Просмотр каталога”, “Оформление заказа” или “Управление товарами”. Она помогает определить функциональные требования к системе с точки зрения пользователей.


## Диаграммы

**Диаграмма классов (Class Diagram)**

**Название:** Диаграмма классов для онлайн-магазина напитков

**Описание:** Описывает структуру системы, показывая основные классы, их атрибуты, методы и взаимосвязи (ассоциация, наследование). Представляет собой статическую структуру приложения.

![Проект (20250522023855)](https://github.com/user-attachments/assets/51f01c7e-9559-49cd-b067-a0f497240e80)



**Диаграмма последовательностей (Sequence Diagram)**

**Название:** Диаграмма последовательности для сценария “Оформление заказа”

**Описание:** Показывает взаимодействие объектов (Customer, ShoppingCart, Order, OrderItem) при оформлении заказа.

![image_2025-05-22_14-28-08](https://github.com/user-attachments/assets/a83fb8b7-df7b-4de7-85c8-5fefe7a85c13)



**Диаграмма вариантов использования (Use Case Diagram)**

**Название:** Диаграмма вариантов использования для онлайн-магазина напитков

**Описание:** Определяет взаимодействие пользователей (Клиент, Администратор) с системой и описывает основные сценарии использования (варианты использования).

![image_2025-05-22_14-31-43](https://github.com/user-attachments/assets/1048df38-f093-4a1f-8741-12c01d94d6b9)



# Этап 2: Создание базы данных для онлайн-магазина напитков #

## 1. Проектирование структуры базы данных ##
**Описание:** База данных спроектирована с учетом требований приложения и включает в себя таблицы для хранения информации о пользователях, продуктах, категориях, брендах, заказах и т.д. Используются связи один-к-одному, один-ко-многим и многие-ко-многим для обеспечения целостности и эффективности хранения данных.

## 2. Создание базы данных (PostgreSQL) ##

```//
-- Скрипт для создания базы данных и таблиц для онлайн-магазина напитков (PostgreSQL)

-- Удаление таблиц (если существуют) - для пересоздания БД
DROP TABLE IF EXISTS ProductReviews;
DROP TABLE IF EXISTS OrderItems;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS ProductInventory;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Brands;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Customers;


-- Создание таблицы Customers
CREATE TABLE Customers (
    CustomerId SERIAL PRIMARY KEY,
    FirstName VARCHAR(255),
    LastName VARCHAR(255),
    Username VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Address VARCHAR(255),
    Phone VARCHAR(20),
    IsAdmin BOOLEAN DEFAULT FALSE
);

-- Создание таблицы Categories
CREATE TABLE Categories (
    CategoryId SERIAL PRIMARY KEY,
    Name VARCHAR(255) UNIQUE NOT NULL,
    Description TEXT
);

-- Создание таблицы Brands
CREATE TABLE Brands (
    BrandId SERIAL PRIMARY KEY,
    Name VARCHAR(255) UNIQUE NOT NULL,
    Description TEXT
);

-- Создание таблицы Products
CREATE TABLE Products (
    ProductId SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    ImageUrl VARCHAR(255),
    CategoryId INT,
    BrandId INT,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (BrandId) REFERENCES Brands(BrandId)
);

-- Создание таблицы Orders
CREATE TABLE Orders (
    OrderId SERIAL PRIMARY KEY,
    CustomerId INT,
    OrderDate TIMESTAMP WITHOUT TIME ZONE DEFAULT (NOW() AT TIME ZONE 'utc'),
    ShippingAddress VARCHAR(255),
    TotalAmount DECIMAL(10, 2) NOT NULL,
    OrderStatus VARCHAR(50) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

-- Создание таблицы OrderItems
CREATE TABLE OrderItems (
    OrderItemId SERIAL PRIMARY KEY,
    OrderId INT,
    ProductId INT,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,  -- Price at time of order
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Создание таблицы ProductReviews
CREATE TABLE ProductReviews (
    ReviewId SERIAL PRIMARY KEY,
    ProductId INT,
    CustomerId INT,
    Rating INT NOT NULL,  -- 1-5 stars
    Comment TEXT,
    ReviewDate TIMESTAMP WITHOUT TIME ZONE DEFAULT (NOW() AT TIME ZONE 'utc'),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

-- Создание таблицы ProductInventory
CREATE TABLE ProductInventory (
    InventoryId SERIAL PRIMARY KEY,
    ProductId INT UNIQUE,  -- One-to-one relationship
    QuantityInStock INT NOT NULL,
    LastStockUpdate TIMESTAMP WITHOUT TIME ZONE DEFAULT (NOW() AT TIME ZONE 'utc'),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Индексы для ускорения запросов (опционально, но рекомендуется)
CREATE INDEX idx_products_categoryid ON Products (CategoryId);
CREATE INDEX idx_products_brandid ON Products (BrandId);
CREATE INDEX idx_orders_customerid ON Orders (CustomerId);
CREATE INDEX idx_orderitems_orderid ON OrderItems (OrderId);
CREATE INDEX idx_orderitems_productid ON OrderItems (ProductId);
CREATE INDEX idx_productreviews_productid ON ProductReviews (ProductId);
CREATE INDEX idx_productinventory_productid ON ProductInventory (ProductId);


-- Заполнение начальными данными (пример)

-- Категории
INSERT INTO Categories (Name, Description) VALUES
('Вода', 'Простая и минеральная вода'),
('Соки', 'Фруктовые и овощные соки'),
('Газированные напитки', 'Лимонады и газированная вода'),
('Энергетики', 'Энергетические напитки');

-- Бренды
INSERT INTO Brands (Name, Description) VALUES
('Coca-Cola', 'Напитки Coca-Cola'),
('Pepsi', 'Напитки Pepsi'),
('Borjomi', 'Минеральная вода Borjomi'),
('Sandora', 'Соки Sandora');

-- Продукты
INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId, BrandId) VALUES
('Coca-Cola 0.5л', 'Газированный напиток Coca-Cola', 25.00, 'coca-cola.jpg', 3, 1),
('Pepsi 0.5л', 'Газированный напиток Pepsi', 24.00, 'pepsi.jpg', 3, 2),
('Borjomi 0.5л', 'Минеральная вода Borjomi', 40.00, 'borjomi.jpg', 1, 3),
('Сок Sandora Яблоко 1л', 'Яблочный сок Sandora', 35.00, 'sandora-apple.jpg', 2, 4);

-- Добавляем инвентарь для продуктов
INSERT INTO ProductInventory (ProductId, QuantityInStock) VALUES
(1, 100),  -- Coca-Cola
(2, 150),  -- Pepsi
(3, 75),   -- Borjomi
(4, 120);  -- Sandora Apple Juice


-- Пользователи
INSERT INTO Customers (FirstName, LastName, Username, PasswordHash, Email, Address, Phone, IsAdmin) VALUES
('John', 'Doe', 'johndoe', 'hashed_password_1', 'john.doe@example.com', '123 Main St', '555-1234', FALSE),
('Jane', 'Smith', 'janesmith', 'hashed_password_2', 'jane.smith@example.com', '456 Oak Ave', '555-5678', FALSE),
('Admin', 'User', 'admin', 'hashed_admin_password', 'admin@example.com', '789 Pine Ln', '555-9012', TRUE);  -- Админ

-- Примеры заказов и элементов заказа
INSERT INTO Orders (CustomerId, OrderDate, ShippingAddress, TotalAmount, OrderStatus) VALUES
(1, '2024-10-27 10:00:00', '123 Main St', 94.00, 'Delivered'),
(2, '2024-10-27 14:30:00', '456 Oak Ave', 70.00, 'Shipped');

INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES
(1, 1, 1, 25.00),  -- Coca-Cola
(1, 2, 1, 24.00),  -- Pepsi
(1, 3, 1, 40.00),  -- Borjomi
(2, 4, 2, 35.00);  -- Sandora Apple Juice

-- Пример отзыва
INSERT INTO ProductReviews (ProductId, CustomerId, Rating, Comment) VALUES
(1, 1, 5, 'Отличная кола!'),
(3, 2, 4, 'Хорошая минеральная вода.');
//
```

## Пояснения к скрипту: ##

Удаление таблиц (DROP TABLE): Скрипт начинается с удаления существующих таблиц, если они есть. Это полезно при разработке для повторного создания базы данных.
Создание таблиц (CREATE TABLE): Создаются таблицы Customers, Categories, Brands, Products, Orders, OrderItems, ProductReviews и ProductInventory с определением типов данных, первичных ключей (PRIMARY KEY) и внешних ключей (FOREIGN KEY).
Внешние ключи (FOREIGN KEY): Устанавливаются внешние ключи для связи между таблицами, обеспечивая целостность данных.
Индексы (CREATE INDEX): Создаются индексы для ускорения запросов к базе данных. Индексы помогают СУБД быстрее находить данные.
Заполнение начальными данными (INSERT INTO): Добавляются примеры данных в таблицы для тестирования и демонстрации работы магазина. Добавлены примеры категорий, брендов, продуктов, пользователей, заказов и отзывов.
TIMESTAMP WITHOUT TIME ZONE: В PostgreSQL рекомендуется использовать TIMESTAMP WITHOUT TIME ZONE для хранения дат и времени, если часовой пояс не имеет значения. Если часовой пояс важен, используйте TIMESTAMP WITH TIME ZONE. В примерах выше установлен UTC.
UNIQUE NOT NULL: Для полей, которые должны быть уникальными и не могут быть NULL, явно указано UNIQUE NOT NULL.
DEFAULT (NOW() AT TIME ZONE ‘utc’): Для полей даты и времени используется значение по умолчанию, равное текущему времени в UTC.
Примеры: Добавлены примеры данных для всех таблиц, включая админа, примеры заказов и отзывов.
ProductInventory: Добавлена таблица и примеры для управления запасами.
Уникальный индекс на ProductId в ProductInventory: Добавлен UNIQUE индекс, для гарантии связи один-к-одному.


# Этап 3: 
**Необходимые библиотеки:**

Npgsql: Для подключения к базе данных PostgreSQL.

ConsoleTables: Для красивого отображения данных в табличном формате в консоли

**Program.cs:** Здесь находится основной цикл программы и обработка пользовательского ввода. Это точка входа в приложение.

**DbUtils.cs:** Здесь находятся функции, которые непосредственно взаимодействуют с базой данных. Это включает подключение к базе данных, выполнение SQL-запросов и обработку результатов.

**Utils.cs:** Здесь находятся вспомогательные функции, которые используются в других частях программы. Например, функции для проверки ввода пользователя и форматирования вывода данных.


# Этап 4: 

**Цель:**  
Отделить логику доступа к базе данных от остальной части приложения.

**Реализация:**

Создаются классы, отвечающие за выполнение операций CRUD (Create, Read, Update, Delete) для каждой сущности (например, Product, Category, Brand). Эти классы обычно называют репозиториями.
В репозиториях инкапсулируется логика подключения к базе данных, выполнения запросов и обработки результатов.
(Опционально) Для репозиториев определяются интерфейсы, чтобы обеспечить возможность легкой замены реализации (например, для тестирования).
В коде используются параметризованные запросы для защиты от SQL-инъекций.
Вся работа с базой данных оборачивается в блоки try-catch для обработки возможных исключений.

**Результат:**

Код, работающий с базой данных, изолирован в отдельном слое.
Остальная часть приложения (UI, BLL) не зависит от конкретной реализации базы данных.
Повышается безопасность и поддерживаемость кода.


# Этап 5:

**Цель:**
Отделить бизнес-логику приложения от UI и DAL.

**Реализация:**

Создаются классы, которые реализуют основную логику приложения (например, валидация данных, расчет цен, применение скидок).
Классы BLL принимают на вход данные от UI и передают их в DAL для сохранения или извлечения.
Классы BLL используют классы DAL для доступа к данным, но не знают деталей реализации DAL.
В BLL могут применяться различные паттерны проектирования, такие как Dependency Injection, Strategy, Factory и другие.

**Результат:**
Бизнес-логика приложения инкапсулирована в отдельном слое.
Легче изменять и расширять бизнес-логику, не затрагивая UI и DAL.
Повышается переиспользуемость и тестируемость кода.




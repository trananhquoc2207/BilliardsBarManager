CREATE DATABASE QuanLyQuanBida1
GO

USE QuanLyQuanBida1
GO

--Food :  món ăn
--TablePlay
--FoodCategory : danh mục món ăn
-- Account : lưu thông tin người dùng
-- Bill : hóa đơn
-- BillInfor : thông tin hóa đơn


CREATE TABLE TablePlay
(	
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL, 

	status NVARCHAR(100) NOT NULL, -- Trống hoặc Có người
)
GO

CREATE TABLE Account
(
	UseName NVARCHAR(100) NOT NULL,
	DisplayName NVARCHAR(100) NOT NULL,
	PassWord NVARCHAR(1000) NOT NULL,
	Type INT NOT NULL DEFAULT 0 --1 là admin , 0 là staff
)
GO


CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
)
go
CREATE TABLE Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
	idCategory INT NOT NULL,
	price FLOAT NOT NULL

	FOREIGN KEY(idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DataCheckIn DATETIME,
	DataCheckOut DATETIME,
	idTable INT NOT NULL,
	soGioChoi int,
	status INT NOT NULL DEFAULT 0--1 là đã thanh toán và 0 là chưa thanh toán

	FOREIGN KEY (idTable) REFERENCES dbo.TablePlay(id)
)
GO

CREATE TABLE billInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT  NOT NULL DEFAULT 0,
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id),
)


-- thêm bàn
DECLARE @i INT = 0
 WHILE @i <= 10
 BEGIN
 INSERT dbo.TablePlay
        ( name, status )
VALUES  ( N'Bàn '+ CAST(@i AS NVARCHAR(100)), -- name - nvarchar(100)
          N'trống'  -- status - nvarchar(100)
          )
SET @i = @i + 1
 END

 SELECT * FROM dbo.TablePlay
GO

INSERT INTO dbo.Account
        ( UseName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'quoc1' , -- UseName - nvarchar(100)
          N'quoc' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          1  -- Type - int
        )
go
INSERT INTO dbo.Account
        ( UseName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'tri1' , -- UseName - nvarchar(100)
          N'tri' , -- DisplayName - nvarchar(100)
          N'2' , -- PassWord - nvarchar(1000)
          0  -- Type - int
        )
SELECT * FROM dbo.Account
GO

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS 
BEGIN
  SELECT * FROM dbo.Account WHERE UseName = @userName
END
GO

EXEC dbo.USP_GetAccountByUserName @userName = N'tan1' -- nvarchar(100)

SELECT * FROM dbo.Food

SELECT * FROM dbo.Account WHERE UseName =N'tan1' AND PassWord = N'1'
SELECT * FROM dbo.TablePlay

GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.TablePlay
GO

EXEC dbo.USP_GetTableList
GO

INSERT dbo.FoodCategory
        ( name )
VALUES  ( N'món ăn'  -- name - nvarchar(100)
          )
INSERT dbo.FoodCategory
        ( name )
VALUES  ( N'Bida'  -- name - nvarchar(100)
          )
-- thêm thức ăn
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'sting', -- name - nvarchar(100)
          1, -- idCategory - int
          10000  -- price - float
          )
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Bida phăng', -- name - nvarchar(100)
          2, -- idCategory - int
          1500  -- price - float
          )
INSERT dbo.Bill
        ( DataCheckIn ,
          DataCheckOut ,
          idTable ,
          soGioChoi ,
          status
        )
VALUES  ( GETDATE() , -- DataCheckIn - datetime
          GETDATE() , -- DataCheckOut - datetime
          1 , -- idTable - int
          NULL , -- soGioChoi - time
          0  -- status - int
        )
INSERT dbo.Bill
        ( DataCheckIn ,
          DataCheckOut ,
          idTable ,
          soGioChoi ,
          status
        )
VALUES  ( GETDATE() , -- DataCheckIn - datetime
          NULL , -- DataCheckOut - datetime
          2 , -- idTable - int
          GETDATE() , -- soGioChoi - time
          0  -- status - int
        )
--them billInf
INSERT dbo.billInfo
        ( idBill, idFood, count )
VALUES  ( 1, -- idBill - int
          1, -- idFood - int
          2  -- count - int
          )
INSERT dbo.billInfo
        ( idBill, idFood, count )
VALUES  ( 2, -- idBill - int
          2, -- idFood - int
          4  -- count - int
          )
--SELECT id FROM dbo.Bill WHERE idTable =1   AND status =0
--SELECT * FROM dbo.billInfo WHERE idBill = 1

--SELECT * FROM dbo.Bill
--SELECT * FROM dbo.billInfo
--SELECT * FROM dbo.Food
--SELECT * FROM dbo.FoodCategory

--SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f
 --WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.status = 0 AND b.idTable =1
 GO
 
CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UseName = @userName AND PassWord = @passWord
END
GO
SELECT * FROM dbo.Account
GO

CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN

	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT = 1
	
	SELECT @isExitsBillInfo = id, @foodCount = b.count 
	FROM dbo.BillInfo AS b 
	WHERE idBill = @idBill AND idFood = @idFood

	IF (@isExitsBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfo	SET count = @foodCount + @count WHERE idFood = @idFood
		ELSE
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE
	BEGIN
		INSERT	dbo.BillInfo
        ( idBill, idFood, count )
		VALUES  ( @idBill, -- idBill - int
          @idFood, -- idFood - int
          @count  -- count - int
          )
	END
END
GO

CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idTable INT
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0	
	
	DECLARE @count INT
	SELECT @count = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBill
	
	IF (@count > 0)
	BEGIN
	
		PRINT @idTable
		PRINT @idBill
		PRINT @count
		
		UPDATE dbo.TablePlay SET status = N'Có người' WHERE id = @idTable		
		
	END		
	ELSE
	BEGIN
	PRINT @idTable
		PRINT @idBill
		PRINT @count
	UPDATE dbo.TablePlay SET status = N'Trống' WHERE id = @idTable	
	end
	
END
GO

CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT dbo.Bill
	        ( DataCheckIn ,
	          DataCheckOut ,
	          idTable ,
	          soGioChoi ,
	          status
	        )
	VALUES  ( GETDATE() , -- DataCheckIn - datetime
	          NULL , -- DataCheckOut - datetime
	          @idTable , -- idTable - int
	          NULL , -- soGioChoi - time
	          0  -- status - int
	        )
END
GO

CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = id FROM Inserted	
	
	DECLARE @idTable INT
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count int = 0
	
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND status = 0
	
	IF (@count = 0)
		UPDATE dbo.TablePlay SET status = N'Trống' WHERE id = @idTable
END
GO
DROP TRIGGER UTG_UpdateBill 
GO

--UPDATE dbo.Bill SET  DataCheckIn = GETDATE(),status = 1 WHERE id = 1

ALTER	TABLE dbo.Bill ADD totalPrice FLOAT
GO
DELETE dbo.billInfo
DELETE dbo.Bill
go

CREATE PROC USP_GetListBillByDate
@checkIn DATETIME, @checkOut DATETIME
AS 
BEGIN
SELECT t.name,b.totalPrice, idTable,DataCheckIn,DataCheckOut FROM dbo.Bill AS b, dbo.TablePlay AS t, dbo.billInfo AS bi,dbo.Food AS f
 WHERE b.DataCheckIn>= @checkIn AND b.DataCheckOut <= b.DataCheckOut AND b.status =1 AND
  t.id = b.idTable AND b.id = bi.idBill AND bi.idFood = f.id
END
GO

SELECT t.name,b.totalPrice, idTable,DataCheckIn,DataCheckOut FROM dbo.Bill AS b, dbo.TablePlay AS t, dbo.billInfo AS bi,dbo.Food AS f
 WHERE  b.status =1 AND
  t.id = b.idTable AND b.id = bi.idBill AND bi.idFood = f.id
  GO
  

--SELECT * FROM dbo.bill
GO

--SELECT * FROM dbo.Account
go
UPDATE dbo.Bill SET soGioChoi = DATEDIFF (MINUTE, DataCheckIn,DataCheckOut) 

--SELECT soGioChoi FROM dbo.bill WHERE id = 15

INSERT dbo.Account
        ( UseName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'' , -- UseName - nvarchar(100)
          N'' , -- DisplayName - nvarchar(100)
          N'' , -- PassWord - nvarchar(1000)
          0  -- Type - int
        )


--SELECT * FROM dbo.Account
--SELECT MAX(id) FROM dbo.Bill
--SELECT b.soGioChoi AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.status = 1 AND b.idTable = 2
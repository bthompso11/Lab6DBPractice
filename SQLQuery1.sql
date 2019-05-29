CREATE PROCEDURE [dbo].[usp_CustomerSelect]
@CustomerID int
AS
BEGIN
    select * from Customers
	where CustomerID = @CustomerID;
END


CREATE PROCEDURE [dbo].[usp_ProductSelect]
@ProductID int
AS
BEGIN
    select * from Products
	where ProductID = @ProductID;
END

CREATE PROCEDURE [dbo].[usp_CustomerUpdate] 
	-- Add the parameters for the stored procedure here
	@CustomerID int,
	@Name varchar(100),
	@Address varchar(50), 
	@City varchar(20), 
	@State char(2), 
	@Zipcode char(15), 
	@ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Update Customers set
		Name = @Name, 
		Address = @Address,
		City = @City,
		State = @State,
		Zipcode = @Zipcode,
		ConcurrencyID = (@ConcurrencyID + 1)
	Where CustomerID = @CustomerID and ConcurrencyID = @ConcurrencyID;

END

CREATE PROCEDURE [dbo].[usp_ProductUpdate] 
	-- Add the parameters for the stored procedure here
	@ProductID int,
	@ProductCode char(10),
	@Description varchar(50), 
	@UnitPrice Money, 
	@OnHandQuantity int, 
	@ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Update Products set
		ProductCode = @ProductCode, 
		Description = @Description,
		UnitPrice = @UnitPrice,
		OnHandQuantity = @OnHandQuantity,
		ConcurrencyID = (@ConcurrencyID + 1)
	Where ProductID = @ProductID and ConcurrencyID = @ConcurrencyID;

END

CREATE PROCEDURE [dbo].[usp_CustomerSelectAll] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
    -- Insert statements for procedure here
	SELECT * from Customers order by CustomerID;
END

CREATE PROCEDURE [dbo].[usp_ProductSelectAll] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
    -- Insert statements for procedure here
	SELECT * from Products order by ProductID;
END

CREATE PROCEDURE [dbo].[usp_CustomerDelete] 
	-- Add the parameters for the stored procedure here
	@CustomerID int, @ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Delete from Customers where CustomerID = @CustomerID and ConcurrencyID = @ConcurrencyID;
END

CREATE PROCEDURE [dbo].[usp_ProductDelete] 
	-- Add the parameters for the stored procedure here
	@ProductID int, @ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Delete from Products where ProductID = @ProductID and ConcurrencyID = @ConcurrencyID;
END

CREATE PROCEDURE [dbo].[usp_CustomerCreate] 
	-- Add the parameters for the stored procedure here
	@CustomerID int output,
	@Name varchar(100),
	@Address varchar(50), 
	@City varchar(20), 
	@State char(2), 
	@Zipcode char(15)
	
	
AS
BEGIN
    -- Insert statements for procedure here
	Insert into Customers (Name, Address, City, State, Zipcode, ConcurrencyID)
	values (@Name, @Address, @City, @State, @Zipcode, 1);
	set @CustomerID = @@IDENTITY;
END

CREATE PROCEDURE [dbo].[usp_ProductCreate] 
	-- Add the parameters for the stored procedure here
	@ProductID int output,
	@ProductCode char(10),
	@Description varchar(50), 
	@UnitPrice decimal, 
	@OnHandQuantity int		
	
AS
BEGIN
    -- Insert statements for procedure here
	Insert into Products (ProductCode, Description, UnitPrice, OnHandQuantity, ConcurrencyID)
	values (@ProductCode, @Description, @UnitPrice, @OnHandQuantity, 1);
	set @ProductID = @@IDENTITY;
END


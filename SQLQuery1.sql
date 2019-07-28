create database WalletDataBase
go

use WalletDataBase;
go
drop table ProductCategories, SaleProducts, SaleOrders;
go

CREATE TABLE [dbo].[ProductCategories] (
    [ID_Category]  INT           IDENTITY (1, 1) NOT NULL primary key,
    [CategoryName] VARCHAR (MAX) NULL
);
GO

CREATE TABLE [dbo].[SaleOrders] (
    [ID_Order]      INT           IDENTITY (1, 1) NOT NULL primary key,
    [SellerName]    VARCHAR (MAX) NULL,
    [SellerAddress] VARCHAR (MAX) NULL,
    [SaleSum]       VARCHAR (MAX) NULL
);
GO

CREATE TABLE [dbo].[SaleProducts] (
    [ID_Product]      INT           IDENTITY (1, 1) NOT NULL primary key,
    [ProductName]     VARCHAR (MAX) NULL,
    [ProductLifeTime] DATE          NULL,
    [ProductPrice]    INT           NULL,
    [ProductCategory] INT           NULL,
    [SaleOrder]       INT foreign key references SaleOrders(ID_Order)
);
go

create procedure InsertProduct
@key varchar(max),
@result varchar(max)
as
begin
	if (ISJSON(@key) = 1)
		begin
			select * from
			OPENJSON(@key) 
			WITH (
					Name varchar(50) '$.Name',
					LifeTime date '$.LifeTime',
					Price int '$.price',
					Category varchar(50) '$.Category',
					SaleOrder varchar(max) '$.Order'
				) as jTable;
			declare record CURSOR for select * from jTable;
			declare @name varchar(50),
					@lifeTime date,
					@price int,
					@category varchar(50),
					@saleOrder varchar(max),
					@prodCategory int;

			open record;
			fetch next from record into @name, @lifeTime, @price, @category, @saleOrder;

			while @@FETCH_STATUS = 0
				begin
					set @prodCategory = (select ProductCategories.ID_Category 
										from ProductCategories 
										where ProductCategories.CategoryName = @category);
					--------------------------
					--insert into SaleOrders--
					if (JSON_VALUE(@saleOrder, '$.id') > 0)
					begin
						update SaleOrders set SellerName = JSON_VALUE(@saleOrder, '$.SellerName'), 
											SellerAddress = JSON_VALUE(@saleOrder, '$.SellerAddress'),
											SaleSum = JSON_VALUE(@saleOrder, '$.SaleSum') 
											where ID_Order = JSON_VALUE(@saleOrder, '$.id');
					end;
					else if (JSON_VALUE(@saleOrder, '$.id') = 0)
						begin
							insert into SaleOrders(ID_Order, SellerName, SellerAddress, SaleSum) 
								values (JSON_VALUE(@saleOrder, '$.id'), JSON_VALUE(@saleOrder, '$.SellerName'), JSON_VALUE(@saleOrder, '$.SellerAddress'), JSON_VALUE(@saleOrder, '$.SaleSum'));
						end;
					--------------------------
					insert into SaleProducts(ProductName, ProductLifeTime, ProductPrice, ProductCategory, SaleOrder)
						--output SaleProducts.ID_Product
						values (@name, @lifeTime, @price, @prodCategory, JSON_VALUE(@saleOrder, '$.id'))
				end;
		end;
	else set @result = null;
	return
end;

go

create procedure GetProduct
@key int = 0,
@result varchar(max) output
as
begin
	if (@key = 0)
		begin
			select ID_Product as [Product.id],
					ProductName as [Product.Name], 
					ProductLifeTime as [Product.LifeTime],
					(select * from ProductCategories where ProductCategories.ID_Category = SaleProducts.ProductCategory) as [Product.Category],
					(select * from SaleOrders where SaleOrders.ID_Order = SaleProducts.SaleOrder for JSON auto) as [Product.Order]
					from SaleProducts for JSON PATH
			return;
		end;
	else if (@key > 0)
		begin
			select ID_Product as [Product.id],
					ProductName as [Product.Name], 
					ProductLifeTime as [Product.LifeTime],
					(select * from ProductCategories where ProductCategories.ID_Category = SaleProducts.ProductCategory) as [Product.Category],
					(select * from SaleOrders where SaleOrders.ID_Order = SaleProducts.SaleOrder for JSON auto) as [Product.Order]
					from SaleProducts 
					where ID_Product = @key 
					for JSON PATH
			return;
		end;
	else
		begin
			delete from SaleProducts where ID_Product = @key;
			set @result = 'sucess';
			return;
		end;
end;
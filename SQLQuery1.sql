use WalletDataBase;
go
drop procedure InsertProduct;
go
create procedure InsertProduct
@key varchar(max),
@result varchar(max)
as
begin
	if (ISJSON(@key) = 1)
		begin
			declare record CURSOR for select * from OPENJSON(@key)
				with (
					Name varchar(50) '$.Name',
					LifeTime date '$.LifeTime',
					Price int '$.price',
					Category varchar(50) '$.Category',
					SaleOrder varchar(max) '$.Order'
				);
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
	else 
		begin
			select @result = null;
		end;
	return
end;



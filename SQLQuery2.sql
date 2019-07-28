

use WalletDataBase;

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
					(select * from ProductCategories where ProductCategories.ID_Category = SaleProducts.ProductCategory for JSON auto) as [Product.Category],
					(select * from SaleOrders where SaleOrders.ID_Order = SaleProducts.SaleOrder for JSON auto) as [Product.Order]
					--ProductCategory as [Product.Category],
					--SaleOrder as [Product.Order]
					from SaleProducts for JSON PATH
			return;
		end;
	else if (@key > 0)
		begin
			select ID_Product as [Product.id],
					ProductName as [Product.Name], 
					ProductLifeTime as [Product.LifeTime],
					(select * from ProductCategories where ProductCategories.ID_Category = SaleProducts.ProductCategory  for JSON auto) as [Product.Category],
					(select * from SaleOrders where SaleOrders.ID_Order = SaleProducts.SaleOrder for JSON auto) as [Product.Order]
					--ProductCategory as [Product.Category],
					--SaleOrder as [Product.Order]
					from SaleProducts 
					where ID_Product = @key 
					for JSON PATH;
			return;
		end;
	else
		begin
			delete from SaleProducts where ID_Product = @key;
			set @result = 'sucess';
			return;
		end;
end;
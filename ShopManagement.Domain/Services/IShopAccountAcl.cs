namespace ShopManagement.Domain.Services;

public interface IShopAccountAcl
{
    (string name, string mobile) GetAccountById(long id);
}
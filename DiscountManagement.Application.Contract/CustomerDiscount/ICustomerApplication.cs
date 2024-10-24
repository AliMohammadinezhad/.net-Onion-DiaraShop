using Framework.Application;

namespace DiscountManagement.Application.Contract.CustomerDiscount;

public interface ICustomerApplication
{
    OperationResult Define(DefineCustomerDiscount command);
    OperationResult Edit(EditCustomerDiscount command);
    EditCustomerDiscount GetDetails(long id);
    List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel);
}
using Framework.Application;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application
{
    public class InventoryApplication : IInventoryApplication
    {
        private readonly IInventoryRepository _repository;

        public InventoryApplication(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public OperationResult Create(CreateInventory command)
        {
            var operation = new OperationResult();
            if (_repository.Exists(x => x.ProductId == command.ProductId))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            
            var inventory = new Inventory(command.ProductId, command.UnitPrice);
            _repository.Create(inventory);
            _repository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditInventory command)
        {
            var operation = new OperationResult();
            var inventory = _repository.Get(command.Id);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);


            if(_repository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            inventory.Edit(command.ProductId, command.UnitPrice);
            _repository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Increase(IncreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = _repository.Get(command.InventoryId);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            const long operatorId = 1; // TODO: add accounting system 
            inventory.Increase(command.Count, operatorId, command.Description);
            _repository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Decrease(DecreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = _repository.Get(command.InventoryId);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            const long operatorId = 1; // TODO: add accounting system 
            inventory.Decrease(command.Count, operatorId, command.Description, command.OrderId);
            _repository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Decrease(List<DecreaseInventory> command)
        {
            var operation = new OperationResult();
            const long operatorId = 1;
            foreach (var item in command)
            {
                var inventory = _repository.GetBy(item.ProductId);
                inventory.Decrease(item.Count, operatorId, item.Description, item.OrderId);
            }
            _repository.SaveChanges();
            return operation.Succeeded();
        }

        public EditInventory GetDetails(long id)
        {
            return _repository.GetDetails(id);
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            return _repository.Search(searchModel);
        }
    }
}

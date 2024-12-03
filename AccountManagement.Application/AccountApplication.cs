using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using Framework.Application;
using Framework.Domain;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IFileUploader fileUploader)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);


            var password = _passwordHasher.Hash(command.Password);
            var picturePath = _fileUploader.Upload(command.ProfilePhoto, $"profilePhotos/{command.FullName}");
            var account = new Account(command.FullName, command.Username, password, command.Mobile,
                command.RoleId, picturePath);

            _accountRepository.Create(account);
            _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (_accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            var picturePath = _fileUploader.Upload(command.ProfilePhoto, $"profilePhotos/{command.FullName}");

            account.Edit(command.FullName, command.Username, command.Mobile, command.RoleId, picturePath);
            _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != account.Password)
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);


            var password = _passwordHasher.Hash(command.Password);
            
            account.ChangePassword(password);
            _accountRepository.SaveChanges();

            return operation.Succeeded();
        }

        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }
    }
}

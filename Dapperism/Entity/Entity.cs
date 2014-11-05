using Microsoft.Practices.EnterpriseLibrary.Validation;
using ValidationResult = Dapperism.Validation.ValidationResult;
using ValidationResults = Dapperism.Validation.ValidationResults;

namespace Dapperism.Entity
{
    public class Entity<TEntity> : IEntity
        where TEntity : class , new()
    {

        private readonly Validator<TEntity> _validationService;

        public Entity()
        {
            _validationService = ValidationFactory.CreateValidator<TEntity>();
        }
        public ValidationResults Validate()
        {
            var vResults = new ValidationResults();
            var validationResults = _validationService.Validate(this);

            if (validationResults.Count == 0)
                return null;

            foreach (var vr in validationResults)
                vResults.Add(new ValidationResult { Key = vr.Key, Message = vr.Message });

            return vResults;
        }

        public bool IsValid()
        {
            return _validationService.Validate(this).IsValid;
        }
        
    }
}

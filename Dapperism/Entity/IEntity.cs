using Dapperism.Validation;

namespace Dapperism.Entity
{
    public interface IEntity
    {
        ValidationResults Validate();
        bool IsValid();
    }
}
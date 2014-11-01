namespace Dapperism.Entities
{
    public interface IEntity
    {
        ValidationResults Validate();
        bool IsValid();
    }
}
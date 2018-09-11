namespace LBH.Data.Repository
{
    public interface IEntity<TIndex>
    {
        TIndex Id { get; set; }
    }
}
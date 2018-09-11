namespace LBHTenancyAPITest.Helpers.Stub
{
    public interface IEntity<TIndex>
    {
        TIndex Id { get; set; }
    }
}

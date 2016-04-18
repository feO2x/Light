namespace Light.BayesianNetwork
{
    public interface INodeBuilder<T>
    {
        T Build();
    }
}
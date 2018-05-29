namespace PipelineFramework
{
    public interface IForEachTaskContext<TContext, TElement> 
    {
        TContext Context { get; }
        TElement Value { get; }
    }
}
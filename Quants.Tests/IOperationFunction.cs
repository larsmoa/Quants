using Quants.Quantities;

namespace Tests.Quants
{
    /// <summary>
    /// Moq doesn't support mocking delegates, but creating an interface is a good workaround.
    /// This interface is for mocking operations in OperationStore and ArithmeticsStore.
    /// </summary>
    public interface IOperationFunction
    {
        // Fulfills Func<QuantityBase, QuantityBase. QuantityBase>
        QuantityBase Operation(QuantityBase left, QuantityBase right);
    }
}
using System;

namespace ErpNextApiInterface.Interface
{
    public interface ITransactionScope : IDisposable
    {
        void Complete();
    }
}

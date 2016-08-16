using System;
using System.Collections.Generic;

namespace MVVM_MEF
{
    public interface IService
    {
        List<Model> DataItems { get; }
    }
}

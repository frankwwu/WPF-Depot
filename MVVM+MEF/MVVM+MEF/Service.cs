using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MVVM_MEF
{
    [Export(typeof(IService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Service : MVVM_MEF.IService
    {
        public Service()
        {
            DataItems = new List<Model>();
            foreach (var name in Enum.GetValues(typeof(Environment.SpecialFolder)))
            {
                string p = Environment.GetFolderPath((Environment.SpecialFolder)name);
                Model m = new Model() { Name = name.ToString(), Folder = p };
                DataItems.Add(m);
            }
        }

        public List<Model> DataItems { get; private set; }
    }
}

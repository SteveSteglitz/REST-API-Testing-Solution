using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RestApiTestSolution.Model;

namespace RestApiTestSolution.ViewModel
{
    public static class ObservableCollectionExtention
    {
        public static void SwapItems(this ObservableCollection<ApiRoute> items, int indexA, int indexB)
        {
            ApiRoute tmp = items[indexA];
            items[indexA] = items[indexB];
            items[indexB] = tmp;
        }
    }
}

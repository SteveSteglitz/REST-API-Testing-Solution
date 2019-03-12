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
    public static class ObservableollectionExtention
    {
        public static void SwapItems(this ObservableCollection<RestApiCallItem> items, int indexA, int indexB)
        {
            RestApiCallItem tmp = items[indexA];
            items[indexA] = items[indexB];
            items[indexB] = tmp;
        }
    }
}

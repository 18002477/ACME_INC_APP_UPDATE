using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME_INC_APP.Models
{
    public class DataPoint
    {
        private int count;
        private string prodCatName;

        public DataPoint(int count, string prodCatName)
        {
            this.count = count;
            this.prodCatName = prodCatName;
        }
    }
}

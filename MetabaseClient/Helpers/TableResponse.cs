using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MetabaseClient.Helpers
{
    /*
     * UNUSED
     */
    public class TableResponse
    {
        public int DatabaseId { get; set; }
        public string Status { get; set; }
        public string Context { get; set; }
        // public int RowCount { get; set; }
        public IList<object[]> Rows { get; set; }
        public IList<string> Columns { get; set; }
    }
}

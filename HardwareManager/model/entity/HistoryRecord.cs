using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class HistoryRecord : Entity
    {
        public DateTime? operationDate;
        public DateTime? OperationDate 
        { 
            get => operationDate;
            set
            {
                if (value != null)
                {
                    OperationDateStr = ((DateTime)value).ToString("G");
                    operationDate = value;
                    return;
                }
                OperationDateStr = null;
                operationDate = null;
            } 
        }
        public string OperationDateStr { get; set; }
        public string OperationType { get; set; }
        public string HardwareType { get; set; }
        public int? InventorialNumber { get; set; }
        public string WorkplaceName { get; set; }

        public HistoryRecord() { }
    }
}

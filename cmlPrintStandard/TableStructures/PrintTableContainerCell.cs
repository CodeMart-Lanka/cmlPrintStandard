using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.TableStructures
{
    public class PrintTableContainerCell : PrintTableCell
    {
        public virtual void SetContent(PrintTableCell cell)
        {
            Content = cell;
        }
        public override string ToString()
        {
            return $"PrintTableContainer[{Index}]  | Content : {Content}";
        }

        public override void ResetProcessingStatus()
        {
            base.ResetProcessingStatus();  
            Content?.ResetProcessingStatus();
        }

        public PrintTableCell Content { get; private set; }
    }
}

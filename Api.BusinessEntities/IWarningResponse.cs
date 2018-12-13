using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessEntities
{
    public interface IWarningResponse
    {
        /// <summary>
        /// The warning message is to be displayed to the user, if any.
        /// </summary>
        string WarningMessage { get; set; }
    }
}

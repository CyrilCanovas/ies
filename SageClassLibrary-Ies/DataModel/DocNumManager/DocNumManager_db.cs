using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public partial class DocNumManager : SageObject
    {
        private string piece = string.Empty;
        public string Piece { get { return piece; } set { piece = value; } }

    }
}

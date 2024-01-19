using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Requests.Example
{
    public class CarUpdateRequest : CarAddRequest , IModelIdentifier
    {
        public int Id { get; set; }
    }
}

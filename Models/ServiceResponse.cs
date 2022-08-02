using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Models
{
    //Wraps responses in data property, sends back true if successful and a readable message rather than complex console response if failed
    public class ServiceResponse<T>
    {
        //Data type T is the actual data like Rpg Characters
        public T? Data { get; set; }
        //Success property we can tell the front end if everything went right 
        public bool Success { get; set; } = true;
        //Message property can be used to send an explanatory message in case of an error 
        public string Message { get; set; } = string.Empty;
    }
}
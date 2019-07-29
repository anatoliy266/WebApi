using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_client
{
    public interface IViewModel
    {
        
    }

    public class ViewModel : IViewModel
    {
        IWebHooker webHooker;
        IJsonParser parser;
        public ViewModel(IWebHooker hooker, IJsonParser parse)
        {
            webHooker = hooker;
            parser = parse;
        }


    }
}

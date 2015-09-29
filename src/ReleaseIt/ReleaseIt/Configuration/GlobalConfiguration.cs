using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseIt.Configuration
{
    public class GlobalConfiguration
    {
        private static GlobalConfiguration _instance;
        private GlobalConfiguration()
        {

        }
        public string GitVersionControl { get; set; }
        public string SvnVersionControl { get; set; }


        public static GlobalConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalConfiguration();

                }
                return _instance;
                ;
            }
        }
    }



    public enum OperatorSystem
    {
        Windows32,
        Windows64,
    }

}

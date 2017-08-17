using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace AutoIssueLA_301
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = 
                #if(!DEBUG)


                        AutoIssueLA_301 myServ = new AutoIssueLA_301();
                        myServ.OnDebug();
                                   //    ServiceBase[] ServicesToRun;
                                   //    ServicesToRun = new ServiceBase[] 
                                   //{ 
                                   //     new AutoIssue()
                                   //};
                                   //    ServiceBase.Run(ServicesToRun);
#else
            AutoIssueLA_301 myServ = new AutoIssueLA_301();
            myServ.OnDebug();
           
#endif


            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new AutoIssueLA_301() 
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}

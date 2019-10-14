using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace Beziex_Op {

    public class BxReadDlog {

        public void  GetFname( out string fileName )
        {
            fileName = null;

            OpenFileDialog  rdFileDlog = new OpenFileDialog();
            SetParam( rdFileDlog );

            if( rdFileDlog.ShowDialog() == false )
                return;

            fileName = rdFileDlog.FileName;
        }

        // -------------------------------------------------------

        private void  SetParam( OpenFileDialog rdFileDlog )
        {
            rdFileDlog.Filter           = "Beziex PATCH (*.gzjson)|*.gzjson";
            rdFileDlog.InitialDirectory = GetInitDir();
        }

        private string  GetInitDir()
        {
            string  exeDir = Path.GetDirectoryName( Assembly.GetEntryAssembly().Location );

            Uri  baseuri = new Uri( exeDir );
            Uri  uri     = new Uri( baseuri, "..\\..\\_demo" );

            return uri.LocalPath;
        }
    }
}

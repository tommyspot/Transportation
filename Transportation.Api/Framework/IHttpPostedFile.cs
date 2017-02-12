using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Api.Framework
{
    public interface IHttpPostedFile
    {
        // Summary:
        //     Gets the size of an uploaded file, in bytes.
        //
        // Returns:
        //     The file length, in bytes.
        int ContentLength { get; }
        //
        // Summary:
        //     Gets the MIME content type of a file sent by a client.
        //
        // Returns:
        //     The MIME content type of the uploaded file.
         string ContentType { get; }
        //
        // Summary:
        //     Gets the fully qualified name of the file on the client.
        //
        // Returns:
        //     The name of the client's file, including the directory path.
         string FileName { get; }
        //
        // Summary:
        //     Gets a System.IO.Stream object that points to an uploaded file to prepare
        //     for reading the contents of the file.
        //
        // Returns:
        //     A System.IO.Stream pointing to a file.
         Stream InputStream { get; }

        // Summary:
        //     Saves the contents of an uploaded file.
        //
        // Parameters:
        //   filename:
        //     The name of the saved file.
        //
        // Exceptions:
        //   System.Web.HttpException:
        //     The System.Web.Configuration.HttpRuntimeSection.RequireRootedSaveAsPath property
        //     of the System.Web.Configuration.HttpRuntimeSection object is set to true,
        //     but filename is not an absolute path.
         void SaveAs(string filename);
    }
}

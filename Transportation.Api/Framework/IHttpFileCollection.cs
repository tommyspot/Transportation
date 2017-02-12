using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Api.Framework
{
    public interface IHttpFileCollection : ICollection, ISerializable
    {
        // Summary:
        //     Gets a string array containing the keys (names) of all members in the file
        //     collection.
        //
        // Returns:
        //     An array of file names.
         string[] AllKeys { get; }

        // Summary:
        //     Gets the object with the specified numerical index from the System.Web.HttpFileCollection.
        //
        // Parameters:
        //   index:
        //     The index of the item to get from the file collection.
        //
        // Returns:
        //     The System.Web.HttpPostedFile specified by index.
         IHttpPostedFile this[int index] { get; }
        //
        // Summary:
        //     Gets the object with the specified name from the file collection.
        //
        // Parameters:
        //   name:
        //     Name of item to be returned.
        //
        // Returns:
        //     The System.Web.HttpPostedFile specified by name.
         IHttpPostedFile this[string name] { get; }

        // Summary:
        //     Copies members of the file collection to an System.Array beginning at the
        //     specified index of the array.
        //
        // Parameters:
        //   dest:
        //     The destination System.Array.
        //
        //   index:
        //     The index of the destination array where copying starts.
         void CopyTo(Array dest, int index);
        //
        // Summary:
        //     Returns the System.Web.HttpPostedFile object with the specified numerical
        //     index from the file collection.
        //
        // Parameters:
        //   index:
        //     The index of the object to be returned from the file collection.
        //
        // Returns:
        //     An System.Web.HttpPostedFile object.
         IHttpPostedFile Get(int index);
        //
        // Summary:
        //     Returns the System.Web.HttpPostedFile object with the specified name from
        //     the file collection.
        //
        // Parameters:
        //   name:
        //     The name of the object to be returned from a file collection.
        //
        // Returns:
        //     An System.Web.HttpPostedFile object.
         IHttpPostedFile Get(string name);
        //
        // Summary:
        //     Returns the name of the System.Web.HttpFileCollection member with the specified
        //     numerical index.
        //
        // Parameters:
        //   index:
        //     The index of the object name to be returned.
        //
        // Returns:
        //     The name of the System.Web.HttpFileCollection member specified by index.
         string GetKey(int index);
        //
        // Summary:
        //     Returns all files that match the specified name.
        //
        // Parameters:
        //   name:
        //     The name to match.
        //
        // Returns:
        //     The collection of files.
         IList<IHttpPostedFile> GetMultiple(string name);

        /// <summary>
        /// Implements the ISerializable interface and raises the deserialization event when the deserialization is complete.
        /// </summary>
        /// <param name="sender">The source of the deserialization event.</param>
        void OnDeserialization(Object sender);
    }
}

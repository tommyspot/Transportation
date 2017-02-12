using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Transportation.Api.Framework
{
    public class HttpFileCollectionWrapper : IHttpFileCollection
    {
        public HttpFileCollectionWrapper(HttpFileCollection files)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            this.innerFiles = files;
        }

        private HttpFileCollection innerFiles = null;

        public string[] AllKeys { get { return this.innerFiles.AllKeys; } }

        public int Count { get { return this.innerFiles.Count; } }

        public IHttpPostedFile this[int index]
        {
            get
            {
                HttpPostedFile item = this.innerFiles[index];

                if (item != null)
                {
                    return new HttpPostedFileWrapper(item);
                }
                else
                {
                    return null;
                }
            }
        }

        public IHttpPostedFile this[string name]
        {
            get
            {
                HttpPostedFile item = this.innerFiles[name];

                if (item != null)
                {
                    return new HttpPostedFileWrapper(item);
                }
                else
                {
                    return null;
                }
            }
        }

        public ICollection Keys { get { return this.innerFiles.Keys; } }

        public void CopyTo(Array dest, int index)
        {
            this.innerFiles.CopyTo(dest, index);
        }

        public IHttpPostedFile Get(int index)
        {
            HttpPostedFile item = this.innerFiles.Get(index);

            if (item != null)
            {
                return new HttpPostedFileWrapper(item);
            }
            else
            {
                return null;
            }
        }

        public IHttpPostedFile Get(string name)
        {
            HttpPostedFile item = this.innerFiles.Get(name);

            if (item != null)
            {
                return new HttpPostedFileWrapper(item);
            }
            else
            {
                return null;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.innerFiles.GetEnumerator();
        }

        public string GetKey(int index)
        {
            return this.innerFiles.GetKey(index);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.innerFiles.GetObjectData(info, context);
        }

        public void OnDeserialization(Object sender)
        {
            this.innerFiles.OnDeserialization(sender);
        }

        void ICollection.CopyTo(Array dest, int index)
        {
            this.innerFiles.CopyTo(dest, index);
        }

        bool ICollection.IsSynchronized { get { return ((ICollection)this.innerFiles).IsSynchronized; } }

        object ICollection.SyncRoot { get { return ((ICollection)this.innerFiles).SyncRoot; } }


        public IList<IHttpPostedFile> GetMultiple(string name)
        {
            List<HttpPostedFile> items = (List<HttpPostedFile>)this.innerFiles.GetMultiple(name);
            IList<IHttpPostedFile> httpPostedFilesWrapper = new List<IHttpPostedFile>();
            foreach (var item in items)
            {
                httpPostedFilesWrapper.Add(new HttpPostedFileWrapper(item));
            }
            return httpPostedFilesWrapper;
        }
    }
}

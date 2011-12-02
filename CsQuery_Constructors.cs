﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jtc.CsQuery.ExtensionMethods;
using Jtc.CsQuery.Implementation;

namespace Jtc.CsQuery
{
    public partial class CsQuery
    {
        
       

        /// <summary>
        /// Creates a new, empty jQuery object.
        /// </summary>
        public CsQuery()
        {

        }

        /// <summary>
        /// Create a new CsQuery object using an existing instance and a selector. if the selector is null or missing, then
        /// it will contain no selection results.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="context"></param>
        public CsQuery(string selector, CsQuery context)
        {
            Create(selector, context);
        }
        /// <summary>
        /// Create a new CsQuery object from HTML, and assign CSS from a JSON string, within a context
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="context"></param>
        public CsQuery(string selector, string cssJson, CsQuery context)
        {
            Create(selector, context);
            AttrSet(cssJson);
        }
        /// <summary>
        /// Create a new CsQuery object from HTML, and assign CSS, within a context
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="context"></param>
        public CsQuery(string selector, IDictionary<string,object> css, CsQuery context)
        {
            Create(selector, context);
            Attr(css);
        }
        protected void Create(string selector, CsQuery context)
        {
            // when creating a new CsQuery from another, leave Dom blank - it will be populated automatically with the
            // contents of the selector.
            CsQueryParent = context;

            if (!String.IsNullOrEmpty(selector))
            {
                Selectors = new CsQuerySelectors(selector);
                AddSelectionRange(Selectors.Select(Document, context.Children()));
            }
        }
        /// <summary>
        /// Create a new CsQuery from a single elemement, using the element's context
        /// </summary>
        /// <param name="element"></param>
        public CsQuery(IDomObject element)
        {
            Document = element.Document;
            AddSelection(element);
        }
        /// <summary>
        /// Create a new CsQuery from a single DOM element
        /// </summary>
        /// <param name="element"></param>
        public CsQuery(IDomObject element, CsQuery context)
        {
            CsQueryParent = context;
            AddSelection(element);
        }
        /// <summary>
        /// Create a new CsQuery object from an existing CsQuery object (or any set of DOM elements).
        /// If the source is a unassociated list of DOM elements, the context of the first element will become
        /// the context of the new CsQuery object.
        /// </summary>
        /// <param name="elements"></param>
        public CsQuery(IEnumerable<IDomObject> elements)
        {
            //List<IDomObject> elList = new List<IDomObject>(elements);
            
            bool first = true;
            if (elements is CsQuery)
            {
                CsQueryParent = (CsQuery)elements;
                first = false;
            }
 
            foreach (IDomObject el in elements)
            {
                if (first)
                {
                    Document = el.Document;
                    first = false;
                }
                Selection.Add(el);
            }
            
        }

        /// <summary>
        /// Create a new CsQuery object from a set of DOM elements, using the DOM of the first element.
        /// could contain more than one context)
        /// </summary>
        /// <param name="elements"></param>
        public CsQuery(IEnumerable<IDomObject> elements, CsQuery context)
        {
            CsQueryParent = context;
            AddSelectionRange(elements);
        }

        /// <summary>
        /// Creates a new DOM. This will DESTROY any existing DOM. This is not the same as Select.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public void Load(string html)
        {
            Load((html ?? "").ToCharArray());
        }
        protected void Load(char[] html)
        {
            Clear();

            CreateNewDom(html);
            HtmlParser.DomElementFactory factory = new HtmlParser.DomElementFactory(Document);
            if (html != null)
            {
                foreach (IDomObject obj in factory.CreateObjects())
                {
                    Document.ChildNodes.Add(obj);
                    AddSelectionRange(Document.ChildNodes);
                }

            }
        }
        /// <summary>
        /// Creates a new DOM. This will DESTROY any existing DOM. This is not the same as Select.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        protected void Load(IEnumerable<IDomObject> elements)
        {
            Clear();
            CreateNewDom();
            ClearSelections();
            Document.ChildNodes.AddRange(elements);
            AddSelectionRange(Document.ChildNodes);
        }
        protected void CreateNewDom(char[] html=null)
        {
            if (html!=null)
            {
                Document = new DomRoot(html);
            }
            else
            {
                Document = new DomRoot();
            }
            Document.DomRenderingOptions = CsQuery.DefaultDomRenderingOptions;
            Document.DocType = CsQuery.DefaultDocType;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SMAADemo
{
    /** Implementation of ITextureProvider via XNA content pipeline with optional logging */
    public class DefaultTextureProvider : ITextureProvider
    {
        private Dictionary<string, Texture2D> Cache;
        private ContentManager Content;
        private ILogger Logger;
        private string Prefix;


        /** Construct a texture provider given a ContentManager
         *
         * @param _Content valid XNA content manager
         * @param _Logger optional logging instance
         * @param _Prefix prefix to add to all textures before loading them
         *   using the supplied content manager. 
         */
        public DefaultTextureProvider(ContentManager _Content, ILogger _Logger = null, 
            string _Prefix = "textures/")
        {
            Logger = _Logger ?? new NullLogger();
            Content = _Content;
            Prefix = _Prefix;

            Debug.Assert(_Content != null);
            Debug.Assert(_Prefix != null);

            Cache = new Dictionary<string, Texture2D>();
        }


        /** Get a texture given a resource name.
         * @param name Unique resource name to be loaded, the prefix supplied
         *   to the constructor will be prepended before passing it to
         *   the content pipeline.
         * @throws If the resource could no be loaded */
        public Texture2D Get(string name)
        {
            if (!Cache.ContainsKey(name))
            {
                Create(name);
            }
            return Cache[name];
        }


        /** Equivalent to Get() in this implementation. */
        public void Prefetch(string name)
        {
            // right now there is no difference between just
            // pre-fetching textures and actually using them.
            Get(name);
        }


        private void Create(string name)
        {
            Cache[name] = Content.Load<Texture2D>(Prefix + name);
            if(Logger != null)
            {
                Logger.Log("Created effect: " + name, 3);
            }
        }
    }
}

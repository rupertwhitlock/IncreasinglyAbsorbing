using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using MediaGarden.Models;
using Orchard.ContentManagement;
using MediaGarden.Pipeline;
using Orchard.DisplayManagement;
using Orchard.ContentManagement.MetaData.Models;

namespace MediaGarden.Services
{
    public interface IMediaGardenService : IDependency
    {

        IEnumerable<ContentTypeDefinition> AllMediaTypes();
        IEnumerable<string> AllStereotypes();

        
        /// <summary>
        /// Begins a new session for querying media locations and returns the database record.
        /// TODO: Should this load existing record for location as well?
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        MediaSessionRecord BeginMediaSession();

        /// <summary>
        /// Provides media source records from a session
        /// </summary>
        /// <param name="mediaSessionId"></param>
        /// <returns></returns>
        IQueryable<MediaSourceRecord> LoadMediaSession(int mediaSessionId);

        /// <summary>
        /// Gets a media source by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IMediaSource GetSource(int id);


        /// <summary>
        /// Builds an IMediaSource model item from a database record
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IMediaSource MediaSourceFromRecord(MediaSourceRecord source);

        /// <summary>
        /// Generates a fake media item (i.e. not a ContentItem) to be used for source preview shapes
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IMediaItem MediaPreviewItemFromSource(IMediaSource source);

        /// <summary>
        /// Pulls media sources for a query session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        IEnumerable<MediaSourceContext> Pull(MediaSessionRecord session, string query, string mediaStereotype = "");
        
        /// <summary>
        /// Picks sources and translates them into content items
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="asDrafts"></param>
        /// <returns></returns>
        IEnumerable<IContent> Pick(IEnumerable<IMediaSource> sources, bool asDrafts = false);
        /// <summary>
        /// Gets a media format by name
        /// </summary>
        /// <param name="formatName"></param>
        /// <returns></returns>
        IMediaFormat GetFormat(string formatName);
        /// <summary>
        /// Finds media formats that match the specified source
        /// TODO: Consider deprecating this method, and force users to use ContextFromMediaSource first
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<IMediaFormat> MatchFormats(IMediaSource source, string mediaStereotype = "");
        /// <summary>
        /// Finds media formats by matching an existing context
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<IMediaFormat> MatchFormats(MediaSourceContext source, string mediaStereotype = "");

        /// <summary>
        /// Determines the best viewer for a media item (with optional override name, although this will be ignored if the specified viewer either doesn't exist or doesn't
        /// support the media item)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="overrideName"></param>
        /// <returns></returns>
        IMediaViewer FindViewer(IMediaItem item, string overrideName = null);

        /// <summary>
        /// Finds any available viewers for the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IEnumerable<IMediaViewer> MatchViewers(IMediaItem item);

        /// <summary>
        /// Gets an absolute location for the media file (using internal storage providers if necessary)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string AbsoluteMediaUrl(IMediaItem item);
    }
}
using System;
using System.Collections.Generic;

namespace Tomboy.FirebaseAddin.Api
{
    public class FirebaseNoteObject{
        
        #region Public Static Methods

        public static FirebaseNoteObject FromJson (string jsonString){
            Hyena.Json.Deserializer deserializer =
                new Hyena.Json.Deserializer (jsonString);
            object obj = deserializer.Deserialize ();

            Hyena.Json.JsonObject jsonObj =
                obj as Hyena.Json.JsonObject;
            return FromJson (jsonObj);
        }

        public static FirebaseNoteObject FromJson (Hyena.Json.JsonObject jsonObj){
            if (jsonObj == null)
                throw new ArgumentException ("jsonObj does not contain a valid FirebaseNoteObject representation");

            // TODO: Checks
			FirebaseNoteObject fbNoteObj = new FirebaseNoteObject ();
            fbNoteObj.Guid = (string) jsonObj ["guid"];

            // TODO: Decide how much is required
            object val = 0;
            string key = "<unknown>";
            try {
                key = TitleElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.Title = (string) val;
                key = NoteContentElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.NoteContent = (string) val;
                key = NoteContentVersionElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.NoteContentVersion = (double) val;

                key = LastChangeDateElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.LastChangeDate = DateTime.Parse ((string) val);
                key = LastMetadataChangeDateElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.LastMetadataChangeDate = DateTime.Parse ((string) val);
                key = CreateDateElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.CreateDate = DateTime.Parse ((string) val);

                key = LastSyncRevisionElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.LastSyncRevision = (int) val;
                key = OpenOnStartupElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.OpenOnStartup = (bool) val;
                key = PinnedElementName;
                if (jsonObj.TryGetValue (key, out val))
                    fbNoteObj.Pinned = (bool) val;

                key = TagsElementName;
                if (jsonObj.TryGetValue (key, out val)) {
                    Hyena.Json.JsonArray tagsJsonArray =
                        (Hyena.Json.JsonArray) val;
                    fbNoteObj.Tags = new List<string> (tagsJsonArray.Count);
                    foreach (string tag in tagsJsonArray)
                        fbNoteObj.Tags.Add (tag);
                }
					
            } catch (InvalidCastException e) {
                Logger.Error("Note '{0}': Key '{1}', value  '{2}' failed to parse due to invalid type",
								fbNoteObj.Guid, key, val);
                throw e;
            }

            return fbNoteObj;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tos the update object.
        /// </summary>
        /// <returns>The update object.</returns>
        public Hyena.Json.JsonObject ToJson (){
            Hyena.Json.JsonObject noteUpdateObj =
                new Hyena.Json.JsonObject ();

            if (string.IsNullOrEmpty (Guid))
                throw new InvalidOperationException ("Cannot create a valid JSON representation without a Guid");

            noteUpdateObj [GuidElementName] = Guid;

            if (!string.IsNullOrEmpty (Command)) {
                noteUpdateObj [CommandElementName] = Command;
                return noteUpdateObj;
            }

            if (Title != null)
                noteUpdateObj [TitleElementName] = Title;
            if (NoteContent != null)
                noteUpdateObj [NoteContentElementName] = NoteContent;
            if (NoteContentVersion.HasValue)
                noteUpdateObj [NoteContentVersionElementName] = NoteContentVersion.Value;

            if (LastChangeDate.HasValue)
                noteUpdateObj [LastChangeDateElementName] =
                    LastChangeDate.Value.ToString (NoteArchiver.DATE_TIME_FORMAT);
            if (LastMetadataChangeDate.HasValue)
                noteUpdateObj [LastMetadataChangeDateElementName] =
                    LastMetadataChangeDate.Value.ToString (NoteArchiver.DATE_TIME_FORMAT);
            if (CreateDate.HasValue)
                noteUpdateObj [CreateDateElementName] =
                    CreateDate.Value.ToString (NoteArchiver.DATE_TIME_FORMAT);

            // TODO: Figure out what we do on client side for this
            //          if (LastSyncRevision.HasValue)
            //              noteUpdateObj [LastSyncRevisionElementName] = LastSyncRevision;
            if (OpenOnStartup.HasValue)
                noteUpdateObj [OpenOnStartupElementName] = OpenOnStartup.Value;
            if (Pinned.HasValue)
                noteUpdateObj [PinnedElementName] = Pinned.Value;

            if (Tags != null) {
                Hyena.Json.JsonArray tagArray =
                    new Hyena.Json.JsonArray ();
                foreach (string tag in Tags)
                    tagArray.Add (tag);
                noteUpdateObj [TagsElementName] = tagArray;
            }

            return noteUpdateObj;
        }

        #endregion

        #region NoteObject Members

        public string Guid { get; set; }

        public string Title { get; set; }

        public string NoteContent { get; set; }

        public double? NoteContentVersion { get; set; }

        public DateTime? LastChangeDate { get; set; }

        public DateTime? LastMetadataChangeDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? LastSyncRevision { get; set; }

        public bool? OpenOnStartup { get; set; }

        public bool? Pinned { get; set; }

        public List<string> Tags { get; set; }

        public string Command { get; set; }

        #endregion

        #region Private Constants

        private const string GuidElementName = "guid";
        private const string TitleElementName = "title";
        private const string NoteContentElementName = "note-content";
        private const string NoteContentVersionElementName = "note-content-version";
        private const string LastChangeDateElementName = "last-change-date";
        private const string LastMetadataChangeDateElementName = "last-metadata-change-date";
        private const string CreateDateElementName = "create-date";
        private const string LastSyncRevisionElementName = "last-sync-revision";
        private const string OpenOnStartupElementName = "open-on-startup";
        private const string PinnedElementName = "pinned";
        private const string TagsElementName = "tags";
        private const string CommandElementName = "command";

        #endregion
    }
}

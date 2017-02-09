using System;
using Tomboy.FirebaseAddin.Api;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Tomboy.FirebaseAddin
{
    public static class NoteConvert
    {
        public static FirebaseNoteObject ToFirebaseNoteObject (Note note)
        {
            FirebaseNoteObject fbNoteObj = new FirebaseNoteObject ();

            fbNoteObj.Guid = note.Id;
            fbNoteObj.Title = note.Title
                .Replace ("&", "&amp;")
                .Replace ("<", "&lt;")
                .Replace (">", "&gt;")
                .Replace ("\"", "&quot;")
                .Replace ("\'", "&apos;");
            fbNoteObj.OpenOnStartup = note.IsOpenOnStartup;
            fbNoteObj.Pinned = note.IsPinned;
            fbNoteObj.CreateDate = note.CreateDate;
            fbNoteObj.LastChangeDate = note.ChangeDate;
            fbNoteObj.LastMetadataChangeDate = note.MetadataChangeDate;

            fbNoteObj.Tags = new List<string> ();
            foreach (Tag tag in note.Tags)
                fbNoteObj.Tags.Add (tag.Name);

            const string noteContentRegex =
                @"^<note-content([^>]+version=""(?<contentVersion>[^""]*)"")?[^>]*((/>)|(>(?<innerContent>.*)</note-content>))$";
            Match m = Regex.Match (note.XmlContent, noteContentRegex, RegexOptions.Singleline);
            Group versionGroup = m.Groups ["contentVersion"];
            Group contentGroup = m.Groups ["innerContent"];

            double contentVersion;
            if (versionGroup.Success &&
                double.TryParse (versionGroup.Value, out contentVersion)) {
                fbNoteObj.NoteContentVersion = contentVersion;
            } else
                fbNoteObj.NoteContentVersion = 0.1;  // TODO: Constants, transformations, etc, if this changes

            if (contentGroup.Success) {
                string [] splits =
                    contentGroup.Value.Split (new char [] {'\n'}, 2);
                if (splits.Length > 1 && splits [1].Length > 0) {
                    StringBuilder builder = new StringBuilder (contentGroup.Value.Length);
                    bool inTag = false;
                    // Strip everything out of first line, except for XML tags
                    // TODO: Handle 'note-title' element differently?
                    //       Ideally we would want to get rid of it completely.
                    foreach (char c in splits [0]) {
                        if (!inTag && c == '<')
                            inTag = true;
                        if (inTag) {
                            builder.Append (c);
                            if (c == '>')
                                inTag = false;
                        }
                    }

                    // Trim leading newline, if there is one
                    if (splits [1][0] == '\n')
                        builder.Append (splits [1], 1, splits [1].Length - 1);
                    else
                        builder.Append (splits [1]);

                    fbNoteObj.NoteContent = builder.ToString ();
                }
            }

            if (fbNoteObj.NoteContent == null)
                fbNoteObj.NoteContent = string.Empty;

            return fbNoteObj;
        }
            
        public static NoteData ToNoteData (FirebaseNoteObject fbNoteObj)
        {
            // NOTE: For now, we absolutely require values for
            //       Guid, Title, NoteContent, and NoteContentVersion
            // TODO: Is this true? What happens if dates are excluded?
            NoteData noteData = new NoteData (NoteUriFromGuid (fbNoteObj.Guid));
            noteData.Title = fbNoteObj.Title
                .Replace ("&amp;", "&")
                .Replace ("&lt;", "<")
                .Replace ("&gt;", ">")
                .Replace ("&quot;", "\"")
                .Replace ("&apos;", "\'");
            noteData.Text = string.Format ("<note-content version=\"{0}\">{1}\n\n{2}</note-content>",
                fbNoteObj.NoteContentVersion,
                fbNoteObj.Title,
                fbNoteObj.NoteContent);
            if (fbNoteObj.LastChangeDate.HasValue)
                noteData.ChangeDate = fbNoteObj.LastChangeDate.Value;
            if (fbNoteObj.LastMetadataChangeDate.HasValue)
                noteData.MetadataChangeDate = fbNoteObj.LastMetadataChangeDate.Value;
            if (fbNoteObj.CreateDate.HasValue)
                noteData.CreateDate = fbNoteObj.CreateDate.Value;
            if (fbNoteObj.OpenOnStartup.HasValue)
                noteData.IsOpenOnStartup = fbNoteObj.OpenOnStartup.Value;
            // TODO: support Pinned -- http://bugzilla.gnome.org/show_bug.cgi?id=433412

            if (fbNoteObj.Tags != null) {
                foreach (string tagName in fbNoteObj.Tags) {
                    Tag tag = TagManager.GetOrCreateTag (tagName);
                    noteData.Tags [tag.NormalizedName] = tag;
                }
            }

            return noteData;
        }

        public static string ToNoteXml (FirebaseNoteObject fbNoteObj)
        {
            NoteData noteData = ToNoteData (fbNoteObj);
            return NoteArchiver.WriteString (noteData);
        }
        
        // TODO: Copied from Note.cs, duplication sucks
        private static string NoteUriFromGuid (string guid)
        {
            return "note://tomboy/" + guid;
        }
    }
}


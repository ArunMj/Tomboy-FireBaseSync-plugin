using System;
using System.Collections;
using System.Collections.Generic;
using FireSharpSimple;
using Gtk;

namespace Tomboy.FirebaseAddin.Api
{
    public class FirebaseTranspoter
    {
        private Hyena.Json.Serializer jsonSerializer = new Hyena.Json.Serializer ();
        private Hyena.Json.Deserializer jsonDeserializer = new Hyena.Json.Deserializer ();
        private FirebaseClient fbClient;

        public FirebaseTranspoter (ConnectionProps conProps)
        {
            fbClient = new FirebaseClient (new FirebaseConfig () {
                BasePath = conProps.FirebaseUrl
            });
        }

        public void UploadToServer (List<FirebaseNoteObject> fnotes, int revision)
        {
            if (fnotes.Count == 0) {
                Logger.Info (" $$$ No notes to upload.  :-(   ");
                return;
            }
            Hyena.Json.JsonObject updaterObject = new Hyena.Json.JsonObject ();

            foreach (FirebaseNoteObject fn in fnotes) {
                Hyena.Json.JsonObject noteinfo = new Hyena.Json.JsonObject () {
                    {"revision", revision       },
                    {"content",  fn.ToJsonObj() }
                };
                updaterObject.Add (fn.Guid, noteinfo);
            }
            //updaterObject.Dump ();
            jsonSerializer.SetInput (updaterObject);

            Logger.Debug ("$$$ UPDATE@UploadToServer: " + jsonSerializer.Serialize ());
            var r = fbClient.Update ("inventory", jsonSerializer.Serialize ());
            Logger.Debug ("$$$ GOT@UploadToServer : " + r.Body);
            fbClient.Update ("info","{\"revision\" : " +  revision  + "}");
        }

        public List<string> GetUidsFromServer ()
        {
            String resp = fbClient.Get ("inventory", QueryBuilder.New ().Shallow (true)).Body;
            if (resp.Trim () == "null") {
                Logger.Info (" $$$ No notes/guids found in server");
                return new List<string> ();
            }
            Logger.Debug ("$$$ [GOT]@GetUidsFromServer :" + resp);
            jsonDeserializer.SetInput (resp);
            Hyena.Json.JsonObject uidDict = (Hyena.Json.JsonObject)jsonDeserializer.Deserialize ();
            var keysCol = uidDict.Keys;
            var keysList = new List<String> (keysCol);
            return keysList;
        }

        public int? GetLatestRevision(){
            String resp = fbClient.Get ("info/revision").Body;
            Logger.Debug ("$$$ [GOT]@GetLatestRevision :" + resp);
            return int.Parse (resp);
        }

        public void DeleteFromSerever (List<String> guids)
        {
            if (guids.Count == 0) {
                Logger.Info (" $$$ No notes to delete in server.  :-|   ");
                return;
            }
            Hyena.Json.JsonObject deleterObject = new Hyena.Json.JsonObject ();
            foreach (var guid in guids) {
                deleterObject.Add (guid, null);
            }
            jsonSerializer.SetInput (deleterObject);
            Logger.Debug ("$$$ UPDATE@DeleteFromSerever : " + jsonSerializer.Serialize ());
            var r = fbClient.Update ("inventory", jsonSerializer.Serialize ());
            Logger.Debug ("$$$ GOT@DeleteFromSerever : " + r.Body);
        }

        public List<FirebaseNoteObject> DownloadNotes (int revision)
        {
            var fnotes = new List<FirebaseNoteObject> ();
            var resp = fbClient.Get ("inventory", QueryBuilder.New ()
                                     .OrderBy ("revision")
                                     .StartAt (revision + 1)).Body;
            if(resp.Trim () == "{}"){
                Logger.Info ("$$$ No notes found in server");
                return fnotes;
            }
            jsonDeserializer.SetInput (resp);
            Hyena.Json.JsonObject inventoryDict = (Hyena.Json.JsonObject)jsonDeserializer.Deserialize ();
            //inventoryDict.Dump ();
            foreach (var item in inventoryDict) {
                var content = (Hyena.Json.JsonObject)((Hyena.Json.JsonObject)item.Value) ["content"];
                var fnote = FirebaseNoteObject.FromJson (content);
                fnote.LastSyncRevision = (int)(((Hyena.Json.JsonObject)item.Value) ["revision"]);
         fnote.Guid = item.Key;
                fnote.ToJsonObj ().Dump ();
                fnotes.Add (fnote);
            }
            return fnotes;
        }
    }
}

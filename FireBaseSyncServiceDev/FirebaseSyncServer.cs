using System;
using Gtk;
using Tomboy.Sync;
using System.Collections.Generic;
using System.Collections;
using Atk;
using Tomboy.FirebaseAddin.Api;
namespace Tomboy.FirebaseAddin
{
    public class FirebaseSyncServer: SyncServer
    {
		private FirebaseTranspoter fbTransporter;
		private List<FirebaseNoteObject> notesToBeUploaded;
		private List<String> guidsMarkedForDeletion;

		public FirebaseSyncServer()
		{
            ConnectionProps connectionProps = new ConnectionProps
            {
                // for testing
                FirebaseUrl = "https://helloworld-31af0.firebaseio.com",   
            };
            fbTransporter = new FirebaseTranspoter(connectionProps);
		}
		public FirebaseSyncServer (ConnectionProps connectionProps){
			//throw new Exception ("dshjfdsjhfjdgfjhgdjhg");
			fbTransporter = new FirebaseTranspoter(connectionProps);
        }

        #region SyncServer interface implementation 
        //
        // Properties
        //
        public SyncLockInfo CurrentSyncLock {
            get {
                //TODO
                return null;
            }
        }

        public string Id {
            //TODO
			get;//{Logger.Debug (" ***  *** * 	asking ID propery  ***  *** ");}
        }

		// NOTE: Only reliable during a transaction
        public int LatestRevision {
            //TODO
			get;//{	Logger.Debug (" ***  *** * 	LatestRevision propery  ***  *** ");}
        }

        //
        // Methods
        //
        /// <summary>
        /// Begins the sync transaction.setup everything
        /// called before method UploadNotes
        /// </summary>
        /// <returns><c>true</c>, if sync transaction was begun, <c>false</c> otherwise.</returns>
        public bool BeginSyncTransaction (){
            notesToBeUploaded = new List<FirebaseNoteObject>();
            guidsMarkedForDeletion = new List<string>();
            Logger.Debug(" ***  BEGINSYNCTRANSACTION");
            return true;
        }

        /// <summary>
        /// called when tomboys cancel the transaction. clean ups are done here.
        /// </summary>
        /// <returns><c>true</c> if this instance cancel sync transaction; otherwise, <c>false</c>.</returns>
        public bool CancelSyncTransaction (){
            Logger.Debug(" ***  CANCELSYNCTRANSACTION");
            notesToBeUploaded.Clear ();
            guidsMarkedForDeletion.Clear();
            return false;
        }

        /// <summary>
        /// Commits the sync transaction.Actual note upload done here
        /// </summary>
        /// <returns><c>true</c>, if sync transaction was commited successfully, <c>false</c> otherwise.</returns>
        public bool CommitSyncTransaction (){
            //TODO
            Logger.Debug(" ***  COMMITSYNCTRANSACTION");
            fbTransporter.UploadToServer(this.notesToBeUploaded,5);
            fbTransporter.DeleteFromSerever (guidsMarkedForDeletion);
            return true;
        }

        public void DeleteNotes (IList<string> deletedNoteUUIDs){
            Logger.Debug(" *** DELETENOTES  (uuids): " + Utils.str(deletedNoteUUIDs));
			foreach (string uuid in deletedNoteUUIDs)
			{
				guidsMarkedForDeletion.Add(uuid);
			}
        }

        /// <summary>
        /// Gets all note UUIDs present in server.
        /// </summary>
        /// <returns>The all note UUI ds.</returns>
        public IList<string> GetAllNoteUUIDs (){
            //TODO
            Logger.Debug(" ***  GETALLNOTEUUIDS () : response --");
            List<string> uids = fbTransporter.GetUidsFromServer();
            return uids;
        }

       
        public IDictionary<string, NoteUpdate> GetNoteUpdatesSince (int revision){
            //TODO
            Logger.Debug(String.Format (" ***  GETNOTEUPDATESSINCE ({0})",revision));
            Dictionary<string, NoteUpdate> updates =
                new Dictionary<string, NoteUpdate> ();
            var serverNotes = fbTransporter.DownloadNotes (revision);
            foreach (var fn  in serverNotes) {
                string noteXml = NoteConvert.ToNoteXml(fn);
                NoteUpdate update = new NoteUpdate (noteXml,
                                                    fn.Title,
                                                    fn.Guid,
                                                    fn.LastSyncRevision.Value);
                updates.Add (fn.Guid, update);
            }
            return updates;
        }

		public bool UpdatesAvailableSince(int revision){
			//TODO
            Logger.Debug(String.Format (" ***  UPDATESAVAILABLESINCE ({0})",revision));
			return false;
		}

        public void UploadNotes (IList<Note> notes){
            //TODO
            Logger.Debug(" ***  UPLOADNOTES(" + Utils.str(notes,x=>((Note)x).Id+"["+((Note)x).Title+"]")+" )");
			foreach (Note note in notes)
			{
				FirebaseNoteObject fn = NoteConvert.ToFirebaseNoteObject(note);
				notesToBeUploaded.Add(fn);
			}
        }

        #endregion


        // ------------------ custom members -----------------
    }
}

